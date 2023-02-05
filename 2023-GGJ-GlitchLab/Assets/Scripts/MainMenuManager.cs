using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    public Vector3 EndPostition = new Vector3(0, 0, 0);
    public bool move = false;
    public GameObject mainMenu;
    public void StartGame()
    {
        move = true;
        mainMenu.SetActive(false);
    }
    public float speed;
    void Update()
    {
        if (!move) return;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, EndPostition, step);
        if (transform.position == EndPostition)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
