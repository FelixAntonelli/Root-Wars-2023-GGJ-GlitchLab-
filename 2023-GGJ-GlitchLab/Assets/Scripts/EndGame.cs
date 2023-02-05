using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform playerOneUiStart;
    [SerializeField] private Transform playerOneUiEnd;
    [SerializeField] private Transform playerTwoUiStart;
    [SerializeField] private Transform playerTwoUiEnd;
    [SerializeField] private Transform playerOneUi;
    [SerializeField] private Transform playerTwoUi;
    [Header("Camera Keyframes")]
    [SerializeField] private Transform start;
    [SerializeField] private Transform overdrive;
    [SerializeField] private Transform end;

    private delegate IEnumerator CameraMoveDel();
    private CameraMoveDel CameraMoveFunc;
    private delegate IEnumerator CameraLerpDel(Transform toMove, Vector3 end, float moveTime);
    private CameraLerpDel LerpFunc;
    
    
    private void Awake()
    {   
        playerManager.GameEnd += OnEndGame;
        LerpFunc = CameraLerp;
        CameraMoveFunc = CameraMove;
        playerOneUi.position = playerOneUiStart.position;
        playerTwoUi.position = playerTwoUiStart.position;
    }

    private void OnEndGame()
    {
        StartCoroutine(CameraMoveFunc());
    }

    private IEnumerator CameraMove()
    {
        //Lerp to overdrive
        // yield return StartCoroutine(CameraLerpFunc(camera.transform, overdrive.position, 0.5f));
        StartCoroutine(LerpFunc(camera.transform, end.position, 1.5f));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(LerpFunc(playerOneUi, playerOneUiEnd.position, 0.8f));
        yield return StartCoroutine(LerpFunc(playerTwoUi, playerTwoUiEnd.position, 0.8f));
    }

    private IEnumerator CameraLerp(Transform toMove, Vector3 end, float moveTime)
    {
        float time = 0;
        Vector3 start = toMove.position;
        while (time < moveTime)
        {
            time += Time.deltaTime;
            toMove.position = Vector3.Lerp(start, end, time / moveTime);
            yield return null;
        }
    }

}
