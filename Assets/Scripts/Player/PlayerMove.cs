using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //変数宣言
    public float moveSpeed;//移動速度
    public float rotateSpeed;//回転速度

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveSpeed = 2.0f;
        rotateSpeed = 75.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.forward * move * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * turn * rotateSpeed * Time.deltaTime);
    }
}
