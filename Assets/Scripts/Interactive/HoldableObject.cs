using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableObject : InteractiveObject
{
    [SerializeField] private Vector3 holdingPosition = Vector3.zero;
    [SerializeField] private Quaternion holdingRotation = Quaternion.identity;

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

    public Vector3 HoldingPosition()
    {
        Vector3 result = holdingPosition;

        result = transform.localRotation * result;
        return result;
    }

    public Quaternion HoldingRotation()
    {
        return holdingRotation;
    }
}