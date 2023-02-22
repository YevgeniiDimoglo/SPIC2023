using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

public class ScoreHolder : MonoBehaviour
{
    [SerializeField]
    private TimerController timer;

    [SerializeField]
    private MoveController moveController;

    [SerializeField]
    private StorageBox storageBox;

    private List<Scoring.ScoreTotal> scores;
    private List<GameObject> scoresBooks;
    private List<GameObject> gameObjects;
    private GameObject[] placebaleAreas;
    private GameObject[,] objects;

    bool calculated = false;

    public struct scoreTableRecord
    {
        public string name;
        public int score;

        public scoreTableRecord(string t, int v)
        {
            this.name = t;
            this.score = v;
        }
    }

    public List<scoreTableRecord> tableRecords = new List<scoreTableRecord>();

    public static bool isRunning = false;

    public static int dustCounter;
    public static int dustOverall;

    public static int Throwing = 0;

    static public int TotalScore;

    // Start is called before the first frame update
    private void Awake()
    {
        dustCounter = dustOverall = GameObject.FindGameObjectsWithTag("Destructible").Length;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (timer.elapsedTime <= 3 && !calculated)
        {
            CalculateOverallScore();
        }
    }

    private void CalculateOverallScore()
    {
        scores = Scoring.getTotals();

        foreach(var item in scores)
        {
            tableRecords.Add(new scoreTableRecord(item.title, item.value));
        }

        scoresBooks = Bookshelf.getTotals();

        foreach (var item in scoresBooks)
        {
            tableRecords.Add(new scoreTableRecord(item.GetComponent<BookObject>().Title(), 30));
        }

        placebaleAreas = GameObject.FindGameObjectsWithTag("PlaceArea");

        foreach (var item in placebaleAreas)
        {
            objects = item.GetComponent<PlaceableArea>().GetObjects();

            foreach (var _object in objects)
            {
                if (_object != null)
                {
                    tableRecords.Add(new scoreTableRecord(_object.name, 50));
                }
            }
        }

        foreach (var _object in objects)
        {
            if (_object != null)
            {
                tableRecords.Add(new scoreTableRecord(_object.name, 50));
            }
        }

        isRunning = moveController.NeverRun();

        foreach (var item in tableRecords)
        {
            TotalScore += item.score;

            if (!isRunning)
            {
                TotalScore += 300;
            }
        }

        calculated = true;

    }
}
