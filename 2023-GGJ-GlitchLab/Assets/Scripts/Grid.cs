using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class Grid : MonoBehaviour
{
   [SerializeField] private SpriteLibrary _spriteLibrary;
   [SerializeField] private Cell cellPrefab;
   [SerializeField] private int gridXAxisSize = 10;
   [SerializeField] private int gridYAxisSize = 10;
   [SerializeField] private float cellSize = 1;

   public List<Cell> Cells = new List<Cell>();
   
   public Vector2 max => new Vector2(gridXAxisSize, gridYAxisSize);

   public static bool HasFlag(uint bitFlag, uint flag) => ((bitFlag & flag) != 0);
   
   private void Awake()
   {
      // int numberOfCellsOnXAxis = Mathf.FloorToInt(gridXAxisSize / cellSize);
      // int numberOfCellsOnYAxis = Mathf.FloorToInt(gridYAxisSize / cellSize);
      // int halfXSize = numberOfCellsOnXAxis / 2;
      // int halfYSize = numberOfCellsOnYAxis / 2;
      Vector2 origin = transform.position;

      for (int i = 0; i < gridXAxisSize; i++)
      {
         for (int j = 0; j < gridYAxisSize; j++)
         {
            Cell newCell = Instantiate(cellPrefab,  new Vector2(i, j), Quaternion.identity, transform);
            newCell.tileData.Init(_spriteLibrary);
            newCell.position = new Vector2(i, j);
            newCell.size = cellSize;
            newCell.transform.localScale = new Vector2(newCell.size, newCell.size);
            Cells.Add(newCell);
         }
      }
   }

   public bool PlaceTile(Vector2 GridPosition, GameData.RootID rootType, out bool connectedToResource)
   {
      // Check if the tile is a soil type
      //    If No, return false.
      //    If Yes, check the four neighbours for a connection point
      //       If no, return false
      //       If yes, Update tile data and update grid 
       connectedToResource = false;
      int cellID = To1D(GridPosition);
      if (Cells[cellID].tileData.type != GameData.TileType.SOIL)
      {
         return false;
      }

      //TOP
      //if above is in bounds and we have a connection going up on the tile we want to place
      if (BoundsCheck(GridPosition + new Vector2(0, 1)) && HasFlag((uint)rootType, (uint)GameData.Connection.Top))
      {
         Vector2 parentTile = GridPosition + new Vector2(0, 1);
         int parentTileID = To1D(parentTile);
         if (HasFlag((uint)Cells[parentTileID].tileData.connections, (uint)GameData.Connection.Bottom)) //if we can connect
         {
            Cells[cellID].tileData.connections = (GameData.Connection)rootType;
            Cells[cellID].tileData.rootID = rootType;
            Cells[cellID].tileData.UpdateSprite();
            connectedToResource = IsNeighbourTileResource(GridPosition, rootType);
            return true;
         }
      }
      
      //LEFT
      //if left is in bounds and we have a connection going left on the tile we want to place
      if (BoundsCheck(GridPosition + new Vector2(-1, 0)) && HasFlag((uint)rootType, (uint)GameData.Connection.Left))
      {
         Vector2 parentTile = GridPosition + new Vector2(-1, 0);
         int parentTileID = To1D(parentTile);
         if (HasFlag((uint)Cells[parentTileID].tileData.connections, (uint)GameData.Connection.Right)) //if we can connect
         {
            Cells[cellID].tileData.connections = (GameData.Connection)rootType;
            Cells[cellID].tileData.rootID = rootType;
            Cells[cellID].tileData.UpdateSprite();
            connectedToResource = IsNeighbourTileResource(GridPosition, rootType);
            return true;
         }
      }
      
      //BOTTOM
      //if below is in bounds and we have a connection going down on the tile we want to place
      if (BoundsCheck(GridPosition + new Vector2(0, -1)) && HasFlag((uint)rootType, (uint)GameData.Connection.Bottom))
      {
         Vector2 parentTile = GridPosition + new Vector2(0, -1);
         int parentTileID = To1D(parentTile);
         if (HasFlag((uint)Cells[parentTileID].tileData.connections, (uint)GameData.Connection.Top)) //if we can connect
         {
            Cells[cellID].tileData.connections = (GameData.Connection)rootType;
            Cells[cellID].tileData.type = GameData.TileType.ROOT;
            Cells[cellID].tileData.rootID = rootType;
            Cells[cellID].tileData.UpdateSprite();
            connectedToResource = IsNeighbourTileResource(GridPosition, rootType);
            return true;
         }
      }
      
      //RIGHT
      //if right is in bounds and we have a connection going right on the tile we want to place
      if (BoundsCheck(GridPosition + new Vector2(1, 0)) && HasFlag((uint)rootType, (uint)GameData.Connection.Right))
      {
         Vector2 parentTile = GridPosition + new Vector2(-1, 0);
         int parentTileID = To1D(parentTile);
         if (HasFlag((uint)Cells[parentTileID].tileData.connections, (uint)GameData.Connection.Left)) //if we can connect
         {
            Cells[cellID].tileData.connections = (GameData.Connection)rootType;
            Cells[cellID].tileData.type = GameData.TileType.ROOT;
            Cells[cellID].tileData.rootID = rootType;
            Cells[cellID].tileData.UpdateSprite();
            connectedToResource = IsNeighbourTileResource(GridPosition, rootType);
            return true;
         }
      }
      
      return false;
   }

   public bool IsNeighbourTileResource(Vector2 gridCell, GameData.RootID rootType)
   {
      //TOP
      if (BoundsCheck(gridCell + new Vector2(0, 1)) && HasFlag((uint)rootType, (uint)GameData.Connection.Top))
      {
         int nCellID = To1D(gridCell + new Vector2(0, 1));
         if (Cells[nCellID].tileData.type == GameData.TileType.RESOURCE)
         {
            return true;
         }
      }
      
      //LEFT
      if (BoundsCheck(gridCell + new Vector2(-1, 0)) && HasFlag((uint)rootType, (uint)GameData.Connection.Left))
      {
         int nCellID = To1D(gridCell + new Vector2(-1, 0));
         if (Cells[nCellID].tileData.type == GameData.TileType.RESOURCE)
         {
            return true;
         }
      }
      
      //BOTTOM
      if (BoundsCheck(gridCell + new Vector2(0, -1)) && HasFlag((uint)rootType, (uint)GameData.Connection.Bottom))
      {
         int nCellID = To1D(gridCell + new Vector2(0, -1));
         if (Cells[nCellID].tileData.type == GameData.TileType.RESOURCE)
         {
            return true;
         }
      }
      
      //RIGHT
      if (BoundsCheck(gridCell + new Vector2(1, 0)) && HasFlag((uint)rootType, (uint)GameData.Connection.Right))
      {
         int nCellID = To1D(gridCell + new Vector2(1, 0));
         if (Cells[nCellID].tileData.type == GameData.TileType.RESOURCE)
         {
            return true;
         }
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
   public bool BoundsCheck(Vector2 pos)
   {
      return (Mathf.Abs(pos.x) == 0 || Mathf.Abs(pos.x) == 1) && (Mathf.Abs(pos.y) == 0 || Mathf.Abs(pos.y) == 1);
   }
}

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
   private void OnSceneGUI()
   {
      Grid grid = target as Grid;

      foreach (Cell cell in grid.Cells)
      {
         Handles.color = Color.red;
         Handles.DrawWireCube(cell.position, new Vector3(cell.size, cell.size, 0));
      }
   }
}
