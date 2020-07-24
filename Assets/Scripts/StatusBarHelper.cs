using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBarHelper : MonoBehaviour
{
    [SerializeField] GameObject statusBar;

    void Start()
    {
        statusBar = GameObject.Find("StatusBar");
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
