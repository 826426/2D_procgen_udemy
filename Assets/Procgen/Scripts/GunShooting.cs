using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunShooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunMuzzle;
    [SerializeField] private ShootingPattern shootingPattern;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float fireRate;
    [SerializeField] private SpriteRenderer gunSprite;
    [SerializeField] private FlipController flip;
    [SerializeField] private CinemachineImpulseSource camImpulse;

    private PlayerInput gunInput;
    private InputAction shootAction;
    private InputAction gunAction;
    private float shootTimer = 0f;
    private bool isVisible = false;
    private bool isShooting = false;

    private void Awake()
    {
        gunInput = GetComponent<PlayerInput>();
        shootAction = gunInput.actions["Shoot"];
        gunAction = gunInput.actions["GunKey"];

        camImpulse = GetComponent<CinemachineImpulseSource>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if(shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
        Shooting();
        GunVisible();
    }

    void GunVisible()
    {
        if (gunAction.WasPerformedThisFrame())
        {
            isVisible = !isVisible;

            gunSprite.enabled = isVisible;
            flip.useMouseForFlipping = isVisible;
        }
    }

    void Shooting()
    {
        if (isVisible && Ammunition.ammoCount > 0)
        {
            if (shootAction.IsPressed() && shootTimer <= 0)
            {
                isShooting = true;
                Ammunition.ammoCount -= 1;
                shootTimer = fireRate;
                Shoot();
            }
            else if(shootAction.WasReleasedThisFrame())
            {
                isShooting = false;
            }
        }
        else
        {
            return;
        }
    }

    void Shoot()
    {
        if(shootingPattern != null)
        {
            shootingPattern.Fire(bulletPrefab, gunMuzzle, bulletSpeed);
        }
        ScreenShake();
    }

    void ScreenShake()
    {
        if(camImpulse != null)
        {
            camImpulse.GenerateImpulse();
        }
    }
}
