using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private GameObject player;//プレイヤーのオブジェクトを入れる変数
    public GameObject bulletPrefab;//弾のプレハブ
    public Transform firePoint;//弾を発射する位置
    public float attackInterval = 1.0f;//攻撃のインターバル
    private float nextAttackTime = 0.0f;//次に攻撃できる時間
    private EnemyStatus enemyStatus; // 敵のステータスを入れる変数

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("PlayerTank");//プレイヤーのオブジェクトを名前から取得
        enemyStatus = GetComponent<EnemyStatus>();// 同じオブジェクトのEnemyStatusを取得
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        //まだ撃てない
        if(Time.time < nextAttackTime) {
            return;
        }

        //Player方向取得
        Vector3 direction = player.transform.position - transform.position;

        //Y軸を無視
        direction.y = 0;

        //Player方向を見る
        transform.rotation = Quaternion.LookRotation(direction);

        //弾の生成処理
        GameObject bulletObject = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        //敵の弾タグを付与
        bulletObject.tag = "EnemyBullet";

        //弾のスクリプトを取得
        BulletController bulletScript =
            bulletObject.GetComponent<BulletController>();

        //発射者を代入
        bulletScript.owner = gameObject;

        //誰が撃ったか確認
        bulletScript.GetBulletOwner();

        //次に攻撃できる時間を更新
        nextAttackTime = Time.time + attackInterval;

        Debug.Log("敵が攻撃");
    }
}
