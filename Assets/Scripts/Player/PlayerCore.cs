using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    [Header("コアを拾った時にインベントリに生成する【UIプレハブ】")]
    public GameObject stockSlotPrefab;

    [Header("インベントリ内のコア出現場所 (空欄のままで自動取得されます)")]
    public Transform spawnAreaTransform;

    [Header("インベントリ画面の大元パネル (空欄のままで自動取得されます)")]
    public GameObject inventoryPanel;

    private void Start()
    {
        // 🎯【非アクティブ対策】まずシーン上で必ず起きている大元の「Canvas」を探す
        Canvas mainCanvas = FindObjectOfType<Canvas>();

        if (mainCanvas != null)
        {
            // Canvasの子供から、非アクティブ（消えている）オブジェクトも含めて根こそぎ名前で探す
            Transform[] allChildren = mainCanvas.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in allChildren)
            {
                if (child.name == "SpawnArea")
                {
                    spawnAreaTransform = child;
                }
                else if (child.name == "CoreInventory")
                {
                    inventoryPanel = child.gameObject;
                }
            }
        }

        // 🔍 最終確認用のログ
        if (spawnAreaTransform == null) Debug.LogError("【エラー】'SpawnArea' がCanvas内に見つかりません！");
        if (inventoryPanel == null) Debug.LogError("【エラー】'CoreInventory' がCanvas内に見つかりません！");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CoreItem"))
        {
            CoreItemObject coreItemObj = other.GetComponent<CoreItemObject>();
            if (coreItemObj == null || coreItemObj.coreData == null) return;

            CoreData pickedData = coreItemObj.coreData;
            Debug.Log($"本物のコア【{pickedData.coreName}】を検知しました！");

            if (stockSlotPrefab != null && spawnAreaTransform != null)
            {
                // 最初からシーン上の本物の SpawnArea を親にして等倍で安全に生成
                GameObject newCoreUI = Instantiate(stockSlotPrefab, spawnAreaTransform, false);

                // 🎯 インベントリ画面が閉じているなら、生成したコアも非表示にする
                if (inventoryPanel != null && !inventoryPanel.activeSelf)
                {
                    newCoreUI.SetActive(false);
                }

                RectTransform rect = newCoreUI.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.transform.SetParent(spawnAreaTransform, false); // 親をSpawnAreaにする
                    rect.localScale = Vector3.one;                      // 大きさを等倍にする
                    rect.anchoredPosition = Vector2.zero;               // 🎯【超重要】位置を親のど真ん中に強制リセット！
                }

                CoreItemUI coreUI = newCoreUI.GetComponent<CoreItemUI>();
                if (coreUI != null)
                {
                    coreUI.SetupShape(pickedData);
                }
            }

            Destroy(other.gameObject);
        }
    }
}