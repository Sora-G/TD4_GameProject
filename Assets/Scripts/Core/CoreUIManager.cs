using UnityEngine;

public class CoreUIManager : MonoBehaviour
{
    // 💡 どこからでもアクセスできるようにする魔法（シングルトン）
    public static CoreUIManager Instance { get; private set; }

    [Header("開閉させるインベントリ全体の親 (CoreInventory)")]
    public GameObject coreInventory;

    [Header("パーツを生成するストックの親 (TempStorage)")]
    public Transform tempStorage;

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
            // 💡 UIを開いた瞬間、ゲームの時間を完全に止める！
            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // 💡 UIを閉じたら、ゲームの時間を1倍（通常通り）に戻す！
            Time.timeScale = 1f;

            // 必要に応じてカーソルをロックする等の処理
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}