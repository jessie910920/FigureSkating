using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 控制子彈時間期間的輸入指令提示與判定
/// </summary>

public class JumpSpinSequenceSystem : MonoBehaviour
{
    public static JumpSpinSequenceSystem Instance;

    [Header("UI")]
    public GameObject panelRoot;                    // 指令提示面板
    public List<Button> commandButtons;             // 面板上固定的三個按鈕（已在 editor 指派）

    [Header("設定")]
    public List<KeyCode> commandSequence;           // 指令順序（例如 W → S → D）

    private int currentIndex = 0;                   // 目前正在輸入第幾個指令
    private bool isRunning = false;                 // 是否正在進行操作流程
    private float maxTime;                          // 操作時間限制（從 BulletTimeManager 取得）

    private void Awake()
    {
        // 建立單例
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// 外部呼叫：開始輸入流程（如由 OrbManager 或 MotionSystem 觸發）
    /// </summary>
    public void StartCommandSequence(List<KeyCode> sequence)
    {
        commandSequence = sequence;
        currentIndex = 0;
        isRunning = true;

        // 取得子彈時間的持續秒數作為操作時間限制
        maxTime = BulletTimeManager.Instance.duration;

        // 顯示提示面板
        panelRoot.SetActive(true);

        // 將指令文字對應到三個按鈕上
        for (int i = 0; i < commandButtons.Count; i++)
        {
            if (i < commandSequence.Count)
            {
                // var txt = commandButtons[i].GetComponentInChildren<TMP_Text>();
                var txt = commandButtons[i].GetComponentInChildren<TMP_Text>();
                txt.text = commandSequence[i].ToString();
                txt.color = Color.black; // 初始為黑色
            }
            else
            {
                commandButtons[i].gameObject.SetActive(false); // 超出長度就隱藏
            }
        }

        StartCoroutine(CommandTimer());
    }

    private void Update()
    {
        // 若不在操作流程中則不偵測
        if (!isRunning) return;
        if (currentIndex >= commandSequence.Count) return;

        // 若有任意按鍵按下
        if (Input.anyKeyDown)
        {
            // 判斷是否為預期的按鍵
            if (Input.GetKeyDown(commandSequence[currentIndex]))
            {
                // 正確輸入：將對應按鈕文字設為綠色
                SetButtonColor(currentIndex, Color.green);
                currentIndex++;
            }
            else
            {
                // 錯誤輸入：將對應按鈕文字設為紅色
                SetButtonColor(currentIndex, Color.red);
                currentIndex++;
                EndSequence(success: false); // 立即結束流程
                return; // 不再繼續處理
            }

            // 若所有指令都已完成輸入
            if (currentIndex >= commandSequence.Count)
            {
                EndSequence(success: true);
            }
        }
    }

    /// <summary>
    /// 設定指定按鈕文字的顏色（用來顯示輸入結果）
    /// </summary>
    private void SetButtonColor(int index, Color color)
    {
        if (index >= commandButtons.Count) return;
        // var txt = commandButtons[index].GetComponentInChildren<TMP_Text>();
        var txt = commandButtons[index].GetComponentInChildren<TMP_Text>();
        txt.color = color;
    }

    /// <summary>
    /// 倒數 maxTime 秒後自動結束流程
    /// </summary>
    private IEnumerator CommandTimer()
    {
        float timer = 0f;
        while (timer < maxTime && currentIndex < commandSequence.Count)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        if (currentIndex < commandSequence.Count)
        {
            // 時間結束但輸入尚未完成，視為失敗
            EndSequence(success: false);
        }
    }

    /// <summary>
    /// 結束流程，根據成功與否進行後續處理
    /// </summary>
    private void EndSequence(bool success)
    {
        isRunning = false;

        // 關閉提示面板
        panelRoot.SetActive(false);

        // 成功或失敗輸出訊息
        if (success)
        {
            Debug.Log("輸入完成，成功！");
            // MotionSystem.Instance.PlaySuccessAnimation(); // 暫不使用動畫
        }
        else
        {
            Debug.Log("時間到或輸入錯誤，失敗！");
            // MotionSystem.Instance.PlayFailAnimation(); // 暫不使用動畫
        }
    }


}
