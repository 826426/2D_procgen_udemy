using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [SerializeField] private float amplitudeX = 2f;
    [SerializeField] private float amplitudeY = 1f;
    [SerializeField] private float frequencyX = 1f;
    [SerializeField] private float frequencyY = 0.5f;
    [SerializeField] private float speed = 2f;

    private Vector2 startPosition;
    private float timeOffset;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    private void FixedUpdate()
    {
        float xOffset = Mathf.Sin(Time.fixedTime * frequencyX + timeOffset) * amplitudeX;
        float yOffset = Mathf.Cos(Time.fixedTime * frequencyY + timeOffset) * amplitudeY;
        Vector2 offset = new Vector2(xOffset, yOffset);

        Vector2 parentPosition = transform.parent != null ? (Vector2)transform.parent.position : Vector2.zero;
        Vector2 targetPosition = startPosition + offset + parentPosition;

        rb.MovePosition(targetPosition);
    }

    public void SetStartPosition(Vector2 position)
    {
        startPosition = position;
        rb.position = position;
    }
}
