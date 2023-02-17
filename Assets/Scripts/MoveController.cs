using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float rotateSpeed;

    private float speed;
    private float inputHorizontal;
    private float inputVertical;
    private Vector3 direction;

    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    private void Start()
    {
        speed = 0;
        inputHorizontal = 0.0f;
        inputVertical = 0.0f;
        direction = transform.forward;
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        updateInputHorizontal();
        updateInputVertical();

        updateMovement();
        updateRotate();
    }

    private void updateInputHorizontal()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
    }

    private void updateInputVertical()
    {
        inputVertical = Input.GetAxisRaw("Vertical");
    }

    private void updateMovement()
    {
        if (inputHorizontal != 0.0f || inputVertical != 0.0f)
        {
            speed = Mathf.Clamp(speed += moveSpeed * Time.deltaTime, 0, maxMoveSpeed);
            direction = Camera.main.transform.right * inputHorizontal + Camera.main.transform.forward * inputVertical;
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
        if (speed > 0)
        {
            Quaternion to = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, to, Time.deltaTime * rotateSpeed);

            FreeFollowCamera fc = Camera.main.GetComponent<FreeFollowCamera>();
            if (inputHorizontal != 0 && inputVertical >= 0 && fc.isTPS() && !fc.hasInput() && Vector3.Angle(transform.forward, Camera.main.transform.forward) < 90)
            {
                Vector3 velocity = Vector3.zero;
                fc.TPScamaeraVector = Vector3.SmoothDamp(fc.TPScamaeraVector, new Vector3(-(transform.forward).x, fc.TPScamaeraVector.y, -(transform.forward).z), ref velocity, 0.05f);
            }
        }
    }
}