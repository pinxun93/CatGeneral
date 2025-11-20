using UnityEngine;



// 讓這個類可以在 Inspector 中顯示和編輯

[System.Serializable]

public class DialogueLine

{

    [Header("角色與內容")]

    [Tooltip("說話的角色名稱。如果留空，則顯示無名對話框。")]

    public string characterName;



    [Tooltip("實際要顯示的對話內容")]

    [TextArea(3, 10)] // 讓文本編輯框變大，方便輸入多行內容

    public string dialogueText;



    [Header("視覺與聲音")]

    [Tooltip("當前角色要顯示的立繪 (Sprite)")]

    public Sprite characterSprite;



    [Tooltip("對話時要播放的音效 (例如：語音、腳步聲、驚嚇聲)")]

    public AudioClip sfx;



    // 未來我們還可以在這裡加入：BGM切換、鏡頭移動指令等

}