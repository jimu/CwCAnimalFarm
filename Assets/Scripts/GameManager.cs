﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public enum GameState { Invalid, StartMenu, Paused, GameOver, Instructions, HighScoresMenu, NewHighScore, EasterEgg, Playing };
    public GameState gameState = GameState.Invalid;
    [SerializeField] EnemySpawner enemySpawner = null;

    private int score = 0;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] int[] ammo = { 40, 0 };
    [SerializeField] TextMeshProUGUI ammoText;

    public static GameManager instance;

    public int playerHits = 3;

    private Hearts hearts;
    private Hearts bones;

    private AudioSource audioSource;
    [SerializeField] public AudioClip playerHurt;
    [SerializeField] public AudioClip playerDead;
    [SerializeField] public AudioClip launch;
    [SerializeField] public AudioClip sfxOutOfAmmo;
    [SerializeField] public AudioClip sfxBadInput;

    [SerializeField] GameObject gameOverPanel = null;
    [SerializeField] GameObject pausePanel = null;
    [SerializeField] GameObject titlePanel = null;
    [SerializeField] GameObject highScoresPanel = null;
    [SerializeField] GameObject newHighScoreDialog = null;
    [SerializeField] GameObject pauseButton = null;
    [SerializeField] GameObject instructionsPanel = null;
    [SerializeField] GameObject easterEggPanel = null;
    [SerializeField] public Text statusBar = null;

    public GameObject pointsGizmoPrefab;
    private Pool pointsGizmoPool;

    private GameObject canvas;

    private void Awake()
    {
        Debug.Log("GameManager Awake");
        instance = this;

        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        ammoText = GameObject.Find("AmmoText").GetComponent<TextMeshProUGUI>();
        hearts = GameObject.Find("Hearts").GetComponent<Hearts>();
        bones = GameObject.Find("Bones").GetComponent<Hearts>();

        canvas = GameObject.Find("Canvas");
        audioSource = GetComponent<AudioSource>();

        pointsGizmoPool = new Pool(pointsGizmoPrefab, 10);

        SetGameState(GameState.StartMenu);

    }

    void Start()
    {
        NetworkManager.instance.Fetch();

        enemySpawner.SpawnEnemy(0);
        enemySpawner.StartSpawning();
        SetScore(0);
        SetAmmo(0, 40);
        SetAmmo(1, 0);
        ammoText.color = Color.white;
    }


    private void SetGameState(GameState state)
    {
        Debug.Log("SetGameState(" + state + ")");
        gameState = state;
        gameOverPanel.SetActive(state == GameState.GameOver);
        pausePanel.SetActive(state == GameState.Paused);
        titlePanel.SetActive(state == GameState.StartMenu);
        highScoresPanel.SetActive(state == GameState.HighScoresMenu);
        newHighScoreDialog.SetActive(state == GameState.NewHighScore);
        pauseButton.SetActive(state == GameState.Playing);
        easterEggPanel.SetActive(state == GameState.EasterEgg);
        instructionsPanel.SetActive(state == GameState.Instructions);

        Time.timeScale = state == GameState.Playing ? 1f : 0f;
    }


    public void InstantiatePointsGizmo(int value, Vector3 position = default)
    {
        //       Debug.Log("InstantiatePointsGizmo(" + value + ")");
        GameObject gizmo = PointsGizmo.Activate(pointsGizmoPool, value, position);
        //gizmo.transform.parent = canvas.transform;
        gizmo.transform.SetParent(canvas.transform, false);
        // Parent of RectTransform is being set with parent property.
        // Consider using the SetParent method instead, with the worldPositionStays argument set to false.
        // This will retain local orientation and scale rather than owrld orientation and scale
        // which can preven tcommon UI scaling issues

        //TODO move the SetParent call to where the prefabs are Instantiated
    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int n)
    {
        SetScore(score + n);
        InstantiatePointsGizmo(n);
    }

    public int AddAmmo(int type, int n)
    {
        return SetAmmo(type, ammo[type] + n);
    }

    public int GetAmmo(int type)
    {
        return ammo[type];
    }





    public int SetScore(int n)
    {
        score = n;
        scoreText.text = score.ToString();
        return score;
    }
    public int SetAmmo(int type, int n)
    {
        Debug.Log("SetAmmo(" + type + ", " + n + ")");
        ammo[type] = n;

        if (type == 0)
        {
            if (n > 0)
                ammoText.text = ammo[type].ToString();
            else
            {
                ammoText.text = "0 EMPTY!";
                ammoText.color = Color.red;
                enemySpawner.SetMurderMode();
            }
        }
        else
        {
            bones.Number = ammo[type];
        }
        return n;
    }


    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }


    public void HurtPlayer(int damage = 1, bool playSound = true)
    {
        playerHits = damage < playerHits ? playerHits - damage : 0;
        hearts.Number = playerHits;

        if (playerHits <= 0)
        {
            Play(playerDead);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetBool("Death_b", true);
            Invoke("GameOver", 2.0f);
        }
        else if (playSound)
            Play(playerHurt);
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        Debug.Log("GAME OVER");

        Text scoreText = gameOverPanel?.transform.Find("ScoreText")?.GetComponent<Text>();
        if (scoreText != null)
            scoreText.text = score.ToString();

        if (score > 0)
            SetGameState(GameState.NewHighScore);
        else
            SetGameState(GameState.GameOver);
    }

    public void OnPausePressed()
    {
        SetGameState(GameState.Paused);
    }

    public void OnRestartPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //SetGameState(GameState.Playing);  // We never get to this statement!
    }
    public void OnStartPressed()
    {
        SetGameState(GameState.Playing);  // We never get to this statement!
    }
    public void OnResumePressed()
    {
        SetGameState(GameState.Playing);
    }
    public void OnHighScoresPressed()
    {
        SetGameState(GameState.HighScoresMenu);
    }

    public void OnEasterEggPressed()
    {
        SetGameState(GameState.EasterEgg);
    }
    public void GimmieAnotherHint()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=-GL5lzMJomY");
    }

    public string SavePlayerName(string name)
    {
        PlayerPrefs.SetString("playerName", name);
        return name;
    }

    public string GetPlayerName()
    {
        return PlayerPrefs.GetString("playerName");
    }

    public void SeeAllHighscoresPressed()
    {
        Application.OpenURL("https://osaka.jimu.net/cwc");
    }

    public void OnInstructionsButtonPressed()
    {
        SetGameState(GameState.Instructions);
    }
}