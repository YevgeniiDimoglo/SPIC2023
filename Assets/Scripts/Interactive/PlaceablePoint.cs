using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceablePoint : InteractiveObject

{
    private GameObject cloneObject;
    private GameObject placedObject;
    private static float angle = 0;

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

        if (cloneObject)
        {
            if (inputeRotateLeft()) angle += 45;
            if (inputeRotateRight()) angle -= 45;
            while (angle >= 360) angle -= 360;
            while (angle < 0) angle += 360;

            cloneObject.transform.rotation = Quaternion.RotateTowards(cloneObject.transform.rotation, Quaternion.Euler(0, angle, 0), 240.0f * Time.deltaTime);

            // Tutorial
            Tutorial tutorial = GameObject.Find("UI").GetComponentInChildren<Tutorial>();
            tutorial.updateRotationTutorialPos(cloneObject.transform.position);
            tutorial.showRotationTutorial();
        }
        if (placedObject)
        {
            placedObject.transform.position = transform.position;
        }
    }

    public override void hover()
    {
        GameObject obj = player.GetComponent<MaidController>().getHolding();
        if (obj == null) return;

        cloneObject = Instantiate(obj);
        cloneObject.transform.position = transform.position;
        cloneObject.transform.rotation = Quaternion.Euler(0, angle, 0);

        Rigidbody rb = cloneObject.GetComponent<Rigidbody>();
        if (rb) rb.useGravity = false;
    }

    private bool inputeRotateLeft()
    {
        if (
            Keyboard.current.qKey.wasPressedThisFrame ||
            Gamepad.current.leftShoulder.wasPressedThisFrame ||
            Gamepad.current.dpad.left.wasPressedThisFrame
           )
        {
            return true;
        }
        return false;
    }

    private bool inputeRotateRight()
    {
        if (
            Keyboard.current.eKey.wasPressedThisFrame ||
            Gamepad.current.rightShoulder.wasPressedThisFrame ||
            Gamepad.current.dpad.right.wasPressedThisFrame
           )
        {
            return true;
        }
        return false;
    }

    public override void unhover()
    {
        Destroy(cloneObject);
        cloneObject = null;

        Tutorial tutorial = GameObject.Find("UI").GetComponentInChildren<Tutorial>();
        tutorial.hideRotationTutorial();
    }

    public override void click()
    {
        GameObject obj = player.GetComponent<MaidController>().getHolding();
        player.GetComponent<MaidController>().drop();

        obj.transform.position = cloneObject.transform.position;
        obj.transform.rotation = Quaternion.Euler(0, angle, 0);

        obj.GetComponent<HoldableObject>().place(this);

        Destroy(cloneObject);
        cloneObject = null;
    }

    public void setObject(GameObject obj)
    {
        placedObject = obj;
    }

    public void releaseObject()
    {
        placedObject = null;
    }

    public GameObject getObject()
    {
        return placedObject;
    }
}