using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractiveObject
{
    [SerializeField] private Vector3 CenterPoint;
    [SerializeField] private float openAngle = 90.0f;
    [SerializeField] private float roateSpeed = 100.0f;

    [SerializeField] private AudioClip doorAudio;
    private AudioSource audioSource;

    private bool open;
    private float angle = 0;

    private GameObject popWin;
    private static bool showTutorial = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Holdable");
        open = false;

        if (doorAudio != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        float targetAngle = (open) ? openAngle : 0;
        if (angle != targetAngle)
        {
            float deltaAngle = roateSpeed * Time.deltaTime;
            if (angle > targetAngle) deltaAngle *= -1;
            if (Mathf.Abs(targetAngle - angle) < Mathf.Abs(deltaAngle)) deltaAngle = targetAngle - angle;

            transform.RotateAround(transform.position + (transform.rotation * CenterPoint), transform.up, deltaAngle);
            angle += deltaAngle;
        }

        if (popWin)
        {
            popWin.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 0.5f);
        }
    }

    public override void hover()
    {
        if (showTutorial)
        {
            popWin = popText("ŠJ‚¯‚é", Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 0.5f), new Vector2(40, 50), 24);
        }

        base.hover();
    }

    public override void unhover()
    {
        if (popWin)
        {
            Destroy(popWin);
            popWin = null;
        }

        base.hover();
    }

    public override void click()
    {
        showTutorial = false;
        if (popWin)
        {
            Destroy(popWin);
            popWin = null;
        }

        open = !open;
        if (doorAudio) audioSource.PlayOneShot(doorAudio);
    }
}