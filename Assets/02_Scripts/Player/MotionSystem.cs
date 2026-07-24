using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//MotionSystem（動作系統）
// 根據光球觸發或操作決定動作狀態
// 控制子彈時間與跳躍過程
// 評估動作結果（成功 / 失敗）

//20250701開發備忘：把光球觸發改成按鍵觸發

public class MotionSystem : MonoBehaviour
{
    public static MotionSystem Instance; // 單例，供其他腳本呼叫

    private Animator animator;

    // 儲存按鍵綁定的行為對應(按鍵->行為)
    private Dictionary<KeyCode, Action> inputActions = new Dictionary<KeyCode, Action>();

    private void Awake()
    {
        // 建立單例
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        animator = GetComponentInChildren<Animator>(); // 取得子物件中的 Animator

        // 在程式碼中註冊按鍵行為
        inputActions[KeyCode.Alpha1] = () => Debug.Log("MotionSystem: 按下 1，可以寫程式碼來擴充新動作");
    }

    void Update()
    {
        // 統一掃描所有註冊的按鍵行為
        foreach (var entry in inputActions)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                entry.Value?.Invoke(); // 呼叫對應的行為
            }
        }
    }

    /// <summary>
    /// 觸發角色跳躍動畫，進入子彈時間
    /// </summary>
    public void TriggerJump()
    {
        if (animator == null) return;

        animator.SetTrigger("Jump_ver2"); // 播放跳躍動畫
        BulletTimeManager.Instance.EnterBulletTime(); // 進入子彈時間

        // 開始跳躍旋轉步伐指令序列
        List<KeyCode> seq = new List<KeyCode> { KeyCode.A, KeyCode.S, KeyCode.D };
        JumpSpinSequenceSystem.Instance.StartCommandSequence(seq);
        Debug.Log("MotionSystem: TriggerJump called, starting command sequence.");
    }

}
