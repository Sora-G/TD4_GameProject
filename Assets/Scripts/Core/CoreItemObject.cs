using UnityEngine;

public class CoreItemObject : MonoBehaviour
{
    [Header("この3Dオブジェクトが持っている形状データ")]
    public CoreData coreData;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && coreData != null)
        {
            renderer.material.color = coreData.coreColor;
        }
    }
}