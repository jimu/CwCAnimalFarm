using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;

public class HighScore
{
    public string name;
    public int score;

    public HighScore(string nameStr, string scoreStr)
    {
        name = nameStr;
        score = Convert.ToInt32(scoreStr);
    }
}
public class NetworkManager : MonoBehaviour
{
    static public NetworkManager instance = null;

    public bool scoresReady = false;
    public HighScore[] scores;

    private bool fetching = false;
    private float nextFetch = 0f;

    private PopulateHighScores listener = null;

    public void Listen(PopulateHighScores listener)
    {
        this.listener = listener;
    }
     

    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
    }

    IEnumerator GetRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Send the request and wait for a response
            yield return request.SendWebRequest();
            callback(request);
        }
    }

    public void Fetch(bool force = false)
    {
        Debug.Log("Fetch(): nextFetch=" + nextFetch + " Time.time=" + Time.time + " fetching=" + (fetching ? "T" : "F"));
        if (!fetching && (nextFetch < Time.realtimeSinceStartup || force))
        {
            fetching = true;
            StartCoroutine(GetRequest("https://osaka.jimu.net/cwc/getscores.php", (UnityWebRequest req) =>
            {
                if (req.isNetworkError || req.isHttpError)
                {
                    Debug.Log($"{req.error}: {req.downloadHandler.text}");
                }
                else
                {
                    Debug.Log(req.downloadHandler.text);
                    string[] rows = req.downloadHandler.text.Split('|');
                    scores = new HighScore[rows.Length / 2];
                    for (int i = 0; i < rows.Length - 2; i += 2)
                    {
                        Debug.Log("i=" + i + " scores.Length=" + scores.Length + " rows.Length=" + rows.Length + " scores.Length=" + scores.Length);
                        string n = rows[i];
                        string s = rows[i + 1];
                        scores[i / 2] = new HighScore(rows[i], rows[i + 1]);
                    }
                    scoresReady = true;
                    fetching = false;
                    nextFetch = Time.realtimeSinceStartup + 15f;
                    if (listener)
                        listener.ShowScores();
                }
            }));
        }
    }
    public void SumbitScore(string name, int score)
    {
        string url = "https://osaka.jimu.net/cwc/savescores.php?name=" + name + "&score=" + score;
        Debug.Log("SubmitScore(" + name + ")");
        Post(url);
    }



    public void Post(string url)
    {
        Debug.Log("Post(" + url + ")");
        scoresReady = false;
        StartCoroutine(GetRequest(url, (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            }
            else
            {
                Debug.Log(req.downloadHandler.text);
            }
            Fetch(true);
        }));
    }

}