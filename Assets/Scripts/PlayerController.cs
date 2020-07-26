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
    [SerializeField] Transform[] bulletPrefab = null;
    [SerializeField] float recoil = -0.25f;
    Vector3 ammoHeight = Vector3.up * 0.25f;
    float cooldown = 0f;
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        animator = GetComponent<Animator>();
        animator.SetBool("Static_b", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0.1f && gm.playerHits > 0)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            animator.SetFloat("Speed_f", Mathf.Abs(h) + Mathf.Abs(v));

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

            if (Time.time > cooldown && !EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButton(0))
                    Fire(0);
                else if (Input.GetMouseButtonDown(1))
                    Fire(1);
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


    void Fire(int type)
    {
        Debug.Log("Fire(" + type + ")");
        cooldown = Time.time + rof;

        if (gm.GetAmmo(type) > 0)
        {

            Vector3 p = transform.position;
            Vector3 facing = transform.forward;
            p.y = Terrain.activeTerrain.SampleHeight(p + facing) + 0.25f;

            facing.y = 0f;

            var bullet = Instantiate(bulletPrefab[type], p, Quaternion.LookRotation(facing));
            gm.AddAmmo(type, -1);
            transform.Translate(transform.forward * recoil, Space.World);  // recoil
            gm.Play(gm.launch);

        }
        else if (type == 0) // only regular ammo
        {
            Debug.Log("Out of ammo");
            gm.Play(gm.sfxOutOfAmmo);
        }
    }
}
