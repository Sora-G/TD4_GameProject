using UnityEngine;
using UnityEngine.UI;

public class CoreBox : MonoBehaviour
{
    public static CoreBox Instance { get; private set; }

    public int gridWidth = 10;
    public int gridHeight = 10;

    [Header("1マスの土台となるプレハブ（ImageとCoreSlotをつけたもの）")]
    public GameObject slotPrefab;

    private CoreSlot[,] gridSlots;

    void Awake()
    {
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
        GenerateGrid();
    }

    // 💡 10×10のグリッドマスを自動生成する
    void GenerateGrid()
    {
        gridSlots = new CoreSlot[gridWidth, gridHeight];

        // すでに子要素にあれば削除
        foreach (Transform child in transform) Destroy(child.gameObject);

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                GameObject slotObj = Instantiate(slotPrefab, transform);
                slotObj.name = $"Slot_{x}_{y}";

                CoreSlot slot = slotObj.GetComponent<CoreSlot>() ?? slotObj.AddComponent<CoreSlot>();
                slot.x = x;
                slot.y = y;

                // 初期の色（うっすら白い四角）
                Image img = slotObj.GetComponent<Image>();
                if (img != null) img.color = new Color(1, 1, 1, 0.1f);

                gridSlots[x, y] = slot;
            }
        }
    }

    // 💡 パーツが設置可能かチェックして配置する関数
    // 💡 パーツが設置可能かチェックして配置する関数（ズレ・消滅防止版）
    public bool TryPlaceItem(CoreItemUI item, int startX, int startY)
    {
        if (item == null || item.coreData == null) return false;
        CoreData data = item.coreData;

        // 【ループ1】置けるかどうかの事前チェック
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                int index = row * 4 + col;
                if (data.shapePattern[index]) // パーツが存在するマスのとき
                {
                    int targetX = startX + col;
                    int targetY = startY + row;

                    // グリッドの枠外にはみ出していないかチェック
                    if (targetX >= gridWidth || targetY >= gridHeight || targetX < 0 || targetY < 0) return false;

                    // すでに他のパーツに埋められていないかチェック
                    if (gridSlots[targetX, targetY].isOccupied && gridSlots[targetX, targetY].placedItem != item)
                    {
                        return false;
                    }
                }
            }
        }

        // 🔥 配置を確定する前に、このアイテムが「以前いた古い場所」のデータをすべて消去してお掃除する！
        ClearItemFromGrid(item);

        // 【ループ2】実際に新しい場所に配置してデータを埋める
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                int index = row * 4 + col;
                if (data.shapePattern[index])
                {
                    int targetX = startX + col;
                    int targetY = startY + row;

                    gridSlots[targetX, targetY].isOccupied = true;
                    gridSlots[targetX, targetY].placedItem = item;

                    // 🎯【修正】背景のグリッド自体を染めるのをやめます！
                    // 背景はうっすら白いままでキープし、パーツ自身のグラフィックをそのまま上に重ねます。
                    Image slotImg = gridSlots[targetX, targetY].GetComponent<Image>();
                    if (slotImg != null)
                    {
                        slotImg.color = new Color(1, 1, 1, 0.1f); // 初期の色を維持
                    }
                }
            }
        }

        // 🎯【超重要】配置成功時にアイテム自体を透明化する処理を完全に削除しました！
        // これにより、綺麗に並んだ黄色いパーツのグラフィックがそのままグリッドの上に残り続けます。

        return true;
    }


    // 💡 アイテムがグリッド上から去るときに、色とデータを確実にリセットする関数
    public void ClearItemFromGrid(CoreItemUI item)
    {
        if (gridSlots == null || item == null) return;

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                // 🎯【ここを修正】配置されているアイテムが一致しているマスをすべてお掃除
                if (gridSlots[x, y].placedItem == item)
                {
                    gridSlots[x, y].isOccupied = false;
                    gridSlots[x, y].placedItem = null;

                    // マスの色を初期のうっすら白い四角（元の状態）に完全に戻す！
                    Image slotImg = gridSlots[x, y].GetComponent<Image>();
                    if (slotImg != null)
                    {
                        slotImg.color = new Color(1, 1, 1, 0.1f);
                    }
                }
            }
        }
    }
}