using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private GameObject player;//プレイヤーのオブジェクトを入れる変数
    public GameObject bulletPrefab;//弾のプレハブ
    public Transform firePoint;//弾を発射する位置
    public float attackInterval = 1.0f;//攻撃のインターバル
    private float nextAttackTime = 0.0f;//次に攻撃できる時間
    private EnemyStatus enemyStatus; // 敵のステータスを入れる変数
    public AudioClip shotSE; // 撃つときのSE

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //同じオブジェクトのEnemyStatusを取得
        enemyStatus = GetComponent<EnemyStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        // player が消えてたら再取得
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");

            // それでもいなければ探索状態に戻る
            if (player == null)
            {
                EnemyController enemyController = GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.currentState = EnemyController.EnemyState.Serch;
                }
                return;
            }
        }

        // Player方向取得
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;

        float distance = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        //Playerの方向を向くための回転を計算
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        //回転処理
        transform.rotation =
            Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                enemyStatus.currentStatus.rotateSpeed * Time.deltaTime
            );

        //距離を保つ処理
        if (distance > enemyStatus.currentStatus.keepDistance)
        {
            transform.position +=
                transform.forward *
                enemyStatus.currentStatus.moveSpeed *
                Time.deltaTime;
        }
        else if (distance < enemyStatus.currentStatus.keepDistance - 2f)
        {
            transform.position -=
                transform.forward *
                enemyStatus.currentStatus.moveSpeed *
                Time.deltaTime;
        }

        if (Time.time < nextAttackTime)
        {
            return;
        }

        Fire();

        //攻撃のクールタイム
        nextAttackTime = Time.time + attackInterval;
    }

    //攻撃処理
    private void Fire()
    {
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

        if(shotSE != null)
        {
            AudioSource.PlayClipAtPoint(shotSE, transform.position, 0.9f);
        }

        //Debug.Log("敵が攻撃");
    }

}
