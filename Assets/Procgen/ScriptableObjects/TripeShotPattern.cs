using UnityEngine;

[CreateAssetMenu(fileName = "TripeShootPattern", menuName = "Shooting Patterns/Triple Shot")]
public class TripeShotPattern : ShootingPattern
{
    [SerializeField] private float spreadAngle = 15f;
    public override void Fire(GameObject bulletPrefab, Transform muzzle, float bulletSpeed)
    {
        for(int i = -1; i <= 1; i++)
        {
            Quaternion spreadRotation = muzzle.rotation * Quaternion.Euler(0, 0, i * spreadAngle);
            GameObject bullet = Instantiate(bulletPrefab, muzzle.position, spreadRotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if(rb != null)
            {
                rb.linearVelocity = (spreadRotation * Vector2.right) * bulletSpeed;
            }
        }
    }
}
