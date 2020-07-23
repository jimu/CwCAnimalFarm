using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBarHelper : MonoBehaviour
{
    [SerializeField] GameObject statusBar;
    // Start is called before the first frame update
    void Start()
    {
        statusBar = GameObject.Find("StatusBar");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        statusBar.SetActive(true);
    }

    private void OnDisable()
    {
        statusBar.SetActive(false);
    }
}
