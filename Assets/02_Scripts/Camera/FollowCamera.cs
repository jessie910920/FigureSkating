using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 6, -20);
    public float smoothTime = 0.2f; // 越小越快跟上

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        // 計算目標位置
        Vector3 targetPos = target.position + offset;

        // 使用 SmoothDamp 平滑移動攝影機
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        // 持續注視目標
        transform.LookAt(target);
    }
}
