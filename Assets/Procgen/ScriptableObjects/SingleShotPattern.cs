using UnityEngine;

[CreateAssetMenu(fileName = "SingleShotPattern", menuName = "Shooting Patterns/Single Shot")]
public class SingleShotPattern : ShootingPattern
{
    public override void Fire(GameObject bulletPrefab, Transform muzzle, float bulletSpeed)
    {
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = muzzle.right * bulletSpeed;
        }
    }
}
