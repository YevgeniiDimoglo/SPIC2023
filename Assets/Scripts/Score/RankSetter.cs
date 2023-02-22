using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RankSetter : MonoBehaviour
{
    [SerializeField]
    private Sprite[] rankImages;

    // Start is called before the first frame update
    void Start()
    {
        GameObject scorePrefab = GameObject.FindGameObjectWithTag("ScoreTable");

        if (scorePrefab.GetComponent<ScoreHolder>().TotalScore == 0)
        {
            transform.GetComponent<Image>().sprite = rankImages[0];
        }

        if (scorePrefab.GetComponent<ScoreHolder>().TotalScore >= 0 && scorePrefab.GetComponent<ScoreHolder>().TotalScore < 100)
        {
            transform.GetComponent<Image>().sprite = rankImages[1];
        }

        if (scorePrefab.GetComponent<ScoreHolder>().TotalScore >= 100 && scorePrefab.GetComponent<ScoreHolder>().TotalScore < 200)
        {
            transform.GetComponent<Image>().sprite = rankImages[2];
        }

        if (scorePrefab.GetComponent<ScoreHolder>().TotalScore >= 200 && scorePrefab.GetComponent<ScoreHolder>().TotalScore < 300)
        {
            transform.GetComponent<Image>().sprite = rankImages[3];
        }

        if (scorePrefab.GetComponent<ScoreHolder>().TotalScore >= 300)
        {
            transform.GetComponent<Image>().sprite = rankImages[4];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
