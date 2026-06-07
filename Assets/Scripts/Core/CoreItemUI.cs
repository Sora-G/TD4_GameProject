using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class CoreItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("このパーツの形状データ")]
    public CoreData coreData;

    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private RectTransform rectTransform;

    void Awake()
    {
        EnsureComponents();
    }

    void Start()
    {
        if (coreData != null) SetupShape(coreData);
    }

    private void EnsureComponents()
    {
        // 🎯【超安全化】GetComponentでエラーが出ないよう、安全に取得を試みる
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();

        // 💡 RectTransformが無い＝UIではない（3DのCubeなど）場合は、
        // UI用のコンポーネントを追加しようとせず、ここで安全に処理を終了させる（エラーを回避）
        if (rectTransform == null) return;

        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        Image existingImage = GetComponent<Image>();
        if (existingImage != null)
        {
            existingImage.enabled = true;
            existingImage.raycastTarget = true;
            existingImage.color = new Color(0, 0, 0, 0);
        }
    }

    public void SetupShape(CoreData data)
    {
        EnsureComponents();
        coreData = data;

        foreach (Transform child in transform) Destroy(child.gameObject);

        // 3Dオブジェクトの元の見た目を消す
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null) meshRenderer.enabled = false;

        float cellSize = 70f;

        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(cellSize * 4, cellSize * 4);
        }
        else
        {
            // 🎯 UIじゃない場合はこれ以降のUI生成処理（Imageの追加など）を行わない
            return;
        }

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                int index = row * 4 + col;
                if (data.shapePattern[index])
                {
                    GameObject segment = new GameObject("Segment", typeof(RectTransform), typeof(Image));
                    segment.transform.SetParent(transform);

                    RectTransform segRect = segment.GetComponent<RectTransform>();
                    if (segRect != null)
                    {
                        segRect.sizeDelta = new Vector2(cellSize, cellSize);
                        segRect.pivot = new Vector2(0, 1);
                        segRect.anchorMin = new Vector2(0, 1);
                        segRect.anchorMax = new Vector2(0, 1);
                        segRect.anchoredPosition = new Vector2(col * cellSize, -row * cellSize);
                    }

                    Image segImage = segment.GetComponent<Image>();
                    if (segImage != null)
                    {
                        segImage.color = data.coreColor;
                        segImage.raycastTarget = false;
                    }

                    Outline outline = segment.AddComponent<Outline>();
                    outline.effectColor = new Color(0f, 0f, 0f, 0.4f);
                    outline.effectDistance = new Vector2(1f, 1f);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (rectTransform == null) return; // 3Dオブジェクトならドラッグさせない

        originalParent = transform.parent;
        Canvas mainCanvas = GetComponentInParent<Canvas>();
        if (mainCanvas != null) transform.SetParent(mainCanvas.transform);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform == null) return;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (rectTransform == null) return;

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
                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = new Vector2(0, 1);
                rectTransform.pivot = new Vector2(0, 1);
                rectTransform.anchoredPosition = Vector2.zero;
                return;
            }
        }

        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = Vector2.zero;
    }
}