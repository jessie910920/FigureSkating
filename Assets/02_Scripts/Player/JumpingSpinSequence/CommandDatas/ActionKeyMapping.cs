using UnityEngine;

[System.Serializable]
public class ActionKeyMapping
{
    public KeyCode triggerKey;         // 觸發按鍵
    public string actionName;          // 動作名稱，顯示用
    public ActionType actionType;      // 屬於哪個系統
    [HideInInspector] public bool isFinished = false; // 系統結束後會設為 true
}

public enum ActionType
{
    AxelJump,
    ToeLoopJump,
    CamelSpin,
    CrabStep,
    // 你可以繼續加其他動作類型
}
