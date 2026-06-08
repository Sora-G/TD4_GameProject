using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour
{
    public void ChangeGameClear()
    {
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.LoadSceneWithFade("GameClearScene");
        }
        else
        {
            SceneManager.LoadScene("GameClearScene");
        }
        Debug.Log("ゲームクリアシーンに移動");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //任意のキーが押されたときの処理
        if (Input.anyKeyDown)
        {
            //現在のシーン名を取得
            string currentSceneName = SceneManager.GetActiveScene().name;

            //現在のシーンがゲームクリアシーンの場合、タイトルシーンに移動
            if (currentSceneName == "GameClearScene")
            {
                //タイトルシーンに移動
                if (FadeManager.Instance != null)
                {
                    FadeManager.Instance.LoadSceneWithFade("TitleScene");
                }
                else
                {
                    SceneManager.LoadScene("TitleScene");
                }
                Debug.Log("タイトルシーンに移動");
            }
        }
    }
}
