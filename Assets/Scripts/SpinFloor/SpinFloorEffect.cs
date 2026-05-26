using System.Collections;
using UnityEngine;

public class SpinFloorEffect : MonoBehaviour
{
    // ‰½•bŒã‚É‹t‘€ì‚É‚È‚é‚©
    [SerializeField] private float reverseStartTime = 0.5f;

    // ‹t‘€ì‚ª‘±‚­ŠÔ
    [SerializeField] private float reverseDuration = 2.0f;

    private Coroutine reverseCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player_move player =
                other.GetComponent<player_move>();

            if (player != null)
            {
                // ‘O‚ÌCoroutine‚ğ~‚ß‚é
                if (reverseCoroutine != null)
                {
                    StopCoroutine(reverseCoroutine);
                }

                reverseCoroutine =
                    StartCoroutine(ReverseControl(player));
            }
        }
    }

    private IEnumerator ReverseControl(player_move player)
    {
        // æ‚Á‚Ä‚©‚ç­‚µ‘Ò‚Â
        yield return new WaitForSeconds(reverseStartTime);

        // « ‚±‚±‚Å°‚©‚ç~‚è‚Ä‚¢‚Ä‚à”½“]‚·‚é
        player.isReverse = true;

        // ˆê’èŠÔ”½“]
        yield return new WaitForSeconds(reverseDuration);

        // Œ³‚É–ß‚·
        player.isReverse = false;

        reverseCoroutine = null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ‰½‚à‚µ‚È‚¢
            // ~‚è‚Ä‚à”½“]Œø‰Ê‚ÍŒp‘±‚·‚é
        }
    }
}