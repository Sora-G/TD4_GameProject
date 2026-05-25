using UnityEngine;

public class EnemySerch : MonoBehaviour
{
    //変数宣言
    private EnemyStatus enemyStatus; // 敵のステータスを入れる変数
    
    public float changeDirectionTime = 3.0f;//方向転換する時間
    private float timer;
    private Quaternion targetRotation;//目標の回転
    
    private GameObject player;//プレイヤーのオブジェクトを入れる変数
    public float serchRange = 10.0f;//プレイヤーを探す範囲

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyStatus = GetComponent<EnemyStatus>();// 同じオブジェクトのEnemyStatusを取得
        player = GameObject.FindGameObjectWithTag("Player");//プレイヤーのオブジェクトをタグから取得
        targetRotation = transform.rotation;//初期の回転を目標の回転に設定
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //巡回する処理
    public void Patrol()
    {
        //前進処理
        transform.position += 
            transform.forward * 
            enemyStatus.currentStatus.moveSpeed * 
            Time.deltaTime;

        //回転処理
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            enemyStatus.currentStatus.rotateSpeed * Time.deltaTime
            );

        //タイマーを加算
        timer += Time.deltaTime;

        //一定時間経過で回転
        if(timer >= changeDirectionTime) {
            //ランダムな角度を生成
            float randomAngle = Random.Range(0, 360);
            //敵を回転させる
            targetRotation = Quaternion.Euler(0, randomAngle, 0);
            //タイマーをリセット
            timer = 0.0f;
        }
    }

    //プレイヤーを発見する処理
    public bool DetectPlayer()
    {
        //プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        
        if(distanceToPlayer <= enemyStatus.currentStatus.serchRange) {
            //Debug.Log("プレイヤーを発見");
            return true;//プレイヤーを発見
        }
        return false;//プレイヤーを発見できなかった
    }
}
