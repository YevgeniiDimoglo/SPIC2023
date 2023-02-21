using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChibiSetter : MonoBehaviour
{
    [SerializeField]
    private ScoreHolder scoreHolder;

    [SerializeField]
    private Sprite[] rankImages;

    // Start is called before the first frame update
    void Start()
    {
        if (scoreHolder.totalScore == 0)
        {
            transform.GetComponent<Image>().sprite = rankImages[0];
        }

        if (scoreHolder.totalScore >= 0 && scoreHolder.totalScore < 200)
        {
            transform.GetComponent<Image>().sprite = rankImages[1];
        }

        if (scoreHolder.totalScore >= 300)
        {
            transform.GetComponent<Image>().sprite = rankImages[2];
        }
    }
}
