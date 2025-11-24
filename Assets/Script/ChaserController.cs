using UnityEngine;

public class ChaserController : MonoBehaviour
{
    [Header("追逐設定")]
    [Tooltip("追逐者的移動速度")]
    public float moveSpeed = 5.0f;

    [Tooltip("追逐的目標（通常是玩家的角色）")]
    public Transform target;

    [Header("狀態設定")]
    [Tooltip("追逐者是否處於主動追逐狀態")]
    public bool isChasing = true;

    private GameManager gameManager;
    private Rigidbody2D rb;

    void Start()
    {
        // 1. 獲取 Rigidbody2D 元件
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("ChaserController Error: 貓妖物件上缺少 Rigidbody2D 元件！請先添加。");
            enabled = false;
            return;
        }

        // 2. 設置 Rigidbody2D
        rb.gravityScale = 0;        // 移除重力影響
        rb.freezeRotation = true;   // 防止旋轉

        // 3. 嘗試自動找到 Tag 為 "Player" 的物件作為目標
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                isChasing = false;
            }
        }

        // 4. 找到 GameManager 實例 (使用 FindAnyObjectByType 解決過時警告)
        gameManager = FindAnyObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("ChaserController Error: 場景中缺少 GameManager 實例！請檢查 Hierarchy。");
        }
    }

    void Update()
    {
        // Update 僅用於狀態檢查
    }

    void FixedUpdate()
    {
        if (isChasing && target != null)
        {
            ChaseTargetPhysics();
        }
        else
        {
            // 如果不追逐，停止移動
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }
    }

    private void ChaseTargetPhysics()
    {
        // 1. 計算追逐者到目標的方向向量
        Vector3 direction = target.position - transform.position;
        direction.z = 0;

        // 2. 使用 linearVelocity 進行物理移動 (解決 velocity 過時警告)
        rb.linearVelocity = direction.normalized * moveSpeed;
    }

    /// <summary>
    /// 當追逐者碰撞到玩家時呼叫 (用於判定失敗條件)
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("玩家被抓住了！觸發 Game Over。");

            // 1. 禁用玩家移動 (呼叫 PlayerMovement 腳本上的公共函式)
            if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
            {
                playerMovement.DisableMovement();
            }

            // 2. 呼叫 Game Manager 處理失敗 (顯示 UI)
            if (gameManager != null)
            {
                gameManager.GameOver();
            }

            // 停止追逐者移動
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
        }
    }
}