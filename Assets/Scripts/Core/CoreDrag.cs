using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CoreDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentReturnTo = null;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        // 半透明化やクリック透過を制御するためにCanvasGroupを使います
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    // ドラッグを始めた瞬間
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("ドラッグ開始");
        parentReturnTo = this.transform.parent;

        // ドラッグ中は、一時的に最前面（Canvasの直下など）に出すと他のUIに隠れません
        this.transform.SetParent(this.transform.parent.parent.parent);

        // ドラッグ中のアイコンがマウスの邪魔をしないように、クリック判定を一時的にオフにする
        canvasGroup.blocksRaycasts = false;
    }

    // ドラッグ中（マウスを動かしている間）
    public void OnDrag(PointerEventData eventData)
    {
        // マウスカーソルの位置にUIを移動させる
        this.transform.position = eventData.position;
    }

    // マウスを離した（ドロップした）瞬間
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("ドラッグ終了");

        // 正しいスロットの上で離されなかった場合は、元のスロットに戻る
        this.transform.SetParent(parentReturnTo);
        this.transform.localPosition = Vector3.zero; // 中央にリセット

        // クリック判定を元に戻す
        canvasGroup.blocksRaycasts = true;
    }
}