using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableObject : InteractiveObject
{
    private PlaceablePoint placedAt;

    protected override void Start()
    {
        placedAt = null;
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
        if (player.GetComponent<MaidController>().getHolding() != null) return;
        player.GetComponent<MaidController>().hold(gameObject);
        GetComponent<Collider>().enabled = false;

        if (placedAt != null)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            placedAt.releaseObject();
            placedAt = null;
        }
    }

    public void place(PlaceablePoint point)
    {
        placedAt = point;

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        point.setObject(gameObject);
    }
}