using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Sprite Library")]
public class SpriteLibrary : ScriptableObject
{
    public static List<Sprite> _spriteLibrary = new List<Sprite>();

    public static Sprite GetSprite(Tile.Owner _owner, Tile.TileType _type)
    {
        return _spriteLibrary[0];
    }
}
