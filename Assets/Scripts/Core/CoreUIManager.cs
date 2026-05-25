using UnityEngine;

public class CoreUIManager : MonoBehaviour
{
    public GameObject CoreUI;

    bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {



            Debug.Log("TAB");

            isOpen = !isOpen;

            CoreUI.SetActive(isOpen);

            // マウス表示
            Cursor.visible = isOpen;

            // マウス固定解除
            Cursor.lockState =
                isOpen ?
                CursorLockMode.None :
                CursorLockMode.Locked;

            // ゲーム停止
            Time.timeScale = isOpen ? 0 : 1;
        }
    }
}