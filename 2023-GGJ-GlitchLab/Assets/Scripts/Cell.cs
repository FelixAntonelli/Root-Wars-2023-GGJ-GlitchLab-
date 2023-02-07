using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2 position;
    public float size;
    public Tile tileData;
    public WaterSource waterSource;
      
    //Connections
    [Flags] enum Connection
    {
      None = 0,
      Top = 1,
      Left = 2,
      Bottom = 4,
      Right = 8,
    }
}
