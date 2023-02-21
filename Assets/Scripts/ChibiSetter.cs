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
        if (scoreHolder.TotalScore == 0)
        {
            transform.GetComponent<Image>().sprite = rankImages[0];
        }

        if (scoreHolder.TotalScore >= 0 && scoreHolder.TotalScore < 200)
        {
            transform.GetComponent<Image>().sprite = rankImages[1];
        }

        if (scoreHolder.TotalScore >= 300)
        {
            transform.GetComponent<Image>().sprite = rankImages[2];
        }
    }
}
