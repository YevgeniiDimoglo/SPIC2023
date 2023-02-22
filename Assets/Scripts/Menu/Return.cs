using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Return : MonoBehaviour
{

    [SerializeField]
    private LevelLoader levelLoader;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            Cursor.visible = true;
            levelLoader.LoadFirstLevel();
        }
    }
}
