using UnityEngine;

public class CoreUIManager : MonoBehaviour
{
    public static CoreUIManager Instance;

    [Header("UI全体の設定")]
    public GameObject coreUI;

    [Header("インベントリの4つのスロット(Slot1~4)を順番に入れる")]
    public GameObject[] inventorySlots; // インスペクターで4つのSlotオブジェクトを設定する

    [Header("表示するコアのプレハブ（または非表示のアイコンオブジェクト）")]
    public GameObject atkCoreIcon;
    public GameObject defCoreIcon;
    public GameObject spdCoreIcon;

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

        // 最初はアイコンを全部非表示にしておく
        if (atkCoreIcon != null) atkCoreIcon.SetActive(false);
        if (defCoreIcon != null) defCoreIcon.SetActive(false);
        if (spdCoreIcon != null) spdCoreIcon.SetActive(false);
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

    // コアを拾った時に呼ばれる関数
    public void AcquireCore(string type)
    {
        Debug.Log(type + " コアを入手！空いているスロットを探します。");

        // 1. まず空いているスロット（子要素がいないマス）を探す
        Transform emptySlot = null;
        foreach (GameObject slotObject in inventorySlots)
        {
            if (slotObject != null && slotObject.transform.childCount == 0)
            {
                emptySlot = slotObject.transform;
                break; // 空きが見つかったのでループを抜ける
            }
        }

        // もし空きスロットがなければ何もしない（インベントリ満杯）
        if (emptySlot == null)
        {
            Debug.LogWarning("インベントリが満杯です！");
            return;
        }

        // 2. 拾った種類に応じたアイコンを選択する
        GameObject targetIcon = null;
        switch (type)
        {
            case "ATK": targetIcon = atkCoreIcon; break;
            case "DEF": targetIcon = defCoreIcon; break;
            case "SPD": targetIcon = spdCoreIcon; break;
        }

        // 3. 見つかった空きスロットの中にアイコンを引っ越しさせて表示する！
        if (targetIcon != null)
        {
            targetIcon.transform.SetParent(emptySlot); // 親を空きスロットに変更
            targetIcon.transform.localPosition = Vector3.zero; // 位置を中央にリセット
            targetIcon.SetActive(true); // 表示する
        }
    }
}