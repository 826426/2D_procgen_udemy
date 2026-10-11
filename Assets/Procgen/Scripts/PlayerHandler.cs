using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] private Walker walker;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CinemachineCamera virtualCamera;
    private GameObject playerInstance;

    void Start()
    {
        StartCoroutine(PlacePlayerAfterGeneration());
    }

    void Update()
    {
        
    }

    public void RegeneratePlayer()
    {
        StartCoroutine(PlacePlayerAfterGeneration());
    }

    private Vector2Int? GetLeftmostFloorPosition()
    {
        Vector2Int? leftmost = null;

        for(int x = walker.GetGridMin().x; x <= walker.GetGridMax().x; x++)
        {
            for (int y = walker.GetGridMin().y; y <= walker.GetGridMax().y; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                if (groundTilemap.HasTile(tilePos))
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (!leftmost.HasValue || pos.x < leftmost.Value.x || (pos.x == leftmost.Value.x && pos.y < leftmost.Value.y))
                    {
                        leftmost = pos;
                    }
                }
            }
        }
        return leftmost;
    }

    private bool IsGenerationComplete()
    {
        for(int x = walker.GetGridMin().x;x <= walker.GetGridMax().x; x++)
        {
            for(int y = walker.GetGridMin().y;y <= walker.GetGridMax().y; y++)
            {
                if(groundTilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private IEnumerator PlacePlayerAfterGeneration()
    {
        while (!IsGenerationComplete())
        {
            yield return new WaitForSeconds(0.1f);
        }
        PlacePlayer();
    }

    private void PlacePlayer()
    {
        if (playerInstance != null)
        {
            Destroy(playerInstance);
        }
        
        Vector2Int? leftmostPos = GetLeftmostFloorPosition();
        if (leftmostPos.HasValue)
        {
            Vector3 worldPos = groundTilemap.CellToWorld(new Vector3Int(leftmostPos.Value.x, leftmostPos.Value.y, 0));
            worldPos += new Vector3(0.5f, 0.5f, 0);
            playerInstance = Instantiate(playerPrefab, worldPos, Quaternion.identity);
        }

        if(virtualCamera != null)
        {
            virtualCamera.Follow = playerInstance.transform;
        }
    }

    public GameObject GetPlayerInstance()
    {
        return playerInstance;
    }
}
