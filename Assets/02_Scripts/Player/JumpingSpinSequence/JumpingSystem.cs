using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 處理跳躍動作的輸入流程（子彈時間）
/// </summary>
public class JumpingSystem : MonoBehaviour
{
    public static JumpingSystem Instance;

    [Header("UI")]
    public GameObject panelRoot;
    public List<Button> commandButtons;

    private List<KeyCode> commandSequence;
    private int currentIndex = 0;
    private bool isRunning = false;
    private float maxTime;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// 改成接收動作類型，由系統自己去資料庫抓序列
    /// </summary>
    public void StartCommandSequence(ActionType actionType)
    {
        
        var command = MotionCommandDatabase.Instance.GetCommand(actionType);
        if (command == null)
        {
            Debug.LogError("無法啟動，找不到動作資料");
            return;
        }

        commandSequence = command.keySequence;
        currentIndex = 0;
        isRunning = true;
        maxTime = BulletTimeManager.Instance.duration;

        // 顯示提示面板
        panelRoot.SetActive(true);

        // 更新按鈕文字與顏色
        for (int i = 0; i < commandButtons.Count; i++)
        {
            if (i < commandSequence.Count)
            {
                var txt = commandButtons[i].GetComponentInChildren<TMP_Text>();
                txt.text = commandSequence[i].ToString();
                txt.color = Color.black;
                commandButtons[i].gameObject.SetActive(true);
            }
            else
            {
                commandButtons[i].gameObject.SetActive(false);
            }
        }

        StartCoroutine(CommandTimer());
    }

    private void Update()
    {
        if (!isRunning) return;
        if (currentIndex >= commandSequence.Count) return;

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(commandSequence[currentIndex]))
            {
                SetButtonColor(currentIndex, Color.green);
                currentIndex++;
            }
            else
            {
                SetButtonColor(currentIndex, Color.red);
                currentIndex++;
                EndSequence(success: false);
                return;
            }

            if (currentIndex >= commandSequence.Count)
            {
                EndSequence(success: true);
            }
        }
    }

    private void SetButtonColor(int index, Color color)
    {
        if (index >= commandButtons.Count) return;
        var txt = commandButtons[index].GetComponentInChildren<TMP_Text>();
        txt.color = color;
    }

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
            EndSequence(success: false);
        }
    }

    private void EndSequence(bool success)
    {
        isRunning = false;
        panelRoot.SetActive(false);

        if (success)
            Debug.Log("跳躍指令成功！");
        else
            Debug.Log("跳躍指令失敗！");
    }
}
