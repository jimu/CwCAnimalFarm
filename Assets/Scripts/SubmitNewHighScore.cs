using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitNewHighScore : MonoBehaviour
{
    [SerializeField] Text nameText = null;// Start is called before the first frame update

    // Update is called once per frame
    public void OnSubmitButtonPressed()
    {
        GameManager gm = GameManager.instance;

        string name = nameText.text;

        if (name.Length > 0)
        {
            NetworkManager.instance.SumbitScore(name, gm.GetScore());
            gm.OnHighScoresPressed();
        }
        else
            gm.Play(GameManager.instance.sfxBadInput);
    }
}
