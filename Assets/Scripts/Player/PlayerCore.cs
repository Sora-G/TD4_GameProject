using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    [Header("生成するUIパーツのプレハブ")]
    public GameObject stockSlotPrefab;

    [Header("SpawnAreaのプレハブ (ここにProjectのプレハブを置いてOK)")]
    public GameObject spawnAreaPrefab;

    [Header("テスト用のコアデータ")]
    public CoreData testCoreData;

    // 内部で実際に使う実体（クローン）用の変数
    private Transform activeSpawnArea;

    void Start()
    {
        // 🎯 1. もしSpawnAreaのプレハブが設定されていたら、画面上（Canvas）に実体化する
        if (spawnAreaPrefab != null)
        {
            Canvas mainCanvas = FindObjectOfType<Canvas>();
            if (mainCanvas != null)
            {
                // Canvasの子供としてSpawnAreaを生成
                GameObject spawnedArea = Instantiate(spawnAreaPrefab, mainCanvas.transform, false);
                activeSpawnArea = spawnedArea.transform;
            }
            else
            {
                Debug.LogError("画面に Canvas が見つかりません！UIを表示できません。");
                return;
            }
        }

        // 🎯 2. コアUIを生成する
        if (stockSlotPrefab != null && testCoreData != null)
        {
            // 独立したクローンとして生成（アセットデータ破損エラーを100%回避）
            GameObject newCore = Instantiate(stockSlotPrefab);
            RectTransform rect = newCore.GetComponent<RectTransform>();

            if (rect != null)
            {
                // 先ほど生成した「ゲーム画面上のSpawnArea」の中に安全に入れる
                if (activeSpawnArea != null)
                {
                    newCore.transform.SetParent(activeSpawnArea, false);
                }
                rect.anchoredPosition = Vector2.zero;
            }

            // 見た目のセットアップ
            CoreItemUI coreUI = newCore.GetComponent<CoreItemUI>();
            if (coreUI != null)
            {
                coreUI.SetupShape(testCoreData);
            }
        }
    }
}