using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Plant : MonoBehaviour
{
    [SerializeField] private GameData.Owner _owner;
    public int score;
    [SerializeField] private GameObject _plant;
    private TMP_Text _scoreCounter;
    private Animator _anim;
    public ParticleSystem _fireworks;

    public TMP_Text ScoreCounter 
    {
        set { _scoreCounter = value; }
    }
    public Animator Anim
    {
        set { _anim = value; }
    }

    private void Awake()
    {
        score = 0;
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
        score += value;

        _scoreCounter.text = $"Score: {score}";

        if (score % 200 == 0) 
        {
            _anim.SetTrigger("CounterCall");
            _fireworks.Play();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Droplet droplet = collision.gameObject.GetComponent<Droplet>();
        if (droplet != null)
        {
            GrowPlant();
            ScorePoint(droplet.GetPointValue());
            droplet.DestroyDroplet();
        }
    }

    private void GrowPlant()
    {
        _plant.transform.localScale += Vector3.one * 0.01f;
    }
}
