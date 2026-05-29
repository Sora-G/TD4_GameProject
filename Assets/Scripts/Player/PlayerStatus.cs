using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    //敵のステータスを管理する構造体
    public struct Status
    {
        public int lives;
        public int hp;
        public int attack;
        public float moveSpeed;
        public float rotateSpeed;
    }

    //変数宣言
    public Status currentStatus;//プレイヤーのステータスを管理する変数

    //プレイヤーが攻撃を受ける処理
    public void TakeDamage(int attackPower)
    {
        if(currentStatus.hp < 1) return;//HPが0未満のときはダメージを受けない
        currentStatus.hp -= attackPower;//ダメージを受ける処理
        Debug.Log("PlayerのHP:" + currentStatus.hp + "Playerの残機:" + currentStatus.lives);
        //HPが0未満になったときの処理
        if (currentStatus.hp < 1)
        {
            if(currentStatus.lives >= 1)
            {
                currentStatus.lives--;
                currentStatus.hp = 100;
            }
            else
            {
                Die();
            }
        }
    }

    public void Die()
    {
        //プレイヤーが倒されたときの処理
        GameManager gm = FindFirstObjectByType<GameManager>();

        if (gm != null)
        {
            gm.PlayerDefeated();//ゲームマネージャーにプレイヤーが倒されたことを伝える
        }
        //Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStatus.lives = 3;//残機を設定
        currentStatus.hp = 100;//HPを設定
        currentStatus.attack = 10;//攻撃力を設定
        currentStatus.moveSpeed = 2.0f;//移動速度を設定
        currentStatus.rotateSpeed = 75.0f;//回転速度を設定
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
