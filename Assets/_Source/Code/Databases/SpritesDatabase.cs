using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "db_sprites", menuName = "Game/Databases/Sprites Database", order = 1)]
public class SpritesDatabase : AKDatabase
{
    public override string Title => "Sprites databae";

    public string[] Strigns;

}
