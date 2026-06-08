using UnityEngine;

public class CoreUIManager : MonoBehaviour
{
    // 💡 どこからでもアクセスできるようにする魔法（シングルトン）
    public static CoreUIManager Instance { get; private set; }

    [Header("開閉させるインベントリ全体の親 (CoreInventory)")]
    public GameObject coreInventory;

    [Header("パーツを生成するストックの親 (SpawnAreaをセット)")]
    public Transform tempStorage; // 🎯 ここにインスペクターから「SpawnArea」をセットしてください

    private bool isOpen = false;

    void Awake()
    {
        // シングルトンの確定
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 💡 ゲーム開始時はインベントリ画面を閉じておく
        if (coreInventory != null)
        {
            coreInventory.SetActive(false);
            isOpen = false;
        }
    }

    void Update()
    {
        // 💡 Tabキーが押されたら開閉を切り替える
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (coreInventory == null) return;

        isOpen = !isOpen;
        coreInventory.SetActive(isOpen);

        if (isOpen)
        {
            // 🛑【修正】ドラッグがフリーズする原因となる Time.timeScale = 0f は廃止しました！
            // 代わりに、戦車の移動スクリプト等で「isOpen が true の時は操作を受け付けない」ように制限するのがおすすめです。

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // 🔥【新機能】インベントリを開いた瞬間、SpawnAreaの中にいる全てのコアUIを一斉に強制表示（アクティブ化）！
            if (tempStorage != null)
            {
                foreach (Transform child in tempStorage)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}