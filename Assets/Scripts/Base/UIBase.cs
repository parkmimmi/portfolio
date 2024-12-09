using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 각 UI 패널에 해당하는 스크립트가 상속받을 Base 스크립트
/// <para>단일 씬 프로젝트로 구성됨에 따라 각 UI 패널이 씬 역할에 해당</para>
/// <para>혹은 씬 상위에 표시되는 UI(팝업, 이펙트 등)의 집합을 관리하는 역할</para>
/// </summary>
public class UIBase : MonoBehaviour
{
    // 상단 SafeArea
    protected RectTransform topSafeArea;
    // 하단 SafeArea
    protected RectTransform botSafeArea;
    // 중앙 콘텐츠 영역 Frame
    protected RectTransform frameArea;

    /// <summary>초기화 함수</summary>
    public virtual void Init()
    {
        topSafeArea = transform.Find("TopSafeArea")?.GetComponent<RectTransform>();
        botSafeArea = transform.Find("BotSafeArea")?.GetComponent<RectTransform>();
        frameArea = transform.Find("Frame")?.GetComponent<RectTransform>();
        transform.localPosition = Vector3.zero;
    }

    /// <summary>로그인 및 중요 선행 프로세스 이후 시점의 초기화 함수</summary>
    public virtual void InitAfter()
    {

    }

    /// <summary>UI 활성화 시 호출</summary>
    public virtual void OnShow()
    {

    }

    /// <summary>UI 비활성화 시 호출</summary>
    public virtual void OnClose()
    {

    }

    /// <summary>UI 활성화 혹은 비활성화 설정</summary>
    /// <param name="_isOpen">활성화 여부</param>
    public void SetActivePanel(bool _isOpen)
    {
        gameObject.SetActive(_isOpen);
        if (_isOpen) OnShow();
        else OnClose();
    }

    /// <summary>기기 별 SafeArea 설정에 따른 프레임 여백 설정</summary>
    /// <param name="_top">상단 여백</param>
    /// <param name="_bot">하단 여백</param>
    public virtual void SetSafeAreaSize(float _top, float _bot)
    {
        // 상단 SafeArea 설정
        if (topSafeArea != null) topSafeArea.sizeDelta = new Vector2(0.0f, _top);
        // 하단 SafeArea 설정
        if (botSafeArea != null) botSafeArea.sizeDelta = new Vector2(0.0f, _bot);
        // 중앙 콘텐츠 영역 Frame 범위 설정
        if (frameArea != null)
        {
            frameArea.offsetMin = new Vector2(0, _bot);
            frameArea.offsetMax = new Vector2(0, -_top);
        }
    }
}
