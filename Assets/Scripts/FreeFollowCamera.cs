using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFollowCamera : MonoBehaviour
{
    [SerializeField] private float CameraSpeed = 5.0f;
    [SerializeField] private float SwitchDistance = 3.0f;
    [SerializeField] private float maxCameraDistance = 5.0f;

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

    private Vector3 TPScamaeraVector = Vector3.back;
    private float cameraDistance = 2.0f;

    private Vector3 lastPlayerPosition;
    private Vector3 PlayerMove;

    private enum STATUS
    {
        MOVING_FPS,
        FPS,
        MOVING_TPS,
        TPS,
    }

    private STATUS status;

    // Start is called before the first frame update
    private void Start()
    {
        status = STATUS.TPS;

        transform.position = TPSTarget.transform.position + TargetOffset + CameraOffset;

        lastPlayerPosition = TPSTarget.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
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
                CameraMoveTo(getFpsPosition(), STATUS.FPS);
                break;

            case STATUS.FPS:
                updateFpsCamera();
                break;

            case STATUS.MOVING_TPS:
                CameraMoveTo(TPSTarget.transform.position + TargetOffset + (TPScamaeraVector) * SwitchDistance + getTPScameraOffset(), STATUS.TPS);
                break;

            case STATUS.TPS:
                updateTPSCamera();
                break;
        }
    }

    private bool CameraMoveTo(Vector3 p, STATUS s)
    {
        transform.position += PlayerMove;
        transform.position = Vector3.MoveTowards(transform.position, p, CameraSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, p) < 0.001f)
        {
            status = s;
            return true;
        }
        return false;
    }

    private void updateFpsCamera()
    {
        syncPlayerRotation();
        if (Input.GetAxis("Mouse ScrollWheel") < 0) // «
        {
            status = STATUS.MOVING_TPS;                     // return to TPS mode
            TPScamaeraVector = -transform.forward;
            return;
        }

        transform.position = getFpsPosition();

        float rotateCount = rotateSpeed * 60.0f * Time.deltaTime;
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotateCount, 0, Space.World);

        transform.Rotate(-Input.GetAxis("Mouse Y") * rotateCount, 0, 0);
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
    }

    private Vector3 getFpsPosition()
    {
        return FPSTarget.transform.position + Quaternion.Euler(0, TPSTarget.transform.eulerAngles.y, 0) * FPSOffset;
    }

    private void updateTPSCamera()
    {
        cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * 60.0f * Time.deltaTime;
        if (cameraDistance > maxCameraDistance) cameraDistance = maxCameraDistance;

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && cameraDistance < SwitchDistance)
        {
            status = STATUS.MOVING_FPS;
            return;
        }

        float rotateCount = rotateSpeed * 60.0f * Time.deltaTime;
        TPScamaeraVector = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotateCount, Vector3.up) * TPScamaeraVector;

        TPScamaeraVector = Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * rotateCount, transform.right) * TPScamaeraVector;

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

        transform.position = TPSTarget.transform.position + TargetOffset + TPScamaeraVector * cameraDistance;

        transform.LookAt(TPSTarget.transform.position + TargetOffset);
        transform.position += getTPScameraOffset();
    }

    private Vector3 getTPScameraOffset()
    {
        return Quaternion.FromToRotation(Vector3.back, new Vector3(TPScamaeraVector.x, 0, TPScamaeraVector.z)) * CameraOffset;
    }

    private void syncPlayerRotation()
    { TPSTarget.transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w); }
}