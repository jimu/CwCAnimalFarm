using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearts : MonoBehaviour
{

    [SerializeField] private int number;
    [SerializeField] private int width;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public int Number
    {
        get { return number; }
        set {
            number = value;
            rectTransform.sizeDelta = new Vector2(width * number, 40);  //hearts=55, bones=13
        }
    }
}
