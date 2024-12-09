using System;
using UnityEngine;

/// <summary>
/// 게임 플레이 전반
/// </summary>
public class GameManager : MonoBehaviour
{
    public Play play;
    public Dialogue dialogue;

    /// <summary>상위 Manager에서 초기화 순서 조정</summary>
    public void Init()
    {
        if (!play) play = GetComponentInChildren<Play>();
        if (!dialogue) dialogue = GetComponentInChildren<Dialogue>();
        play.Init(new PlaySystem(), Manager.instance.uiManager.GetUI<UIPlay>());
        dialogue.Init(new DialogueSystem(), Manager.instance.uiManager.GetUI<UIDialogue>());
    }
}
