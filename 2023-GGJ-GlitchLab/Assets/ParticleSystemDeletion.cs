using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemDeletion : MonoBehaviour
{
    //to change

    private void Start()
    {
        Destroy(this.transform.gameObject, 5f);
    }
}
