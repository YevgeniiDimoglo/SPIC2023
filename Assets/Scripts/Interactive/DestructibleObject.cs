using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : InteractiveObject
{
    [SerializeField]
    private Graffity graffity;
    protected override void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Destructible");
        base.Start();
    }

    public override void hover()
    {
        // Debug 

        //Material replaceM = GetComponent<Renderer>().material;
        //replaceM = hoverMaterial;
        //GetComponent<Renderer>().material = replaceM;
    }

    public override void click()
    {
        //graffity.DestroySquare(transform.GetSiblingIndex());
        ScoreHolder.dustCounter -= 1;
        Destroy(gameObject);
        //Destroy(this.GetComponent<BoxCollider>());
    }
}
