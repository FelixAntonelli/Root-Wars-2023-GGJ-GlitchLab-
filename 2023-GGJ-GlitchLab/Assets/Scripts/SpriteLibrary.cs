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

    private Dictionary<GameData.RootID, int> tileDict = new Dictionary<GameData.RootID, int>
    {
        { GameData.RootID.NOT_ROOT, -1 },
        { GameData.RootID.FOUR_WAY, 0 },
        { GameData.RootID.THREE_WAY_RDL, 1 },
        { GameData.RootID.THREE_WAY_URD, 2 },
        { GameData.RootID.THREE_WAY_URL, 3 },
        { GameData.RootID.THREE_WAY_UDL, 4 },
        { GameData.RootID.TWO_WAY_UL, 5 },
        { GameData.RootID.TWO_WAY_DL, 6 },
        { GameData.RootID.TWO_WAY_RD, 7 },
        { GameData.RootID.TWO_WAY_UR, 8 },
        { GameData.RootID.STRAIGHT_H, 9 },
        { GameData.RootID.STRAIGHT_V, 10 }
    };

    public Sprite GetSprite(GameData.Owner owner, GameData.TileType type, GameData.RootID root)
    {
        int typeIndex = (int)type - 1;
        int rootIndex = tileDict[root];

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
