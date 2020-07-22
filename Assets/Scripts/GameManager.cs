using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public enum GameState { Invalid, StartMenu, Paused, GameOver, HighScoresMenu, Playing };
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
    [SerializeField] public AudioClip sfxOutOfAmmo;

    [SerializeField] GameObject gameOverPanel = null;
    [SerializeField] GameObject pausePanel;

    public GameObject pointsGizmoPrefab;
    private Pool pointsGizmoPool;

    private GameObject canvas;

    private void Awake()
    {
        instance = this;
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        ammoText = GameObject.Find("AmmoText").GetComponent<TextMeshProUGUI>();
        hearts = GameObject.Find("Hearts").GetComponent<Hearts>();
        audioSource = GetComponent<AudioSource>();

        pointsGizmoPool = new Pool(pointsGizmoPrefab, 10);

        canvas = GameObject.Find("Canvas");
        gameOverPanel?.SetActive(false);

        //pausePanel = Resources.Load<GameObject>("PausePanel");
        //Debug.Log("pausePanel: " + (pausePanel == null ? "NOT loaded" : "loaded"));
        //pausePanel.transform.SetParent(canvas.transform);

    }

    void Start()
    {
        enemySpawner.SpawnEnemy(0);
        enemySpawner.SpawnEnemy(0);

        enemySpawner.StartSpawning();
        SetScore(0);
        SetAmmo(40);
        //TODO work on placement
        /*InstantiatePointsGizmo(11, Vector3.zero);
        InstantiatePointsGizmo(12, Vector3.one * 100);
        InstantiatePointsGizmo(13, Vector3.forward * 200);
        */
        Time.timeScale = 1.0f;
    }


    private void SetGameState(GameState state)
    {
        gameState = state;
        gameOverPanel.SetActive(state == GameState.GameOver);
        pausePanel.SetActive(state == GameState.Paused);

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

    public void AddScore(int n)
    {
        // TODO show score animation

        SetScore(score + n);
        InstantiatePointsGizmo(n);
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

    public void OnPausePressed()
    {
        SetGameState(GameState.Paused);
    }

    public void OnRestartPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SetGameState(GameState.Playing);
    }
    public void OnResumePressed()
    {
        SetGameState(GameState.Playing);
    }
}
