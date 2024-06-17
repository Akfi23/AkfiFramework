using System;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class AKDatabase : ScriptableObject
{
    public abstract string Title { get; }
    public virtual string RemoteKey { get; } = "";
    
    public void GetGenerationData(out AKGenerationData[] generationData)
    {
        Generate(out generationData);
    }

    protected virtual void Generate(out AKGenerationData[] generationData)
    {
        generationData = Array.Empty<AKGenerationData>();
    }
    
    public virtual void UpdateRemoteData(string jsonData)
    {
        JsonUtility.FromJsonOverwrite(jsonData, this);
    }
        
#if UNITY_EDITOR

    public virtual void UpdateAKType()
    {
            
    }
    
    [Button][GUIColor(1, 0.6f, 0.4f)]
    public virtual void ResetData()
    {
            
    }
        
#endif
}
