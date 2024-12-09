using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// DialogueSystem과 UIDialogue의 연결 담당
/// </summary>
public class Dialogue : MonoBehaviour, IPresenter<DialogueSystem, UIDialogue>
{
    [Header("System Model")]
    [SerializeField] private DialogueSystem model;
    [Header("UI View")]
    [SerializeField] private UIDialogue view;

    // 인스펙터 확인용
    public DataUtils.TreeNode<DialogueData> dialogueTree;

    // 타이핑 코루틴
    private Coroutine typingCoroutine = null;

    public void Init(DialogueSystem _model, UIDialogue _view)
    {
        model = _model;
        view = _view;
        if (!view) return;
        model.Init();
        Bind();

        // 인스펙터 확인용
        dialogueTree = model.DialogueRoot;
    }

    public void Bind()
    {
        // 모델 이벤트 연결
        model.BindOnStartTyping(() => view.SetEnabledNextImage(false));
        model.BindOnEndTyping(() => view.SetEnabledNextImage(true));
        model.BindOnNextTyping(StartTypingText);
        model.BindOnEndDialogue(() => view.SetActivePanel(false));
        model.BindOnChangedText(view.SetText);
        model.BindOnEnterBrunch(new Action<string[]>[] 
        {
            view.SetTextToSelectionButtons
            , x => view.SetActiveSelections(true) 
            , x => view.SetNextButtonInteractable(false)
        });
        model.BindOnSelectedSelection(new Action<int>[]
        {
            x => view.SetActiveSelections(false)
            , x => view.SetNextButtonInteractable(true)
        });

        // UI(View) 이벤트 연결
        view.AddEventToNextButton(model.Next);
        view.AddEventsToSelectionButtons(model.SelectSelection);
    }

    public void Unbind()
    {
        // 모델 이벤트 연결 해제
        model.UnbindOnStartTyping();
        model.UnbindOnEndTyping();
        model.UnbindOnNextTyping();
        model.UnbindOnEndDialogue();
        model.UnbindOnChangedText();
        model.UnbindOnEnterBrunch();
        model.UnbindOnSelectedSelection();

        // UI(View) 이벤트 연결 해제
        view.RemoveEventFromNextButton();
        view.RemoveEventsFromSelectionButtons();
    }

    /// <summary>튜토리얼 다이얼로그 시작 => UIPlay.tutorialButton 클릭 이벤트에 연결</summary>
    public void StartTutorial()
    {
        model.SetBeginNode();
        view.SetActivePanel(true);
        StartTypingText();
    }

    /// <summary>타이핑 연출 코루틴 시작</summary>
    private void StartTypingText()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(model.TypingText());
    }
}
