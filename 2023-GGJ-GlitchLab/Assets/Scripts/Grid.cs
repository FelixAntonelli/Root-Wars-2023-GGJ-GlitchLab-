using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class Grid : MonoBehaviour
{
   [SerializeField] private int gridXAxisSize = 10;
   [SerializeField] private int gridYAxisSize = 10;
   [SerializeField] private float cellSize = 1;

   public List<Cell> Cells = new List<Cell>();
   
   public Vector2 max => new Vector2(gridXAxisSize * cellSize, gridYAxisSize * cellSize);

   public class Cell
   {
      public Vector2 position;
      public float size;
      public Tile tileData = new Tile();
      
      //Connections
      bool above = false;
      bool left = false;
      bool below = false;
      bool right = false;
   }

   enum Connection
   {
      
   }

   public static bool HasFlag(uint bitFlag, uint flag) => ((bitFlag & flag) != 0);
   
   private void Awake()
   {
      int numberOfCellsOnXAxis = Mathf.FloorToInt(gridXAxisSize / cellSize);
      int numberOfCellsOnYAxis = Mathf.FloorToInt(gridYAxisSize / cellSize);
      // int halfXSize = numberOfCellsOnXAxis / 2;
      // int halfYSize = numberOfCellsOnYAxis / 2;
      Vector2 origin = transform.position;

      for (int i = 0; i < numberOfCellsOnXAxis; i++)
      {
         for (int j = 0; j < numberOfCellsOnYAxis; j++)
         {
            Cell newCell = new Cell();
            newCell.position = new Vector2(i * cellSize, j * cellSize);
            newCell.size  = cellSize;
            Cells.Add(newCell);
         }
      }
   }

   public bool PlaceTile(Vector2 GridPosition)
   {
      // Check if the tile is a soil type
      //    If No, return false.
      //    If Yes, check the four neighbours for a connection point
      //       If no, return false
      //       If yes, Update tile data and update grid 
      
      int cellID = To1D(GridPosition);
      if (Cells[cellID].tileData.type != Tile.TileType.SOIL)
      {
         return false;
      }

  
      
      return false;
   }

   public int To1D(Vector2 pos)
   {
      return (int)(pos.y * gridXAxisSize + pos.x); //Can just hard cast to int as we shouldn't be passing in non-aligned positions
   }

   public Vector2 To2D(int index)
   {
      return new Vector2(index % gridXAxisSize, index / gridXAxisSize);
   }
}

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
   private void OnSceneGUI()
   {
      Grid grid = target as Grid;

      foreach (Grid.Cell cell in grid.Cells)
      {
         Handles.color = Color.red;
         Handles.DrawWireCube(cell.position, new Vector3(cell.size, cell.size, 0));
      }
   }
}
