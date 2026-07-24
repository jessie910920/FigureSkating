using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBillboard : MonoBehaviour
{
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        // 讓物件面對攝影機
        Vector3 lookDir = transform.position - cam.position;
        // lookDir.y = 0; // 移除 Y 軸影響（可選）（只旋轉 Y 軸，保持垂直）
        transform.forward = lookDir;
    }

}
