using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameData.Owner _owner;
    [SerializeField] public GameData.TileType type;
    [SerializeField] public GameData.RootID rootID;
    [SerializeField] public GameData.Connection connections;
    [SerializeField] private GameObject _wayTowardsPlant;
    public Tile()
    {
        _owner = GameData.Owner.NEUTRAL;
        type = GameData.TileType.SOIL;
        rootID = GameData.RootID.NOT_ROOT;
        connections = GameData.Connection.None;
    }

    private void Awake()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void UpdateSprite()
    {
        GameObject sprlib = GameObject.FindGameObjectWithTag("SpriteLibrary");
        _spriteRenderer.sprite = sprlib.GetComponent<SpriteLibrary>().GetSprite(_owner, type, rootID);
    }

    public static bool HasFlag(uint bitFlag, uint flag) => ((bitFlag & flag) != 0);
}
