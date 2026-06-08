using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 1キー → 正方形を選んでゲーム開始
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectStageAndLoad(StageType.Square);
        }
        // 2キー → 長方形を選んでゲーム開始
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectStageAndLoad(StageType.Rectangle);
        }
        // 3キー → 円形を選んでゲーム開始
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectStageAndLoad(StageType.Circle);
        }

        //if(Input.anyKeyDown)
        //{
        //    SceneManager.LoadScene("GameScene");
        //    Debug.Log("ゲームシーンに移動");
        //}
    }

    private void SelectStageAndLoad(StageType type)
    {
        // 選択したステージタイプを保持
        StageDataCarrier.SelectedStageType = type;

        // シーン遷移
        

        if(FadeManager.Instance != null)
        {
            FadeManager.Instance.LoadSceneWithFade("GameScene");
        }
        else
        {
            SceneManager.LoadScene("GameScene");
        }
        Debug.Log($"{type} ステージを選択。ゲームシーンに移動します。");
    }
}
