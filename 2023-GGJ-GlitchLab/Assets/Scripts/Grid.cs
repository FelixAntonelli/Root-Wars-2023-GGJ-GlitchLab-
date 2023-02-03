using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Grid : MonoBehaviour
{
   [SerializeField] private int gridAxisSize = 10;
   [SerializeField] private float cellSize = 1;

   public List<Cell> Cells = new List<Cell>();

   public class Cell
   {
      public Vector3 position;
      public float size;
   }

   private void Awake()
   {
      int numberOfCellsOnAxis = Mathf.FloorToInt(gridAxisSize / cellSize);
      int halfSize = numberOfCellsOnAxis / 2;
      Vector3 origin = transform.position;

      for (int i = -halfSize; i < halfSize; i++)
      {
         for (int j = -halfSize; j < halfSize; j++)
         {
            Cell newCell = new Cell();
            newCell.position = origin + new Vector3(i * cellSize, j * cellSize, 0);
            newCell.size  = cellSize;
            Cells.Add(newCell);
         }
      }
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
