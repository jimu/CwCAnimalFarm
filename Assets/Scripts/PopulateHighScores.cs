using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class PopulateHighScores : MonoBehaviour
{
    [SerializeField] Text ranksText = null;
    [SerializeField] Text namesText = null;
    [SerializeField] Text scoresText = null;
    [SerializeField] Text loadingText = null;

    public void OnEnable()
    {
        Debug.Log("OnEnable called");
        if (NetworkManager.instance)
        {
            NetworkManager.instance.Listen(this);
            ShowScores();
        }
        else
        {
            Debug.Log("NetworkManager is null");
        }
    }

    public bool ShowScores()
    {
        Debug.Log("ShowScores() called");
        NetworkManager nm = NetworkManager.instance;

        nm.Fetch();

        string names = "";
        string scores = "";
        string ranks = "";

        if (nm != null)
        {
            Debug.Log("nm.scoresReady:" + nm.scoresReady);

            ranksText.gameObject.SetActive(nm.scoresReady);
            namesText.gameObject.SetActive(nm.scoresReady);
            scoresText.gameObject.SetActive(nm.scoresReady);
            loadingText.gameObject.SetActive(!nm.scoresReady);

            if (nm.scoresReady)
            {
                for (int i = 0; i < nm.scores.Length; ++i)
                {
                    ranks += (i + 1) + "\n";
                    names += nm.scores[i].name + "\n";
                    scores += nm.scores[i].score.ToString() + "\n";
                }

                if (names.Length > 0)
                {
                    ranksText.text = ranks.Substring(0, ranks.Length - 1);
                    namesText.text = names.Substring(0, names.Length - 1);
                    scoresText.text = scores.Substring(0, scores.Length - 1);
                }
                 
                return true;
            }
        }

        return false;
    }
}
