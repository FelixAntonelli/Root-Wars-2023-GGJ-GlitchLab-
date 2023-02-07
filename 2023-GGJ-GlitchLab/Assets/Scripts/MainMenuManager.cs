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
    public GameObject startMusic;
    public GameObject mainMusic;
    public void StartGame()
    {
        move = true;
        
       // mainMenu.SetActive(false);
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    private void Start()
    {
        mainMusic.GetComponent<FMODUnity.StudioEventEmitter>().Play();
        StartCoroutine(LoadAsync());
    }

    public float speed;
    void Update()
    {
        if (!move) return;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, EndPostition, step);
        startMusic.SetActive(true);
        if (transform.position == EndPostition)
        {
            mainMusic.SetActive(true);
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
