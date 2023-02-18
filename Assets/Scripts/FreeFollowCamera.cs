using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreeFollowCamera : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float CameraSpeed = 5.0f;

    [SerializeField] private float SwitchDistance = 3.0f;
    [SerializeField] private float maxCameraDistance = 5.0f;
    [SerializeField] private float zoomSpeed = 1.0f;

    [Header("FPS")]
    [SerializeField] private GameObject FPSTarget;

    [SerializeField] private Vector3 FPSOffset = new Vector3(0, 0, 0.06f);
    [SerializeField] private float FPSMaxVerticalAngle = 60;
    [SerializeField] private float FPSMinVerticalAngle = -30;

    [Header("TPS")]
    [SerializeField] private GameObject TPSTarget;

    [SerializeField] private Vector3 TargetOffset = new Vector3(0, 1.0f, 0);
    [SerializeField] private Vector3 CameraOffset = new Vector3(0.5f, 0.0f, 0.0f);
    [SerializeField] private float TPSMaxVerticalAngle = 60.0f;
    [SerializeField] private float TPSMinVerticalAngle = -10.0f;
    [SerializeField] private float rotateSpeed = 5.0f;

    public Vector3 TPScamaeraVector = Vector3.back;
    private float cameraDistance = 2.0f;

    private Vector3 lastPlayerPosition;
    private Vector3 PlayerMove;

    private Vector3 InputValue;                 // Horizonal Rotate / Vertical Rotate / Zoom

    private enum STATUS
    {
        MOVING_FPS,
        FPS,
        MOVING_TPS,
        TPS,
        STOP
    }

    private STATUS status;
    private STATUS tempStatus;

    // Start is called before the first frame update
    private void Start()
    {
        status = STATUS.TPS;
        tempStatus = status;

        transform.position = TPSTarget.transform.position + TargetOffset + CameraOffset;

        lastPlayerPosition = TPSTarget.transform.position;

        InputValue = Vector3.zero;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateInputValue();

        if (Application.isFocused)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        PlayerMove = TPSTarget.transform.position - lastPlayerPosition;
        lastPlayerPosition = TPSTarget.transform.position;

        switch (status)
        {
            case STATUS.MOVING_FPS:
                if (CameraMoveTo(getFpsPosition()))
                {
                    status = STATUS.FPS;
                }
                break;

            case STATUS.FPS:
                updateFpsCamera();
                break;

            case STATUS.MOVING_TPS:
                Vector3 offset = getTPScameraOffset();
                if (CameraMoveTo(TPSTarget.transform.position + TargetOffset + TPScamaeraVector * SwitchDistance + offset) || CameraCollision(offset))
                {
                    cameraDistance = SwitchDistance;
                    status = STATUS.TPS;
                }
                break;

            case STATUS.TPS:
                updateTPSCamera();
                break;

            case STATUS.STOP:
                // STOP Following (Update by others)
                break;
        }
    }

    private void UpdateInputValue()
    {
        InputValue = Vector3.zero;

        Vector2 MouseDelta = Mouse.current.delta.ReadValue();
        InputValue.x = Mathf.Clamp(MouseDelta.x, -1, 1);
        InputValue.y = Mathf.Clamp(-MouseDelta.y, -1, 1);
        if (Mouse.current.scroll.ReadValue().y != 0) InputValue.z = Mathf.Clamp(Mouse.current.scroll.ReadValue().y, -0.5f, 0.5f);

        if (Gamepad.current != null)
        {
            Vector2 RightStickInput = Gamepad.current.rightStick.ReadValue();
            if (RightStickInput != Vector2.zero)
            {
                InputValue.x = Mathf.Clamp(RightStickInput.x, -1, 1);
                InputValue.y = Mathf.Clamp(-RightStickInput.y, -1, 1);
            }
            if (Gamepad.current.dpad.up.isPressed) InputValue.z = 0.05f;
            if (Gamepad.current.dpad.down.isPressed) InputValue.z = -0.05f;
        }
    }

    private bool CameraMoveTo(Vector3 p)
    {
        transform.position += PlayerMove;
        transform.position = Vector3.MoveTowards(transform.position, p, CameraSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, p) < 0.001f)
        {
            return true;
        }
        return false;
    }

    private void updateFpsCamera()
    {
        if (InputValue.z < 0) // «
        {
            status = STATUS.MOVING_TPS;                     // return to TPS mode
            TPScamaeraVector = -transform.forward;
            return;
        }

        transform.position = getFpsPosition();

        float rotateCount = rotateSpeed * 60.0f * Time.deltaTime;
        transform.Rotate(0, InputValue.x * rotateCount, 0, Space.World);

        transform.Rotate(InputValue.y * rotateCount, 0, 0);
        float currectRotateX = transform.rotation.eulerAngles.x;
        if (currectRotateX > 180) currectRotateX -= 360;
        if (currectRotateX < -180) currectRotateX += 360;
        if (currectRotateX > FPSMaxVerticalAngle) transform.Rotate(-(currectRotateX - FPSMaxVerticalAngle), 0, 0);
        if (currectRotateX < FPSMinVerticalAngle) transform.Rotate(-(currectRotateX - FPSMinVerticalAngle), 0, 0);

        if (currectRotateX < -20)
        {
            // ãŒü‚¬
            GetComponent<Camera>().nearClipPlane = 0.15f;
        }
        else
        {
            // ‰ºŒü‚«
            GetComponent<Camera>().nearClipPlane = 0.08f;
        }

        syncPlayerRotation();
    }

    private Vector3 getFpsPosition()
    {
        return FPSTarget.transform.position + Quaternion.Euler(0, TPSTarget.transform.eulerAngles.y, 0) * FPSOffset;
    }

    private void updateTPSCamera()
    {
        cameraDistance -= InputValue.z * zoomSpeed * Time.deltaTime;
        if (cameraDistance > maxCameraDistance) cameraDistance = maxCameraDistance;

        if (InputValue.z > 0 && cameraDistance < SwitchDistance)
        {
            status = STATUS.MOVING_FPS;
            return;
        }

        float rotateCount = rotateSpeed * 60.0f * Time.deltaTime;
        TPScamaeraVector = Quaternion.AngleAxis(InputValue.x * rotateCount, Vector3.up) * TPScamaeraVector;
        TPScamaeraVector = Quaternion.AngleAxis(InputValue.y * rotateCount, transform.right) * TPScamaeraVector;

        float VerticalAngle = Vector3.Angle(TPScamaeraVector, new Vector3(TPScamaeraVector.x, 0, TPScamaeraVector.z));
        if (TPScamaeraVector.y < 0) VerticalAngle *= -1;
        if (VerticalAngle > TPSMaxVerticalAngle)
        {
            TPScamaeraVector = Quaternion.AngleAxis(-(VerticalAngle - TPSMaxVerticalAngle), transform.right) * TPScamaeraVector;
        }
        if (VerticalAngle < TPSMinVerticalAngle)
        {
            TPScamaeraVector = Quaternion.AngleAxis(-(VerticalAngle - TPSMinVerticalAngle), transform.right) * TPScamaeraVector;
        }

        Vector3 offset = getTPScameraOffset();
        transform.position = TPSTarget.transform.position + TargetOffset + TPScamaeraVector * cameraDistance + offset;

        transform.LookAt(TPSTarget.transform.position + TargetOffset + offset);

        CameraCollision(offset);
    }

    private bool CameraCollision(Vector3 offset)
    {
        RaycastHit hit;
        Vector3 target = TPSTarget.transform.position + TargetOffset + offset;

        if (Physics.Linecast(target, transform.position, out hit, ~(1 << LayerMask.NameToLayer("Player"))))
        {
            Debug.Log(hit.collider.gameObject.name);
            transform.position = target - transform.forward * (hit.distance - 0.1f);
            return true;
        }
        return false;
    }

    private Vector3 getTPScameraOffset()
    {
        Vector3 HorizonVector = new Vector3(TPScamaeraVector.x, 0, TPScamaeraVector.z);
        Vector3 offset = Quaternion.AngleAxis(Vector3.Angle(Vector3.back, HorizonVector), Vector3.Cross(Vector3.back, HorizonVector)) *
                         Quaternion.AngleAxis(Vector3.Angle(HorizonVector, TPScamaeraVector), transform.right) * CameraOffset;
        RaycastHit hit;
        if (Physics.Linecast(TPSTarget.transform.position + TargetOffset, TPSTarget.transform.position + TargetOffset + offset, out hit))
        {
            return offset.normalized * hit.distance * 0.8f;
        }
        return offset;
    }

    private void syncPlayerRotation()
    { TPSTarget.transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w); }

    public bool hasInput()
    {
        return InputValue != Vector3.zero;
    }

    public bool isTPS()
    {
        return status == STATUS.TPS;
    }

    public void StopFollow()
    {
        tempStatus = status;
        status = STATUS.STOP;
    }

    public void startFollow(Vector3 forward)
    {
        switch (tempStatus)
        {
            case STATUS.MOVING_FPS:
            case STATUS.FPS:
                status = STATUS.MOVING_FPS;
                break;

            case STATUS.MOVING_TPS:
            case STATUS.TPS:
                TPScamaeraVector = forward.normalized;
                status = STATUS.MOVING_TPS;
                break;
        }
    }
}