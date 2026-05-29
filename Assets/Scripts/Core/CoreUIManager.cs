using UnityEngine;
using UnityEngine.UI;

public class CoreUIManager : MonoBehaviour
{
    public static CoreUIManager Instance;

    [Header("UI全体の設定")]
    public GameObject coreUI;
    public Transform tempStorage; // TempStorageのRectTransformを入れる場所

    [Header("プレハブ設定")]
    public GameObject stockSlotPrefab; // 先ほど作った StockSlotPrefab を入れる

    [Header("コアの色設定")]
    public Color atkColor = Color.red;
    public Color defColor = Color.blue;
    public Color spdColor = Color.yellow;

    private bool isOpen = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        coreUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleCoreUI();
        }
    }

    void ToggleCoreUI()
    {
        isOpen = !isOpen;
        coreUI.SetActive(isOpen);
        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = isOpen ? 0 : 1;
    }

    // コアを拾った時に呼ばれる関数（何個でも新しく生成する）
    public void AcquireCore(string type)
    {
        Debug.Log(type + " コアを入手！新しいストックマスを生成します。");

        if (stockSlotPrefab == null || tempStorage == null)
        {
            Debug.LogError("UIManagerのインスペクター設定が不足しています！");
            return;
        }

        // 1. プレハブから新しくマス（スロット）を生成し、TempStorageの子供にする
        GameObject newSlot = Instantiate(stockSlotPrefab, tempStorage);
        newSlot.name = type + "_StockSlot";

        // 2. そのマスの色を、拾ったコアの種類に合わせて塗り替える
        Image slotImage = newSlot.GetComponent<Image>();
        if (slotImage != null)
        {
            switch (type)
            {
                case "ATK": slotImage.color = atkColor; break;
                case "DEF": slotImage.color = defColor; break;
                case "SPD": slotImage.color = spdColor; break;
            }
        }

        // 3. 元々Slotに付いている「CoreSlot」などのスクリプトが誤作動しないよう調整
        // （必要に応じて、ドラッグ可能なコンポーネントをここで制御できます）

        // 自動でインベントリ画面を開く
        if (!isOpen) ToggleCoreUI();
    }
}