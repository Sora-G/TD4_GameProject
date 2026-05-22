using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    //敵のステータスを管理する構造体
    public struct Status 
    {         
        public int hp;
        public int attack;
        public float moveSpeed;
        public float rotateSpeed;
        public float serchRange;
        public float lostRange;
        public float attackRange;
        public float keepDistance;
    }

    //敵の種類を管理する列挙型
    public enum EnemyType
    {
        Normal,
        Fast,
        Hard,
        Aggressive
    }

    //変数宣言
    public EnemyType currentType;//敵の種類を管理する変数
    public Status currentStatus;//敵のステータスを管理する変数

    //敵の種類に応じてステータスを設定する関数
    void SetStaus(EnemyType type) 
    {
        switch (type) { 
            case EnemyType.Normal://通常の敵のステータスを設定

                currentStatus.hp = 100;//HPを設定
                currentStatus.attack = 10;//攻撃力を設定
                currentStatus.moveSpeed = 2.0f;//移動速度を設定
                currentStatus.rotateSpeed = 90.0f;//回転速度を設定
                currentStatus.serchRange = 10.0f;//索敵範囲を設定
                currentStatus.lostRange = 15.0f;//索敵を失う範囲を設定
                currentStatus.attackRange = 14.0f;//攻撃範囲を設定
                currentStatus.keepDistance = 8.0f;//距離を保つ範囲を設定

                break;

            case EnemyType.Fast://速い敵のステータスを設定

                currentStatus.hp = 50;
                currentStatus.attack = 5;
                currentStatus.moveSpeed = 6.0f;
                currentStatus.rotateSpeed = 180.0f;
                currentStatus.serchRange = 10.0f;
                currentStatus.lostRange = 15.0f;
                currentStatus.attackRange = 14.0f;
                currentStatus.keepDistance = 8.0f;

                break;

            case EnemyType.Hard://硬い敵のステータスを設定

                currentStatus.hp = 200;
                currentStatus.attack = 20;
                currentStatus.moveSpeed = 1.0f;
                currentStatus.rotateSpeed = 45.0f;
                currentStatus.serchRange = 10.0f;
                currentStatus.lostRange = 15.0f;
                currentStatus.attackRange = 14.0f;
                currentStatus.keepDistance = 8.0f;

                break;

            case EnemyType.Aggressive://攻撃的な敵のステータスを設定

                currentStatus.hp = 150;
                currentStatus.attack = 15;
                currentStatus.moveSpeed = 4.0f;
                currentStatus.rotateSpeed = 120.0f;
                currentStatus.serchRange = 15.0f;
                currentStatus.lostRange = 20.0f;
                currentStatus.attackRange = 19.0f;
                currentStatus.keepDistance = 8.0f;

                break;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetStaus(currentType);//敵の種類に応じてステータスを設定
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
