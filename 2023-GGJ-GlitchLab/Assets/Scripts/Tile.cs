using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Owner _owner;
    public TileType type;
    public RootID rootID;
    public ValidExits validExits;

    public Tile()
    {
        _owner = Owner.NEUTRAL;
        type = TileType.SOIL;
        rootID = RootID.NOT_ROOT;
        validExits = ValidExits.NONE;
    }

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

    public enum RootID
    {
        NOT_ROOT = 0,
        FOUR_WAY = 1,
        THREE_WAY_RDL = 2,
        THREE_WAY_URD = 3,
        THREE_WAY_URL = 4,
        THREE_WAY_UDL = 5,
        TWO_WAY_UL = 6,
        TWO_WAY_DL = 7,
        TWO_WAY_RD = 8,
        TWO_WAY_UR = 9,
        STRAIGHT_H = 10,
        STRAIGHT_V = 11


    }

    [Flags]
    public enum ValidExits
    {
        NONE = 0,
        UP = 1,
        RIGHT = 2,
        DOWN = 4,
        LEFT = 8
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
        _spriteRenderer.sprite = FindObjectOfType<SpriteLibrary>().GetSprite(_owner, type, rootID);
    }

    public static bool HasFlag(uint bitFlag, uint flag) => ((bitFlag & flag) != 0);
}
