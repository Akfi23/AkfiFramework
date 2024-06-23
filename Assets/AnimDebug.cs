using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.Animations;
using UnityEngine;

public class AnimDebug : MonoBehaviour
{
    public AnimatorController Controller;
    public float duration;

    private float timer;
    private bool isTimerFinish;
    
    public void Update()
    {
        if(isTimerFinish) return;
        
        timer += Time.deltaTime;
        
        if (timer >= duration)
        {
            AKDebug.Log("Timer END");
            isTimerFinish = true;
        }
    }

    [Button]
    public void DebugClipDuration()
    {
        
        foreach (var clip in Controller.animationClips)
        {
            int clipDuration = (int)((clip.length * 1f) * 60f);
            int minutes = clipDuration / 60;
            int seconds = clipDuration % 60;
            string formattedTime = minutes.ToString("0") + "." + seconds.ToString("00");

            if (float.TryParse(formattedTime, out var floatValue))
            {
                duration = floatValue;
            }
        }
    }

    [Button]
    public void DebugClipTransitions()
    {
        foreach (var layer in Controller.layers)
        {
            foreach (var state in layer.stateMachine.states)
            {
                AKDebug.Log(state.state.motion.averageDuration);
                
                int clipDuration = (int)((state.state.motion.averageDuration * 1f) * 60f);
                int minutes = clipDuration / 60;
                int seconds = clipDuration % 60;
                string formattedTime = minutes.ToString("0") + "." + seconds.ToString("00");

                if (float.TryParse(formattedTime, out var floatValue))
                {
                    AKDebug.Log(floatValue);
                }
                
                var transitionCount = state.state.transitions.Length;
                for (int i = 0; i < transitionCount; i++)
                {
                    var transition = state.state.transitions[i];

                    Debug.Log("Transition from " + state.state.name + " to " + transition.destinationState.name);
                    AKDebug.Log(transition.duration + " DURATION");
                }
            }
        }
    }

    [Button]
    public void DebugClipNameToHash()
    {
        foreach (var layer in Controller.layers)
        {
            foreach (var state in layer.stateMachine.states)
            {
                AKDebug.Log(state.state.nameHash + " NAME IS " + state.state.name);
            }
        }
    }

    public void AnimEnd()
    {
        AKDebug.Log("ANIM END");
    }
}
