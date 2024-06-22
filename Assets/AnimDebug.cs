using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.Animations;
using UnityEngine;

public class AnimDebug : MonoBehaviour
{
    public AnimatorController Controller;
    public float duration;

    [Button]
    public void DebugCLipDuration()
    {
        foreach (var clip in Controller.animationClips)
        {
            AKDebug.Log(clip.name); 
            int clipDuration = (int)((clip.length * 1f) * 60f);
            AKDebug.Log(clipDuration);
            int minutes = clipDuration / 60;
            int seconds = clipDuration % 60;
            string formattedTime = minutes.ToString("0") + "." + seconds.ToString("00");

            if (float.TryParse(formattedTime, out var floatValue))
            {
                duration = floatValue;
            }
        }
    }
}
