using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("移動設定")]
    [Tooltip("玩家的移動速度")]
    public float moveSpeed = 7.0f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isMovementEnabled = true; // 新增狀態旗標

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("PlayerMovement Error: 玩家物件上缺少 Rigidbody2D 元件！");
            enabled = false;
            return;
        }

        rb.gravityScale = 0;        // 移除重力影響
        rb.freezeRotation = true;   // 防止角色旋轉
    }

    void Update()
    {
        // 只有當移動被啟用時才讀取輸入
        if (isMovementEnabled)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            moveInput = new Vector2(moveX, moveY).normalized;
        }
        else
        {
            // 如果移動被禁用，清空輸入
            moveInput = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            // 根據輸入和速度設定 Rigidbody 的速度
            rb.linearVelocity = moveInput * moveSpeed;
        }
    }

    /// <summary>
    /// 【✅ 關鍵函式】：供 ChaserController 呼叫來停止玩家移動
    /// </summary>
    public void DisableMovement()
    {
        if (!isMovementEnabled) return;

        isMovementEnabled = false;

        // 清除 Rigidbody 的速度，確保立即停止
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        // 為了節省資源，可以直接禁用這個腳本
        this.enabled = false;

        Debug.Log("Player Movement Disabled: Caught by Chaser!");
    }
}