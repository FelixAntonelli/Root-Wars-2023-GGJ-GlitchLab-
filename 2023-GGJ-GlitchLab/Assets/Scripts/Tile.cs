using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Owner _owner;
    private TileType _type;
    private Sprite _sprite;

    public Tile(Owner owner, TileType type)
    {
        _owner = owner;
        _type = type;
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
        _spriteRenderer.sprite = GameObject.FindObjectOfType<SpriteLibrary>().GetSprite(_owner, _type);
    }

}
