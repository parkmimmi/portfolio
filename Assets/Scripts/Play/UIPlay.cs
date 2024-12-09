using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

using BindUtils.UI;

/// <summary>
/// 게임 플레이 UI 컴포넌트 접근
/// </summary>
public class UIPlay : UIBase
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject buttonGroup;
    [SerializeField] private Button startButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button[] tileButtonArr;

    /// <summary>점수 텍스트 갱신</summary>
    /// <param name="_score">현재 점수</param>
    public void UpdateScore(int _score)
    {
        scoreText.text = $"{_score} Score";
    }

    /// <summary>버튼 그룹 오브젝트 활성화 및 비활성화</summary>
    /// <param name="_isActive">활성화 여부</param>
    public void SetActiveButtonGroup(bool _isActive)
    {
        buttonGroup.SetActive(_isActive);
    }

    // 버튼 타일의 색상 조정
    public void SetTileColor(int _idx, Color _color)
    {
        tileButtonArr[_idx].image.color = _color;
    }

    public void SetTileColorAll(Color _color)
    {
        foreach (Button b in tileButtonArr)
        {
            b.image.color = _color;
        }
    }

    /// <returns>타일 버튼의 초기 색상 배열 반환</returns>
    public Color[] GetDefaultTileColorArr()
    {
        Color[] returnArr = new Color[tileButtonArr.Length];
        for (int i = 0; i < tileButtonArr.Length; i++)
        {
            returnArr[i] = tileButtonArr[i].image.color;
        }
        return returnArr;
    }

    /// <summary>시작 버튼 이벤트 연결</summary>
    public void AddEventToStartButton(Action _action)
    {
        ButtonEx.AddEvent(startButton, _action);
    }


    /// <summary>시작 버튼 이벤트 연결 해제</summary>
    public void RemoveEventFromStartButton()
    {
        ButtonEx.RemoveEvent(startButton);
    }

    public void AddEventsToTileButtons(Action<int> _action)
    {
        for (int i = 0; i < tileButtonArr.Length; i++)
        {
            // 타일 역할을 하는 버튼에 이벤트 연결
            int temp = i;
            ButtonEx.AddEvent(tileButtonArr[temp], () => _action(temp));

            // 기존 타일의 색상 캐싱
            tileButtonArr[temp].image.color = Color.white;
        }
    }

    public void RemoveEventsFromTileButtons()
    {
        foreach (Button b in tileButtonArr)
        {
            ButtonEx.RemoveEvent(b);
        }
    }

    public void AddEventToTutorialButton(Action _action)
    {
        ButtonEx.AddEvent(tutorialButton, _action);
    }

    public void RemoveEventToTutorialButton()
    {
        ButtonEx.RemoveEvent(tutorialButton);
    }

    /// <summary>타일 버튼 조작 가능 여부 설정</summary>
    /// <param name="_idx">설정할 타일 버튼의 인덱스</param>
    /// <param name="_canInteract">조작 가능 여부</param>
    public void SetTileInteractable(int _idx, bool _canInteract)
    {
        tileButtonArr[_idx].interactable = _canInteract;
    }

    /// <summary>타일 버튼 조작 가능 여부 일괄 설정</summary>
    /// <param name="_canInteract">조작 가능 여부</param>
    public void SetTileInteractableAll(bool _canInteract)
    {
        foreach (Button b in tileButtonArr)
        {
            b.interactable = _canInteract;
        }
    }
}
