using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbTrigger : MonoBehaviour
{
    public float disappearDelay = 0.2f; // 觸發後延遲消失時間
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // 只有第一次有效
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            // 當玩家觸碰光球
            MotionSystem.Instance.TriggerJump();

            // 通知 BulletTimeManager 開啟子彈時間
            // BulletTimeManager.Instance.EnterBulletTime();

            // 做消失效果或直接刪除
            StartCoroutine(Disappear());
        }
    }

    private System.Collections.IEnumerator Disappear()
    {
        // 這裡可以換成特效或縮小動畫
        yield return new WaitForSeconds(disappearDelay);
        Destroy(gameObject);
    }

}
