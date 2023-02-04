using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Plant : MonoBehaviour
{
    [SerializeField] private GameData.Owner _owner;
    [SerializeField] private int _score;
    private TMP_Text _scoreCounter;

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

    private void ScorePoint(int value)
    {
        _score += value;

        _scoreCounter.text = $"Score: {_score}";
    }


    public void SetTextUI(TMP_Text text) => _scoreCounter = text;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Droplet droplet = collision.gameObject.GetComponent<Droplet>();
        if (droplet != null)
        {
            ScorePoint(droplet.GetPointValue());
            droplet.DestroyDroplet();
        }
    }
}
