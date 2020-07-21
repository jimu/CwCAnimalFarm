using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DestroyOutOfBounds : MonoBehaviour
{
    float minX = -30f, maxX = 30f, minZ = -30f, maxZ = 30f;
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
            Destroy(gameObject);

            if (gameObject.CompareTag("Enemy"))
                Debug.Log("GAME OVER");
        }
    }
}
