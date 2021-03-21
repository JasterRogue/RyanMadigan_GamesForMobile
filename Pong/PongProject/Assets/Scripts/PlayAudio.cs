using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.loop = true;
        myAudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
