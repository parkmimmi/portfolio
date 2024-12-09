using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using DataUtils;
using BindUtils;

[Serializable]
public class DialogueSystem
{
    public const string DATA_PATH = "CSV/DialogueData";

    // 다이얼로그 데이터 캐싱
    [SerializeField] private TreeNode<DialogueData> dialogueNode = null;
    [SerializeField] private TreeNode<DialogueData> dialogueRoot = null;
    public TreeNode<DialogueData> DialogueRoot
    {
        get { return dialogueRoot; }
        private set { dialogueRoot = value; }
    }
    [SerializeField] private Dictionary<int, DialogueSelectionData> selectionDic = null;
    public Dictionary<int, DialogueSelectionData> SelectionDic
    {
        get { return selectionDic; }
        private set { selectionDic = value; }
    }

    private string currText = "";

    // 타이핑 중 상태 변수
    private bool isTyping = false;

    // 타이핑 속도 조절을 위한 캐싱
    private WaitForSeconds currWs = null;
    private WaitForSeconds typingWs = new WaitForSeconds(0.05f);
    private WaitForSeconds skipWs = new WaitForSeconds(0.001f);

    // 실행 이벤트
    public event Action OnStartTyping;
    public event Action OnEndTyping;
    public event Action OnNextTyping;
    public event Action OnEndDialogue;
    public event Action<string> OnChangedText;
    public event Action<string[]> OnEnterBrunch;
    public event Action<int> OnSelectedSelection;

    #region Bind Method
    public void BindOnStartTyping(Action _event)
    {
        ActionEx.Binding(ref OnStartTyping, _event);
    }

    public void BindOnEndTyping(Action _event)
    {
        ActionEx.Binding(ref OnEndTyping, _event);
    }

    public void BindOnNextTyping(Action _event)
    {
        ActionEx.Binding(ref OnNextTyping, _event);
    }

    public void BindOnEndDialogue(Action _event)
    {
        ActionEx.Binding(ref OnEndDialogue, _event);
    }

    public void BindOnChangedText(Action<string> _event)
    {
        ActionEx.Binding(ref OnChangedText, _event);
    }

    public void BindOnEnterBrunch(Action<string[]>[] _events)
    {
        ActionEx.Binding(ref OnEnterBrunch, _events);
    }

    public void BindOnSelectedSelection(Action<int>[] _events)
    {
        ActionEx.Binding(ref OnSelectedSelection, _events);
    }
    #endregion
    
    #region Unbind Method
    public void UnbindOnStartTyping()
    {
        ActionEx.Unbinding(ref OnStartTyping);
    }

    public void UnbindOnEndTyping()
    {
        ActionEx.Unbinding(ref OnEndTyping);
    }

    public void UnbindOnNextTyping()
    {
        ActionEx.Unbinding(ref OnNextTyping);
    }

    public void UnbindOnEndDialogue()
    {
        ActionEx.Unbinding(ref OnEndDialogue);
    }

    public void UnbindOnChangedText()
    {
        ActionEx.Unbinding(ref OnChangedText);
    }

    public void UnbindOnEnterBrunch()
    {
        ActionEx.Unbinding(ref OnEnterBrunch);
    }

    public void UnbindOnSelectedSelection()
    {
        ActionEx.Unbinding(ref OnSelectedSelection);
    }
    #endregion

    public void Init()
    {
        DialogueParser parser = new DialogueParser();
        DialogueRoot = parser.GetDialogueRoot(DATA_PATH);
        dialogueNode = DialogueRoot;
        SelectionDic = parser.GetSelectionDictionay($"{DATA_PATH}_Selection");
    }

    /// <summary>초기화 :: 루트 노드를 현재 노드로 설정</summary>
    public void SetBeginNode()
    {
        dialogueNode = dialogueRoot;
    }

    /// <summary>타이핑 연출 코루틴</summary>
    public IEnumerator TypingText()
    {
        // 타이핑 시작 이벤트 실행
        OnStartTyping?.Invoke();

        // 초기화
        currWs = typingWs;
        currText = "";
        isTyping = true;

        // 텍스트 1자씩 추가
        for (int i = 0; i < dialogueNode.Data.text.Length; i++)
        {
            currText += dialogueNode.Data.text[i];
            OnChangedText?.Invoke(currText);
            yield return currWs;
        }

        isTyping = false;

        // 타이핑 종료 이벤트 실행
        OnEndTyping?.Invoke();
    }

    /// <summary>타이핑 스피드 빠르게 설정</summary>
    public void SetTypingSpeedForSkip()
    {
        currWs = skipWs;
    }

    /// <summary>
    /// 다이얼로그 진행
    /// <list type="bullet">
    /// <item>타이핑 중일 경우: 빠른 속도로 변경하여 타이핑</item>
    /// <item>타이핑 완료 상태의 경우: 다음 다이얼로그 혹은 선택지 출력</item>
    /// </list>
    /// </summary>
    public void Next()
    {
        // 아직 타이핑 중일 경우 빠르게 출력 우선 진행
        if (isTyping)
        {
            SetTypingSpeedForSkip();
            return;
        }

        // 다음 다이얼로그가 선택 옵션이 있을 경우
        if (dialogueNode.Data.selectionGroupNum > 0)
        {
            // 선택지 진입 이벤트 실행
            OnEnterBrunch?.Invoke(GetSelectionTextArr(GetSelectionDataList(dialogueNode.Data.selectionGroupNum)));
        }
        else
        {
            // 마지막 다이얼로그인 경우
            if (dialogueNode.Data.isEnd)
            {
                // 다이얼로그 종료 이벤트 실행
                OnEndDialogue?.Invoke();
                return;
            }
            // 다음 다이얼로그 노드 갱신
            dialogueNode = dialogueNode.Children[0];
            // 다음 다이얼로그 타이핑 이벤트 실행
            OnNextTyping?.Invoke();
        }
    }

    /// <param name="_groupNum">선택지 그룹 번호</param>
    /// <returns>그룹 번호(<paramref name="_groupNum"/>)가 동일한 그룹의 선택지 데이터 리스트 반환</returns>
    public List<DialogueSelectionData> GetSelectionDataList(int _groupNum)
    {
        List<DialogueSelectionData> list = new List<DialogueSelectionData>();
        foreach (DialogueSelectionData data in selectionDic.Values)
        {
            if (data.groupNum == _groupNum) list.Add(data);
        }
        return list;
    }

    public string[] GetSelectionTextArr(List<DialogueSelectionData> _dataList)
    {
        string[] returnArr = new string[_dataList.Count];
        for (int i = 0; i < _dataList.Count; i++)
        {
            returnArr[i] = _dataList[i].text;
        }
        return returnArr;
    }

    /// <summary>
    /// 선택지 선택 진행
    /// </summary>
    /// <param name="_selectionIdx">선택지 버튼 인덱스</param>
    public void SelectSelection(int _selectionIdx)
    {
        // 선택한 버튼에 따라 다음 다이얼로그 갱신
        dialogueNode = dialogueNode.Children[_selectionIdx];
        // 옵션 선택 이벤트 실행
        OnSelectedSelection?.Invoke(_selectionIdx);
        // 다음 타이핑 이벤트 실행
        OnNextTyping?.Invoke();
    }
}
