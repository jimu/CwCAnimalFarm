using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string tip = "";

    static string defaultTip = "";
    static Color bold = new Color(1f, 1f, 1f);
    static Color gray = new Color(0.7f, 0.7f, 0.7f);
    static Color gold = new Color(1f, 0.9f, 0f);
    static ToolTip instance = null;


    public static void SetDefaultTip(string tip)
    {
        defaultTip = tip;
        Text statusText = GameManager.instance.statusBar;
        if (statusText != null && statusText.color != bold)
        {
            statusText.text = defaultTip;
            statusText.color = defaultTip == "Please turn sound on!" ? gold : gray;
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        GameManager.instance.statusBar.color = bold;
        GameManager.instance.statusBar.text = tip;
    }
    public void OnPointerExit(PointerEventData data)
    {
        GameManager.instance.statusBar.color = gray;
        GameManager.instance.statusBar.text = defaultTip;
    }
}
