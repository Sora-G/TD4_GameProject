using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class CoreItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("このパーツの形状データ")]
    [SerializeField] private CoreData _coreData;
    public CoreData coreData
    {
        get => _coreData;
        set
        {
            _coreData = value;
            if (_coreData != null) SetupShape(_coreData);
        }
    }

    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private RectTransform rectTransform;
    private LayoutElement layoutElement;

    void Awake()
    {
        EnsureComponents();
    }

    void Start()
    {
        if (_coreData != null) SetupShape(_coreData);
    }

    void OnEnable()
    {
        if (_coreData != null) SetupShape(_coreData);
    }

    private void EnsureComponents()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();

        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        if (layoutElement == null)
        {
            layoutElement = GetComponent<LayoutElement>();
            if (layoutElement == null) layoutElement = gameObject.AddComponent<LayoutElement>();
        }

        Image existingImage = GetComponent<Image>();
        if (existingImage != null)
        {
            existingImage.enabled = true;
            existingImage.raycastTarget = true;
            existingImage.color = new Color(0, 0, 0, 0); // 土台自体は完全に透明にする
        }
    }

    public void SetupShape(CoreData data)
    {
        EnsureComponents();
        _coreData = data;

        // 古いマスを非表示にして安全に削除
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.SetActive(false);
            child.name = "Destroying";
            Destroy(child);
        }

        if (data == null || data.shapePattern == null || data.shapePattern.Length < 16) return;

        // 🎯 システム上の1マスの基本サイズは「70f」として計算する
        float cellSize = 70f;

        int minRow = 4, maxRow = -1, minCol = 4, maxCol = -1;
        bool hasPattern = false;

        for (int index = 0; index < 16; index++)
        {
            if (data.shapePattern[index])
            {
                int row = index / 4;
                int col = index % 4;

                if (row < minRow) minRow = row;
                if (row > maxRow) maxRow = row;
                if (col < minCol) minCol = col;
                if (col > maxCol) maxCol = col;
                hasPattern = true;
            }
        }

        if (!hasPattern) return;

        int widthBlocks = (maxCol - minCol) + 1;
        int heightBlocks = (maxRow - minRow) + 1;

        // 🎯 実際のサイズを計算（2マスなら 70 * 2 = 140）
        float totalWidth = widthBlocks * cellSize;
        float totalHeight = heightBlocks * cellSize;

        // 🎯【超重要】親のGridLayoutGroupによるサイズ強制上書きを「無視」させる設定
        if (layoutElement != null)
        {
            // これらを true にすることで、親の自動整列に殺されず、140x140 が維持されます
            layoutElement.ignoreLayout = false;
            layoutElement.minWidth = totalWidth;
            layoutElement.minHeight = totalHeight;
            layoutElement.preferredWidth = totalWidth;
            layoutElement.preferredHeight = totalHeight;
        }

        // 🎯 スケールは変な挙動（ドラッグ時のズレなど）を防ぐために「1」に正しく戻します
        if (rectTransform != null)
        {
            rectTransform.localScale = Vector3.one;
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.sizeDelta = new Vector2(totalWidth, totalHeight); // ここで140x140に確定
        }

        // 🎯 見た目だけを半分（35ピクセル相当）にするための描画用cellSize
        float visualCellSize = cellSize * 0.5f; // = 35f

        // 有効な範囲内でループを回して中身（赤いグラフィック）を生成
        for (int row = minRow; row <= maxRow; row++)
        {
            for (int col = minCol; col <= maxCol; col++)
            {
                int index = row * 4 + col;
                if (data.shapePattern[index])
                {
                    GameObject segment = new GameObject("Segment", typeof(RectTransform), typeof(Image));
                    segment.transform.SetParent(transform);

                    RectTransform segRect = segment.GetComponent<RectTransform>();
                    if (segRect != null)
                    {
                        segRect.anchorMin = new Vector2(0, 1);
                        segRect.anchorMax = new Vector2(0, 1);
                        segRect.pivot = new Vector2(0, 1);

                        // 🎯 赤いマスの見た目のサイズ自体を半分にする（35x35にする）
                        segRect.sizeDelta = new Vector2(visualCellSize, visualCellSize);

                        // 🎯 配置する座標の計算も半分（35ピクセル間隔）にする
                        float posX = (col - minCol) * visualCellSize;
                        float posY = -(row - minRow) * visualCellSize;
                        segRect.anchoredPosition = new Vector2(posX, posY);
                    }

                    Image segImage = segment.GetComponent<Image>();
                    if (segImage != null)
                    {
                        Color solidColor = data.coreColor;
                        solidColor.a = 1f;
                        segImage.color = solidColor;
                        segImage.raycastTarget = true;
                    }

                    DragRelay relay = segment.AddComponent<DragRelay>();
                    relay.targetDragHandler = this;
                }
            }
        }

        Canvas.ForceUpdateCanvases();
        if (transform.parent != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CoreBox.Instance != null)
        {
            CoreBox.Instance.ClearItemFromGrid(this);
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }

        originalParent = transform.parent;

        Canvas mainCanvas = GetComponentInParent<Canvas>();
        if (mainCanvas != null)
        {
            transform.SetParent(mainCanvas.transform);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        CoreSlot targetSlot = null;
        foreach (var result in results)
        {
            CoreSlot slot = result.gameObject.GetComponent<CoreSlot>();
            if (slot != null)
            {
                targetSlot = slot;
                break;
            }
        }

        if (targetSlot != null && CoreBox.Instance != null)
        {
            if (CoreBox.Instance.TryPlaceItem(this, targetSlot.x, targetSlot.y))
            {
                transform.SetParent(targetSlot.transform);
                if (rectTransform != null)
                {
                    rectTransform.anchorMin = new Vector2(0, 1);
                    rectTransform.anchorMax = new Vector2(0, 1);
                    rectTransform.pivot = new Vector2(0, 1);
                    rectTransform.anchoredPosition = Vector2.zero;
                }
                return;
            }
        }

        transform.SetParent(originalParent);
        if (rectTransform != null) rectTransform.anchoredPosition = Vector2.zero;
    }
}

public class DragRelay : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CoreItemUI targetDragHandler;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (targetDragHandler != null) targetDragHandler.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (targetDragHandler != null) targetDragHandler.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (targetDragHandler != null) targetDragHandler.OnEndDrag(eventData);
    }
}