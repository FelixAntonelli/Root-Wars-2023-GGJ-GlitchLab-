using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Grid _tileGrid;
    [SerializeField] SpriteLibrary lib;

    #region input vars
    public PlayerControllerActions _playerInput;

    private InputAction _playerMovement1Input;
    private InputAction _playerMovement2Input;

    private InputAction _player1Confirm;
    private InputAction _player2Confirm;

    private InputAction _player1Change;
    private InputAction _player2Change;

    #endregion

    public Vector2 _player1Pos;
    public Vector2 _player2Pos;

    [SerializeField] RectTransform _player1Obj;
    [SerializeField] RectTransform _player2Obj;

    [SerializeField] RawImage _player1Image;
    [SerializeField] RawImage _player2Image;

    uint _player1TileIndex;
     uint _player2TileIndex;

    private Vector2 _maxGridSize;
    

    private void Awake()
    {
        _playerInput = new PlayerControllerActions();
        _playerInput.Enable();
    }

    private void OnEnable()
    {
        _playerMovement1Input = _playerInput.playerMovement.Player1Movement;
        _playerMovement2Input = _playerInput.playerMovement.Player2Movement;

        _player1Confirm = _playerInput.playerMovement.Player1Confirm;
        _player2Confirm = _playerInput.playerMovement.Player2Confirm;

        _player1Change = _playerInput.playerMovement.Player1Change;
        _player2Change = _playerInput.playerMovement.Player2Change;



        _playerMovement1Input.Enable();
        _playerMovement2Input.Enable();

        _player1Confirm.Enable();
        _player2Confirm.Enable();

        _player1Change.Enable();
        _player2Change.Enable();



        _playerMovement1Input.performed += MovePlayer1;
        _playerMovement2Input.performed += MovePlayer2;

        _player1Confirm.performed += ConfirmTilePlacementPlayer1;
        _player2Confirm.performed += ConfirmTilePlacementPlayer2;

        _player1Change.performed += SwitchSelectedTilePlayer1;
        _player2Change.performed += SwitchSelectedTilePlayer2;
    }

    private void OnDisable()
    {
        _playerMovement1Input.Disable();
        _playerMovement2Input.Disable();

        _player1Confirm.Disable();
        _player2Confirm.Disable();

        _player1Change.Disable();
        _player2Change.Disable();
    }



    private void Start()
    {
        _player1Obj.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);   // the 1f is the size of the cell    i will check this throughly tomorrow
        _player1Obj.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);

        _player2Obj.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
        _player2Obj.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);

        _maxGridSize = _tileGrid.max;   //why is the size of the grid multiplied by the size of the cell? this sort of breaks things as for example if the x is 20 and the size is 0.5 then the max index is set to 10 on th x axis 

        _player1Pos = new Vector2((int)Mathf.Lerp(0, _maxGridSize.x, 0.25f), _maxGridSize.y - 1);    //sets them at the start
        _player1Obj.transform.position = new Vector3(_player1Pos.x, _player1Pos.y, 0);

        _player2Pos = new Vector2((int)Mathf.Lerp(0, _maxGridSize.x, 0.75f), _maxGridSize.y - 1);
        _player2Obj.transform.position = new Vector3(_player2Pos.x, _player2Pos.y, 0);
    }


    //give this the texture and this changes the raw images/icon of the player
    public void SetImagePlayer1(Texture newImage) => _player1Image.texture = newImage;
    public void SetImagePlayer2(Texture newImage) => _player2Image.texture = newImage;



    //this gets called when the arrow keys or WASD keys are pressed and moves the player
    private void MovePlayer1(InputAction.CallbackContext context)
    {
        var contextVal = context.ReadValue<Vector2>();

        if ((Mathf.Abs(contextVal.x) == 0 || Mathf.Abs(contextVal.x) == 1) && (Mathf.Abs(contextVal.y) == 0 || Mathf.Abs(contextVal.y) == 1))   //checks if the vectro2 input is valid
        {
            Vector2 newPos = _player1Pos + contextVal;

            if (newPos == _player2Pos)   //checks if its going to overlap the other player
                return;

            if (newPos.x >= _maxGridSize.x || newPos.y >= _maxGridSize.y || newPos.x < 0 || newPos.y < 0)   //checks if its inside the grid
                return;

            _player1Pos = newPos;   //sets the new position
            _player1Obj.transform.position = new Vector3(_player1Pos.x, _player1Pos.y, 0);  //sets the new pos of the obj,  to change
        }
    }
    private void MovePlayer2(InputAction.CallbackContext context)
    {
        var contextVal = context.ReadValue<Vector2>();

        if ((Mathf.Abs(contextVal.x) == 0 || Mathf.Abs(contextVal.x) == 1) && (Mathf.Abs(contextVal.y) == 0 || Mathf.Abs(contextVal.y) == 1))
        {
            Vector2 newPos = _player2Pos + contextVal;

            if (newPos == _player1Pos)
                return;


            if (newPos.x >= _maxGridSize.x || newPos.y >= _maxGridSize.y || newPos.x < 0 || newPos.y < 0)
                return;

            _player2Pos = newPos;
            _player2Obj.transform.position = new Vector3(_player2Pos.x, _player2Pos.y, 0);
        }
    }


    // this in the future will be when the player confirms the placement of the tile he has choosen
    private void ConfirmTilePlacementPlayer1(InputAction.CallbackContext context)
    {
        bool connectedToResource;
       _tileGrid.PlaceTile(_player1Pos,(GameData.RootID)_player1TileIndex, out connectedToResource);
    }
    private void ConfirmTilePlacementPlayer2(InputAction.CallbackContext context)
    {
        bool connectedToResource;
        _tileGrid.PlaceTile(_player2Pos, (GameData.RootID)_player2TileIndex, out connectedToResource);
    }


    // this is for the switching of the tiles like cycling through them, i erased the blackboard so i dont remember if this is what we decided but anyway its here in case delete
    private void SwitchSelectedTilePlayer1(InputAction.CallbackContext context)
    {
        if (_player1TileIndex + 1 == 11)
            _player1TileIndex = 0;
        else
            _player1TileIndex++;


     // SetImagePlayer1(lib.GetSprite(GameData.Owner.PLAYER_1, GameData.TileType.ROOT,   ).texture);


    }
    private void SwitchSelectedTilePlayer2(InputAction.CallbackContext context)
    {
        if (_player2TileIndex + 1 == 11)
            _player2TileIndex = 0;
        else
            _player2TileIndex++;
    }


}
