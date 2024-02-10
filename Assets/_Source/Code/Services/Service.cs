using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Service : IAKService
{
    [Inject]
    private SpritesDatabase _database;
    
    [Inject]
    private void Init()
    {
        DebugStrings();
        AKDebug.Log("DEBUG");
    }

    public void DebugStrings()
    {
        foreach (var targetString in _database.Strigns)
        {
            AKDebug.Log(targetString);
        }
    }
}
