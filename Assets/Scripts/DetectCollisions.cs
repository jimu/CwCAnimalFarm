using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ammo"))
        {
            Destroy(other.gameObject);
            gameObject.SetActive(false);
            GameManager.instance.AddScore(1);
        }
        else if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            Debug.Log("Game Over (HIT)");
            Time.timeScale = 0f;
        }
    }
}
