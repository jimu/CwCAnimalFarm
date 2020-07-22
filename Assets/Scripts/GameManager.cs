using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState { Invalid, StartMenu, PauseMenu, FinishMenu, HighScoresMenu, Playing };
    public GameState gameState = GameState.Invalid;
    [SerializeField] EnemySpawner enemySpawner = null;

    private int score = 0;
    [SerializeField] TextMeshProUGUI scoreText;

    private int ammo = 0;
    [SerializeField] TextMeshProUGUI ammoText;

    public static GameManager instance;

    private int playerHits = 3;

    private Hearts hearts;

    private AudioSource audioSource;
    [SerializeField] public AudioClip playerHurt;
    [SerializeField] public AudioClip playerDead;
    [SerializeField] public AudioClip launch;

    [SerializeField] GameObject gameOverPanel = null;

    private void Awake()
    {
        instance = this;
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        ammoText = GameObject.Find("AmmoText").GetComponent<TextMeshProUGUI>();
        hearts = GameObject.Find("Hearts").GetComponent<Hearts>();
        audioSource = GetComponent<AudioSource>();
        
    }

    void Start()
    {
        enemySpawner.SpawnEnemy(0);
        enemySpawner.SpawnEnemy(0);

        enemySpawner.StartSpawning();
        SetScore(0);
        SetAmmo(100);
    }


    public void AddScore(int n)
    {
        // TODO show score animation

        SetScore(score + n);
    }

    public void AddAmmo(int n)
    {
        SetAmmo(ammo + n);
    }

    public int GetAmmo()
    {
        return ammo;
    }

    public int SetScore(int n)
    {
        score = n;
        scoreText.text = score.ToString();
        return score;
    }
    public void SetAmmo(int n)
    {
        ammo = n;
        ammoText.text = ammo.ToString();
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
            GameOver();
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
        gameOverPanel?.SetActive(true);
    }
}
