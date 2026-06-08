using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    // シングルトン（どこからでも FadeManager.Instance で呼べるようにする）
    public static FadeManager Instance { get; private set; }

    [Header("フェード用の画像")]
    public Image fadeImage;

    [Header("フェードにかかる時間（秒）")]
    public float fadeTime = 0.5f;

    private bool isFading = false;

    void Awake()
    {
        // シーンをまたいでもこのオブジェクトが消えないようにする仕組み
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 起動時は最初から画面を真っ暗にしておき、フワッと明るくする
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 1); // 小文字の color に修正済
            StartCoroutine(FadeIn());
        }
    }

    //外部（StageSelectManagerなど）から呼び出す関数
    public void LoadSceneWithFade(string sceneName)
    {
        if (isFading) return; // すでにフェード中なら無視
        StartCoroutine(FadeSequence(sceneName));
    }

    // 暗転して、シーンを切り替えて、明転する一連の流れ
    private IEnumerator FadeSequence(string sceneName)
    {
        isFading = true;

        // 1. 暗転（フェードアウト）
        yield return StartCoroutine(FadeOut());

        // 2. シーン切り替え
        yield return SceneManager.LoadSceneAsync(sceneName);

        // 3. 明転（フェードイン）
        yield return StartCoroutine(FadeIn());

        isFading = false;
    }

    // 徐々に暗くする（透明 0 -> 不透明 1）
    private IEnumerator FadeOut()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            float time = 0;

            while (time < fadeTime)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Clamp01(time / fadeTime);
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }
    }

    // 徐々に明るくする（不透明 1 -> 透明 0）
    private IEnumerator FadeIn()
    {
        if (fadeImage != null)
        {
            float time = 0;

            while (time < fadeTime)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Clamp01(1 - (time / fadeTime));
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }

            fadeImage.gameObject.SetActive(false);
        }
    }
}