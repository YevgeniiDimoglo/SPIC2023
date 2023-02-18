using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaidController : MonoBehaviour
{
    [SerializeField] private GameObject holdedObject;
    [SerializeField] private GameObject holdingPosition;

    // Start is called before the first frame update
    private void Start()
    {
        holdedObject = null;
    }

    // Update is called once per frame
    private void Update()
    {
        if (holdedObject != null)
        {
            holdedObject.transform.position = holdingPosition.transform.position;
            holdedObject.transform.rotation = holdingPosition.transform.rotation;

            if (inputRightClick())
            {
                drop();
            }
        }

        if (inputleftClick())
        {
            CameraRaycast ray = Camera.main.GetComponent<CameraRaycast>();
            if (ray && ray.CameraTarget())
            {
                ray.CameraTarget().GetComponent<InteractiveObject>().click();
            }
        }
    }

    private bool inputleftClick()
    {
        if (
            Mouse.current.leftButton.wasPressedThisFrame ||
            (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
           )
        {
            return true;
        }

        return false;
    }

    private bool inputRightClick()
    {
        if (
            Mouse.current.rightButton.wasPressedThisFrame ||
            (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
           )
        {
            return true;
        }

        return false;
    }

    public void hold(GameObject obj)
    {
        if (holdedObject != null) return;
        holdedObject = obj;
    }

    public void drop()
    {
        holdedObject.transform.position = transform.position + transform.forward * 0.5f + transform.up;
        holdedObject.transform.rotation = transform.rotation;
        holdedObject.GetComponent<Collider>().enabled = true;

        holdedObject = null;
    }

    public GameObject getHolding()
    {
        return holdedObject;
    }
}