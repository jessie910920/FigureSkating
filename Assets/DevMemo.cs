using UnityEngine;

public class DevMemo : MonoBehaviour
{
    /// <summary>
    /// 20240911 開發備忘：
    /// 上次做到哪裡：
    // 遊戲核心邏輯
        //  你希望三個系統（JumpingSystem、SpinSystem、SequenceSystem）共用一套動作流程。
        // 在進入動作後，分為三個階段（準備 → 主體 → 結束），而動作觸發不再依靠光球，而是玩家自己隨時按下特定按鍵。
    // ActionTriggerManager
        // 負責偵測玩家輸入的按鍵，決定要進入哪一個動作。
        // 會暫停 PlayerMovement，啟動子彈時間，並把動作丟給對應的系統（例如 JumpingSystem.Instance.StartCommandSequence(actionType)）。
        // 動作結束後，恢復 PlayerMovement 和正常時間。
    // 動作序列資料庫
        // 我們計劃建立一個「動作資料庫」來儲存不同動作（例如 Axel 跳、Toe Loop 跳、Camel Spin、Crab Step）的指令序列。
        // 三個系統（Jump/Spin/Sequence）不再需要外部傳入整個指令序列，而是只接收一個 ActionType，然後自己去資料庫撈取對應的指令序列。(然後比對玩家的輸入和指令序列相符程度。)
    // 目前狀態
        // 架構已經整理好，可以呼叫 JumpingSystem.Instance.StartCommandSequence(actionType) 這樣的形式。
        // 還缺的就是把資料庫填完整（也就是 ActionType → KeyCode 序列的對應表）。
    /// </summary>
}
