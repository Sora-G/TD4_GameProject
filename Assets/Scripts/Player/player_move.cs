using UnityEngine;

public class player_move : MonoBehaviour
{
    //変数宣言
    public float moveSpeed;//移動速度

    // 操作反転フラグ
    public bool isReverse = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveSpeed = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");

        // =====================
        // 操作反転
        // =====================
        if (isReverse)
        {
            move *= -1;
            turn *= -1;
        }

        transform.Translate(Vector3.forward * move * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * turn * 100f * Time.deltaTime);
    }
}
