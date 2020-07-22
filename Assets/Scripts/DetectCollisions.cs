using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour
{
    private Enemy enemy;
    private GameManager gm;
    // Start is called before the first frame update
    private void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        gm = GameManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ammo"))
        {
            Destroy(other.gameObject);
            enemy?.ApplyDamage(1);
        }
        else if (other.CompareTag("Player"))
        {
            gm.HurtPlayer();
            gameObject.SetActive(false);
        }
    }
}
