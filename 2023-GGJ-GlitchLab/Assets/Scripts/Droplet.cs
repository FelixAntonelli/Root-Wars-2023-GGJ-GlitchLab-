using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droplet : MonoBehaviour
{
    [SerializeField] private int _pointValue = 10;
    [SerializeField] private float _moveSpeed = 1.0f;
    [SerializeField] private Vector3 _moveVector = Vector3.zero;
    [SerializeField] private Tile _destinationTile = null;
    [SerializeField] private GameObject _splashEffect;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public int GetPointValue()
    {
        return _pointValue;
    }

    public void DestroyDroplet()
    {
        // Maybe invoke a particle effect before destroying itself
        Instantiate(_splashEffect, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, _destinationTile.transform.position) < 0.05f)
        {
            transform.position = _destinationTile.transform.position;
            _moveVector = GetNextDestination();
        }
        else if (_moveVector.magnitude == 0.0f)
        {
            _moveVector = CalculateNewMoveVector();
        }
        else
        {
            transform.position += _moveVector * _moveSpeed * Time.deltaTime;
        }
    }

    private Vector3 GetNextDestination()
    {
        _destinationTile = _destinationTile.GetWayTowardsPlant();
        return CalculateNewMoveVector();
    }

    private Vector3 CalculateNewMoveVector()
    {
        return Vector3.Normalize(_destinationTile.transform.position - transform.position);
    }


    public void SetNewDestinationTile(Tile newTile)
    {
        _destinationTile = newTile;
    }
}
