using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;



public class Grid : MonoBehaviour
{
    [SerializeField] private SpriteLibrary _spriteLibrary;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private WaterSource waterPrefab;
    [SerializeField] private Plant plantPrefab;
    [SerializeField] private int gridXAxisSize = 10;
    [SerializeField] private int gridYAxisSize = 10;
    [SerializeField] private float cellSize = 1;
    [Header("Gen Values")]
    [SerializeField] private Vector2Int _waterSourcesRange;
    [SerializeField] private Vector2Int _obstaclesRange;

    public float CellSize
    {
        get { return cellSize; }
    }

    public List<Cell> Cells = new List<Cell>();

    public Vector2 max => new Vector2(gridXAxisSize, gridYAxisSize);

    public static bool HasFlag(uint bitFlag, uint flag) => ((bitFlag & flag) != 0);

    private void Awake()
    {
        Vector2 origin = transform.position;

        for (int i = 0; i < gridXAxisSize; i++)
        {
            for (int j = 0; j < gridYAxisSize; j++)
            {
                Cell newCell = Instantiate(cellPrefab, new Vector2(i, j), Quaternion.identity, transform);
                newCell.tileData.Init(_spriteLibrary);
                // newCell.position = new Vector2(i, j);
                newCell.size = cellSize;
                newCell.transform.localScale = new Vector2(newCell.size, newCell.size);
                Cells.Add(newCell);
            }
        }

        int waterSourceCount = Random.Range(_waterSourcesRange.x, _waterSourcesRange.y);
        int obstacleCount = Random.Range(_obstaclesRange.x, _obstaclesRange.y);

        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < waterSourceCount; i++)
            {
                bool spotFound = false;
                while (!spotFound)
                {
                    int xPos =  j == 0 ? Random.Range(0, (int)max.x / 2) : Random.Range((int)max.x/2, (int)max.x);
                    int yPos = Random.Range(0, (int)max.y - 2);
                    int ID = To1D(new Vector2(xPos, yPos));

                    if (Cells[ID].tileData._owner == GameData.Owner.NEUTRAL)
                    {
                        SetTile(ID, GameData.Connection.None, GameData.Owner.NEUTRAL, GameData.TileType.RESOURCE);
                        Cells[ID].waterSource = Instantiate(waterPrefab, Cells[ID].transform);
                        spotFound = true;
                    }
                }
            }

