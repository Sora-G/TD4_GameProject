using UnityEngine;

public class SpinFloorEffect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player_move player =
                other.GetComponent<player_move>();

            if (player != null)
            {
                player.isReverse = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player_move player =
                other.GetComponent<player_move>();

            if (player != null)
            {
                player.isReverse = false;
            }
        }
    }
}