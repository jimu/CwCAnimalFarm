using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] float speed = 5;
    private float downSpeed = 1.0f;
    private Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        if (enemy != null)
            speed = enemy.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy!= null && enemy.hits <= 0)
            transform.Translate(Vector3.down * Time.deltaTime * downSpeed);
        else if (enemy == null || !enemy.eating)
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
