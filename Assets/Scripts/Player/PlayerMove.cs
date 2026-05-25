using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //変数宣言
    private PlayerStatus playerStatus;//プレイヤーのステータスを入れる変数

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();//同じオブジェクトのPlayerStatusを取得
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.forward * move * playerStatus.currentStatus.moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * turn * playerStatus.currentStatus.rotateSpeed * Time.deltaTime);
    }
}
