using UnityEngine;

public class BulletController : MonoBehaviour
{
    //変数宣言
    public GameObject owner;//弾の発射元を特定するための変数
    public float moveSpeed;//弾の移動速度

    //弾の発射元を特定するための関数
    public void GetBulletOwner()
    {
        if (owner.CompareTag("Player"))
        {
            //Debug.Log("Playerが発射した");
        }

        if (owner.CompareTag("Enemy"))
        {
            //Debug.Log("Enemyが発射した");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //持ち主が消えたら消える
        if (owner == null)
        {
            Destroy(gameObject);
            return;
        }

        //弾の発射元がPlayerで、当たったオブジェクトがEnemyの場合
        if (owner.CompareTag("Player") && other.CompareTag("Enemy"))
        {
            EnemyStatus enemy = other.GetComponent<EnemyStatus>();//当たったオブジェクトのEnemyStatusを取得

            if(enemy != null)
            {
                PlayerStatus player = owner.GetComponent<PlayerStatus>();//弾の発射元のPlayerStatusを取得
                enemy.TakeDamage(player.currentStatus.attack);//EnemyのTakeDamage関数を呼び出してダメージを与える
            }

            //Debug.Log("Playerの弾がEnemyに当たった");
            Destroy(gameObject);//弾を消す
        }
        //弾の発射元がEnemyで、当たったオブジェクトがPlayerの場合
        else if (owner.CompareTag("Enemy") && other.CompareTag("Player"))
        {
            PlayerStatus player = other.GetComponent<PlayerStatus>();//当たったオブジェクトのPlayerStatusを取得

            if (player != null)
            {
                EnemyStatus enemy = owner.GetComponent<EnemyStatus>();//弾の発射元のEnemyStatusを取得
                player.TakeDamage(enemy.currentStatus.attack);//PlayerのTakeDamage関数を呼び出してダメージを与える
            }

            //Debug.Log("Enemyの弾がPlayerに当たった");
            Destroy(gameObject);//弾を消す
        }
        //弾の発射元がPlayerで、当たったオブジェクトがHardWallの場合
        else if (owner.CompareTag("Player") && other.CompareTag("HardWall"))
        {
            //Debug.Log("Enemyの弾がPlayerに当たった");
            Destroy(gameObject);//弾を消す
        }
        //弾の発射元がPlayerで、当たったオブジェクトがHardWallの場合
        else if (owner.CompareTag("Enemy") && other.CompareTag("HardWall"))
        {
            //Debug.Log("Enemyの弾がPlayerに当たった");
            Destroy(gameObject);//弾を消す
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveSpeed = 15.0f;//弾の移動速度を設定

        Destroy(gameObject, 3.0f);//３秒後に弾が消える
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);//弾を前方に移動させる
    }
}
