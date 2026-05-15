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
            Debug.Log("Playerが発射した");
        }

        if (owner.CompareTag("NormalEnemy"))
        {
            Debug.Log("Enemyが発射した");
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
