using System;
using System.Collections.Generic;
using UnityEngine;

// 負責偵測動作觸發按鍵，暫停滑行並交給對應系統處理
public class ActionTriggerManager : MonoBehaviour
{
    [Header("系統參考")]
    public PlayerMovent playerMovement;
    // public UIController uiController;

    [Header("動作對應表")]
    public List<ActionKeyMapping> keyMappings; // 可在 Inspector 編輯每個按鍵對應哪種動作與名稱

    private bool isPerformingAction = false;

    void Update()
    {
        if (isPerformingAction) return;

        foreach (var map in keyMappings)
        {
            if (Input.GetKeyDown(map.triggerKey))
            {
                StartCoroutine(TriggerAction(map));
                break;
            }
        }
    }

    private System.Collections.IEnumerator TriggerAction(ActionKeyMapping map)
    {
        isPerformingAction = true;

        // 暫停滑行
        if (playerMovement != null)
            playerMovement.enabled = false;

        // 顯示提示 UI
        // if (uiController != null)
        //     uiController.ShowActionName("開始 " + map.actionName);

        // 啟動子彈時間
        BulletTimeManager.Instance.EnterBulletTime();

        // 呼叫對應系統
        switch (map.actionType)
        {
            case ActionType.AxelJump:
                JumpingSystem.Instance.StartCommandSequence(map.actionType);
                break;
            case ActionType.ToeLoopJump:
                JumpingSystem.Instance.StartCommandSequence(map.actionType);
                break;
            case ActionType.CamelSpin:
                SpinSystem.Instance.StartCommandSequence(map.actionType);
                break;
            case ActionType.CrabStep:
                SequenceSystem.Instance.StartCommandSequence(map.actionType);
                break;
        }

        // 等待動作結束（系統會呼叫 ActionFinished）
        yield return new WaitUntil(() => map.isFinished);

        // 結束子彈時間
        BulletTimeManager.Instance.ExitBulletTime();

        // 關閉 UI
        // if (uiController != null)
        //     uiController.HideActionName();

        // 恢復滑行
        if (playerMovement != null)
            playerMovement.enabled = true;

        isPerformingAction = false;
    }
}
