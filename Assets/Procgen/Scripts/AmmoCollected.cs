using UnityEngine;

public class AmmoCollected : MonoBehaviour
{
    [SerializeField] private int ammoAmount = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Ammunition.ammoCount += ammoAmount;
            Destroy(gameObject);
        }
    }
}
