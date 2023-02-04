using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpriteLibrary : MonoBehaviour
{
    [SerializeField] private List<Sprite> _neutralSprites   = new List<Sprite>();
    [SerializeField] private List<Sprite> _playerOneSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> _playerTwoSprites = new List<Sprite>();
    [SerializeField] private Sprite _errorSprite;

    public Sprite GetSprite(GameData.Owner owner, GameData.TileType type, GameData.RootID root)
    {
        int typeIndex = (int)type - 1;
        int rootIndex = (int)root - 1;

        switch (owner)
        {
            case GameData.Owner.NEUTRAL:
                {
                    if (typeIndex < 0)
                    {
                        return _errorSprite;
                    }
                    return _neutralSprites[typeIndex];
                }
            case GameData.Owner.PLAYER_1:
                {
                    if (rootIndex < 0)
                    {
                        return _errorSprite;
                    }
                    return _playerOneSprites[rootIndex];
                }
            case GameData.Owner.PLAYER_2:
                {
                    if (rootIndex < 0)
                    {
                        return _errorSprite;
                    }
                    return _playerTwoSprites[rootIndex];
                }
        }

        return null;
    }
}
