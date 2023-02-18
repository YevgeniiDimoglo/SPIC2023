using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] 
    private LevelLoader levelLoader;
    // Start is called before the first frame update
    public void PlayGame()
    {
        levelLoader.LoadNextLevel();
        Cursor.visible = false;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
