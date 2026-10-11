using Unity.VisualScripting;
using UnityEngine;

public class EnemyScaler : MonoBehaviour
{
    [SerializeField] private float minScale = 2f;
    [SerializeField] private float maxScale = 3f;

    public float CurrentScale {  get; private set; }
    public float MinScale => minScale;
    public float MaxScale => maxScale;
    void Start()
    {
        CurrentScale = Random.Range(minScale, maxScale);
        transform.localScale = new Vector3(CurrentScale, CurrentScale, 1);
    }
}
