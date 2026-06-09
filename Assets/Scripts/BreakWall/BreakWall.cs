using UnityEngine;

public class BreakWall : MonoBehaviour
{
    public int hp = 3;
    public AudioClip breakSE; // 壁が壊れるときのSE

    public void Damage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            if (breakSE != null)
            {
                AudioSource.PlayClipAtPoint(breakSE, transform.position, 0.9f);
            }
            Destroy(gameObject);
        }
    }
}