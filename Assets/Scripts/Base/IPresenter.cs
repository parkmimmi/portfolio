using UnityEngine;

/// <summary>
/// Presenter Interface
/// <list type="bullet">
/// <item>시스템(Model) 스크립트와 UI(UIBase)스크립트의 상호 이벤트 연결 담당</item>
/// <item></item>
/// </list>
/// </summary>
public interface IPresenter<M, V>
{
    /// <summary>초기화</summary>
    public void Init(M _m, V _v);
    /// <summary>이벤트 연결</summary>
    public void Bind();
    /// <summary>이벤트 연결 해제</summary>
    public void Unbind();
}
