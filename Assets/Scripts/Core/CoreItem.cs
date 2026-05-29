using UnityEngine;

public class CoreItem : MonoBehaviour
{
    // どのコアUIを表示するかを識別するためのIDや種類
    public string coreType = "ATK";

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーが触れたら
        if (other.CompareTag("Player"))
        {
            // UIManagerにコアの種類を渡してUI表示を依頼する
            CoreUIManager.Instance.AcquireCore(coreType);

            // フィールド上のコア自体は消去する
            Destroy(gameObject);
        }
    }
}