using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponRotation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    private Vector3 mousePos;
    private Camera mainCam;
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        GunRotation();
    }

    void GunRotation()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        mousePos = mainCam.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, 0f));
        Vector3 rotation = mousePos - transform.position;
        float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        if(rotationZ > 86f || rotationZ < -86f)
        {
            sprite.flipY = true;
        }
        else
        {
            sprite.flipY = false;
        }

        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }
}
