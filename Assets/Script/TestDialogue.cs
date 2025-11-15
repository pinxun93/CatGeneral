using UnityEngine;

public class TestDialogue : MonoBehaviour
{
    // 連結 TypewriterEffect 腳本
    public TypewriterEffect typewriterEffect;

    // 測試用的對話文本
    private string testDialogue = "「這是一個測試對話。在充滿壓抑的學校裡，你必須保持安靜，否則...」";
    private string nextDialogue = "「...否則你將會被處罰。」";

    void Start()
    {
        // 確保連結了 TypewriterEffect
        if (typewriterEffect == null)
        {
            typewriterEffect = GetComponent<TypewriterEffect>();
        }

        // 遊戲開始時，顯示第一段對話
        typewriterEffect.StartDisplay(testDialogue);
    }

    void Update()
    {
        // 處理玩家輸入 (滑鼠左鍵或 Enter 鍵)
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            // 1. 如果文字還在打字中，則直接跳過顯示完整的文字
            if (typewriterEffect.IsTyping())
            {
                typewriterEffect.SkipTyping();
            }
            // 2. 如果文字已經顯示完畢，則進入下一行對話 (這裡我們用測試文本替換)
            else
            {
                // 為了測試方便，我們在下一行對話和當前對話間切換
                string current = typewriterEffect.dialogueText.text;

                if (current == testDialogue)
                {
                    typewriterEffect.StartDisplay(nextDialogue);
                }
                else
                {
                    typewriterEffect.StartDisplay(testDialogue);
                }
            }
        }
    }
}