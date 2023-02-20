using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float rotateSpeed;

    private float speed;
    private Vector2 InputValue;
    private Vector3 direction;

    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    private void Start()
    {
        speed = 0;
        InputValue = Vector2.zero;
        direction = transform.forward;
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        updateInput();

        updateMovement();
        updateRotate();
    }

    private void updateInput()
    {
        InputValue = Vector2.zero;
        if (Keyboard.current.dKey.isPressed) InputValue.x = 1;      // D
        if (Keyboard.current.aKey.isPressed) InputValue.x = -1;     // A
        if (Keyboard.current.wKey.isPressed) InputValue.y = 1;      // W
        if (Keyboard.current.sKey.isPressed) InputValue.y = -1;     // S
        if (Gamepad.current != null && Gamepad.current.leftStick.ReadValue() != Vector2.zero) InputValue = Gamepad.current.leftStick.ReadValue();
    }

    private void updateMovement()
    {
        if (InputValue != Vector2.zero)
        {
            speed = Mathf.Clamp(speed += moveSpeed * Time.deltaTime, 0, maxMoveSpeed);
            direction = Camera.main.transform.right * InputValue.x + Camera.main.transform.forward * InputValue.y;
            direction.y = 0.0f;
            direction.Normalize();
        }
        else
        {
            speed *= 0.5f;
            if (speed <= 0.001f) speed = 0.0f;
        }
        _rigidbody.velocity = direction * speed;
    }

    private void updateRotate()
    {
        FreeFollowCamera fc = Camera.main.GetComponent<FreeFollowCamera>();
        if (fc != null && !fc.isTPS()) return;
        if (speed > 0)
        {
            Quaternion to = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, to, Time.deltaTime * rotateSpeed);

            if (InputValue.x != 0 && InputValue.y >= 0 && fc.isTPS() && !fc.hasInput() && Vector3.Angle(transform.forward, Camera.main.transform.forward) < 90)
            {
                Vector3 velocity = Vector3.zero;
                fc.TPScamaeraVector = Vector3.SmoothDamp(fc.TPScamaeraVector, new Vector3(-(transform.forward).x, fc.TPScamaeraVector.y, -(transform.forward).z), ref velocity, 0.05f);
            }
        }
    }
}