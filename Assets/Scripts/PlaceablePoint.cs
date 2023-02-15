using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceablePoint : InteractiveObject
{
    private GameObject cloneObject;

    protected override void Start()
    {
        cloneObject = null;
        player = GameObject.FindWithTag("Player");
    }

    protected override void Update()
    {
        if (player.GetComponent<MaidController>().getHolding())
            gameObject.layer = LayerMask.NameToLayer("Holdable");
        else
            gameObject.layer = LayerMask.NameToLayer("");
    }

    public override void hover()
    {
        GameObject obj = player.GetComponent<MaidController>().getHolding();
        if (obj == null) return;

        cloneObject = Instantiate(obj);
        cloneObject.transform.position = transform.position;
        cloneObject.transform.rotation = Quaternion.identity;
        Rigidbody rb = cloneObject.GetComponent<Rigidbody>();
        if (rb) rb.useGravity = false;
    }

    public override void unhover()
    {
        Destroy(cloneObject);
        cloneObject = null;
    }

    public override void click()
    {
        GameObject obj = player.GetComponent<MaidController>().getHolding();
        player.GetComponent<MaidController>().drop();

        obj.transform.position = cloneObject.transform.position;
        obj.transform.rotation = cloneObject.transform.rotation;

        Destroy(cloneObject);
        cloneObject = null;
    }
}