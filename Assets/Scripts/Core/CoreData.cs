using UnityEngine;

[CreateAssetMenu(fileName = "NewCoreData", menuName = "CoreSystem/CoreData")]
public class CoreData : ScriptableObject
{
    public string coreName;
    public bool[] shapePattern = new bool[16];

    // 🎯【ここを追加！】コアごとの固有カラーを設定できるようにする
    public Color coreColor = Color.red;
}