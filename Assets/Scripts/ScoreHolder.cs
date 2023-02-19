using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHolder : MonoBehaviour
{
    public float dustPercent = 0f;
    public int paintingScore = 0;
    public int booksScore = 0;
    public int totalScore = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
