using UnityEngine;
using UnityEngine.UI;

public class HeartSystem : MonoBehaviour
{
    [SerializeField] private const int HEART_ROW_SIZE = 8;
    [SerializeField] private const int HEART_OFFSET = 64;

    [SerializeField] private Sprite[] heartSprites;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private RectTransform heartContainer;
    private float lastHealth = -1f;
    void Start()
    {
        InitializeHearts();
    }

    void Update()
    {
        if(PlayerHealth.Health != lastHealth)
        {
            UpdateHearts();
            lastHealth = PlayerHealth.Health;
        }
    }

    void InitializeHearts()
    {
        foreach(Transform child in heartContainer)
        {
            Destroy(child.gameObject);
        }

        int maxHearts = Mathf.Min(Mathf.CeilToInt(PlayerHealth.Health), PlayerHealth.clampHealth);

        for(int i = 0; i < maxHearts; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartContainer);
            RectTransform heartRect = newHeart.GetComponent<RectTransform>();

            float x = (i % HEART_ROW_SIZE) * HEART_OFFSET;
            float y = (i / HEART_ROW_SIZE) * HEART_OFFSET;
            heartRect.anchoredPosition = new Vector2(x, y);
        }

        UpdateHearts();
    }

    void UpdateHearts()
    {
        int index = 0;
        float currentHealth = PlayerHealth.Health;
        int lastFullHeart = Mathf.FloorToInt(currentHealth);

        foreach(Transform heart in heartContainer)
        {
            Image heartImage = heart.GetComponent<Image>();

            if(index > lastFullHeart)
            {
                heartImage.sprite = heartSprites[0];
            }
            else if(index == lastFullHeart)
            {
                float fractionalHealth = currentHealth - lastFullHeart;
                int frame = Mathf.FloorToInt(fractionalHealth * 4);
                heartImage.sprite = heartSprites[frame];
            }
            else
            {
                heartImage.sprite = heartSprites[4];
            }

            index++;
        }
    }
}
