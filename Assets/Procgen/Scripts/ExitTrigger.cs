using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private Walker walker;

    public void SetWalker(Walker walkerReference)
    {
        walker = walkerReference;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(walker != null)
            {
                walker.TriggerRestart();
            }
        }
    }
}
