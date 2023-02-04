using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public enum Owner
    {
        NEUTRAL = 0,
        PLAYER_1 = 1,
        PLAYER_2 = 2
    }

    public enum TileType
    {
        ROOT = 0,
        SOIL = 1,
        RESOURCE = 2,
        OBSTACLE = 3
    }

    [Flags] public enum RootID
    {
        NOT_ROOT = 0,
        FOUR_WAY = 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4,
        THREE_WAY_RDL = 1 << 4 | 1 << 3 | 1 << 2, 
        THREE_WAY_URD = 3,
        THREE_WAY_URL = 4,
        THREE_WAY_UDL = 5,
        TWO_WAY_UL = 6,
        TWO_WAY_DL = 7,
        TWO_WAY_RD = 8,
        TWO_WAY_UR = 9,
        STRAIGHT_H = 10,
        STRAIGHT_V = 11
    }

    [Flags] public enum Connection
    {
        None = 0,
        Top = 1,
        Left = 2,
        Bottom = 4,
        Right = 8,
    }
}
