#if FMOD_INSTALLED

//Written by Ruben Hulzebosch, ChatGTP and troubleshooted by Hans & Yvens

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FootstepSoundSurface : MonoBehaviour
{
    [Header("FMOD Settings")]
    public EventReference footstepEvent; // Reference to the FMOD footstep event

    private EventInstance footstepEventInstance; // FMOD Event instance

    [Header("Raycast Settings")]
    public float raycastDistance = 10f; // Distance to check below the player for surface detection

    [Header("FMOD Parameter Values (Read-Only)")]
    public float grassParameter;  // Current value of the "Grass" parameter
    public float dirtParameter;   // Current value of the "Dirt" parameter
    public float waterParameter;  // Current value of the "Water" parameter
    public float sandParameter;   // Current value of the "Sand" parameter
    public float woodParameter;   // Current value of the "Wood" parameter
    public float metalParameter;  // Current value of the "Metal" parameter
    public float stoneParameter;  // Current value of the "Stone" parameter

    private string currentSurfaceTag = "Grass"; // Default surface type

    // Start is called before the first frame update
    void Start()
    {
        // Create an instance of the footstep event
        footstepEventInstance = RuntimeManager.CreateInstance(footstepEvent);
    }

    // This method detects the surface below the player using a raycast and sets the FMOD parameter
    private void DetectSurface()
    {
        RaycastHit hit;

        // Shoot a raycast down from the player's position to detect the surface below
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, raycastDistance))
        {
            currentSurfaceTag = hit.collider.tag;
            UpdateFootstepParameter(currentSurfaceTag);
        }
        else
        {
            currentSurfaceTag = "Grass"; // Default to grass if no surface is detected
            UpdateFootstepParameter(currentSurfaceTag);
        }
    }

    // This method sets the FMOD parameter for the current surface
    private void UpdateFootstepParameter(string surfaceTag)
    {
        if (footstepEventInstance.isValid())
        {
            // Reset all parameters to 0
            footstepEventInstance.setParameterByName("Grass", 0f);
            footstepEventInstance.setParameterByName("Dirt", 0f);
            footstepEventInstance.setParameterByName("Water", 0f);
            footstepEventInstance.setParameterByName("Sand", 0f);
            footstepEventInstance.setParameterByName("Wood", 0f);
            footstepEventInstance.setParameterByName("Metal", 0f);
            footstepEventInstance.setParameterByName("Stone", 0f);

            // Set the correct parameter based on the surface tag
            switch (surfaceTag)
            {
                case "Grass":
                    footstepEventInstance.setParameterByName("Grass", 1f);
                    break;
                case "Dirt":
                    footstepEventInstance.setParameterByName("Dirt", 1f);
                    break;
                case "Water":
                    footstepEventInstance.setParameterByName("Water", 1f);
                    break;
                case "Sand":
                    footstepEventInstance.setParameterByName("Sand", 1f);
                    break;
                case "Wood":
                    footstepEventInstance.setParameterByName("Wood", 1f);
                    break;
                case "Metal":
                    footstepEventInstance.setParameterByName("Metal", 1f);
                    break;
                case "Stone":
                    footstepEventInstance.setParameterByName("Stone", 1f);
                    break;
                default:
                    footstepEventInstance.setParameterByName("Grass", 1f); // Default to grass
                    break;
            }
        }
    }

    // This method will update the public variables in the Inspector to display the current FMOD parameter values
    private void UpdateInspectorParameterValues()
    {
        footstepEventInstance.getParameterByName("Grass", out grassParameter);
        footstepEventInstance.getParameterByName("Dirt", out dirtParameter);
        footstepEventInstance.getParameterByName("Water", out waterParameter);
        footstepEventInstance.getParameterByName("Sand", out sandParameter);
        footstepEventInstance.getParameterByName("Wood", out woodParameter);
        footstepEventInstance.getParameterByName("Metal", out metalParameter);
        footstepEventInstance.getParameterByName("Stone", out stoneParameter);
    }

    // This method will be called when the player triggers a footstep via an animation event
    private void OnFootstep(AnimationEvent animationEvent)
    {
        DetectSurface();
        PlayFootstep();
        UpdateInspectorParameterValues(); // Ensure FMOD parameter values are updated in the Inspector
    }

    // This method will be called to play the footstep sound
    public void PlayFootstep()
    {
        if (footstepEventInstance.isValid())
        {
            footstepEventInstance.start(); // Play the footstep sound
        }
    }

    // Release the FMOD instance when done
    private void OnDestroy()
    {
        footstepEventInstance.release();
    }

    // This will draw a visual representation of the raycast in both Scene and Game views
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastDistance);
    }
}

#endif
