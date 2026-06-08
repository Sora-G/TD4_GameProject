using UnityEngine;

public class DamageFloor : MonoBehaviour
{
    public int damage = 10;
    public float damageInterval = 1.0f;

    private float nextDamageTime = 0f;

    private void OnTriggerStay(Collider other)
    {
        // 相手が「Player」のタグを持っているかチェック
        if (other.CompareTag("Player"))
        {
            // 現在の時間が、次のダメージ発生時間を超えているかチェック
            if (Time.time >= nextDamageTime)
            {
                PlayerStatus player = other.GetComponent<PlayerStatus>();

                if (player != null)
                {
                    // プレイヤーにダメージを与える
                    player.TakeDamage(damage);

                    // 次のダメージ発生時間を設定（現在の時間 + インターバル秒）
                    nextDamageTime = Time.time + damageInterval;

                    Debug.Log($"ダメージ床によるダメージ！ 残りHP: {player.currentStatus.hp}");
                }
            }
        }
    }

    //プレイヤーが床から出たらタイマーをリセットする
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nextDamageTime = 0f;
        }
    }
}