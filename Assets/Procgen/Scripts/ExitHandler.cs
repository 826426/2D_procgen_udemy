using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitHandler : MonoBehaviour
{
    [SerializeField] private Walker walker;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private GameObject exitPrefab;
    [SerializeField] private PlayerHandler playerHandler;
    private GameObject exitInstance;

    void Start()
    {
        StartCoroutine(PlaceExitAfterRegeneration());
    }

    public void RegenerateExit()
    {
        StartCoroutine(PlaceExitAfterRegeneration());
    }

    private IEnumerator PlaceExitAfterRegeneration()
    {
        while (!IsGenerationComplete() || playerHandler == null || playerHandler.GetPlayerInstance() == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        PlaceExit();
    }
    private bool IsGenerationComplete()
    {
        for (int x = walker.GetGridMin().x; x <= walker.GetGridMax().x; x++)
        {
            for (int y = walker.GetGridMin().y; y <= walker.GetGridMax().y; y++)
            {
                if (groundTilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private Vector2Int? GetFarthestFloorPosition(Vector3Int playerPos)
    {
        Vector2Int? farthest = null;
        float maxDistance = -1f;

        for(int x = walker.GetGridMin().x;x <= walker.GetGridMax().x; x++)
        {
            for(int y = walker.GetGridMin().y;y <= walker.GetGridMax().y; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                if (groundTilemap.HasTile(tilePos))
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    float distance = Vector2Int.Distance(new Vector2Int(playerPos.x, playerPos.y), pos);

                    if(distance > maxDistance)
                    {
                        maxDistance = distance;
                        farthest = pos;
                    }
                }
            }
        }
        return farthest;
    }

    private void PlaceExit()
    {
        if(exitInstance != null)
        {
            Destroy(exitInstance);
        }

        GameObject player = playerHandler.GetPlayerInstance();
        Vector3 playerWorldPos = player.transform.position;
        Vector3Int playerTilePos = groundTilemap.WorldToCell(playerWorldPos);

        Vector2Int? farthestPos = GetFarthestFloorPosition(playerTilePos);

        if (farthestPos.HasValue)
        {
            Vector3 worldPos = groundTilemap.CellToWorld(new Vector3Int(farthestPos.Value.x, farthestPos.Value.y, 0));

            worldPos += new Vector3(0.5f, 0.5f, 0);

            exitInstance = Instantiate(exitPrefab, worldPos, Quaternion.identity);

            ExitTrigger exitTrigger = exitInstance.GetComponent<ExitTrigger>();
            if(exitTrigger != null)
            {
                exitTrigger.SetWalker(walker);
            }
        }
    }
}
