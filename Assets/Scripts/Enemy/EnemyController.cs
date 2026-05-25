using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // 敵の状態を管理する列挙型
    public enum EnemyState
    {
        Serch,      // プレイヤーを探す状態
        Chase,      // プレイヤーを追いかける状態
        Attack,     // プレイヤーを攻撃する状態
    }

    //変数宣言
    public EnemyState currentState; // 現在の状態を保持する変数(初期は探索状態)
    private EnemyStatus enemyStatus; // 敵のステータスを入れる変数
    private EnemySerch enemySerch; // 敵の探索処理を入れる変数
    private EnemyChase enemyChase; // 敵の追跡処理を入れる変数
    private EnemyAttack enemyAttack; // 敵の攻撃処理を入れる変数

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyStatus = GetComponent<EnemyStatus>();// 同じオブジェクトのEnemyStatusを取得
        enemySerch = GetComponent<EnemySerch>();// 同じオブジェクトのEnemySerchを取得
        enemyChase = GetComponent<EnemyChase>();// 同じオブジェクトのEnemyChaseを取得
        enemyAttack = GetComponent<EnemyAttack>();// 同じオブジェクトのEnemyAttackを取得
        currentState = EnemyState.Serch;// 初期状態を探索状態に設定
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState) {
            case EnemyState.Serch:
                // プレイヤーを探す処理
                enemySerch.Patrol();// プレイヤーを探す処理を呼び出す
                if(enemySerch.DetectPlayer()) {
                    currentState = EnemyState.Chase;// プレイヤーを発見したら追いかける状態に遷移
                    //Debug.Log("探索→追跡");
                }
                //Debug.Log("探索中");
                break;

            case EnemyState.Chase:
                // プレイヤーを追いかける処理
                enemyChase.ChasePlayer();// プレイヤーを追いかける処理を呼び出す
                if(enemyChase.LostPlayer()) {
                    currentState = EnemyState.Serch;// プレイヤーを見失ったら探索状態に遷移
                    //Debug.Log("追跡→探索");
                }
                if (enemyChase.AttackPlayer()){
                    currentState = EnemyState.Attack;// プレイヤーを攻撃する距離に入ったら攻撃状態に遷移
                    //Debug.Log("追跡→攻撃");
                }
                //Debug.Log("追跡中");
                break;

            case EnemyState.Attack:
                // プレイヤーを攻撃する処理
                enemyAttack.Attack();// プレイヤーを攻撃する処理を呼び出す
                //Debug.Log("攻撃中");
                break;
        }
    }
}
