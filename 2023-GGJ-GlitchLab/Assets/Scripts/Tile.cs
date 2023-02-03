using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private Owner _owner;
    private TileType _type;

    public Tile()
    {
        _owner = Owner.NEUTRAL;
        _type = TileType.SOIL;
    }

    public enum Owner
    {
        NEUTRAL = 0,
        PLAYER_1 = 1,
        PLAYER_2 = 2
    }

    public enum TileType
    {
        SOIL = 0,
        ROOT = 1,
        RESOURCE = 2,
        OBSTACLE = 3
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
