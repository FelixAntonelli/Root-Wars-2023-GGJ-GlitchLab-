using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Grid _tileGrid;

    public PlayerControllerActions _playerInput;

    private InputAction _playerMovement1Input;
    private InputAction _playerMovement2Input;


    public Vector2 _player1Pos;
    public Vector2 _player2Pos;


    private void Awake()
    {
        _playerInput = new PlayerControllerActions();
    }


    private void OnEnable()
    {
        _playerMovement1Input = _playerInput.playerMovement.Player1Movement;
        _playerMovement2Input = _playerInput.playerMovement.Player2Movement;

        _playerMovement1Input.Enable();
        _playerMovement2Input.Enable();

        _playerMovement1Input.performed += MovePlayer1;
        _playerMovement2Input.performed += MovePlayer2;
    }

    private void OnDisable()
    {
        _playerMovement1Input.Disable();
        _playerMovement2Input.Disable();
    }



    private void Start()
    {
        _player1Pos = new Vector2Int(3, 0);
        _player2Pos = new Vector2Int(7, 0);
    }

    private void MovePlayer1(InputAction.CallbackContext context) 
    {
        _player1Pos += context.ReadValue<Vector2>();
    }

    private void MovePlayer2(InputAction.CallbackContext context)
    {
        _player2Pos += context.ReadValue<Vector2>();
    }
}
