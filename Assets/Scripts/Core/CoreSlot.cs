using UnityEngine;
using UnityEngine.EventSystems;

public class CoreSlot : MonoBehaviour, IDropHandler
{
    // このスロットの上に他のUIがドロップされた時
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + " にドロップされました");

        // ドラッグされてきたオブジェクトの CoreDrag スクリプトを取得
        CoreDrag dragObject = eventData.pointerDrag.GetComponent<CoreDrag>();

        if (dragObject != null)
        {
            // パターンA: スロットが空の場合（そのまま中に入れる）
            if (transform.childCount == 0)
            {
                dragObject.parentReturnTo = this.transform;
            }
            // パターンB: すでに中に別のコアがいる場合（位置を入れ替える！）
            else
            {
                // 今このスロットにいる既存のコア（子要素）を取得
                Transform existingCore = transform.GetChild(0);

                // ドラッグ元（元々コアが置いてあった場所、あるいはTempStorage）の情報を取得
                Transform previousSlot = dragObject.parentReturnTo;

                // 既存のコアを、ドラッグ元のスロットに引っ越しさせる
                existingCore.SetParent(previousSlot);
                existingCore.localPosition = Vector3.zero; // 位置リセット

                // ドラッグしてきたコアの帰り道を、このスロットに変更する
                dragObject.parentReturnTo = this.transform;
            }
        }
    }
}