using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doNotDestory : MonoBehaviour
{


    void Start()
    {
       //  
        DontDestroyOnLoad(this.gameObject);
    }

}
