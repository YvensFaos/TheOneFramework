#if FMOD_INSTALLED

    //updated by Ruben H
    using System.Collections;
    using UnityEngine;
    using Yarn.Unity;
    using FMODUnity;
    using FMOD.Studio;
    using System;

    public class FMODEventScript : PlayerActivatable
    {
        public EventReference fmodSoundEvent;

        private bool isPlaying = false;

        void PlayOneShot(EventReference eventReference, Vector3 position)
        {
            EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(eventReference);
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
            instance.start();
            isPlaying = true;
            StartCoroutine(WaitForSoundToFinish(instance));
        }

        private IEnumerator WaitForSoundToFinish(EventInstance instance)
        {
            PLAYBACK_STATE state = PLAYBACK_STATE.PLAYING;
            while (state != PLAYBACK_STATE.STOPPED)
            {
                yield return null;
                instance.getPlaybackState(out state);
            }
            isPlaying = false;
            instance.setUserData(IntPtr.Zero);
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
        }

        protected override void OnActivate()
        {
            if (fmodSoundEvent.IsNull == false)
            {
                PlayOneShot(fmodSoundEvent, transform.position);
            }
        }

        protected override bool IsActivated()
        {
            return isPlaying;
        }
    }
#else
    using System.Diagnostics;
    using UnityEngine;

    public class FMODEventScript : PlayerActivatable
    {
        void Awake() 
        {
            UnityEngine.Debug.LogError("FMODEventTrigger needs FMOD to be installed.");
        }

        protected override void OnActivate() 
        {
            UnityEngine.Debug.LogError("FMOD is not installed. The event will not activate");
        }  

        protected override bool IsActivated()
        {
            return false;
        }
    }
#endif