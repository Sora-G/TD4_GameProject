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

    private Rigidbody rb;//マップのブロックを探知する変数

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyStatus = GetComponent<EnemyStatus>();// 同じオブジェクトのEnemyStatusを取得
        player = GameObject.FindGameObjectWithTag("Player");//プレイヤーのオブジェクトをタグから取得
        targetRotation = transform.rotation;//初期の回転を目標の回転に設定
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //巡回する処理
    public void Patrol()
    {

        // 前方にRayを飛ばす
        Ray ray = new Ray(
            transform.position + Vector3.up * 0.5f,
            transform.forward
        );

        RaycastHit hit;

        // 壁を検知
        if (Physics.Raycast(ray, out hit, 1.5f))
        {
            if (hit.collider.CompareTag("HardWall") ||
                hit.collider.CompareTag("BreakWall"))
            {
                // 真後ろを向く
                targetRotation =
                    Quaternion.Euler(
                        0,
                        transform.eulerAngles.y + 180,
                        0
                    );

                // タイマーリセット
                timer = 0.0f;

                return;
            }
        }

        //前進処理
        Vector3 move =
        transform.forward *
        enemyStatus.currentStatus.moveSpeed *
        Time.deltaTime;

        rb.MovePosition(rb.position + move);

        //回転処理
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            enemyStatus.currentStatus.rotateSpeed * Time.deltaTime
            );

        //タイマーを加算
        timer += Time.deltaTime;

        // ランダム方向転換
        if (timer >= changeDirectionTime)
        {
            float randomAngle = Random.Range(0, 360);

            targetRotation =
                Quaternion.Euler(0, randomAngle, 0);

            timer = 0.0f;
        }
    }

    //プレイヤーを発見する処理
    public bool DetectPlayer()
    {
        //プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= enemyStatus.currentStatus.serchRange)
        {
            //Debug.Log("プレイヤーを発見");
            return true;//プレイヤーを発見
        }
        return false;//プレイヤーを発見できなかった
    }
}
