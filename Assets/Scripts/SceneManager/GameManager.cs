using UnityEngine;

public class GameManager : MonoBehaviour
{
    //変数宣言
    public int enemyCount;//敵の数
    public bool isGameOver = false;//ゲームオーバーかどうか
    public GameClearManager gameClearManager;//ゲームクリアマネージャーの参照
    public GameOverManager gameOverManager;//ゲームオーバーマネージャーの参照

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
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("You Win!!");

        if (gameClearManager != null)
        {
            gameClearManager.ChangeGameClear();
        }
        else
        {
            Debug.LogError("GameClearManager が GameManager にセットされていません！");
        }
    }

    //ゲームに敗北したときの処理
    void LoseGame()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("You Lose...");

        if (gameClearManager != null)
        {
            gameClearManager.ChangeGameClear();
        }
        else
        {
            Debug.LogError("GameOverManager が GameManager にセットされていません！");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("敵の数: " + enemyCount);
    }
}
