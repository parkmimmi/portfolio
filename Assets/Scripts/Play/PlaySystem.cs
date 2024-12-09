using System;
using System.Collections.Generic;
using Random = System.Random;

using BindUtils;

/// <summary>
/// 게임 플레이에 필요한 데이터와 로직 처리
/// </summary>
public class PlaySystem
{
    /*
     :: 게임 플레이 규칙 ::
     1. 9개의 타일에 불이 들어오는 순서를 기억한다.
     2. 동일한 순서로 동일한 타일을 클릭한다.
     3. 성공 시 점수 +1
     */
    public const int MAX_COUNT_TILE = 6;
    public const int MIN_COUNT_TILE = 3;

    public int myScore = 0;
    public int selectIdx = 0;

    // 현재 스테이지 정답 타일 리스트(최대 수 일정하지 않음)
    public List<int> currTileList = new List<int>();

    public event Action OnStartGame;
    public event Action OnFailed;
    public event Action OnChangedStage;
    public event Action<int> OnCheckCorrectTile;

    #region Bind Method
    public void BindOnStartGame(Action[] _events)
    {
        ActionEx.Binding(ref OnStartGame, _events);
    }

    public void BindOnChangedStage(Action[] _events)
    {
        ActionEx.Binding(ref OnChangedStage, _events);
    }

    public void BindOnFailed(Action _event)
    {
        ActionEx.Binding(ref OnFailed, _event);
    }

    public void BindOnCheckCorrectTile(Action<int>[] _events)
    {
        ActionEx.Binding(ref OnCheckCorrectTile, _events);
    }
    #endregion

    #region Unbind Method
    public void UnbindOnStartGame()
    {
        ActionEx.Unbinding(ref OnStartGame);
    }

    public void UnbindOnChangedStage()
    {
        ActionEx.Unbinding(ref OnChangedStage);
    }

    public void UnbindOnFailed()
    {
        ActionEx.Unbinding(ref OnFailed);
    }

    public void UnbindOnCheckCorrectTile()
    {
        ActionEx.Unbinding(ref OnCheckCorrectTile);
    }
    #endregion

    /// <summary>게임 시작 처리</summary>
    public void GameStart()
    {
        myScore = 0;
        selectIdx = 0;
        ChangeStage();
        OnStartGame?.Invoke();
    }

    /// <summary>다음 스테이지 설정</summary>
    public void Next()
    {
        myScore++;
        selectIdx = 0;
        ChangeStage();
    }

    // 실패
    /// <summary>스테이지 실패 처리</summary>
    public void Failed()
    {
        OnFailed?.Invoke();
    }

    /// <summary>플레이어의 선택 순서 인덱스 증가 처리</summary>
    public void IncrSelectIdx()
    {
        selectIdx++;
    }

    /// <summary>스테이지 변경(최초 시작, 스테이지 진행 등)</summary>
    public void ChangeStage()
    {
        currTileList = GetNewStageList();
        OnChangedStage?.Invoke();
    }

    /// <returns>선택한 인덱스(<paramref name="_tileIdx"/>) 타일의 정답 여부 체크</returns>
    public bool IsCorrectTile(int _tileIdx)
    {
        return currTileList[selectIdx] == _tileIdx;
    }

    /// <returns>스테이지 클리어 여부 반환</returns>
    public bool IsClearStage()
    {
        return selectIdx == currTileList.Count;
    }

    /// <summary>선택된 타일 체크</summary>
    public void SelectTile(int _tileIdx)
    {
        // 선택 타일의 정답 여부 확인
        if (IsCorrectTile(_tileIdx))
        {
            IncrSelectIdx();
            OnCheckCorrectTile?.Invoke(_tileIdx);

            if (IsClearStage())
            {
                Next();
                return;
            }
            return;
        }

        Failed();
    }

    /// <returns>스테이지 타일 정보 생성 및 반환</returns>
    public List<int> GetNewStageList()
    {
        List<int> returnList = new List<int>();

        int rMax = new Random().Next(MIN_COUNT_TILE, MAX_COUNT_TILE);
        Random rIdx = new Random();
        for (int i = 0; i < rMax; i++)
        {
            int idx = rIdx.Next(0, 9);
            while (returnList.Contains(idx))
            {
                idx = rIdx.Next(0, 9);
            }
            returnList.Add(idx);
        }
        return returnList;
    }
}
