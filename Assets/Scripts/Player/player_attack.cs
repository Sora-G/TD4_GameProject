using UnityEngine;

public class player_attack : MonoBehaviour
{
    //変数宣言
    public Vector3 attackPosition;//攻撃する位置
    public Vector3 attackRotation;//攻撃する方向

    public GameObject bulletPrefab;//弾のプレハブ

    public float attackInterval = 1.0f;//攻撃のインターバル
    private float nextAttackTime = 0.0f;//次に攻撃できる時間

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            attackPosition = transform.position;//攻撃位置をプレイヤーの位置に設定
            attackRotation = transform.eulerAngles;//攻撃方向をプレイヤーの向きに設定
            
            if(Time.time < nextAttackTime) {
                return;
            }

            //弾の生成処理
            GameObject bulletObject = 
                Instantiate(
                    bulletPrefab, 
                    attackPosition, 
                    Quaternion.Euler(attackRotation)
                );

            //プレイヤーの弾タグを付与
            bulletObject.tag = "PlayerBullet";

            //弾のスクリプトを取得
            bullet bulletScript = bulletObject.GetComponent<bullet>();

            //発射者を代入
            bulletScript.owner = gameObject;

            //誰が撃ったか確認
            bulletScript.GetBulletOwner();

            //次に攻撃できる時間を更新
            nextAttackTime = Time.time + attackInterval;
        }
    }
}
