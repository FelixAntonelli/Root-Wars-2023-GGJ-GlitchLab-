using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] public GameData.Owner _owner;
    [SerializeField] public GameData.TileType type;
    [SerializeField] public GameData.RootID rootID;
    [SerializeField] public GameData.Connection connections;
    [SerializeField] public GameObject _wayTowardsPlant;
    
    private SpriteLibrary _library;
    
    public Tile()
    {
        _owner = GameData.Owner.NEUTRAL;
        type = GameData.TileType.SOIL;
        rootID = GameData.RootID.NOT_ROOT;
        connections = GameData.Connection.None;
    }
    public void Init(SpriteLibrary library)
    {
        _library = library;
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        UpdateSprite(); 
    }

    public void UpdateSprite()
    {
        // GameObject sprlib = GameObject.FindGameObjectWithTag("SpriteLibrary");
        _spriteRenderer.sprite = _library.GetSprite(_owner, type, rootID);
    }

    public Tile GetWayTowardsPlant()
    {
        return _wayTowardsPlant.GetComponent<Tile>();
    }

    public static bool HasFlag(uint bitFlag, uint flag) => ((bitFlag & flag) != 0);
}
