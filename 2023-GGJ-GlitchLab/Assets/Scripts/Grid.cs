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

   public class Cell
   {
      public Vector2 position;
      public float size;
      //TODO: Add Tile here
   }

   private void Awake()
   {
      int numberOfCellsOnXAxis = Mathf.FloorToInt(gridXAxisSize / cellSize);
      int numberOfCellsOnYAxis = Mathf.FloorToInt(gridYAxisSize / cellSize);
      int halfXSize = numberOfCellsOnXAxis / 2;
      int halfYSize = numberOfCellsOnYAxis / 2;
      Vector2 origin = transform.position;

      for (int i = -halfXSize; i < halfXSize; i++)
      {
         for (int j = -halfYSize; j < halfYSize; j++)
         {
            Cell newCell = new Cell();
            newCell.position = origin + new Vector2(i * cellSize, j * cellSize);
            newCell.size  = cellSize;
            Cells.Add(newCell);
         }
      }
   }

   // public bool PlaceTile()
   // {
   //    
   // }

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
