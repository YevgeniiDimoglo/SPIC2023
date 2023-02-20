using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject RotationTutorial;

    private void Start()
    {
        hideRotationTutorial();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void updateRotationTutorialPos(Vector3 pos)
    {
        RotationTutorial.transform.position = Camera.main.WorldToScreenPoint(pos);
    }

    public void showRotationTutorial()
    {
        RotationTutorial.SetActive(true);
    }

    public void hideRotationTutorial()
    {
        RotationTutorial.SetActive(false);
    }
}