using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

using BindUtils.UI;

public class UIDialogue : UIBase
{
    [Header("Dialogue UI")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button nextButton;
    [SerializeField] private Image nextImage;

    [Header("Options UI")]
    [SerializeField] private GameObject optionGroupObj;
    [SerializeField] private Button[] arrSelectionButtons;
    [SerializeField] private TextMeshProUGUI[] arrSelectionTexts;

    public override void Init()
    {
        base.Init();
        SetActiveSelections(false);
        SetActivePanel(false);
    }

    /// <summary>다이얼로그 텍스트 갱신</summary>
    /// <param name="_text">출력 텍스트</param>
    public void SetText(string _text)
    {
        text.text = _text;
    }

    /// <summary>진행 버튼 조작 가능 여부 설정</summary>
    public void SetNextButtonInteractable(bool _canInteract)
    {
        nextButton.interactable = _canInteract;
    }

    /// <summary>진행 안내 화살표 아이콘 이미지 활성화 여부 설정</summary>
    public void SetEnabledNextImage(bool _isEnabled)
    {
        nextImage.enabled = _isEnabled;
    }

    /// <summary>선택지 버튼 그룹 활성 및 비활성 설정</summary>
    public void SetActiveSelections(bool _isActive)
    {
        optionGroupObj.SetActive(_isActive);
    }

    /// <summary>진행 버튼 이벤트 연결</summary>
    public void AddEventToNextButton(Action _action)
    {
        ButtonEx.AddEvent(nextButton, _action);
    }

    /// <summary>진행 버튼 이벤트 연결 해제</summary>
    public void RemoveEventFromNextButton()
    {
        ButtonEx.RemoveEvent(nextButton);
    }

    /// <summary>선택지 버튼 텍스트 일괄 설정</summary>
    /// <param name="_texts">선택지 텍스트 배열</param>
    public void SetTextToSelectionButtons(string[] _texts)
    {
        for (int i = 0; i < arrSelectionButtons.Length; i++)
        {
            arrSelectionButtons[i].gameObject.SetActive(i < _texts.Length);
            if (i >= _texts.Length) continue;
            arrSelectionTexts[i].text = _texts[i];
        }
    }

    /// <summary>선택지 버튼 이벤트 일괄 연결</summary>
    /// <param name="_action">선택지 인덱스를 인자로 갖는 이벤트 연결</param>
    public void AddEventsToSelectionButtons(Action<int> _action)
    {
        for (int i = 0; i < arrSelectionButtons.Length; i++)
        {
            int temp = i;
            ButtonEx.AddEvent(arrSelectionButtons[temp], () => _action(temp));
        }
    }
    
    /// <summary>선택지 버튼 이벤트 일괄 해제</summary>
    public void RemoveEventsFromSelectionButtons()
    {
        foreach (Button b in arrSelectionButtons)
        {
            ButtonEx.RemoveEvent(b);
        }
    }
}
