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
    public float recoveryTime;

    static private GameManager gm;

    public bool eating = false;
    
    private Animator animator;
    private HealthBar healthBar;
    [SerializeField] AudioClip hitSound = null;
    [SerializeField] AudioClip killSound = null;
    [SerializeField] public AudioClip escapeSound = null;



    void Awake()
    {
        healthBar = gameObject.GetComponent<HealthBar>();
        animator = gameObject.GetComponent<Animator>();
        gm = GameManager.instance;
    }

    private void OnEnable()
    {
        hits = maxHits;
        healthBar.SetHealth(1f);
        Walk();
        GetComponent<HealthBar>()?.SetEnabled();
        Debug.Log("OnEnable");
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
            Eat();
        }

    }


    public void SetDying()
    {
        // Can only "die" once (healthBar will be enabled unless alive)
        if (healthBar.enabled)
        {
            healthBar.SetEnabled(false);
            gm.AddScore(value);
            gm.Play(killSound);
        }
    }

    void Eat()
    {
        eating = true;
        animator?.SetBool("Eat_b", eating);
        animator?.SetFloat("Speed_f", 0f);

        Invoke("Walk", recoveryTime);
    }

    void Walk()
    {
        eating = false;
        animator?.SetBool("Eat_b", eating);
        animator?.SetFloat("Speed_f", speed / 4f);
    }
}

