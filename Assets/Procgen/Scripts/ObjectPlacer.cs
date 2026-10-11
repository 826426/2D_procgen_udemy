using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int prefabNumber = 5;
    [SerializeField] private Walker walker;
    [SerializeField] private Tilemap groundTilemap;

    private List<GameObject> placeObjects = new List<GameObject>();
    void Start()
    {
        StartCoroutine(PlaceObjectsAfterGeneration());
    }


    void Update()
    {
        
    }

    public void OnLevelGenerated()
    {
        StartCoroutine(PlaceObjectsAfterGeneration());
    }

    private IEnumerator PlaceObjectsAfterGeneration()
    {
        while (!IsGenerationComplete()){
            yield return new WaitForSeconds(0.05f);
        }
        PlaceObjects();
    }

    private bool IsGenerationComplete()
    {
        if(walker == null || groundTilemap == null) return false;
        for(int x = walker.GetGridMin().x; x <= walker.GetGridMax().x; x++)
        {
            for(int y = walker.GetGridMin().y; y <= walker.GetGridMax().y; y++)
            {
                if(groundTilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void ClearPlaceObjects()
    {
        foreach(GameObject obj in placeObjects)
        {
            if(obj != null)
            {
                Destroy(obj);
            }
        }
        placeObjects.Clear();
    }

    private List<Vector3Int> GetFloorPosition()
    {
        List<Vector3Int> floorPosition = new List<Vector3Int>();
        
        for(int x = walker.GetGridMin().x; x <= walker.GetGridMax().x; x++)
        {
            for(int y = walker.GetGridMin().y; y <= walker.GetGridMax().y; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                if (groundTilemap.HasTile(tilePos))
                {
                    floorPosition.Add(tilePos);
                }
            }
        }
        return floorPosition;
    }

    private void PlaceObjects()
    {
        ClearPlaceObjects();

        if(prefab == null || walker == null)
        {
            return;
        }

        List<Vector3Int> floorPosition = GetFloorPosition();
        if(floorPosition.Count == 0)
        {
            return;
        }
        
        int prefabToPlace = Mathf.Min(prefabNumber, floorPosition.Count);

        for(int i = 0; i < prefabToPlace; i++)
        {
            if(floorPosition.Count == 0)
            {
                break;
            }
            int randomIndex = Random.Range(0, floorPosition.Count);
            Vector3Int tilePos = floorPosition[randomIndex];

            Vector3 worldPos = groundTilemap.CellToWorld(tilePos) + new Vector3(0.5f, 0.5f, 0f);

            GameObject objectPrefab = Instantiate(prefab, worldPos, Quaternion.identity, transform);
            placeObjects.Add(objectPrefab);

            floorPosition.RemoveAt(randomIndex);
        }
    }
}
