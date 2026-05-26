using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    public int attack = 1;
    public int defense = 1;
    public float speed = 5f;

    private void OnTriggerEnter(Collider other)
    {
        Core core = other.GetComponent<Core>();

        if (core != null)
        {
            ApplyCore(core.coreType);

            Destroy(other.gameObject);
        }
    }

    void ApplyCore(CoreType type)
    {
        switch (type)
        {
            case CoreType.Attack:

                attack += 1;

                Debug.Log(
                    "ATK UP! åªç›ÇÃçUåÇóÕ : " + attack
                );

                break;

            case CoreType.Defense:

                defense += 1;

                Debug.Log(
                    "DEF UP! åªç›ÇÃñhå‰óÕ : " + defense
                );

                break;

            case CoreType.Speed:

                speed += 1f;

                Debug.Log(
                    "SPD UP! åªç›ÇÃë¨ìx : " + speed
                );

                break;
        }
    }
}