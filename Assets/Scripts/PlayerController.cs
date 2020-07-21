using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    int minX = -10, maxX = 10, minZ = -10, maxZ = 10;

    [SerializeField] float speed = 10f;
    [SerializeField] float rof = 0.5f;
    [SerializeField] Transform bulletPrefab;
    float cooldown = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float x = Mathf.Clamp(transform.position.x + h * Time.deltaTime * speed, minX, maxX);
        float z = Mathf.Clamp(transform.position.z + v * Time.deltaTime * speed, minZ, maxZ);
        transform.position = new Vector3(x, 0f, z);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit at " + hit.point);
            transform.LookAt(hit.point);
        }

        if (Time.time > cooldown && Input.GetMouseButton(0))
        {
            cooldown = Time.time + rof;
            Fire();
        }
    }

    void Fire()
    {
        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(transform.forward));
    }
}
