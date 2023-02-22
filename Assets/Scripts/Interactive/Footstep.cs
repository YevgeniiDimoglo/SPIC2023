using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstep : MonoBehaviour
{
    private AudioSource audioSource;  // Start is called before the first frame update
    private List<Floor> floors;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.05f;
        floors = new List<Floor>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Floor floor = other.GetComponent<Floor>();
        if (floor)
        {
            floors.Add(floor);
            // Hit floor
            if (floors.Count == 1 && floor.getFootstepAudio())
            {
                audioSource.PlayOneShot(floor.getFootstepAudio());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Floor floor = other.GetComponent<Floor>();
        if (floor)
        {
            floors.Remove(floor);
        }
    }
}