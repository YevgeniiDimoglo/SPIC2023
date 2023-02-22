using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NewScore 
{
    public string name;
    public int score;

    public NewScore(string name, int score) 
    {
        this.name = name;
        this.score = score;
    }
}
