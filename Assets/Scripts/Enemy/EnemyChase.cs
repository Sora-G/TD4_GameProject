using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    //変数宣言
    private GameObject player;//プレイヤーのオブジェクトを入れる変数
    private EnemyStatus enemyStatus; // 敵のステータスを入れる変数

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");//プレイヤーのオブジェクトをタグから取得
        enemyStatus = GetComponent<EnemyStatus>();// 同じオブジェクトのEnemyStatusを取得
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChasePlayer()
    {
        //プレイヤーの方向を計算
        Vector3 direction = player.transform.position - transform.position;
        
        //Y軸を無視
        direction.y = 0;
        
        //Player方向を見る
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.LookRotation(direction),
            enemyStatus.currentStatus.rotateSpeed * Time.deltaTime
        );

        //前進処理
        transform.position +=
            transform.forward *
            enemyStatus.currentStatus.moveSpeed *
            Time.deltaTime;
    }

    public bool LostPlayer()
    {
        //プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        //見失う距離
        if(distanceToPlayer >= enemyStatus.currentStatus.lostRange) {
            return true;
        }
        return false;
    }

    public bool AttackPlayer()
    {
        //プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        //攻撃する距離
        if(distanceToPlayer <= enemyStatus.currentStatus.attackRange) {
            return true;
        }
        return false;
    }
}
