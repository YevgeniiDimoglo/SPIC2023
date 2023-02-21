using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RankSetter : MonoBehaviour
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

        if (scoreHolder.TotalScore >= 0 && scoreHolder.TotalScore < 100)
        {
            transform.GetComponent<Image>().sprite = rankImages[1];
        }

        if (scoreHolder.TotalScore >= 100 && scoreHolder.TotalScore < 200)
        {
            transform.GetComponent<Image>().sprite = rankImages[2];
        }

        if (scoreHolder.TotalScore >= 200 && scoreHolder.TotalScore < 300)
        {
            transform.GetComponent<Image>().sprite = rankImages[3];
        }

        if (scoreHolder.TotalScore >= 300)
        {
            transform.GetComponent<Image>().sprite = rankImages[4];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
