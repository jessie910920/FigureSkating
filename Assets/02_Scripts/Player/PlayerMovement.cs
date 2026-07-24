using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PlayerMovent
    // 負責滑行、加速度、慣性模擬
    // 處理輸入（鍵盤、滑鼠）
    // 觸發動畫（滑行、跳躍、跌倒）

[RequireComponent(typeof(Rigidbody))] // 強制掛載 Rigidbody 組件
public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed = 20f;                 // 最大速度
    public float accelerationRate = 10f;       // 加速速率
    public float rotationSpeed = 10f;           // 轉向速度

    private Rigidbody rb;
    private Transform cam;

    private Vector3 moveInput;
    private Vector3 currentVelocity;             // 記錄目前水平速度

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        cam = Camera.main.transform;
        currentVelocity = Vector3.zero;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = cam.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cam.right;
        camRight.y = 0;
        camRight.Normalize();

        moveInput = (camForward * v + camRight * h).normalized;
    }

    void FixedUpdate()
    {
        // 目標速度 (水平方向)
        Vector3 targetVelocity = moveInput * maxSpeed;

        // 水平方向目前速度 (不含 y 軸)
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        // 慢慢朝目標速度移動，實現加速/減速
        Vector3 newHorizontalVelocity = Vector3.MoveTowards(horizontalVelocity, targetVelocity, accelerationRate * Time.fixedDeltaTime);

        // 更新剛體速度，保留垂直分量
        rb.linearVelocity = new Vector3(newHorizontalVelocity.x, rb.linearVelocity.y, newHorizontalVelocity.z);

        // 轉向
        if (moveInput.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
