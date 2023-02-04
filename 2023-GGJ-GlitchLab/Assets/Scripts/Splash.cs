using System.Collections;
using UnityEngine;

public class Splash : MonoBehaviour
{
    public void Start()
    {
        StartCoroutine(DestroySelf());
    }

    private IEnumerator DestroySelf()
    {
        Debug.Log("Timer started");
        yield return new WaitForSeconds(1.25f);
        Destroy(gameObject);
    }
}
