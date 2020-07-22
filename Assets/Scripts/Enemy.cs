using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hits;
    public int maxHits;
    public float speed;
    public int value;

    static private GameManager gm;

    private HealthBar healthBar;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip killSound;
    [SerializeField] public AudioClip escapeSound;

    void Awake()
    {
        healthBar = gameObject.GetComponent<HealthBar>();
        gm = GameManager.instance;
    }

    private void OnEnable()
    {
        hits = maxHits;
        healthBar.SetHealth(1f);
        GetComponent<HealthBar>()?.SetEnabled();
    }

    public void ApplyDamage(int damage)
    {
        hits = Math.Max(0, hits - damage);

        if (hits < 1)
            SetDying();
        else if (healthBar != null)
        {
            healthBar.SetHealth(hits * 1f / maxHits);
            gm.Play(hitSound);
        }

    }


    public void SetDying()
    {
        GetComponent<HealthBar>()?.SetEnabled(false);

        //gameObject.SetActive(false);
        gm.AddScore(value);
        // TODO show score animation
        gm.Play(killSound);
    }

}

