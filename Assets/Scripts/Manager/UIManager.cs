using UnityEngine;

public class UIManager : MonoBehaviour
{
    // UI Canvas 최상위 루트
    public Transform uiRoot;

    public UIBase[] uiBaseArr;

    /// <summary>상위 Manager에서 초기화 순서 조정</summary>
    public void Init()
    {
        if (uiRoot == null) uiRoot = FindFirstObjectByType<Canvas>().transform;
        uiBaseArr = uiRoot.GetComponentsInChildren<UIBase>();
        foreach (UIBase b in uiBaseArr)
        {
            b.Init();
        }
    }

    public T GetUI<T>() where T : UIBase
    {
        foreach (UIBase b in uiBaseArr)
        {
            if (b is T)
            {
                return b as T;
            }
        }
        return null;
    }
}
