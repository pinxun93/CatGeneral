using UnityEngine;
using System.Collections.Generic; // 用於 List
using System.Linq; // 用於 Shuffle (打亂)

public class JigsawController : MonoBehaviour
{
    // 所有拼圖塊，需要在 Inspector 中連結
    public PuzzlePiece[] pieces;
    public float snapDistance = 0.5f; // 判斷是否對齊的距離容許值

    [Header("打亂設定")]
    [Tooltip("拼圖塊初始打亂的邊界範圍")]
    public Vector2 shuffleAreaMin = new Vector2(-10f, -5f);
    public Vector2 shuffleAreaMax = new Vector2(10f, 5f);

    [Header("流程控制")]
    public UIManager uiManager;
    public string nextSceneName = "Room3";

    // 追蹤正確位置的數量
    private int piecesInPlace = 0;

    private void Start()
    {
        // 確保陣列有內容
        if (pieces == null || pieces.Length == 0)
        {
            pieces = FindObjectsByType<PuzzlePiece>(FindObjectsSortMode.None);
            Debug.Log($"JigsawController: 場景中找到 {pieces.Length} 個拼圖塊。");
        }

        // 初始化並打亂
        InitializeAndShuffle();
    }

    // 將 Start 中的初始化邏輯抽取出來
    public void InitializeAndShuffle()
    {
        if (pieces.Length == 0) return;

        // 1. 記錄正確的目標位置
        InitializeTargetPositions();

        // 2. 打亂拼圖到一個初始位置
        ShufflePieces();
    }

    // 1. 記錄目標位置
    private void InitializeTargetPositions()
    {
        Debug.Log("JigsawController: 正在記錄拼圖的目標位置...");
        foreach (var piece in pieces)
        {
            // 將拼圖當前的位置 (即編輯器中排好的位置) 記錄為目標位置
            piece.targetPosition = piece.transform.position;
        }
    }

    // 2. 隨機打亂拼圖的位置
    private void ShufflePieces()
    {
        Debug.Log("JigsawController: 開始打亂拼圖位置...");

        // 使用 List 來儲存所有拼圖的目標位置 (避免位置重疊)
        List<Vector3> targetPositionsList = pieces.Select(p => p.targetPosition).ToList();

        // 將目標位置列表隨機排序
        targetPositionsList = targetPositionsList.OrderBy(x => Random.value).ToList();

        // 將隨機排序後的位置，分配給拼圖塊的當前位置
        for (int i = 0; i < pieces.Length; i++)
        {
            // 將拼圖的當前位置，設定為打亂後的目標位置
            // 注意：這裡使用 Vector2.zero 作為 Z 軸，避免 Z 軸混亂
            Vector3 randomTarget = targetPositionsList[i];
            pieces[i].transform.position = new Vector3(randomTarget.x, randomTarget.y, pieces[i].transform.position.z);

            // 由於打亂的位置可能導致拼圖塊重疊，如果需要將它們移到另一個區域，
            // 則應該使用隨機座標生成器 (如下面的備註所示)
        }

        // ** (可選的更簡單打亂方法：將所有拼圖塊移動到一個集中的混亂區域) **
        /*
        foreach (var piece in pieces)
        {
            float randomX = Random.Range(shuffleAreaMin.x, shuffleAreaMax.x);
            float randomY = Random.Range(shuffleAreaMin.y, shuffleAreaMax.y);
            piece.transform.position = new Vector3(randomX, randomY, piece.transform.position.z);
        }
        */

        Debug.Log("JigsawController: 拼圖位置打亂完成。");
    }

    // 檢查是否有拼圖塊已移動到正確位置 (由 PuzzlePiece.OnMouseUp 呼叫)
    public void CheckForSnap(PuzzlePiece piece)
    {
        // 計算當前位置與目標位置之間的距離 (忽略 Z 軸)
        float distance = Vector2.Distance(piece.transform.position, piece.targetPosition);

        if (distance <= snapDistance)
        {
            // 距離在容許範圍內：吸附到位
            piece.transform.position = piece.targetPosition;
            piecesInPlace++;
            CheckIfSolved();
        }
        else
        {
            // 離開正確位置：如果它本來在位，則計數減一
            if (piecesInPlace > 0 && Vector2.Distance(piece.transform.position, piece.targetPosition) <= snapDistance)
            {
                piecesInPlace--;
            }
        }
    }

    // 檢查是否所有拼圖都已到位
    private void CheckIfSolved()
    {
        if (piecesInPlace == pieces.Length)
        {
            Debug.Log("拼圖解鎖成功！");
            // 解鎖成功後，可以禁用拖曳或播放動畫
            // ...

            // 轉換場景 (確保 SceneManager 已經設定好)
            // SceneManager.LoadScene(nextSceneName);
        }
    }
}