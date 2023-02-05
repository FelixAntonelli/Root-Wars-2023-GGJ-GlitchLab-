using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    public Vector3 EndPostition = new Vector3(0, 0, 0);
    public bool move = false;
    public bool loadup = false;
    public GameObject mainMenu;
    public void StartGame()
    {
        move = true;
       // mainMenu.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(LoadAsync());
    }

    public float speed;
    void Update()
    {
        if (!move) return;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, EndPostition, step);

        if (transform.position == EndPostition)
        {
            move = false;
            loadup = true;
        }
    }

    IEnumerator LoadAsync()
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SampleScene");
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (loadup)
            {
                asyncLoad.allowSceneActivation = true;
                loadup = false;
                yield return null;
            }
            yield return null;
        }

    }

}
