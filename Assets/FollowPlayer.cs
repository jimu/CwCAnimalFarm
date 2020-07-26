using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Transform player = null;
    Vector3 offset;
    Vector3 origin;
    Vector3 porigin;
    Vector3 corigin;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position - player.position;
        corigin = transform.position;
        porigin = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = player.position + offset;
        transform.position = corigin + (player.position - porigin) / 2;
    }
}
