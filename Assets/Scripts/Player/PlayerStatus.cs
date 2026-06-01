using UnityEngine;
using UnityEngine.UI;

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
    public UnityEngine.UI.Slider hpSlider;//HPを表示するスライダー
    public UnityEngine.UI.Slider lifeSlider;//残機を表示するスライダー

    //プレイヤーが攻撃を受ける処理
    public void TakeDamage(int attackPower)
    {
        if(currentStatus.hp < 1) return;//HPが0未満のときはダメージを受けない
        currentStatus.hp -= attackPower;//ダメージを受ける処理

        UpdateHPSlider();//HPスライダーを更新する処理

        Debug.Log("PlayerのHP:" + currentStatus.hp + "Playerの残機:" + currentStatus.lives);
        //HPが0未満になったときの処理
        if (currentStatus.hp < 1)
        {
            if(currentStatus.lives >= 1)
            {
                currentStatus.lives--;//残機を減らす処理
                currentStatus.hp = 100;//体力を戻す処理

                UpdateHPSlider();//HPスライダーを更新する処理
                UpdateLifeSlider();//残機スライダーを更新する処理
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

    //残機スライダーを更新する処理
    private void UpdateHPSlider()
    {
        if (hpSlider != null)
        {
            hpSlider.value = currentStatus.hp;//HPスライダーの値を更新する処理
        }
    }

    //残機スライダーを更新する処理
    private void UpdateLifeSlider()
    {
        if (lifeSlider != null)
        {
            lifeSlider.value = currentStatus.lives;//残機スライダーの値を更新する処理
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStatus.lives = 3;//残機を設定
        currentStatus.hp = 100;//HPを設定
        currentStatus.attack = 10;//攻撃力を設定
        currentStatus.moveSpeed = 2.0f;//移動速度を設定
        currentStatus.rotateSpeed = 75.0f;//回転速度を設定

        // HPスライダーがまだ割り当てられていない場合、シーン内から探す
        if (hpSlider == null)
        {
            GameObject hpBarObj = GameObject.Find("PlayerHPBar");
            if (hpBarObj != null)
            {
                hpSlider = hpBarObj.GetComponent<UnityEngine.UI.Slider>();
            }
        }

        // 残機スライダーがまだ割り当てられていない場合、シーン内から探す
        if (lifeSlider == null)
        {
            GameObject lifeBarObj = GameObject.Find("PlayerLifeBar");
            if (lifeBarObj != null) 
            {
                lifeSlider = lifeBarObj.GetComponent<UnityEngine.UI.Slider>();
            }
        }

        // HPスライダーが見つかった場合、最大値を設定して現在のHPを反映
        if (hpSlider != null)
        {
            hpSlider.maxValue = currentStatus.hp; // 最大値を100にする
            UpdateHPSlider();                    // 現在のHP（100）をバーに反映
        }

        // 残機スライダーが見つかった場合、最大値を設定して現在の残機を反映
        if (lifeSlider != null)
        {
            lifeSlider.maxValue = currentStatus.lives; // 最大値を3にする
            UpdateLifeSlider();                       // 現在の残機（3）をバーに反映
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHPSlider();
    }
}
