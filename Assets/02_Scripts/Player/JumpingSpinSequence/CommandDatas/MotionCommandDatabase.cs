using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 動作資料庫，儲存各個動作對應的指令序列與顯示名稱
/// </summary>
public class MotionCommandDatabase : MonoBehaviour
{
    // 單例模式方便存取
    public static MotionCommandDatabase Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 定義動作資料結構
    [System.Serializable]
    public class MotionCommand
    {
        public ActionType actionType;
        public string displayName;
        public List<KeyCode> keySequence;
    }

    // 可從 Inspector 指派的動作列表
    public List<MotionCommand> motionCommands;

    /// <summary>
    /// 依照動作類型取得對應資料
    /// </summary>
    public MotionCommand GetCommand(ActionType type)
    {
        foreach (var cmd in motionCommands)
        {
            if (cmd.actionType == type) return cmd;
        }
        Debug.LogWarning("找不到對應的動作類型：" + type);
        return null;
    }
}
