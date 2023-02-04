using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpriteLibrary : MonoBehaviour
{
    [SerializeField] private List<Sprite> _neutralSprites   = new List<Sprite>();
    [SerializeField] private List<Sprite> _playerOneSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> _playerTwoSprites = new List<Sprite>();

    public Sprite GetSprite(Tile.Owner owner, Tile.TileType type, Tile.RootID root)
    {
        int typeIndex = (int)type - 1;
        int rootIndex = (int)root - 1;

        switch(owner)
        {
            case Tile.Owner.NEUTRAL:
                {
                    return _neutralSprites[typeIndex];
                }
            case Tile.Owner.PLAYER_1:
                {
                    return _playerOneSprites[rootIndex];
                }
            case Tile.Owner.PLAYER_2:
                {
                    return _playerTwoSprites[rootIndex];
                }
        }

        return null;
    }
}
