using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// PlaySystem과 UIPlay의 연결 담당
/// </summary>
public class Play : MonoBehaviour, IPresenter<PlaySystem, UIPlay>
{
    public const float COLOR_ACTIVE_TIME = 0.4f;

    [SerializeField] private PlaySystem model; 
    [SerializeField] private UIPlay view;

    // Scriptable Object
    [SerializeField] private TileInfo tileInfo;

    private Coroutine colorCoroutine = null;
    private WaitForSeconds ws;

    public void Init(PlaySystem _model, UIPlay _view)
    {
        model = _model;
        view = _view;
        if (!view) return;
        ws = new WaitForSeconds(COLOR_ACTIVE_TIME);
        Bind();
    }

    public void Bind()
    {
        // 모델 이벤트 연결
        model.BindOnStartGame(new Action[]
        {
            () => view.SetActiveButtonGroup(false)
            , () => view.SetTileInteractableAll(false)
        });
        model.BindOnChangedStage(new Action[] 
        { 
            StartShowTiles
            , () => view.UpdateScore(model.myScore)
        });
        model.BindOnFailed(() => view.SetActiveButtonGroup(true));
        model.BindOnCheckCorrectTile(new Action<int>[]
        {
            idx => view.SetTileInteractable(idx, false)
            , idx => view.SetTileColor(idx, tileInfo.colorArr[model.selectIdx - 1])
        });

        // UI(View) 이벤트 연결
        view.AddEventToStartButton(model.GameStart);
        view.AddEventsToTileButtons(model.SelectTile);
        view.AddEventToTutorialButton(Manager.instance.gameManager.dialogue.StartTutorial);
    }

    public void Unbind()
    {
        // 모델 이벤트 연결 해제
        model.UnbindOnStartGame();
        model.UnbindOnChangedStage();
        model.UnbindOnFailed();
        model.UnbindOnCheckCorrectTile();

        // UI(View) 이벤트 연결 해제
        view.RemoveEventFromStartButton();
        view.RemoveEventsFromTileButtons();
        view.RemoveEventToTutorialButton();
    }

    /// <summary>스테이지 타일 색상 연출 코루틴 시작</summary>
    private void StartShowTiles()
    {
        // 색상 초기화
        view.SetTileColorAll(tileInfo.defaultColor);

        // 이미 실행 중인 코루틴이 있을 경우 중단
        if (colorCoroutine != null) StopCoroutine(colorCoroutine);
        // 색상 순차 활성화 코루틴 실행 및 완료 콜백
        colorCoroutine = StartCoroutine(ShowTileColor(model.currTileList, () => view.SetTileInteractableAll(true)));
    }

    /// <summary>스테이지 타일 색상 연출 코루틴</summary>
    private IEnumerator ShowTileColor(List<int> _tileIdxList, Action _callback)
    {
        for (int i = 0; i < _tileIdxList.Count; i++)
        {
            view.SetTileColor(_tileIdxList[i], tileInfo.colorArr[i]);
            yield return ws;
            view.SetTileColor(_tileIdxList[i], Color.white);
        }

        _callback?.Invoke();
    }
}
