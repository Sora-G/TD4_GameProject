using UnityEngine;

public class BreakWall : MonoBehaviour
{
    public int hp = 3;

    public void Damage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}