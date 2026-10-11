using UnityEngine;

public class SortingSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private int frontSortingOrder = 2;
    [SerializeField] private int backSortingOrder = 0;
    [SerializeField] private float angleThreshold = 0f;
    private WeaponRotation weaponRotation;
    void Start()
    {
        weaponRotation = GetComponent<WeaponRotation>();
    }

    void Update()
    {
        AdjustSortingOrder();
    }

    void AdjustSortingOrder()
    {
        float rotationZ = transform.rotation.eulerAngles.z;

        if(rotationZ > 180f)
        {
            rotationZ -= 360f;
        }

        if(rotationZ > angleThreshold)
        {
            sprite.sortingOrder = frontSortingOrder;
        }
        else
        {
            sprite.sortingOrder = backSortingOrder;
        }
    }
}
