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
        NOT_ROOT = Connection.None,
        FOUR_WAY = Connection.Top | Connection.Left | Connection.Bottom | Connection.Right,
        THREE_WAY_RDL = Connection.Right | Connection.Bottom | Connection.Left, 
        THREE_WAY_URD = Connection.Top | Connection.Right | Connection.Bottom,
        THREE_WAY_URL = Connection.Top | Connection.Right | Connection.Left,
        THREE_WAY_UDL = Connection.Top | Connection.Bottom | Connection.Left,
        TWO_WAY_UL = Connection.Top | Connection.Left,
        TWO_WAY_DL = Connection.Bottom | Connection.Left,
        TWO_WAY_RD = Connection.Right | Connection.Bottom,
        TWO_WAY_UR = Connection.Top | Connection.Right,
        STRAIGHT_H = Connection.Top | Connection.Bottom,
        STRAIGHT_V = Connection.Left | Connection.Right
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
