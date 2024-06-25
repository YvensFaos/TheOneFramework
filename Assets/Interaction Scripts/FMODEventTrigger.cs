// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Yarn.Unity;
// using FMODUnity;
// using FMOD.Studio;
// using System;

// public class FMODEventScript : PlayerActivatable 
// {
//     public string fmodSoundEvent = "event:/Example";

//     private bool isPlaying = false;

//     void PlayOneShot(string path, Vector3 position)
//     {
//         EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(path);
//         instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
//         instance.start();
//         isPlaying = true;
//         StartCoroutine(WaitForSoundToFinish(instance));
//     }

// 	private IEnumerator WaitForSoundToFinish(EventInstance instance)
// 	{
// 		PLAYBACK_STATE state = PLAYBACK_STATE.PLAYING;
// 		while (state != PLAYBACK_STATE.STOPPED)
// 		{
// 			yield return null;
// 			instance.getPlaybackState(out state);
// 		}
//         isPlaying = false;
// 		instance.setUserData(IntPtr.Zero);
// 		instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
// 		instance.release();
// 	}    

//     override protected void OnActivate()
//     {
//         if (!string.IsNullOrEmpty(fmodSoundEvent)) {
//             PlayOneShot(fmodSoundEvent, transform.position);
//         }
//     }

//     override protected bool IsActivated() 
//     {
//         return isPlaying;
//     }
// }