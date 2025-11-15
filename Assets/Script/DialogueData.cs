using UnityEngine;
using System.Collections.Generic; // 用來使用 List

// 讓 Unity 可以在 Project 視窗中創建這個資產文件
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [Header("對話設定")]
    [Tooltip("此段對話的標題，方便在專案中識別")]
    public string dialogueTitle = "Scene_1_Intro";

    [Tooltip("此段對話所使用的背景圖")]
    public Sprite background;

    [Header("對話列表")]
    [Tooltip("按照順序排列的對話行")]
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}