            for (int i = 0; i < obstacleCount; i++)
            {
                bool spotFound = false;
                while (!spotFound)
                {
                    int xPos = j == 0 ? Random.Range(0, (int)max.x / 2) : Random.Range((int)max.x / 2, (int)max.x);
                    int yPos = Random.Range(0, (int)max.y - 2);
                    int ID = To1D(new Vector2(xPos, yPos));

                    if (Cells[ID].tileData._owner == GameData.Owner.NEUTRAL && Cells[ID].tileData.type != GameData.TileType.RESOURCE)
                    {
                        SetTile(ID, GameData.Connection.None, GameData.Owner.NEUTRAL, GameData.TileType.OBSTACLE);
                        spotFound = true;
                    }
                }
            }
        }
    }

    public void SetSpawn(Vector2 player1Pos, Vector2 player2Pos, out Plant player1Plant, out Plant player2Plant)
    {
        int player1CellID = To1D(player1Pos);
        int player2CellID = To1D(player2Pos);

        SetTile(player1CellID, GameData.Connection.Top | GameData.Connection.Left | GameData.Connection.Bottom | GameData.Connection.Right, GameData.Owner.PLAYER_1);
        SetTile(player2CellID, GameData.Connection.Top | GameData.Connection.Left | GameData.Connection.Bottom | GameData.Connection.Right, GameData.Owner.PLAYER_2);
        
        Vector3 player1PlantPos = player1Pos + new Vector2(0, 1);
        Vector3 player2PlantPos = player2Pos + new Vector2(0, 1);
        
        player1Plant = Instantiate(plantPrefab, player1PlantPos, Quaternion.identity);
        player2Plant = Instantiate(plantPrefab, player2PlantPos, Quaternion.identity);
        
        Cells[player1CellID].tileData._wayTowardsPlant = player1Plant.gameObject;
        Cells[player2CellID].tileData._wayTowardsPlant = player2Plant.gameObject;
    }

    public void SetTile(int cellID, GameData.Connection rootType, GameData.Owner player, GameData.TileType type = GameData.TileType.ROOT)
    {
        Cells[cellID].tileData.connections = rootType;
        Cells[cellID].tileData.rootID = (GameData.RootID)rootType;
        Cells[cellID].tileData._owner = player;
        Cells[cellID].tileData.type = type;
        Cells[cellID].tileData.UpdateSprite();
    }

    public bool PlaceTile(Vector2 GridPosition, GameData.RootID rootType, GameData.Owner player, out bool connectedToResource)
    {
        // Check if the tile is a soil type
        //    If No, return false.
        //    If Yes, check the four neighbours for a connection point
        //       If no, return false
        //       If yes, Update tile data and update grid 
        int resourceGain = 0;
        int resourceLoss = 0;
        bool placeTile = false;
        connectedToResource = false;
        List<WaterSource> sources = new List<WaterSource>();

        int cellID = To1D(GridPosition);
        if (Cells[cellID].tileData.type != GameData.TileType.SOIL)
        {
            return false;
        }

        #region TopCheck

        if (HasFlag((uint)rootType, (uint)GameData.Connection.Top)) //do we have an up connection for this piece
        {
            if (!BoundsCheck(GridPosition + new Vector2(0, 1))) //does the connection go off the screen
            {
                resourceLoss++;
            }
            else
            {
                Cell n = Cells[To1D(GridPosition + new Vector2(0, 1))];
                if (!(n.tileData._owner == player || n.tileData._owner == GameData.Owner.NEUTRAL)) //if the other player owns the tile then its a resource loss
                {
                    resourceLoss--;
                }
                else
                {
                    switch (n.tileData.type)
                    {
                        case GameData.TileType.ROOT:
                            //Check if connection
                            if (HasFlag((uint)n.tileData.connections, (uint)GameData.Connection.Bottom))
                            {
                                //assign tile
                                placeTile = true;
                                Cells[cellID].tileData._wayTowardsPlant = n.gameObject;
                            }
                            else
                            {
                                resourceLoss++;
                            }
                            break;
                        case GameData.TileType.SOIL:
                            //Neutral
                            break;
                        case GameData.TileType.RESOURCE:
                            sources.Add(n.waterSource);
                            resourceGain++;
                            break;
                        case GameData.TileType.OBSTACLE:
                            resourceLoss--;
                            break;
                    }
                }
            }
        }

        #endregion

        #region LeftCheck

        if (HasFlag((uint)rootType, (uint)GameData.Connection.Left)) //do we have an left connection for this piece
        {
            if (!BoundsCheck(GridPosition + new Vector2(-1, 0))) //does the connection go off the screen
            {
                resourceLoss++;
            }
            else
            {
                Cell n = Cells[To1D(GridPosition + new Vector2(-1, 0))];
                if (!(n.tileData._owner == player || n.tileData._owner == GameData.Owner.NEUTRAL)) //if the other player owns the tile then its a resource loss
                {
                    resourceLoss--;
                }
                else
                {
                    switch (n.tileData.type)
                    {
                        case GameData.TileType.ROOT:
                            //Check if connection
                            if (HasFlag((uint)n.tileData.connections, (uint)GameData.Connection.Right))
                            {
                                //assign tile
                                placeTile = true;
                                Cells[cellID].tileData._wayTowardsPlant = n.gameObject;
                            }
                            else
                            {
                                resourceLoss++;
                            }
                            break;
                        case GameData.TileType.SOIL:
                            //Neutral
                            break;
                        case GameData.TileType.RESOURCE:
                            sources.Add(n.waterSource);
                            resourceGain++;
                            break;
                        case GameData.TileType.OBSTACLE:
                            resourceLoss--;
                            break;
                    }
                }
            }
        }
        #endregion

        #region BottomCheck

        if (HasFlag((uint)rootType, (uint)GameData.Connection.Bottom)) //do we have an bottom connection for this piece
        {
            if (!BoundsCheck(GridPosition + new Vector2(0, -1))) //does the connection go off the screen
            {
                resourceLoss++;
            }
            else
            {
                Cell n = Cells[To1D(GridPosition + new Vector2(0, -1))];
                if (!(n.tileData._owner == player || n.tileData._owner == GameData.Owner.NEUTRAL)) //if the other player owns the tile then its a resource loss
                {
                    resourceLoss--;
                }
                else
                {
                    switch (n.tileData.type)
                    {
                        case GameData.TileType.ROOT:
                            //Check if connection
                            if (HasFlag((uint)n.tileData.connections, (uint)GameData.Connection.Top))
                            {
                                //assign tile
                                placeTile = true;
                                Cells[cellID].tileData._wayTowardsPlant = n.gameObject;
                            }
                            else
                            {
                                resourceLoss++;
                            }
                            break;
                        case GameData.TileType.SOIL:
                            //Neutral
                            break;
                        case GameData.TileType.RESOURCE:
                            sources.Add(n.waterSource);
                            resourceGain++;
                            break;
                        case GameData.TileType.OBSTACLE:
                            resourceLoss--;
                            break;
                    }
                }
            }
        }

        #endregion

        #region RightCheck

        if (HasFlag((uint)rootType, (uint)GameData.Connection.Right)) //do we have an bottom connection for this piece
        {
            if (!BoundsCheck(GridPosition + new Vector2(1, 0))) //does the connection go off the screen
            {
                resourceLoss++;
            }
            else
            {
                Cell n = Cells[To1D(GridPosition + new Vector2(1, 0))];
                if (!(n.tileData._owner == player || n.tileData._owner == GameData.Owner.NEUTRAL)) //if the other player owns the tile then its a resource loss
                {
                    resourceLoss--;
                }
                else
                {
                    switch (n.tileData.type)
                    {
                        case GameData.TileType.ROOT:
                            //Check if connection
                            if (HasFlag((uint)n.tileData.connections, (uint)GameData.Connection.Left))
                            {
                                //assign tile
                                placeTile = true;
                                Cells[cellID].tileData._wayTowardsPlant = n.gameObject;
                            }
                            else
                            {
                                resourceLoss++;
                            }
                            break;
                        case GameData.TileType.SOIL:
                            //Neutral
                            break;
                        case GameData.TileType.RESOURCE:
                            sources.Add(n.waterSource);
                            resourceGain++;
                            break;
                        case GameData.TileType.OBSTACLE:
                            resourceLoss--;
                            break;
                    }
                }
            }
        }
        #endregion

        if (placeTile)
        {
            SetTile(cellID, (GameData.Connection)rootType, player);
            Debug.Log($"Gained: {resourceGain} and lost: {resourceLoss}");
            if (resourceGain > 0)
            {
                foreach (WaterSource src in sources)
                {
                    src.ConnectTile(Cells[cellID].tileData);
                }
                connectedToResource = true;
            }
            return true;
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
        return (int)(pos.x * gridYAxisSize + pos.y); //Can just hard cast to int as we shouldn't be passing in non-aligned positions
    }

    public Vector2 To2D(int index)
    {
        return new Vector2(index % gridYAxisSize, index / gridXAxisSize);
    }
    public bool BoundsCheck(Vector2 pos)
    {
        return !(pos.x >= max.x || pos.y >= max.y || pos.x < 0 || pos.y < 0);
    }
}
/*
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
*/
