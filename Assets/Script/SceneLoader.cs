using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Tooltip("要載入的下一個場景名稱")]
    public string nextSceneName = "Room3";

    /// <summary>
    /// 公開方法：供 UI Button 的 OnClick() 事件呼叫，執行場景切換。
    /// </summary>
    public void LoadNextScene()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("SceneLoader Error: nextSceneName 尚未設定！請在 Inspector 中填入目標場景名稱。");
            return;
        }

        // 載入場景前，建議清除 UI 焦點，防止 Unity 內部錯誤
        // 確保引用了 using UnityEngine.EventSystems;
        /*
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        */

        if (Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            Debug.Log($"場景切換：正在載入 {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError($"SceneLoader Error: 場景 '{nextSceneName}' 不存在或未添加到 Build Settings 中！請檢查名稱。");
        }
    }
}