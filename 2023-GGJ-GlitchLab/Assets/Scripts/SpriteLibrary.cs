using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpriteLibrary : MonoBehaviour
{
    [SerializeField] private List<Sprite> _neutralSprites   = new List<Sprite>();
    [SerializeField] private List<Sprite> _playerOneSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> _playerTwoSprites = new List<Sprite>();

    public Sprite GetSprite(Tile.Owner _owner, Tile.TileType _type)
    {
        int typeIndex = (int)_type;

        switch(_owner)
        {
            case Tile.Owner.NEUTRAL:
                {
                    return _neutralSprites[typeIndex];
                }
            case Tile.Owner.PLAYER_1:
                {
                    return _playerOneSprites[typeIndex];
                }
            case Tile.Owner.PLAYER_2:
                {
                    return _playerTwoSprites[typeIndex];
                }
        }

        return null;
    }
}
