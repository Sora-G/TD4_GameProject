using UnityEngine;

public class player_attack : MonoBehaviour
{
    //変数宣言
    public Vector3 attackPosition;//攻撃する位置
    public Vector3 attackRotation;//攻撃する方向

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            attackPosition = transform.position;
            attackRotation = transform.eulerAngles;
            Debug.Log("攻撃");
            Debug.Log("攻撃位置:"+attackPosition);
            Debug.Log("攻撃方向:"+attackRotation);
        }
    }
}
