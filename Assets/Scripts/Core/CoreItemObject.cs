using UnityEngine;

public class CoreItemObject : MonoBehaviour
{
    [Header("この3Dオブジェクトが持っている形状データ")]
    public CoreData coreData;

    void Start()
    {
        // 💡 修正：data ではなく coreData に統一しました
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && coreData != null)
        {
            renderer.material.color = coreData.coreColor;
        }
    }
}