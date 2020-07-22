using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DestroyOutOfBounds : MonoBehaviour
{
    float minX = -41f, maxX = 41f, minZ = -5f, maxZ = 41f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < minX || transform.position.x > maxX ||
            transform.position.z < minZ || transform.position.z > maxZ)
        {
            if (gameObject.CompareTag("Enemy"))
            {
                GameManager.instance.HurtPlayer(1, false);
                GameManager.instance.Play(gameObject.GetComponent<Enemy>().escapeSound);
            }
            gameObject.SetActive(false);
        }
    }
}
