using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] private FMODUnity.StudioEventEmitter MovmentEmitter;
    [SerializeField] private FMODUnity.StudioEventEmitter PlaceEmitter;
    [SerializeField] private FMODUnity.StudioEventEmitter DenyEmitter;
    [SerializeField] private FMODUnity.StudioEventEmitter SelectEmitter;



    [SerializeField] private bool ForceEndGame;
    
    [SerializeField] Grid _tileGrid;
    [SerializeField] SpriteLibrary _spriteLib;

    public Action GameEnd;
    
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

    public GameObject[] _tilesShownPlayer1 = new GameObject[4];
    public GameObject[] _tilesShownPlayer2 = new GameObject[4];

    private int[] _availableTilesPlayer1 = new int[4] { 0, 0, 0, 0 };
    private int[] availableTilesPlayer2 = new int[4] { 0, 0, 0, 0 };

    private int _selectedSlotIndexPlayer1;
    private int _selectedSlotIndexPlayer2;

    private bool _disabledMovementPlayer1;
    private bool _disabledMovementPlayer2;

    [SerializeField] GameObject _player1Obj;
    [SerializeField] GameObject _player2Obj;

    public Plant player1Plant;
    public Plant player2Plant;

    private float timeDisabled = 3;

    private Vector2 _maxGridSize;

    //ui stuff
    [SerializeField] TMP_Text _clockText;
    [SerializeField] TMP_Text _PlayerOneText;
    [SerializeField] TMP_Text _PlayerTwoText;

    private float _currTimer = 60;
    private float _currSec = 1;
    private bool doTimer = true;


    // Lerping stuff for the player boxes.
    private Coroutine _p1Lerp;
    private Coroutine _p2Lerp;
    
    private delegate IEnumerator PlayerIconLerpDel(Transform transform, Vector2 target, float lerpTime, Coroutine coroutine);
    private PlayerIconLerpDel PlayerIconLerpFunc;
    
    private Coroutine p1IconLerpCo;
    private Coroutine p2IconLerpCo;
    
    private delegate IEnumerator PlayerSelectionLerpDel(Transform transform, Vector2 target, float lerpTime, Coroutine coroutine);
    private PlayerSelectionLerpDel PlayerSelectionLerpFunc;
    
    private Coroutine p1SelectionLerpCo;
    private Coroutine p2SelectionLerpCo;
    
    private float _lerpSpeed = 0.18f;
    private float _lerpAccuracy = 0.05f;

    [SerializeField] Animator _wobbleClock; 
    [SerializeField] Animator _scoreAnimPlayer1; 
    [SerializeField] Animator _scoreAnimPlayer2;
    [SerializeField] GameObject _fireworkEffect;


    //private Coroutine _p1LerpBox;
    //private Coroutine _p2LerpBox;

    [SerializeField] GameObject _player1Marker;
    [SerializeField] GameObject _player2Marker;


    private void Awake()
    {
        _playerInput = new PlayerControllerActions();
        _playerInput.Enable();
        PlayerIconLerpFunc = Lerp;
        PlayerSelectionLerpFunc = Lerp;
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
        #region setting the size of everything to match with the sie of the cell 
        _player2Obj.transform.localScale = new Vector3(_tileGrid.CellSize, _tileGrid.CellSize, _tileGrid.CellSize);
        _player1Obj.transform.localScale = new Vector3(_tileGrid.CellSize, _tileGrid.CellSize, _tileGrid.CellSize);

        for (int i = 0; i < 4; i++)
        {
            _tilesShownPlayer1[i].transform.localScale = new Vector3(_tileGrid.CellSize, _tileGrid.CellSize, _tileGrid.CellSize);
        }
        for (int i = 0; i < 4; i++)
        {
            _tilesShownPlayer2[i].transform.localScale = new Vector3(_tileGrid.CellSize, _tileGrid.CellSize, _tileGrid.CellSize);
        }

        float selectorScale = 0.85f;
        _player1Marker.transform.localScale = new Vector3(_tileGrid.CellSize * selectorScale, _tileGrid.CellSize * selectorScale, _tileGrid.CellSize * selectorScale);
        _player2Marker.transform.localScale = new Vector3(_tileGrid.CellSize * selectorScale, _tileGrid.CellSize * selectorScale, _tileGrid.CellSize * selectorScale);

        #endregion


        _clockText.text = $"Timer: {_currTimer}";


        StartSetTiles(1);
        StartSetTiles(2);

        SetSpritePlayer2(_spriteLib.GetSprite(GameData.Owner.PLAYER_2, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[availableTilesPlayer2[_selectedSlotIndexPlayer2]]));
        SetSpritePlayer1(_spriteLib.GetSprite(GameData.Owner.PLAYER_1, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[_availableTilesPlayer1[_selectedSlotIndexPlayer1]]));

        _maxGridSize = _tileGrid.max;

        _player1Pos = new Vector2(Mathf.Floor(_maxGridSize.x / 4), _maxGridSize.y - 2);    //sets them at the start
        _player1Obj.transform.position = new Vector3(_player1Pos.x, _player1Pos.y, 0);

        //Debug.Log(_maxGridSize.x / 4);   //5
        //Debug.Log(Mathf.Floor(_maxGridSize.x / 4));    //5
       
        _player2Pos = new Vector2(Mathf.Floor(_maxGridSize.x - _player1Pos.x) - 1, _maxGridSize.y - 2);
        _player2Obj.transform.position = new Vector3(_player2Pos.x, _player2Pos.y, 0);


        _tileGrid.SetSpawn(new Vector2(_player1Pos.x, _player1Pos.y + 1), new Vector2(_player2Pos.x, _player2Pos.y +1), out player1Plant, out player2Plant);

        player1Plant.ScoreCounter = _PlayerOneText;
        player2Plant.ScoreCounter = _PlayerTwoText;

        player1Plant.Anim = _scoreAnimPlayer1;
        player2Plant.Anim = _scoreAnimPlayer2;

       // player1Plant.Effect = _fireworkEffect;
        //player2Plant.Effect = _fireworkEffect;

        StartCoroutine(LerpSelectionBoxBelow(_player1Marker, _tilesShownPlayer1[0].transform.position, GameData.Owner.PLAYER_1));
        StartCoroutine(LerpSelectionBoxBelow(_player2Marker, _tilesShownPlayer2[0].transform.position, GameData.Owner.PLAYER_2));
    }


    public void SetSpritePlayer1(Sprite newImage) => _player1Obj.GetComponent<SpriteRenderer>().sprite = newImage;
    public void SetSpritePlayer2(Sprite newImage) => _player2Obj.GetComponent<SpriteRenderer>().sprite = newImage;


    private void MovePlayer1(InputAction.CallbackContext context)
    {
        if (context.control.device == Gamepad.all[0])
        {
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
                // MovmentEmitter.Play();
                // _p1Lerp = StartCoroutine(LerpSelectionBox(_player1Obj, newPos, GameData.Owner.PLAYER_1));
                if (p1IconLerpCo != null)
                {
                    StopCoroutine(p1IconLerpCo);
                }
                p1IconLerpCo = StartCoroutine(PlayerIconLerpFunc(_player1Obj.transform, newPos, 0.2f, p1IconLerpCo));
            }
        }
        
    }
    private void MovePlayer2(InputAction.CallbackContext context)
    {
        if (context.control.device == Gamepad.all[1])       
        {
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
                // MovmentEmitter.Play();
                // _p2Lerp = StartCoroutine(LerpSelectionBox(_player2Obj, newPos, GameData.Owner.PLAYER_2));
                if (p2IconLerpCo != null)
                {
                    StopCoroutine(p2IconLerpCo);
                }
                p2IconLerpCo = StartCoroutine(PlayerIconLerpFunc(_player2Obj.transform, newPos, 0.2f, p2IconLerpCo));
            }
        }        
    }


    private void ConfirmTilePlacementPlayer1(InputAction.CallbackContext context)
    {
        if (context.control.device == Gamepad.all[0])
        {
            if (_disabledMovementPlayer1)
                return;

            bool connectedToResource;
            if (_tileGrid.PlaceTile(_player1Pos, _spriteLib.SpriteIndexToRootID[_availableTilesPlayer1[_selectedSlotIndexPlayer1]], GameData.Owner.PLAYER_1, out connectedToResource))
            {
                SetRandomIndex(_selectedSlotIndexPlayer1, 1);
                // PlaceEmitter.Play();
                SetSpritePlayer1(_spriteLib.GetSprite(GameData.Owner.PLAYER_1, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[_availableTilesPlayer1[_selectedSlotIndexPlayer1]]));
            }
            else
            {
                // DenyEmitter.Play();
            }
        }
    }
    private void ConfirmTilePlacementPlayer2(InputAction.CallbackContext context)
    {
        if (context.control.device == Gamepad.all[1])
        {
            if (_disabledMovementPlayer2)
                return;

            bool connectedToResource;
            if (_tileGrid.PlaceTile(_player2Pos, _spriteLib.SpriteIndexToRootID[availableTilesPlayer2[_selectedSlotIndexPlayer2]], GameData.Owner.PLAYER_2, out connectedToResource))
            {
                SetRandomIndex(_selectedSlotIndexPlayer2, 2);
                // PlaceEmitter.Play();
                SetSpritePlayer2(_spriteLib.GetSprite(GameData.Owner.PLAYER_2, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[availableTilesPlayer2[_selectedSlotIndexPlayer2]]));
            }
            else
            {
                // DenyEmitter.Play();
            }
        }
    }


    private void SwitchSelectedTilePlayer1(InputAction.CallbackContext context)
    {
        if (context.control.device == Gamepad.all[0])
        {
            if (_selectedSlotIndexPlayer1 + 1 == 4)
                _selectedSlotIndexPlayer1 = 0;
            else
                _selectedSlotIndexPlayer1++;

            SetSpritePlayer1(_spriteLib.GetSprite(GameData.Owner.PLAYER_1, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[_availableTilesPlayer1[_selectedSlotIndexPlayer1]]));
            // StartCoroutine(LerpSelectionBoxBelow(_player1Marker, _tilesShownPlayer1[_selectedSlotIndexPlayer1].transform.position, GameData.Owner.PLAYER_1));

            if (p1SelectionLerpCo != null)
            {
                StopCoroutine(p1SelectionLerpCo);
            }
            p1SelectionLerpCo = StartCoroutine(PlayerSelectionLerpFunc(_player1Marker.transform, _tilesShownPlayer1[_selectedSlotIndexPlayer1].transform.position, 0.2f, p1SelectionLerpCo));
            // SelectEmitter.Play();
        }
    }
    private void SwitchSelectedTilePlayer2(InputAction.CallbackContext context)
    {
        if (context.control.device == Gamepad.all[1])
        {
            if (_selectedSlotIndexPlayer2 + 1 == 4)
                _selectedSlotIndexPlayer2 = 0;
            else
                _selectedSlotIndexPlayer2++;

            SetSpritePlayer2(_spriteLib.GetSprite(GameData.Owner.PLAYER_2, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[availableTilesPlayer2[_selectedSlotIndexPlayer2]]));
            // StartCoroutine(LerpSelectionBoxBelow(_player2Marker, _tilesShownPlayer2[_selectedSlotIndexPlayer2].transform.position, GameData.Owner.PLAYER_2));
            if (p2SelectionLerpCo != null)
            {
                StopCoroutine(p2SelectionLerpCo);
            }
            p2SelectionLerpCo = StartCoroutine(PlayerSelectionLerpFunc(_player2Marker.transform, _tilesShownPlayer2[_selectedSlotIndexPlayer2].transform.position, 0.2f, p2SelectionLerpCo));
            // SelectEmitter.Play();
        }
    }


    private void PlayerResetCallPlayer1(InputAction.CallbackContext context)
    {
        if (context.control.device == Gamepad.all[0])
        {
            if (_disabledMovementPlayer1)
                return;

            for (int i = 0; i < 4; i++)
            {
                SetRandomIndex(i, 1);
            }

            SetSpritePlayer1(_spriteLib.GetSprite(GameData.Owner.PLAYER_1, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[_availableTilesPlayer1[_selectedSlotIndexPlayer1]]));

            StartCoroutine(DisableTimer(1));
        }
    }
    private void PlayerResetCallPlayer2(InputAction.CallbackContext context)
    {
        if (context.control.device == Gamepad.all[1])
        {
            if (_disabledMovementPlayer2)
                return;

            for (int i = 0; i < 4; i++)
            {
                SetRandomIndex(i, 2);
            }

            SetSpritePlayer2(_spriteLib.GetSprite(GameData.Owner.PLAYER_2, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[availableTilesPlayer2[_selectedSlotIndexPlayer2]]));


            StartCoroutine(DisableTimer(2));
        }
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
            _availableTilesPlayer1[indexToChange] = Random.Range(0, 11);
            _tilesShownPlayer1[indexToChange].GetComponent<SpriteRenderer>().sprite = _spriteLib.GetSprite(GameData.Owner.PLAYER_1, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[_availableTilesPlayer1[indexToChange]]);
        }
        else
        {
            availableTilesPlayer2[indexToChange] = Random.Range(0, 11);
            _tilesShownPlayer2[indexToChange].GetComponent<SpriteRenderer>().sprite = _spriteLib.GetSprite(GameData.Owner.PLAYER_2, GameData.TileType.ROOT, _spriteLib.SpriteIndexToRootID[availableTilesPlayer2[indexToChange]]);
        }
    }
    IEnumerator DisableTimer(int playerNum)
    {
        if (playerNum == 1)
        {
            _disabledMovementPlayer1 = true;
        }
        else
        {
            _disabledMovementPlayer2 = true;
        }


        yield return new WaitForSeconds(timeDisabled);


        if (playerNum == 1)
        {
            _disabledMovementPlayer1 = false;
        }
        else
        {
            _disabledMovementPlayer2 = false;
        }
    }

    private IEnumerator Lerp(Transform transform, Vector2 target, float lerpTime, Coroutine coroutine)
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


    private void Update()
    {
        if (!doTimer)
        {
            return;
        }

        if (ForceEndGame)
        {
            ForceEndGame = false;
            doTimer = false;
            GameEnd?.Invoke();
            return;
        }
        
        if (_currSec > 0)
            _currSec -= Time.deltaTime;
        else 
        {
            _currSec = 1;
            _currTimer -= 1;
            _clockText.text = $"Timer: {_currTimer}";
            if (_currTimer == 0)
            {
                doTimer = false;
                GameEnd?.Invoke();
            }
            if (_currTimer < 21) 
            {
                _wobbleClock.SetBool("TimerRunningOut", true);
            }
        }
    }


    private IEnumerator LerpSelectionBoxBelow(GameObject selectionBox, Vector3 newPos, GameData.Owner owner)
    {
        while (Vector3.Distance(selectionBox.transform.position, newPos) > _lerpAccuracy)
        {
            selectionBox.transform.position = Vector3.Lerp(selectionBox.transform.position, newPos, _lerpSpeed * 0.8f);
            yield return new WaitForEndOfFrame();
        }

        selectionBox.transform.position = newPos;

        //if (owner == GameData.Owner.PLAYER_1)
        //{
        //    _p1LerpBox = null;
        //}
        //else
        //{
        //    _p2LerpBox = null;
        //}

        yield break;
    }



}
