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
        Instance = this;
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
                // マス目UIを生成してCoreBoxの子要素にする
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
    public bool TryPlaceItem(CoreItemUI item, int startX, int startY)
    {
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
                    if (gridSlots[targetX, targetY].isOccupied) return false;
                }
            }
        }

        // 【ループ2】チェック合格！実際に配置してマスを埋める
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
                }
            }
        }
        return true;
    }
}