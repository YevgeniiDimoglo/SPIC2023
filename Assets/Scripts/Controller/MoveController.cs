using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float maxMoveSpeed = 5.0f;
    [SerializeField] private float maxRunSpeed = 10.0f;
    [SerializeField] private float rotateSpeed = 5.0f;

    private float speed;
    private Vector2 InputValue;
    private Vector3 direction;

    private bool runMode;
    private bool neverRun = true;

    private Rigidbody _rigidbody;
    private Animator _animator;

    // Start is called before the first frame update
    private void Start()
    {
        speed = 0;
        InputValue = Vector2.zero;
        direction = transform.forward;
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        runMode = false;
    }

    public bool NeverRun()
    {
        return neverRun;
    }

    // Update is called once per frame
    private void Update()
    {
        updateRunMode();
        updateInput();

        updateMovement();
        updateRotate();
    }

    private void updateRunMode()
    {
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame) runMode = true;
        if (Keyboard.current.leftShiftKey.wasReleasedThisFrame) runMode = false;
        if (Gamepad.current != null)
        {
            if (Gamepad.current.leftStickButton.wasPressedThisFrame) runMode = !runMode;
        }
        if (runMode) neverRun = false;
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
            speed = Mathf.Clamp(speed += moveSpeed * Time.deltaTime, 0, (runMode) ? maxRunSpeed : maxMoveSpeed);
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

        _animator.SetFloat("Speed", speed);
        _animator.speed = (speed >= 0.1) ? Mathf.Clamp(speed / maxMoveSpeed, 0.1f, 1.5f) : 1;
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