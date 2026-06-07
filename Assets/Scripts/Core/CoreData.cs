using UnityEngine;

[CreateAssetMenu(fileName = "NewCoreData", menuName = "CoreSystem/CoreData")]
public class CoreData : ScriptableObject
{
    public string coreName;              // パーツの名前
    public Color coreColor = Color.red; // パーツの色

    [Header("パーツの形状 (4x4のマス目で、チェックを入れたところがブロックになる)")]
    // 💡 16個の要素（4行×4列）で形を表します
    public bool[] shapePattern = new bool[16];
}