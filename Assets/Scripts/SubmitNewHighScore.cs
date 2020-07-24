﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubmitNewHighScore : MonoBehaviour
{
    [SerializeField] InputField nameInputField = null;
    [SerializeField] Button submitButton = null;
    [SerializeField] Text scoreText = null;

    // Update is called once per frame
    private GameManager gm;


    private void Init()
    {
        if (gm == null)
        {
            gm = GameManager.instance;
        }
    }

    private void Start()
    {
        Init();
        Debug.Log("SubmitNewHighScore.Start");
        Debug.Log("  Start GM=" + GameManager.instance == null ? "NULL" : "NOTNULL");
    }

    private void OnEnable()
    {
        Init();
        scoreText.text = gm.GetScore().ToString();
        EnableNameField();
    }

    void EnableNameField()
    {
        nameInputField.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        nameInputField.text = gm.GetPlayerName();
        Debug.Log("Set Listener");
        nameInputField.gameObject.SetActive(true);
        nameInputField.Select();
        nameInputField.ActivateInputField();
    }


    void ValueChangeCheck()
    {
        submitButton.interactable = nameInputField.text.Length > 2 && nameInputField.text[0] != ' ';
    }

    public void OnSubmitButtonPressed()
    {
        string name = nameInputField.text;

        if (name.Length > 0)
        {
            NetworkManager.instance.SumbitScore(name, gm.GetScore());
            gm.SavePlayerName(name);
            gm.OnHighScoresPressed();
        }
        else
            gm.Play(GameManager.instance.sfxBadInput);
    }
}
