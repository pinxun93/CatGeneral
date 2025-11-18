using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public List<string> inventory = new List<string>();

    public void AddItem(string itemName)
    {
        if (!inventory.Contains(itemName))
        {
            inventory.Add(itemName);
            Debug.Log($"[物品獲得] 成功獲得: {itemName}");
            // 這裡可以觸發 UI 提示...
        }
    }
}
