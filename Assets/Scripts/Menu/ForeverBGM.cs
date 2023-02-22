using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForeverBGM : MonoBehaviour
{
    [SerializeField] private AudioClip BGM;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (BGM)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = BGM;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}