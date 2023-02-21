using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreSetter : MonoBehaviour
{
    public GameObject scorePrefab;

    public TMP_Text scoreCounter;
    // Start is called before the first frame update
    void Start()
    {
        scorePrefab = GameObject.FindGameObjectWithTag("ScoreTable");
    }

    // Update is called once per frame
    void Update()
    {
        //if (this.name.Equals("Books")) scoreCounter.text = "Books: " + scorePrefab.GetComponent<ScoreHolder>().booksScore.ToString();
        //if (this.name.Equals("Dust")) scoreCounter.text = "Dust: " + scorePrefab.GetComponent<ScoreHolder>().dustPercent.ToString() + " %";
        //if (this.name.Equals("Picture")) scoreCounter.text = "Pictures: " + scorePrefab.GetComponent<ScoreHolder>().paintingScore.ToString();
        //if (this.name.Equals("Total")) scoreCounter.text = "Total Score: " + scorePrefab.GetComponent<ScoreHolder>().totalScore.ToString();
    }
}
