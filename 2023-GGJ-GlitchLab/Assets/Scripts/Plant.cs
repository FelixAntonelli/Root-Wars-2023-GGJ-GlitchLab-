using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private GameData.Owner _owner;
    [SerializeField] private int _score;

    private void Awake()
    {
        _score = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScorePoint()
    {

    }
}
