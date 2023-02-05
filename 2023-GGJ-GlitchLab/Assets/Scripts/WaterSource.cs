using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource : MonoBehaviour
{
    [SerializeField] private int _reservoireSize = 20;
    [SerializeField] private float _spawningInterval = 2.0f;
    [SerializeField] private List<Tile> _connectingTiles = new List<Tile>(4);
    [SerializeField] private GameObject _waterDroplet;

    private delegate IEnumerator WaterSpawner();
    private WaterSpawner _waterSpawnerFunc;

    private void Awake()
    {
        _waterSpawnerFunc = SpawnWater;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(_waterSpawnerFunc());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConnectTile(Tile tile)
    {
        if(!_connectingTiles.Contains(tile))
        {
            _connectingTiles.Add(tile);
        }
    }

    public void RemoveConnection(Tile tile)
    {
        if(_connectingTiles.Contains(tile))
        {
            _connectingTiles.Remove(tile);
        }
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnWater()
    {
        while (_reservoireSize > 0)
        {
            yield return new WaitForSeconds(_spawningInterval);
            if (_connectingTiles.Count > 0)
            {
                foreach (Tile tile in _connectingTiles)
                {
                    if (_reservoireSize > 0)
                    {
                        Droplet newDroplet = Instantiate(_waterDroplet, gameObject.transform.position, Quaternion.identity).GetComponent<Droplet>();
                        newDroplet.SetNewDestinationTile(tile);
                        _reservoireSize -= 1;
                    }
                }
            }
        }
        yield break;
    }
}
