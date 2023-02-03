using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteLibrary : MonoBehaviour
{
    public List<Sprite> _spriteLibrary = new List<Sprite>();

    public Sprite GetSprite(Tile.Owner _owner, Tile.TileType _type)
    {
        return _spriteLibrary[0];
    }
}
