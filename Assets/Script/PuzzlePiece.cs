using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    private Vector3 offset;             // 滑鼠點擊點與物件原點的偏移
    private float zCoord;               // 保持物件的 Z 軸不變
    private JigsawController controller; // 拼圖控制器

    // 該拼圖在完成狀態時應有的世界座標位置
    [HideInInspector] public Vector3 targetPosition;

    void Start()
    {
        // 嘗試自動找到場景中的 JigsawController
        controller = FindFirstObjectByType<JigsawController>();
        if (controller == null)
        {
            Debug.LogError("PuzzlePiece Error: 場景中找不到 JigsawController！");
        }
    }

    // 1. 滑鼠按下時
    void OnMouseDown()
    {
        // 紀錄 Z 軸，確保拖曳時物件不會跑到背景後面
        zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // 計算滑鼠點擊點與物件原點的偏移量
        offset = gameObject.transform.position - GetMouseWorldPos();

        // 拖曳時，將拼圖Z軸拉到前面，以顯示在其他拼圖之上 (如果需要)
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
    }

    // 輔助函式：將滑鼠螢幕座標轉換為世界座標
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    // 2. 滑鼠拖曳時
    void OnMouseDrag()
    {
        // 讓物件跟隨滑鼠移動
        transform.position = GetMouseWorldPos() + offset;
    }

    // 3. 滑鼠放開時
    void OnMouseUp()
    {
        // 恢復Z軸，或讓它回到標準位置
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        // 通知控制器，該拼圖已經移動，可以檢查是否已對齊
        controller.CheckForSnap(this);
    }
}