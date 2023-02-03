using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteLibrary
{
    public static List<Sprite> _spriteLibrary = new List<Sprite>();

    public static Sprite GetSprite(Tile.Owner _owner, Tile.TileType _type)
    {
        return _spriteLibrary[0];
    }
}
