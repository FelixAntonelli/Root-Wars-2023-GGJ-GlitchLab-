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
        THREE_WAY_URD = 1 << 1 | 1 << 4 | 1 << 3,
        THREE_WAY_URL = 1 << 1 | 1 << 4 | 1 << 2,
        THREE_WAY_UDL = 1 << 1 | 1 << 3 | 1 << 2,
        TWO_WAY_UL = 1 << 1 | 1 << 2,
        TWO_WAY_DL = 1 << 3 | 1 << 2,
        TWO_WAY_RD = 1 << 4 | 1 << 3,
        TWO_WAY_UR = 1 << 1 | 1 << 4,
        STRAIGHT_H = 1 << 2 | 1 << 4,
        STRAIGHT_V = 1 << 1 | 1 << 3
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
