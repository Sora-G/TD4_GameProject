using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void ChangeGameOver()
    {
        SceneManager.LoadScene("GameOverScene");
        Debug.Log("ゲームオーバーシーンに移動");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //何かキーが押されたときの処理
        if (Input.anyKeyDown)
        {
            //現在のシーン名を取得
            string currentSceneName = SceneManager.GetActiveScene().name;

            //現在のシーンがゲームオーバーシーンの場合、タイトルシーンに移動
            if (currentSceneName == "GameOverScene")
            {
                //タイトルシーンに移動
                SceneManager.LoadScene("TitleScene");
                Debug.Log("タイトルシーンに移動");
            }
        }
    }
}
