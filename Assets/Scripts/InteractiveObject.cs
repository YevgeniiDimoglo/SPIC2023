using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField] protected Material hoverMaterial;
    protected Material[] oriMaterial;

    protected GameObject player;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        oriMaterial = GetComponent<Renderer>().materials;

        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    public virtual void hover()
    {
        Material[] replaceM = GetComponent<Renderer>().materials;
        for (int i = 0; i < replaceM.Length; i++)
        {
            replaceM[i] = hoverMaterial;
        }
        GetComponent<Renderer>().materials = replaceM;
    }

    public virtual void unhover()
    {
        GetComponent<Renderer>().materials = oriMaterial;
    }

    public virtual void click()
    {
    }
}