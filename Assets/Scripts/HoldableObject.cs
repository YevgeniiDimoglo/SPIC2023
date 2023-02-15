using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableObject : InteractiveObject
{
    protected override void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Holdable");
        base.Start();
    }

    public override void hover()
    {
        if (player.GetComponent<MaidController>().getHolding() != null) return;
        base.hover();
    }

    public override void click()
    {
        player.GetComponent<MaidController>().hold(gameObject);
        GetComponent<Collider>().enabled = false;
    }
}