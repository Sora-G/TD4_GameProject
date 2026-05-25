using UnityEngine;

public class GameManager : MonoBehaviour
{
    //変数宣言
    public int enemyCount;//敵の数
    public bool isGameOver = false;//ゲームオーバーかどうか

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    //敵が倒されたときの処理
    public void EnemyDefeated()
    {
        enemyCount--;
        if (enemyCount <= 0)
        {
            WinGame();
        }
    }

    //プレイヤーが倒されたときの処理
    public void PlayerDefeated()
    {
        LoseGame();
    }

    //ゲームに勝利したときの処理
    void WinGame()
    {
        isGameOver = true;
        Debug.Log("You Win!!");
    }

    //ゲームに敗北したときの処理
    void LoseGame()
    {
        isGameOver = true;
        Debug.Log("You Lose...");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("敵の数: " + enemyCount);
    }
}
