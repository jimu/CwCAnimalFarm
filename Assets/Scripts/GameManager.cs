using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    public enum GameState { Invalid, StartMenu, PauseMenu, FinishMenu, HighScoresMenu, Playing };
    public GameState gameState = GameState.Invalid;
    [SerializeField] EnemySpawner enemySpawner;

    private int score = 0;
    [SerializeField] TextMeshProUGUI scoreText;

    private int ammo = 0;
    [SerializeField] TextMeshProUGUI ammoText;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        ammoText = GameObject.Find("AmmoText").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        enemySpawner.StartSpawning();
        SetScore(0);
        SetAmmo(100);
    }


    public void AddScore(int n)
    {
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
}
