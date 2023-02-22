using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaidController : MonoBehaviour
{
    [SerializeField] private GameObject holdedObject;
    [SerializeField] private GameObject holdingPosition;
    [SerializeField] private AudioClip atmosphereAudio;

    [SerializeField] private AudioClip pikcAudio;
    [SerializeField] private AudioClip placeItemAudio;

    private Animator _animator;
    private AudioSource audioSource;

    // Start is called before the first frame update
    private void Start()
    {
        holdedObject = null;
        _animator = GetComponent<Animator>();
        if (atmosphereAudio != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = atmosphereAudio;
            audioSource.loop = true;
            audioSource.volume = 0.15f;
            audioSource.Play();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (holdedObject != null)
        {
            holdedObject.transform.rotation = holdingPosition.transform.rotation * holdedObject.GetComponent<HoldableObject>().HoldingRotation();
            holdedObject.transform.position = holdingPosition.transform.position + holdedObject.GetComponent<HoldableObject>().HoldingPosition();

            if (inputRightClick() && (Camera.main.GetComponent<FreeFollowCamera>().isTPS() || Camera.main.GetComponent<FreeFollowCamera>().isFPS()))
            {
                // Drop
                holdedObject.transform.position = transform.position + transform.forward * 0.3f + transform.up;
                holdedObject.transform.rotation = transform.rotation;
                holdedObject.GetComponent<Rigidbody>().AddForce(transform.forward * 10.0f, ForceMode.Impulse);

                ScoreHolder.Throwing += 1;

                release();
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

        _animator.SetBool("Holding", holdedObject != null);

        // Debug
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            List<Scoring.ScoreTotal> Totals = Scoring.getTotals();
            foreach (Scoring.ScoreTotal total in Totals)
                Debug.Log(total.title + ": " + total.value);
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
        obj.GetComponent<Collider>().enabled = false;
        holdedObject = obj;

        if (audioSource && pikcAudio)
        {
            audioSource.PlayOneShot(pikcAudio);
        }
    }

    public void release()
    {
        holdedObject.GetComponent<Collider>().enabled = true;

        holdedObject = null;

        if (audioSource && placeItemAudio)
        {
            audioSource.PlayOneShot(placeItemAudio);
        }
    }

    public GameObject getHolding()
    {
        return holdedObject;
    }
}