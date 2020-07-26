using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardAmmo : MonoBehaviour
{
    [SerializeField] float speed = 5;

    void Update()
    {
//w        Vector3 b4 = transform.position;
        Vector3 dest = transform.position + transform.forward * Time.deltaTime * speed;
//        Debug.Log("dest = " + dest);

//        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.yellow);
        dest.y = Terrain.activeTerrain.SampleHeight(dest) + 0.5f;
        transform.position = dest; //Vector3.MoveTowards(transform.position, dest, Time.deltaTime * speed);
        //Debug.Log(b4 + " - " + dest);
        // transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
