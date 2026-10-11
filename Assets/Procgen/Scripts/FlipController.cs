using UnityEngine;
using UnityEngine.InputSystem;

public class FlipController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Rigidbody2D entity;

    [Header("Flipping References")]
    public bool useMouseForFlipping = false;
    [SerializeField] private bool flipX = true;
    private bool isFacingRight = true;
    void Start()
    {
        
    }

    void Update()
    {
        if (useMouseForFlipping)
        {
            FlipBasedOnMouse();
        }
        else
        {
            FlipBasedOnInput();
        }
    }

    private void FlipBasedOnInput()
    {
        if(Mathf.Abs(entity.linearVelocityX) > 0.1f)
        {
            bool shouldFaceRight = entity.linearVelocityX > 0;
            if(shouldFaceRight != isFacingRight)
            {
                FlipSprite(shouldFaceRight);
            }
        }
    }

    private void FlipBasedOnMouse()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        bool shouldFaceRight = mouseWorldPos.x > transform.position.x;
        if (shouldFaceRight != isFacingRight)
        {
            FlipSprite(shouldFaceRight);
        }
    }

    private void FlipSprite(bool faceRight)
    {
        isFacingRight = faceRight;
        if (flipX)
        {
            sprite.flipX = !isFacingRight;
        }
        else
        {
            //sprite.flipY = isFacingRight;
            transform.rotation = Quaternion.Euler(0, faceRight ? 0 : 180, 0);
        }
    }

    public void SetFlippingMode(bool useMouse)
    {
        useMouseForFlipping = useMouse;
    }
}
