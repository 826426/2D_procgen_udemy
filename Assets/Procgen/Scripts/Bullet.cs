using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;
    private CircleCollider2D bulletCollider;

    void Start()
    {
        Destroy(gameObject, lifeTime);    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Solid")) 
        {
            Destroy(gameObject);
        }
    }
}
