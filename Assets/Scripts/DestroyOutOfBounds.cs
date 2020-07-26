using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DestroyOutOfBounds : MonoBehaviour
{
    float minX = -41f, maxX = 41f;
    float minZ = -25f, maxZ = 41f;
    float minY = -8f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < minX || transform.position.x > maxX ||
            transform.position.z < minZ || transform.position.z > maxZ ||
            transform.position.y < minY)
        {
            // Escape if object is an enemy and is not falling down through floor
            if (gameObject.CompareTag("Enemy") && transform.position.y >= minY )
            {
                GameManager.instance.HurtPlayer(1, false);
                GameManager.instance.Play(gameObject.GetComponent<Enemy>().escapeSound);
            }
            gameObject.SetActive(false);
        }
    }
}
