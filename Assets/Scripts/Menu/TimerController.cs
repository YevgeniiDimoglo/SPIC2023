using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerController : MonoBehaviour
{
    [SerializeField]
    private LevelLoader levelLoader;

    public TMP_Text timerCounter;

    private TimeSpan timePlaying;

    [SerializeField]
    private float elapsedTime = 180f;

    // Start is called before the first frame update
    private void Start()
    {
        timerCounter.text = "Time: 05:00.00";
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime -= Time.deltaTime;
        timePlaying = TimeSpan.FromSeconds(elapsedTime);
        string timePlayingStr = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
        timerCounter.text = timePlayingStr;

        if (elapsedTime <= 0)
        {
            levelLoader.LoadNextLevel();
        }
    }
}