using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TestIakService : IAKService
{
    [Inject]
    private SpritesDatabase _database;
    
    [Inject]
    private void Init()
    {
        DebugStrings();
    }

    public void DebugStrings()
    {
        foreach (var targetString in _database.Strigns)
        {
            AKDebug.Log(targetString);
        }
    }
}
