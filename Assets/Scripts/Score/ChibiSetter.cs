using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChibiSetter : MonoBehaviour
{

    [SerializeField]
    private Sprite[] rankImages;

    // Start is called before the first frame update
    void Start()
    {
         GameObject scorePrefab = GameObject.FindGameObjectWithTag("ScoreTable");


        if (ScoreHolder.TotalScore <= 500)
        {
            transform.GetComponent<Image>().sprite = rankImages[0];
        }

        if (ScoreHolder.TotalScore >= 500 && ScoreHolder.TotalScore < 1000)
        {
            transform.GetComponent<Image>().sprite = rankImages[1];
        }

        if (ScoreHolder.TotalScore >= 1000)
        {
            transform.GetComponent<Image>().sprite = rankImages[2];
        }
    }
}
