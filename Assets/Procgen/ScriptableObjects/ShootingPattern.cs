using UnityEngine;

public abstract class ShootingPattern : ScriptableObject
{
    public abstract void Fire(GameObject bulletPrefab, Transform muzzle, float bulletSpeed);
}
