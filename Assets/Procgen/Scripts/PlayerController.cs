using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D player;
    private SpriteRenderer sprite;
    private Animator anim;
    private PlayerInput playerInput;

    private Vector2 moveInput;
    private InputAction moveAction;

    [SerializeField] private bool IsDead = false;
    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();

        player.gravityScale = 0f;
        player.constraints = RigidbodyConstraints2D.FreezeRotation;

        moveAction = playerInput.actions["Move"];
    }
    void Start()
    {
        
    }

    void Update()
    {
        SetAnim();

        if(PlayerHealth.Health <= 0)
        {
            IsDead = true;
        }
    }

    void FixedUpdate()
    {
        if(PlayerHealth.Health > 0)
        {
            Move();
        }
        else
        {
            player.linearVelocity = Vector2.zero;
        }
    }

    void Move()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        Vector2 velocity = moveInput.normalized * moveSpeed;
        player.linearVelocity = velocity;
    }

    private void SetAnim()
    {
        if (!IsDead)
        {
            if(player.linearVelocity != Vector2.zero)
            {
                anim.SetBool("IsMoving", true);
            }
            if(player.linearVelocity == Vector2.zero)
            {
                anim.SetBool("IsMoving", false);
            }
        }
        else 
        {
            anim.SetBool("IsDead", IsDead);
        }
    }

    private void ResetHealth()
    {
        PlayerHealth.Health = 5.0f;
        IsDead = false;
        RestartLevel();
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
