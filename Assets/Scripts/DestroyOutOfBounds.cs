using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DestroyOutOfBounds : MonoBehaviour
{
    float minX = -41f, maxX = 41f, minZ = -41f, maxZ = 41f;
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
            gameObject.SetActive(false);

            if (gameObject.CompareTag("Enemy"))
                Debug.Log("GAME OVER");
        }
    }
}
