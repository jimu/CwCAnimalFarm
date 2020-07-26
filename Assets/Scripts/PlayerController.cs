using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    GameManager gm;

    int minX = -25, maxX = 25, minZ = -25, maxZ = 25;

    [SerializeField] float speed = 10f;
    [SerializeField] float rof = 0.5f;
    [SerializeField] Transform bulletPrefab = null;
    [SerializeField] float recoil = -0.25f;
    Vector3 ammoHeight = Vector3.up * 0.25f;
    float cooldown = 0f;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0.1f)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            float x = Mathf.Clamp(transform.position.x + h * Time.deltaTime * speed, minX, maxX);
            float z = Mathf.Clamp(transform.position.z + v * Time.deltaTime * speed, minZ, maxZ);
            float y = Terrain.activeTerrain.SampleHeight(transform.position);
            transform.position = new Vector3(x, y, z);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 lookAt = hit.point;
                lookAt.y = transform.position.y;
                transform.LookAt(lookAt);
            }

            if (Time.time > cooldown && Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                cooldown = Time.time + rof;
                Fire();
            }
//            Debug.DrawRay(transform.position, transform.forward* 5, Color.green);
//            Debug.DrawRay(transform.position, transform.forward * -2, Color.red);

            if (Input.GetKeyDown(KeyCode.P))
                gm.OnPausePressed();
            //if (Input.GetKeyDown(KeyCode.Minus))
            //    gm.HurtPlayer();
            //if (Input.GetKeyDown(KeyCode.Equals))
            //    gm.AddScore(1);
        }
    }


    void Fire()
    {
        if (gm.GetAmmo() > 0)
        {

            Vector3 p = transform.position;
            Vector3 facing = transform.forward;
            p.y = Terrain.activeTerrain.SampleHeight(p + facing) + 0.25f;

            facing.y = 0f;

            var bullet = Instantiate(bulletPrefab, p, Quaternion.LookRotation(facing));
            gm.AddAmmo(-1);
            transform.Translate(transform.forward * recoil, Space.World);  // recoil
            gm.Play(gm.launch);

        }
        else
        {
            Debug.Log("Out of ammo");
            gm.Play(gm.sfxOutOfAmmo);
        }
    }
}
