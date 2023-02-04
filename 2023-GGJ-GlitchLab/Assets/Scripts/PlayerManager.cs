using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Grid _tileGrid;
    [SerializeField] SpriteLibrary _spriteLib;

    #region input vars
    public PlayerControllerActions _playerInput;

    private InputAction _playerMovement1Input;
    private InputAction _playerMovement2Input;

    private InputAction _player1Confirm;
    private InputAction _player2Confirm;

    private InputAction _player1Change;
    private InputAction _player2Change;

    private InputAction _player1Refresh;
    private InputAction _player2Refresh;

    #endregion

    public Vector2 _player1Pos;
    public Vector2 _player2Pos;


    public GameObject[] tilesShownPlayer1 = new GameObject[4];
    public GameObject[] tilesShownPlayer2 = new GameObject[4];

    private int[] availableTilesPlayer1 = new int[4] { 0, 0, 0, 0 };
    private int[] availableTilesPlayer2 = new int[4] { 0, 0, 0, 0 };

    public int selectedSlotIndexPlayer1;
    public int selectedSlotIndexPlayer2;

    private bool disabledMovementPlayer1;
    private bool disabledMovementPlayer2;


    [SerializeField] GameObject _player1Obj;
    [SerializeField] GameObject _player2Obj;

    private float timeDisabled = 3;

    private Vector2 _maxGridSize;

    // Lerping stuff for the player boxes.
    private Coroutine _p1Lerp;
    private Coroutine _p2Lerp;
    
    private delegate IEnumerator PlayerIconLerpDel(Transform transform, Vector2 target, float lerpTime, Coroutine coroutine);
    private PlayerIconLerpDel PlayerIconLerpFunc;
    
    private Coroutine p1Co;
    private Coroutine p2Co;
    
    private float _lerpSpeed = 0.18f;
    private float _lerpAccuracy = 0.05f;


    private void Awake()
    {
        _playerInput = new PlayerControllerActions();
        _playerInput.Enable();
        PlayerIconLerpFunc = PlayerIconLerp;
    }

    private void OnEnable()
    {
        _playerMovement1Input = _playerInput.playerMovement.Player1Movement;
        _playerMovement2Input = _playerInput.playerMovement.Player2Movement;

        _player1Confirm = _playerInput.playerMovement.Player1Confirm;
        _player2Confirm = _playerInput.playerMovement.Player2Confirm;

        _player1Change = _playerInput.playerMovement.Player1Change;
        _player2Change = _playerInput.playerMovement.Player2Change;

        _player1Refresh = _playerInput.playerMovement.Player1Refresh;
        _player2Refresh = _playerInput.playerMovement.Player2Refresh;



        _player1Refresh.Enable();
        _player2Refresh.Enable();

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

        _player1Refresh.performed += PlayerResetCallPlayer1;
        _player2Refresh.performed += PlayerResetCallPlayer2;


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
        _player2Obj.transform.localScale = new Vector3(_tileGrid.CellSize, _tileGrid.CellSize, _tileGrid.CellSize);
        _player1Obj.transform.localScale = new Vector3(_tileGrid.CellSize, _tileGrid.CellSize, _tileGrid.CellSize);

        StartSetTiles( 1);
        StartSetTiles( 2);

        SetSpritePlayer2(_spriteLib.GetSprite(GameData.Owner.PLAYER_2, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[availableTilesPlayer2[selectedSlotIndexPlayer2]]));
        SetSpritePlayer1(_spriteLib.GetSprite(GameData.Owner.PLAYER_1, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[availableTilesPlayer1[selectedSlotIndexPlayer1]]));

        _maxGridSize = _tileGrid.max;

        _player1Pos = new Vector2((int)Mathf.Lerp(0, _maxGridSize.x, 0.25f), _maxGridSize.y - 2);    //sets them at the start
        _player1Obj.transform.position = new Vector3(_player1Pos.x, _player1Pos.y, 0);

        _player2Pos = new Vector2((int)Mathf.Lerp(0, _maxGridSize.x, 0.75f), _maxGridSize.y - 2);
        _player2Obj.transform.position = new Vector3(_player2Pos.x, _player2Pos.y, 0);

        _tileGrid.SetSpawn(new Vector2(_player1Pos.x, _player1Pos.y + 1), new Vector2(_player2Pos.x, _player2Pos.y +1));
    }


    public void SetSpritePlayer1(Sprite newImage) => _player1Obj.GetComponent<SpriteRenderer>().sprite = newImage;
    public void SetSpritePlayer2(Sprite newImage) => _player2Obj.GetComponent<SpriteRenderer>().sprite = newImage;


    private void MovePlayer1(InputAction.CallbackContext context)
    {
        // if (disabledMovementPlayer1)
        //     return;

        var contextVal = context.ReadValue<Vector2>();

        if ((Mathf.Abs(contextVal.x) == 0 || Mathf.Abs(contextVal.x) == 1) && (Mathf.Abs(contextVal.y) == 0 || Mathf.Abs(contextVal.y) == 1) &&
            _p1Lerp == null)   //checks if the vectro2 input is valid
        {
            Vector2 newPos = _player1Pos + contextVal;

            if (newPos == _player2Pos)   //checks if its going to overlap the other player
                return;

            if (newPos.x >= _maxGridSize.x || newPos.y >= _maxGridSize.y || newPos.x < 0 || newPos.y < 0)   //checks if its inside the grid
                return;

            _player1Pos = newPos;   //sets the new position
            //_player1Obj.transform.position = new Vector3(_player1Pos.x, _player1Pos.y, 0);  //sets the new pos of the obj,  to change

            // _p1Lerp = StartCoroutine(LerpSelectionBox(_player1Obj, newPos, GameData.Owner.PLAYER_1));
            if (p1Co != null)
            {
                StopCoroutine(p1Co);
            }
            p1Co = StartCoroutine(PlayerIconLerpFunc(_player1Obj.transform, newPos, 0.2f, p1Co));
        }
    }
    private void MovePlayer2(InputAction.CallbackContext context)
    {
        // if (disabledMovementPlayer2)
        //     return;

        var contextVal = context.ReadValue<Vector2>();

        if ((Mathf.Abs(contextVal.x) == 0 || Mathf.Abs(contextVal.x) == 1) && (Mathf.Abs(contextVal.y) == 0 || Mathf.Abs(contextVal.y) == 1) &&
            _p2Lerp == null)
        {
            Vector2 newPos = _player2Pos + contextVal;

            if (newPos == _player1Pos)
                return;

            if (newPos.x >= _maxGridSize.x || newPos.y >= _maxGridSize.y || newPos.x < 0 || newPos.y < 0)
                return;

            _player2Pos = newPos;
            //_player2Obj.transform.position = new Vector3(_player2Pos.x, _player2Pos.y, 0);

            // _p2Lerp = StartCoroutine(LerpSelectionBox(_player2Obj, newPos, GameData.Owner.PLAYER_2));
            if (p2Co != null)
            {
                StopCoroutine(p2Co);
            }
            p2Co = StartCoroutine(PlayerIconLerpFunc(_player2Obj.transform, newPos, 0.2f, p2Co));
        }
    }


    private void ConfirmTilePlacementPlayer1(InputAction.CallbackContext context)
    {
        if (disabledMovementPlayer1)
            return;

        bool connectedToResource;
        if (_tileGrid.PlaceTile(_player1Pos, _spriteLib.SpriteIndexToRootID[availableTilesPlayer1[selectedSlotIndexPlayer1]], GameData.Owner.PLAYER_1, out connectedToResource))
        {
            SetRandomIndex(selectedSlotIndexPlayer1, 1);

            SetSpritePlayer1(_spriteLib.GetSprite(GameData.Owner.PLAYER_1, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[availableTilesPlayer1[selectedSlotIndexPlayer1]]));
        }
    }
    private void ConfirmTilePlacementPlayer2(InputAction.CallbackContext context)
    {
        if (disabledMovementPlayer2)
            return;

        bool connectedToResource;
        if (_tileGrid.PlaceTile(_player2Pos, _spriteLib.SpriteIndexToRootID[availableTilesPlayer2[selectedSlotIndexPlayer2]], GameData.Owner.PLAYER_2, out connectedToResource))
        {
            SetRandomIndex(selectedSlotIndexPlayer2, 2);

            SetSpritePlayer2(_spriteLib.GetSprite(GameData.Owner.PLAYER_2, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[availableTilesPlayer2[selectedSlotIndexPlayer2]]));
        }
    }


    private void SwitchSelectedTilePlayer1(InputAction.CallbackContext context)
    {
        if (selectedSlotIndexPlayer1 + 1 == 4)
            selectedSlotIndexPlayer1 = 0;
        else
            selectedSlotIndexPlayer1++;

        SetSpritePlayer1(_spriteLib.GetSprite(GameData.Owner.PLAYER_1, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[availableTilesPlayer1[selectedSlotIndexPlayer1]]));
    }
    private void SwitchSelectedTilePlayer2(InputAction.CallbackContext context)
    {
        if (selectedSlotIndexPlayer2 + 1 == 4)
            selectedSlotIndexPlayer2 = 0;
        else
            selectedSlotIndexPlayer2++;

        SetSpritePlayer2(_spriteLib.GetSprite(GameData.Owner.PLAYER_2, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[availableTilesPlayer2[selectedSlotIndexPlayer2]]));
    }



    private void PlayerResetCallPlayer1(InputAction.CallbackContext context)
    {
        if (disabledMovementPlayer1)
            return;

        for (int i = 0; i < 4; i++)
        {
            SetRandomIndex(i, 1);
        }

        SetSpritePlayer1(_spriteLib.GetSprite(GameData.Owner.PLAYER_1, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[availableTilesPlayer1[selectedSlotIndexPlayer1]]));

        StartCoroutine(DisableTimer(1));
    }
    private void PlayerResetCallPlayer2(InputAction.CallbackContext context)
    {
        if (disabledMovementPlayer2)
            return;

        for (int i = 0; i < 4; i++)
        {
            SetRandomIndex(i, 2);
        }

        SetSpritePlayer2(_spriteLib.GetSprite(GameData.Owner.PLAYER_2, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[availableTilesPlayer2[selectedSlotIndexPlayer2]]));


        StartCoroutine(DisableTimer(2));
    }


    private void StartSetTiles(int playerNum) 
    {
        for (int i = 0; i < 4; i++)
        {
            SetRandomIndex(i, playerNum);
        }
    }
    private void SetRandomIndex(int indexToChange, int playerNum)
    {
        if (playerNum == 1)
        {
            availableTilesPlayer1[indexToChange] = Random.Range(0, 11);
            tilesShownPlayer1[indexToChange].GetComponent<SpriteRenderer>().sprite = _spriteLib.GetSprite(GameData.Owner.PLAYER_1, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[availableTilesPlayer1[indexToChange]]);
        }
        else
        {
            availableTilesPlayer2[indexToChange] = Random.Range(0, 11);
            tilesShownPlayer2[indexToChange].GetComponent<SpriteRenderer>().sprite = _spriteLib.GetSprite(GameData.Owner.PLAYER_2, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[availableTilesPlayer2[indexToChange]]);
        }
    }
    IEnumerator DisableTimer(int playerNum)
    {
        if (playerNum == 1)
        {
            disabledMovementPlayer1 = true;
        }
        else
        {
            disabledMovementPlayer2 = true;
        }


        yield return new WaitForSeconds(timeDisabled);


        if (playerNum == 1)
        {
            disabledMovementPlayer1 = false;
        }
        else
        {
            disabledMovementPlayer2 = false;
        }
    }

    private IEnumerator PlayerIconLerp(Transform transform, Vector2 target, float lerpTime, Coroutine coroutine)
    {
        Vector2 start =  transform.position;
        float time = 0;
        while (time < lerpTime)
        {
            time += Time.deltaTime;
            transform.position = Vector2.Lerp(start, target, time / lerpTime);
            yield return null;
        }
        coroutine = null;
    }

    private IEnumerator LerpSelectionBox(GameObject playerBox, Vector3 newPos, GameData.Owner owner)
    {
        while (Vector3.Distance(playerBox.transform.position, newPos) > _lerpAccuracy)
        {
            playerBox.transform.position = Vector3.Lerp(playerBox.transform.position, newPos, _lerpSpeed);
            yield return new WaitForEndOfFrame();
        }

        playerBox.transform.position = newPos;

        if (owner == GameData.Owner.PLAYER_1)
        {
            _p1Lerp = null;
        }
        else
        {
            _p2Lerp = null;
        }

        yield break;
    }
}
