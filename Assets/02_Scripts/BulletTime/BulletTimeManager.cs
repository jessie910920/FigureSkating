using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制時間流速

public class BulletTimeManager : MonoBehaviour
{
    public static BulletTimeManager Instance;   // 公開靜態變數，用於單例模式（全域唯一實例）
    [Header("Time Settings")]
    public float bulletTimeScale = 0.5f;     // 子彈時間的時間倍率
    public float duration = 5.0f;            // 子彈時間持續秒數（實際時間，不受 timeScale 影響）

    private bool inBulletTime = false;  // 紀錄當前是否處於子彈時間狀態

    private void Awake()
    {
        // 建立單例模式：如果沒有現存實例，就設為自己；否則摧毀重複物件
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 進入子彈時間的公開方法，可供其他腳本呼叫
    public void EnterBulletTime()
    {
        if (inBulletTime) return;  // 如果已經在子彈時間中，就不重複觸發  

        inBulletTime = true;  

        Time.timeScale = bulletTimeScale;                     // 降低整體時間倍率  
        Time.fixedDeltaTime = 0.02f * Time.timeScale;         // 調整物理更新間隔，維持模擬穩定  

        StartCoroutine(ExitAfterRealSeconds(duration));       // 啟動協程，倒數結束後恢復正常時間
    }

    // 協程：根據真實時間倒數結束子彈時間（Time.unscaledDeltaTime 不受 timeScale 影響）
    private System.Collections.IEnumerator ExitAfterRealSeconds(float realTime)
    {
        float elapsed = 0f;
        while (elapsed < realTime)
        {
            yield return null;                         // 等待一幀  
            elapsed += Time.unscaledDeltaTime;         // 每幀累加實際經過的時間  
        }

        ExitBulletTime();  // 到時間後，恢復正常時間
    }

    public void ExitBulletTime()
    {
        Time.timeScale = 1f;               // 時間倍率回到正常  
        Time.fixedDeltaTime = 0.02f;       // 還原物理模擬更新頻率  
        inBulletTime = false;              // 更新狀態
    }

}
