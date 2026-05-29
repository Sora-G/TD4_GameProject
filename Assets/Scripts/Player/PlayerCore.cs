using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    public int attack = 1;
    public int defense = 1;
    public float speed = 5f;

    private void OnTriggerEnter(Collider other)
    {
        // 触れたオブジェクトから Core スクリプトを取得
        Core core = other.GetComponent<Core>();

        if (core != null)
        {
            // --- 修正ポイント ---
            // coreType (enum) を文字列（"ATK", "DEF", "SPD"）に変換してUIマネージャーに送る
            string typeStr = "";
            switch (core.coreType)
            {
                case CoreType.Attack: typeStr = "ATK"; break;
                case CoreType.Defense: typeStr = "DEF"; break;
                case CoreType.Speed: typeStr = "SPD"; break;
            }

            // UIマネージャーに通知して、インベントリ内にアイコンを表示させる！
            if (CoreUIManager.Instance != null)
            {
                CoreUIManager.Instance.AcquireCore(typeStr);
            }

            // コアを消去する
            Destroy(other.gameObject);
        }
    }

    // ★このステータスを上げる関数は、後で「装備エリアにドロップした瞬間」に呼び出すようにします
    public void ApplyCore(string type)
    {
        switch (type)
        {
            case "ATK":
                attack += 1;
                Debug.Log("装備完了！現在の攻撃力 : " + attack);
                break;
            case "DEF":
                defense += 1;
                Debug.Log("装備完了！現在の防御力 : " + defense);
                break;
            case "SPD":
                speed += 1f;
                Debug.Log("装備完了！現在の速度 : " + speed);
                break;
        }
    }
}