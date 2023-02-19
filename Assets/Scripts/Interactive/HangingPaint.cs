using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HangingPaint : InteractiveObject
{
    [SerializeField] private float rotateSpeed = 60.0f;
    [SerializeField] private float cameraDistance = 1;
    [SerializeField] private Vector3 centerOffset;
    private Vector3 offset; // Offset with Rotate;
    private bool rotateMode;

    protected override void Start()
    {
        rotateMode = false;
        gameObject.layer = LayerMask.NameToLayer("Holdable");
        base.Start();
    }

    protected override void Update()
    {
        if (!rotateMode) return;

        offset = Offset();

        if (Camera.main.transform.position != transform.position + offset + transform.forward * cameraDistance)
        {
            // Move Camera in front of painting
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, transform.position + offset + transform.forward * cameraDistance, 0.3f);
            Camera.main.transform.LookAt(transform.position + offset);
        }
        else
        {
            float input = InputRotate();
            if (input != 0)
            {
                transform.RotateAround(transform.position + offset, transform.forward, input * rotateSpeed * Time.deltaTime);
            }

            if (InputBack())
            {
                rotateMode = false;

                player.SetActive(true);

                Camera.main.GetComponent<FreeFollowCamera>().startFollow(transform.forward);
            }
        }
    }

    public override void click()
    {
        rotateMode = true;
        player.SetActive(false);
        Camera.main.GetComponent<FreeFollowCamera>().StopFollow();
    }

    private Vector3 Offset()
    {
        Vector3 offset = centerOffset;
        offset = Quaternion.AngleAxis(Vector3.Angle(Vector3.forward, transform.forward), Vector3.Cross(Vector3.forward, transform.forward)) * offset;
        offset = Quaternion.AngleAxis(Vector3.Angle(Vector3.up, transform.up), Vector3.Cross(Vector3.up, transform.up)) * offset;
        return offset;
    }

    private bool InputBack()
    {
        return (
            Mouse.current.rightButton.wasPressedThisFrame ||
            (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
        );
    }

    private float InputRotate()
    {
        float input = 0.0f;

        if (Keyboard.current.qKey.isPressed) input = -1;
        if (Keyboard.current.eKey.isPressed) input = 1;
        if (Gamepad.current != null)
        {
            if (Gamepad.current.dpad.left.isPressed || Gamepad.current.leftShoulder.isPressed) input = -1;
            if (Gamepad.current.dpad.right.isPressed || Gamepad.current.rightShoulder.isPressed) input = -1;
        }

        return input;
    }
}