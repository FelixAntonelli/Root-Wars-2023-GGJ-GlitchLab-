using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private float _speed;

    private float _halfBounds;
    private Vector3 _speedVector;

    private void Awake()
    {
        _halfBounds = GetComponent<Renderer>().bounds.extents.x;
        _speedVector = new Vector3(_speed, 0.0f, 0.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _speedVector * Time.deltaTime;

        if (transform.position.x > 22.0f + _halfBounds)
        {
            ResetCloud();
        }
    }

    private void ResetCloud()
    {
        transform.position = new Vector3(_halfBounds * -3.0f, transform.position.y, transform.position.z);
    }
}
