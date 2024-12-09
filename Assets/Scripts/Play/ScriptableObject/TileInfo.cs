using UnityEngine;

[CreateAssetMenu(fileName = "TileInfo", menuName = "Scriptable Objects/TileInfo")]
public class TileInfo : ScriptableObject
{
    public Color[] colorArr = new Color[9];
    public Color defaultColor = Color.white;
}
