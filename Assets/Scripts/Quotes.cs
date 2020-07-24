using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quotes : MonoBehaviour
{
    static readonly string[] quotes =
    {
        "Four legs good, two legs bad",
        "All animals are equal, but some animals are more equal than others",
        "I will work harder – Boxer",
        "No sentimentality, comrade! – Snowball",
        "Napoleon is always right – Boxer",
        "Donkeys live a long time. None of you has ever seen a dead donkey – Benjamin",
        "There, comrades, is the answer to all our problems. It is summed up in a single word – Man",
        "Whatever goes upon two legs is an enemy – 1st Commandment",
        "Whatever goes upon four legs, or has wings, is a friend – 2nd Commandment",
        "No animal shall wear clothes – 3rd Commandment",
        "No animal shall sleep in a bed – 4th Commandment",
        "No animal shall drink alcohol – 5th Commandment",
        "No animal shall kill any other animal – 6th Commandment",
        "All animals are equal – 7th Commandment",
        "The only good human being is a dead one"
    };

    [SerializeField] Text quoteText;
    int nextQuote = 0;
    float nextTime = 0;

    const float QUOTE_DURATION = 10f;

    void Start()
    {
        ChangeQuote();        
    }

    private void OnEnable()
    {
        ChangeQuote();
    }

    private void ChangeQuote(bool force = false)
    {
        if (Time.realtimeSinceStartup > nextTime || force)
        {
            quoteText.text = quotes[nextQuote];
            nextQuote = (nextQuote + 1) % quotes.Length;
            nextTime = Time.realtimeSinceStartup + QUOTE_DURATION;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            ChangeQuote(true);
        ChangeQuote();
    }

}
