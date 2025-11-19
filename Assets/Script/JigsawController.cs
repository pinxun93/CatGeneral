using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class JigsawController : MonoBehaviour
{
    // --- 核心設定 ---
    [Header("核心設定")]
    [Tooltip("場景中所有掛載 PuzzlePiece 的拼圖塊")]
    public PuzzlePiece[] pieces;

    [Tooltip("拼圖塊與目標位置的最大容許距離，達到此距離則吸附")]
    public float snapDistance = 0.5f;

    // --- 流程與狀態 ---
    [Header("流程與狀態控制")]
    [Tooltip("解謎成功後要載入的下一個場景名稱")]
    public string nextSceneName = "Room3";

    private int piecesInPlace = 0;      // 當前正確位置的數量
    private bool isSolved = false;     // 解謎狀態鎖，防止重複觸發


    void Start()
    {
        // 嘗試自動查找場景中的所有 PuzzlePiece (修正警告版)
        if (pieces == null || pieces.Length == 0)
        {
            pieces = FindObjectsByType<PuzzlePiece>(FindObjectsSortMode.None);
        }

        InitializeAndShuffle();
    }

    /// <summary>
    /// 初始化所有拼圖的目標位置並進行打亂。
    /// </summary>
    public void InitializeAndShuffle()
    {
        if (pieces.Length == 0)
        {
            Debug.LogError("JigsawController Error: 場景中找不到任何拼圖塊！");
            return;
        }

        InitializeTargetPositions();
        ShufflePieces();

        Debug.Log("JigsawController: 拼圖系統已準備就緒。");
    }

    /// <summary>
    /// 記錄拼圖塊在編輯器中擺放的正確位置。
    /// </summary>
    private void InitializeTargetPositions()
    {
        foreach (var piece in pieces)
        {
            // 記錄當前位置 (Z軸使用0，假設拼圖在XY平面上)
            piece.targetPosition = new Vector3(piece.transform.position.x, piece.transform.position.y, 0f);
        }
    }

    /// <summary>
    /// 將所有拼圖塊的位置進行隨機置換 (Fisher-Yates 打亂)。
    /// </summary>
    private void ShufflePieces()
    {
        piecesInPlace = 0;

        // 提取並準備打亂的位置列表
        List<Vector3> shuffledPositions = new List<Vector3>();
        foreach (var piece in pieces)
        {
            shuffledPositions.Add(new Vector3(piece.targetPosition.x, piece.targetPosition.y, 0f));
        }

        // Fisher-Yates 演算法
        int n = shuffledPositions.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector3 temp = shuffledPositions[i];
            shuffledPositions[i] = shuffledPositions[j];
            shuffledPositions[j] = temp;
        }

        // 分配打亂後的位置
        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].transform.position = new Vector3(
                shuffledPositions[i].x,
                shuffledPositions[i].y,
                pieces[i].transform.position.z // 維持Z軸
            );
        }
    }


    /// <summary>
    /// 由 PuzzlePiece.OnMouseUp 呼叫，檢查拼圖塊是否需要吸附。
    /// </summary>
    public void CheckForSnap(PuzzlePiece piece)
    {
        float distance = Vector2.Distance(piece.transform.position, piece.targetPosition);

        if (distance <= snapDistance)
        {
            // 吸附到位
            piece.transform.position = piece.targetPosition;
        }

        // 每次放下拼圖都重新計算一次到位數量，避免計數錯誤
        RecalculateInPlaceCount();

        CheckIfSolved();
    }

    /// <summary>
    /// 精確計算當前有多少個拼圖已到位。
    /// </summary>
    private void RecalculateInPlaceCount()
    {
        int count = 0;
        foreach (var piece in pieces)
        {
            float distance = Vector2.Distance(piece.transform.position, piece.targetPosition);
            if (distance <= snapDistance)
            {
                count++;
            }
        }
        piecesInPlace = count;
    }


    /// <summary>
    /// 檢查是否所有拼圖都已到位，並啟動延遲切換場景。
    /// </summary>
    private void CheckIfSolved()
    {
        if (isSolved || piecesInPlace < pieces.Length) return;

        isSolved = true;
        Debug.Log($"拼圖解鎖成功！將於 3 秒後切換到場景: {nextSceneName}");

        StartCoroutine(HandlePuzzleSolved());
    }

    /// <summary>
    /// 處理解謎成功後的流程 (延遲 3 秒 -> 轉換場景)。
    /// </summary>
    private IEnumerator HandlePuzzleSolved()
    {
        const float delayDuration = 3.0f;

        // 1. 等待 3 秒
        yield return new WaitForSeconds(delayDuration);

        // 2. 轉換場景
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            if (Application.CanStreamedLevelBeLoaded(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogError($"JigsawController Error: 場景 '{nextSceneName}' 不存在或未添加到 Build Settings 中！請檢查名稱。");
            }
        }
        else
        {
            Debug.LogError("JigsawController Error: nextSceneName 未設置！無法轉換場景。");
        }
    }
}