using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    Transform healthBar;
    [SerializeField] Texture boxTexture;
    private Vector2 size = new Vector2(32, 7);
    
    static new Camera camera = null;
    static Texture2D greenTexture = null;
    static Texture2D redTexture = null;
    static Texture2D blackTexture = null;

    float height;
    [SerializeField] float health = 1.0f;

    static private void Init()
    {
        if (camera == null)
        {
            Debug.Log("Init");
            camera = Camera.main;

            redTexture = MakeTexture(Color.red);
            greenTexture = MakeTexture(Color.green);
            blackTexture = MakeTexture(Color.black);
        }

    }

    static Texture2D MakeTexture(Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }

    private void Awake()
    {
        Init();
        BoxCollider bc = gameObject.GetComponent<BoxCollider>();
        height = bc.bounds.size.y;
    }


    private void OnGUI()
    {
        Vector2 v1 = camera.WorldToScreenPoint(transform.position + new Vector3(0, height, 0));
        GUI.DrawTexture(new Rect(v1.x - 16, Screen.height - v1.y - 3 - 5, 32, 7), blackTexture);
        GUI.DrawTexture(new Rect(v1.x - 15, Screen.height - v1.y - 2 - 5, 30 * health, 5), greenTexture);
    }

    public float SetHealth(float health)
    {
        return this.health = Mathf.Clamp(health, 0f, 1f);
    }

    public bool SetEnabled(bool enabled = true)
    {
        return this.enabled = enabled;
    }    
}
