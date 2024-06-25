using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class SimpleAudioTrigger : PlayerActivatable 
{
    public AudioClip audioClip;

    override protected void OnActivate()
    {        
        if (audioClip != null)
        {
            PlayAudioAndDestroy();
        }
    }

    private void PlayAudioAndDestroy()
    {
        GameObject audioObject = new GameObject("One Shot Audio");
        audioObject.transform.position = transform.position;

        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = audioClip;

        audioSource.Play();

        Destroy(audioObject, audioClip.length);
    }
}
