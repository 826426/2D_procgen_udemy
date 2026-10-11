using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth.Health -= 0.25f;
            Destroy(gameObject);
        }
        if (collision.CompareTag("Solid"))
        {
            Destroy (gameObject);
        }
    }
}
