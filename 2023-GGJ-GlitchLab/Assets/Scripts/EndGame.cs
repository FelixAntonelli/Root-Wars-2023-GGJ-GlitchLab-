using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private TMP_Text playerOneScoreText;
    [SerializeField] private TMP_Text playerTwoScoreText;
    [SerializeField] private Transform winnerTextStart;
    [SerializeField] private Transform winnerTextEnd;
    [SerializeField] private Transform winnerTextUi;
    [SerializeField] private TMP_Text winnerText;
    [SerializeField] private Transform menuButtonStart;
    [SerializeField] private Transform menuButtonEnd;
    [SerializeField] private Transform menuButton;
    [SerializeField] private Transform bannerTransform;
    [SerializeField] private Transform timerUITransform;
    [Header("Camera Keyframes")]
    [SerializeField] private Transform start;
    [SerializeField] private Transform overdrive;
    [SerializeField] private Transform end;

    private bool loadUp = false;
    
    private delegate IEnumerator CameraMoveDel();
    private CameraMoveDel CameraMoveFunc;
    private delegate IEnumerator CameraLerpDel(Transform toMove, Vector3 end, float moveTime, LerpType type);
    private CameraLerpDel LerpFunc;
    
    private delegate IEnumerator CountUpDel(TMP_Text text, int target, float countTime, LerpType type);
    private CountUpDel CountUpFunc;


    private enum LerpType
    {
        STRAIGHT = 0,
        QUADRATIC = 1,
        CUBIC = 2,
        QUARTIC = 3,
        QUINTIC = 4,
        SINO = 5,
        EXPO = 6,
        CIRC = 7
    }
    
    private void Awake()
    {
        playerManager.GameEnd += OnEndGame;
        LerpFunc = CameraLerp;
        CameraMoveFunc = CameraMove;
        CountUpFunc = CountUp;
        playerOneUi.position = playerOneUiStart.position;
        playerTwoUi.position = playerTwoUiStart.position;
        winnerTextUi.position = winnerTextStart.position;
        menuButton.position = menuButtonStart.position;
    }

    private void OnEndGame()
    {
        StartCoroutine(CameraMoveFunc());
    }

    private IEnumerator CameraMove()
    {
        //Lerp to overdrive
        // yield return StartCoroutine(CameraLerpFunc(camera.transform, overdrive.position, 0.5f));
        // playerOneScoreText.text = "Score: " + playerManager.player1Plant.score.ToString();
        // playerTwoScoreText.text = "Score: " + playerManager.player2Plant.score.ToString();

        StartCoroutine(LerpFunc(camera.transform, end.position, 1.5f, LerpType.CUBIC));
        timerUITransform.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.0f);

        StartCoroutine(LerpFunc(playerOneUi, playerOneUiEnd.position, 0.8f, LerpType.QUADRATIC));
        yield return StartCoroutine(LerpFunc(playerTwoUi, playerTwoUiEnd.position, 0.8f, LerpType.QUADRATIC));
        
        StartCoroutine(CountUp(playerOneScoreText, playerManager.player1Plant.score, 4f, LerpType.QUINTIC));
        StartCoroutine(CountUp(playerTwoScoreText, playerManager.player2Plant.score, 4f, LerpType.QUINTIC));

        yield return new WaitForSeconds(5f);
        winnerText.text = playerManager.player1Plant.score == playerManager.player2Plant.score ? "It's a DRAW!!" :
            playerManager.player1Plant.score > playerManager.player2Plant.score ? "Player One Wins!!" : "Player Two Wins!!";
        yield return StartCoroutine(LerpFunc(winnerTextUi, winnerTextEnd.position, 0.5f, LerpType.QUADRATIC));
        
        StartCoroutine(BannerBounce(bannerTransform, 7, 15));

        yield return new WaitForSeconds(2.5f);
        StartCoroutine(LerpFunc(menuButton, menuButtonEnd.position, 1f, LerpType.CUBIC));
    }

    private IEnumerator BannerBounce(Transform banner, float radius, float time)
    {
        float t = 0;
        Vector3 startPos = banner.position;
        while (true)
        {
            t += Time.deltaTime;
            if (t >= time)
            {
                t = 0;
            }
            banner.transform.position = startPos +  new Vector3(0 , Mathf.Sin(360 * (t / time)) * radius, 0);
            yield return null;
        }
    }

    private IEnumerator CountUp(TMP_Text text, int target, float countTime, LerpType type)
    {
        float time = 0;
        while (time < countTime)
        {
            time += Time.deltaTime;
            switch (type)
            {
                case LerpType.STRAIGHT:
                    text.text = "Score: " + Mathf.Ceil(target * (time / countTime));
                    break;
                case LerpType.QUADRATIC:
                    text.text = "Score: " + Mathf.Ceil(target * Quadratic(time / countTime));
                    break;
                case LerpType.CUBIC:
                    text.text = "Score: " + Mathf.Ceil(target * Cubic(time / countTime));
                    break;
                case LerpType.QUARTIC:
                    text.text = "Score: " + Mathf.Ceil(target * Quartic(time / countTime));
                    break;
                case LerpType.QUINTIC:
                    text.text = "Score: " + Mathf.Ceil(target * Quintic(time / countTime));
                    break;
                case LerpType.SINO:
                    text.text = "Score: " + Mathf.Ceil(target * Sinusoidal(time / countTime));
                    break;
                case LerpType.EXPO:
                    break;
                case LerpType.CIRC:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            yield return null;
        }
        text.text = "Score: " + target;
    }

    private IEnumerator CameraLerp(Transform toMove, Vector3 end, float moveTime, LerpType type)
    {
        float time = 0;
        Vector3 start = toMove.position;
        while (time < moveTime)
        {
            time += Time.deltaTime;
            switch (type)
            {
                case LerpType.STRAIGHT:
                    toMove.position = Vector3.Lerp(start, end, time / moveTime);
                    break;
                case LerpType.QUADRATIC:
                    toMove.position = Vector3.Lerp(start, end, Quadratic(time / moveTime));
                    break;
                case LerpType.CUBIC:
                    toMove.position = Vector3.Lerp(start, end, Cubic(time / moveTime));
                    break;
                case LerpType.QUARTIC:
                    toMove.position = Vector3.Lerp(start, end, Quartic(time / moveTime));
                    break;
                case LerpType.QUINTIC:
                    toMove.position = Vector3.Lerp(start, end, Quintic(time / moveTime));
                    break;
                case LerpType.SINO:
                    toMove.position = Vector3.Lerp(start, end, Sinusoidal(time / moveTime));
                    break;
                case LerpType.EXPO:
                    toMove.position = Vector3.Lerp(start, end, Exponential(time / moveTime));
                    break;
                case LerpType.CIRC:
                    toMove.position = Vector3.Lerp(start, end, Circular(time / moveTime));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            yield return null;
        }
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadAsync());
        loadUp = true;
    }
    
    public IEnumerator LoadAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (loadUp)
            {
                asyncLoad.allowSceneActivation = true;
                loadUp = false;
                yield return null;
            }
            yield return null;
        }

    }

    public static float Quadratic(float val)
    {
        if ((val *= 2f) < 1f) return 0.5f * val * val;
        return -0.5f * ((val -= 1f) * (val - 2f) - 1f);
    }

    public static float Cubic(float val)
    {
        if ((val *= 2f) < 1f) return 0.5f * val * val * val;
        return 0.5f * ((val -= 2f) * val * val + 2f);
    }

    public static float Quartic(float val)
    {
        if ((val *= 2f) < 1f) return 0.5f * val * val * val * val;
        return -0.5f * ((val -= 2f) * val * val * val - 2f);
    }

    public static float Quintic(float val)
    {
        if ((val *= 2f) < 1f) return 0.5f * val * val * val * val * val;
        return 0.5f * ((val -= 2f) * val * val * val * val + 2f);
    }

    public static float Sinusoidal(float val) => 0.5f * (1f - math.cos(Mathf.PI * val));

    public static float Exponential(float val)
    {
        if (val <= 0f) return 0f;
        if (val >= 1f) return 1f;

        if (val < 0.5f)
        {
            return 0.5f * Mathf.Pow((20f * val) - 10f, 2);
        }
        else
        {
            return -0.5f * Mathf.Pow((-20f * val) + 10f, 2) + 1f;
        }
    }

    public static float Circular(float val)
    {
        if (val < 0.5f)
        {
            return 0.5f * (1f - Mathf.Sqrt(1f - 4f * (val * val)));
        }
        else
        {
            return 0.5f * (Mathf.Sqrt(-((2f * val) - 3f) * ((2f * val) - 1f)) + 1f);
        }
    }

    public static float Back(float val)
    {
        if (val < 0.5f)
        {
            float f = 2f * val;
            return 0.5f * (f * f * f - f * math.sin(f * Mathf.PI));
        }
        else
        {
            float f = (1f - (2f * val - 1f));
            return 0.5f * (1f - (f * f * f - f * math.sin(f * Mathf.PI))) + 0.5f;
        }
    }

    public static float Elastic(float val)
    {
        if (val < 0.5f)
        {
            return 0.5f * math.sin(1f * (Mathf.PI * 0.5f) * (2f * val)) * Mathf.Pow(0.5f * ((2f * val) - 1f), 2);
        }
        else
        {
            return 0.5f * (math.sin(-1f * (Mathf.PI * 0.5f) * ((2f * val - 1f) + 1f)) * Mathf.Pow(-0.5f * (2f * val - 1f), 2) + 2f);
        }
    }



}
