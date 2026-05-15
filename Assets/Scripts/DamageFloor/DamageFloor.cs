using UnityEngine;

public class DamageFloor : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ダメージ！");
        }
    }
}