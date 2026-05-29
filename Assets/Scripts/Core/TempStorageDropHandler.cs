using UnityEngine;
using UnityEngine.EventSystems;

public class TempStorageDropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // 1. ドラッグされてきたオブジェクト（アイコンなど）を取得
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject != null)
        {
            // 2. もし装備エリア（EquipArea）など、ストック（TempStorage）の外から来たものかチェック
            // (すでにTempStorageの中にあるマスのドラッグなら無視する)
            if (!droppedObject.transform.IsChildOf(this.transform))
            {
                // 3. 落ちてきたアイコンの名前や種類からコアのタイプ（ATK, DEF, SPD）を判定
                string coreType = "ATK"; // デフォルト
                if (droppedObject.name.Contains("DEF")) coreType = "DEF";
                else if (droppedObject.name.Contains("SPD")) coreType = "SPD";

                Debug.Log($"装備からストックに戻されました: {coreType}");

                // 4. UIManagerの機能を使って、新しくストックマスを作って追加する
                if (CoreUIManager.Instance != null)
                {
                    CoreUIManager.Instance.AcquireCore(coreType);
                }

                // 5. 元々装備エリアにあった古いアイコン（ドラッグ元）を削除する
                // (AcquireCore側で新しく生成するため、古いものは消してOK)
                Destroy(droppedObject);
            }
        }
    }
}