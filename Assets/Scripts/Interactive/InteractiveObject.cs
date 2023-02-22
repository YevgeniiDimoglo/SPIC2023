using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField] protected TMP_FontAsset textFont;
    [SerializeField] protected Material hoverMaterial;
    protected Material[] oriMaterial;

    protected GameObject player;

    protected GameObject UIobj;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        oriMaterial = GetComponent<Renderer>().materials;

        player = GameObject.FindWithTag("Player");
        UIobj = GameObject.FindWithTag("UI");
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
            if (i == 1)
            {
                replaceM[i] = hoverMaterial;
            }
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

    protected GameObject popImage(Sprite sprite, Vector3 pos, Vector2 size)
    {
        if (!UIobj) return null;
        GameObject pop = new GameObject("Image");
        pop.transform.position = pos;
        pop.transform.parent = UIobj.transform;

        Image popImg = pop.AddComponent<Image>();
        popImg.sprite = sprite;

        pop.GetComponent<RectTransform>().sizeDelta = size;

        return pop;
    }

    protected GameObject popText(string txt, Vector3 pos, Vector2 size, float fontSize)
    {
        if (!UIobj) return null;
        GameObject pop = new GameObject("Text");
        pop.transform.position = pos;
        pop.transform.parent = UIobj.transform;

        TextMeshProUGUI text = pop.AddComponent<TextMeshProUGUI>();
        text.font = textFont;
        text.text = txt;
        text.fontSize = fontSize;

        pop.GetComponent<RectTransform>().sizeDelta = size;

        return pop;
    }
}