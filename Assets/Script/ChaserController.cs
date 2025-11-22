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

    void Start()
    {
        // 嘗試在遊戲開始時自動找到 Tag 為 "Player" 的物件作為目標
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogError("ChaserController Error: 場景中找不到 Tag 為 'Player' 的目標物件！");
                isChasing = false;
            }
        }
    }

    void Update()
    {
        if (isChasing && target != null)
        {
            ChaseTarget();
        }
    }

    private void ChaseTarget()
    {
        Vector3 direction = target.position - transform.position;
        direction.z = 0;
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// 當追逐者碰撞到其他物件時呼叫 (用於判定失敗條件)
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 假設玩家的 Tag 是 "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("玩家被抓住了！準備禁用玩家移動。");

            // 1. 停止追逐者自己的追逐行為
            isChasing = false;

            // 2. 【關鍵修正】：呼叫玩家腳本上的公共停止函式
            if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
            {
                playerMovement.DisableMovement();
            }

            // 這裡可以呼叫 Game Manager 處理遊戲失敗畫面
            // 例如: FindObjectOfType<GameManager>().GameOver();
        }
    }
}