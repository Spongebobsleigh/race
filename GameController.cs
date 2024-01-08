using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // -------------------------------------------------------
    /// <summary>
    /// ゲームステート.
    /// </summary>
    // -------------------------------------------------------
    public enum PlayState
    {
        None,
        Ready,
        Play,
        Finish,
    }

    // 現在のステート.
    public PlayState CurrentState = PlayState.None;

    //! カウントダウンスタートタイム.
    [SerializeField] int countStartTime = 5;

    //! カウントダウンテキスト.
    [SerializeField] Text countdownText = null;
    //! タイマーテキスト.
    [SerializeField] Text timerText = null;
    // カウントダウンの現在値.
    float currentCountDown = 0;
    // ゲーム経過時間現在値.
    float timer = 0;

    // ラップテキスト.
    [SerializeField] Text lapText = null;

    //! プレイヤー.
    [SerializeField] PlayerController player = null;

    // リトライUI.
    [SerializeField] GameObject retryUI = null;

    void Start()
    {
        CountDownStart();
        player.LapEvent.AddListener(OnLap);
        player.GoalEvent.AddListener(OnGoal);

        timerText.text = "Time : 000.0 s";
        lapText.text = "Lap : 0/" + player.GoalLap;
        retryUI.SetActive(false);
    }

    void Update()
    {
        timerText.text = "Time : 000.0 s";
        // ステートがReadyのとき.
        if (CurrentState == PlayState.Ready)
        {
            // 時間を引いていく.
            currentCountDown -= Time.deltaTime;

            int intNum = 0;
            // カウントダウン中.
            if (currentCountDown <= (float)countStartTime && currentCountDown > 0)
            {
                // int(整数)に.
                intNum = (int)Mathf.Ceil(currentCountDown);
                countdownText.text = intNum.ToString();
            }
            else if (currentCountDown <= 0)
            {
                // 開始.
                StartPlay();
                intNum = 0;
                countdownText.text = "Start!!";

                // Start表示を少しして消す.
                StartCoroutine(WaitErase());
            }
        }
        // ステートがPlayのとき.
        else if (CurrentState == PlayState.Play)
        {
            timer += Time.deltaTime;
            timerText.text = "Time : " + timer.ToString("000.0") + " s";
        }
        else
        {
            // timer = 0;
            // timerText.text = "Time : 000.0 s";
            timerText.text = "Time : " + timer.ToString("000.0") + " s";
        }
    }

    // -------------------------------------------------------
    /// <summary>
    /// カウントダウンスタート.
    /// </summary>
    // -------------------------------------------------------
    void CountDownStart()
    {
        currentCountDown = (float)countStartTime;
        SetPlayState(PlayState.Ready);
        countdownText.gameObject.SetActive(true);
        player.OnStart();
    }

    // -------------------------------------------------------
    /// <summary>
    /// ゲームスタート.
    /// </summary>
    // -------------------------------------------------------
    void StartPlay()
    {
        Debug.Log("Start!!!");
        SetPlayState(PlayState.Play);
    }

    // -------------------------------------------------------
    /// <summary>
    /// 少し待ってからStart表示を消す.
    /// </summary>
    // -------------------------------------------------------
    IEnumerator WaitErase()
    {
        yield return new WaitForSeconds(2f);
        countdownText.gameObject.SetActive(false);
    }

    // -------------------------------------------------------
    /// <summary>
    /// 現在のステートの設定.
    /// </summary>
    /// <param name="state"> 設定するステート. </param>
    // -------------------------------------------------------
    void SetPlayState(PlayState state)
    {
        CurrentState = state;
        player.CurrentState = state;
    }

    // -------------------------------------------------------
    /// <summary>
    /// ラップ数変化時処理.
    /// </summary>
    // -------------------------------------------------------
    void OnLap()
    {
        var current = player.LapCount;
        var goalLap = player.GoalLap;

        lapText.text = "Lap : " + current + "/" + goalLap;
    }

    // -------------------------------------------------------
    /// <summary>
    /// ゴール時処理.
    /// </summary>
    // -------------------------------------------------------
    void OnGoal()
    {
        CurrentState = PlayState.Finish;
        countdownText.text = "Goal!!!";
        countdownText.gameObject.SetActive(true);
        retryUI.SetActive(true);
    }

    // -------------------------------------------------------
    /// <summary>
    /// リトライボタンクリックコールバック.
    /// </summary>
    // -------------------------------------------------------
    public void OnRetryButtonClicked()
    {
        retryUI.SetActive(false);
        timerText.text = "Time : 000.0 s";
        lapText.text = "Lap : 1/" + player.GoalLap;
        player.OnRetry();

        CountDownStart();
    }
}