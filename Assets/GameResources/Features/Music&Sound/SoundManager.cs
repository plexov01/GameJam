using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClipRefsSO audioClipRefsSo;
    
    private void Start()
    {
        //addlistener to event
    }

    private void SomeOne_OnSomething(object sender, EventArgs e)
    {
        //need to make sound 3D
        PlaySound(audioClipRefsSo.someSound, Camera.main.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
}
