using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("核心元件")]
    public TypewriterEffect typewriterEffect;
    public TextMeshProUGUI nameText; // 用來顯示角色名稱的 TMP 元件
    public GameObject dialoguePanel; // 整個對話框的 Panel，用於顯示和隱藏
    public Image characterImage;     // <<<<< 用來顯示立繪的 Image 元件
    public SpriteRenderer backgroundRenderer; // 用於顯示背景的 2D 元件


    [Header("場景控制")]
    // 連結到你的 GameSceneController 實例
    public SceneController sceneController;

    [Header("數據")]
    [Tooltip("當前要播放的 DialogueData 資產")]
    public DialogueData currentDialogue;

    // 內部狀態追蹤
    private int currentLineIndex = 0;

    void Start()
    {
        Debug.Log("--- 1. DialogueManager 腳本已啟動 ---"); // <<<<<< 加入這行
        // 確保核心元件已連結
        if (typewriterEffect == null)
        {
            typewriterEffect = GetComponent<TypewriterEffect>();
        }

        // 遊戲開始時，如果設定了對話數據，就開始播放
        if (currentDialogue != null)
        {
            Debug.Log("--- 2. 準備啟動對話 StartDialogue() ---"); // <<<<<< 加入這行
            StartDialogue(currentDialogue);
        }
        else
        {
            Debug.LogError("--- 錯誤：Current Dialogue 數據資產為空！請檢查 Inspector。 ---"); // <<<<<< 加入這行
            dialoguePanel.SetActive(false); // 如果沒有數據，就隱藏對話框
        }
    }

    /// <summary>
    /// 開始播放指定的 DialogueData
    /// </summary>
    public void StartDialogue(DialogueData data)
    {
        currentDialogue = data;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);

        // 設置背景
        if (backgroundRenderer != null && currentDialogue.background != null)
        {
            backgroundRenderer.sprite = currentDialogue.background;
            backgroundRenderer.gameObject.SetActive(true); // 確保背景是啟動的
        }
        else if (backgroundRenderer != null)
        {
            // 如果沒有指定背景，可以選擇隱藏或保留舊的背景
            // backgroundRenderer.gameObject.SetActive(false); 
        }

        DisplayNextLine();
    }

    /// <summary>
    /// 顯示下一行對話
    /// </summary>
    public void DisplayNextLine()
    {
        // 檢查是否還有對話行
        if (currentLineIndex >= currentDialogue.dialogueLines.Count)
        {
            EndDialogue();
            return;
        }


        // 取得當前的對話行數據
        DialogueLine line = currentDialogue.dialogueLines[currentLineIndex];


        // --- 1. 更新 UI 元素 ---
        // 【A. 處理角色名稱 (旁白時清空名稱框)】
        // 如果 characterName 為空，則顯示空字串
        nameText.text = line.characterName;

        // 【B. 處理角色立繪 (旁白時隱藏立繪)】
        if (characterImage != null)
        {
            // 檢查這行對話是否有指定立繪
            bool shouldShowSprite = line.characterSprite != null;

            if (shouldShowSprite)
            {
                // 如果有立繪，則設置並激活 GameObject
                characterImage.sprite = line.characterSprite;
            }
            // 統一控制立繪 GameObject 的激活狀態
            characterImage.gameObject.SetActive(shouldShowSprite);
        }

        // 更新角色名稱
        nameText.text = line.characterName;

        // 新增 Log 檢查賦值結果
        Debug.Log($"檢查名稱元件：nameText 是否為 null？ ({nameText == null})");
        Debug.Log($"賦值名稱：{nameText.text}");

        // 播放音效 (如果有的話)
        if (line.sfx != null)
        {
            // 這裡需要一個 AudioSource 元件來播放
            // 假設你已經有一個 AudioSource 在 DialogueManager 的 GameObject 上
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(line.sfx);
            }
        }

        // 更新角色立繪
        if (characterImage != null)
        {
            // 如果這行沒有指定立繪，我們就隱藏或清空，模擬角色離開
            characterImage.sprite = line.characterSprite;
            characterImage.gameObject.SetActive(line.characterSprite != null);
        }

        // --- 2. 啟動打字效果 ---
        typewriterEffect.StartDisplay(line.dialogueText);

        // 準備進入下一行
        currentLineIndex++;

    }

    /// <summary>
    /// 結束對話，隱藏 UI
    /// </summary>
    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        Debug.Log("對話流程結束！準備切換場景...");
        // 這裡可以觸發場景切換、遊戲狀態改變或自由移動模式

        // --- 【新增邏輯】: 呼叫場景控制器進行切換 ---
        if (sceneController != null)
        {
            sceneController.LoadNextGameScene();
        }
        else
        {
            Debug.LogError("Scene Controller 未連結，無法切換場景！");
        }
    }

    // 將玩家的輸入邏輯從 TestDialogue 轉移到這裡
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            // 確保對話框是啟動狀態
            if (dialoguePanel.activeSelf)
            {
                // 如果文字還在打字中，則跳過
                if (typewriterEffect.IsTyping())
                {
                    typewriterEffect.SkipTyping();
                }
                // 如果文字已經顯示完畢，則進入下一行
                else
                {
                    DisplayNextLine();
                }
            }
        }
    }
}