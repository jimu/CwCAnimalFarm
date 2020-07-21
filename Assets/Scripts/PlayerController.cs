using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    int minX = -10, maxX = 10, minZ = -10, maxZ = 10;

    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float x = Mathf.Clamp(transform.position.x * h * Time.deltaTime * speed, minX, maxX);
        float z = Mathf.Clamp(transform.position.z * v * Time.deltaTime * speed, minZ, maxZ);
        transform.Translate(new Vector3(x, 0f, z));

    }
}
