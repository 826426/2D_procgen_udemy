using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int baseBulletCount = 1;
    [SerializeField] private int maxBulletCount = 5;
    [SerializeField] private float bulletSpreadAngle = 2f;

    private Transform player;
    private Rigidbody2D playerRb;
    private float nextFireTime;
    private EnemyScaler scaler;
    private bool isPlayerDetected;
    private bool wasPlayerDetectedLastFrame;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scaler = GetComponent<EnemyScaler>();
        nextFireTime = Time.time;
        if(fireRate <= 0.1f)
        {
            fireRate = 0.5f;
        }
        if(bulletPrefab == null)
        {
            enabled = false;
        }
        if(detectionRadius <= 0)
        {
            detectionRadius = 5f;
        }
        FindPlayer();
    }
    void Update()
    {
        if(player == null)
        {
            FindPlayer();
            isPlayerDetected = false;
            return;
        }

        PlayerDetection();
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if(playerObj != null)
        {
            player = playerObj.transform;
            playerRb = playerObj.GetComponent<Rigidbody2D>();
        }
    }

    void Shoot()
    {
        if(player == null || bulletPrefab == null) return;

        float scaleFactor = scaler != null ? scaler.CurrentScale : 1f;
        int bulletCount = Mathf.RoundToInt(Mathf.Lerp(baseBulletCount, maxBulletCount, scaler != null ? (scaleFactor - scaler.MinScale) / (scaler.MaxScale - scaler.MinScale) : 0f));

        Vector2 playerPos = playerRb != null ? playerRb.position : (Vector2)player.position;
        Vector2 direction = (playerPos - rb.position).normalized;

        if (bulletCount == 1)
        {
            SpawnBullet(direction);
        }
        else
        {
            float startAngle = -bulletSpreadAngle * (bulletCount - 1) / 2f;
            for(int i = 0; i < bulletCount; i++)
            {
                float angle = startAngle + i * bulletSpreadAngle;
                Vector2 spreadDirection = Quaternion.Euler(0, 0, angle) * direction;
                SpawnBullet(spreadDirection);
            }
        }
    }

    void SpawnBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, rb.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if(bulletRb != null)
        {
            bulletRb.linearVelocity = direction * bulletSpeed;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void PlayerDetection()
    {
        float distanceToPlayer = Vector2.Distance(rb.position, playerRb != null ? playerRb.position : (Vector2)player.position);
        isPlayerDetected = distanceToPlayer <= detectionRadius;

        if(isPlayerDetected && !wasPlayerDetectedLastFrame)
        {
            nextFireTime = Time.time;
        }
        if(isPlayerDetected && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }

        wasPlayerDetectedLastFrame = isPlayerDetected;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
