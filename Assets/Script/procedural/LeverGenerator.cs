using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class LeverGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject UncopyableTablePrefab;
    public GameObject copyableTablePrefab;
    public GameObject PlayerTablePrefab;
    public GameObject PlayerChairPrefab;
    public GameObject chairPrefab;
    public GameObject obstaclePrefab;
   // public GameObject playerStartPrefab;
    
    public GameObject exitPrefab;

    [Header("Generation Settings")]
    public int roomWidth = 20;
    public int roomDepth = 15;
    public int minTables = 4;
    public int maxTables = 8;
    public int minObstacles = 2;
    public int maxObstacles = 5;

    [Header("NavMesh")]
    public NavMeshSurface navMeshSurface;

    private List<Vector3> occupiedPositions = new List<Vector3>();
    private GameObject playerStart;
    private GameObject copyTable;

    void Start()
    {
        GenerateClassroom();
    }

    void GenerateClassroom()
    {

        // Place key elements first
        PlaceKeyElements();

        // Generate tables and chairs
        GenerateTables();

        // Generate obstacles
        GenerateObstacles();

        // Build NavMesh
        BuildNavMesh();

        // Verify path accessibility
        VerifyAccessibility();
    }

    void PlaceKeyElements()
    {
        // Place player start at the front
        /*Vector3 playerPos = new Vector3(roomWidth / 2, 0, 1);
        playerStart = Instantiate(playerStartPrefab, playerPos, Quaternion.identity);
        occupiedPositions.Add(playerPos);*/

        // Player Table
        Vector3 TablePos = new Vector3(roomWidth / 2, 0, 3);
        copyTable = Instantiate(PlayerTablePrefab, TablePos, Quaternion.Euler(new Vector3(0, 90, 0)));
        occupiedPositions.Add(TablePos);

        // Player chair
        Vector3 chairOffsets = new Vector3(0, 0, -1f); // South
        Vector3 chairPos = TablePos + chairOffsets;
        if (IsPositionValid(chairPos, 1f))
        {
            Instantiate(PlayerChairPrefab, chairPos, Quaternion.Euler(new Vector3(0, 90, 0)));
            occupiedPositions.Add(chairPos);
        }

    }

    void GenerateTables()
    {
        int tableCount = Random.Range(minTables, maxTables + 1);

        for (int i = 0; i < tableCount; i++)
        {
            Vector3 tablePos = FindValidPosition(3f);
            if (tablePos != Vector3.zero)
            {
                // Instantiate table
                GameObject table = Instantiate(copyableTablePrefab, tablePos, Quaternion.Euler(new Vector3(0, 90, 0)));
                occupiedPositions.Add(tablePos);

                if (i==0) table.transform.tag = "GoodStudent"; // Au moins une bonne table
                table.transform.tag = Random.Range(0, 100) < 50 ?  "GoodStudent" : "BadStudent"; // le reste est aléatoire
                

                // Place chairs around the table
                PlaceChairsAroundTable(tablePos);
            }
        }
    }

    void PlaceChairsAroundTable(Vector3 tablePos)
    {
        Vector3 chairOffsets = new Vector3(0, 0, -0.6f); // South
        Vector3 chairPos = tablePos + chairOffsets;

            Debug.Log(chairPos);
            Instantiate(chairPrefab, chairPos, Quaternion.Euler(new Vector3(0, 90, 0)));
            occupiedPositions.Add(chairPos);

    }

    void GenerateObstacles()
    {
        int obstacleCount = Random.Range(minObstacles, maxObstacles + 1);

        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 obstaclePos = FindValidPosition(3f);
            if (obstaclePos != Vector3.zero)
            {
                Instantiate(obstaclePrefab, obstaclePos+new Vector3(0,1,0), Quaternion.identity);
                occupiedPositions.Add(obstaclePos);
            }
        }
    }

    Vector3 FindValidPosition(float minDistance)
    {
        int attempts = 0;
        int maxAttempts = 50;

        while (attempts < maxAttempts)
        {
            Vector3 candidatePos = new Vector3(
                Random.Range(2, roomWidth - 2),
                0,
                Random.Range(3, roomDepth - 3)
            );

            if (IsPositionValid(candidatePos, minDistance))
            {
                return candidatePos;
            }

            attempts++;
        }

        return Vector3.zero; // No valid position found
    }

    Vector3 FindValidPositionNear(Vector3 referencePos, float maxDistance)
    {
        int attempts = 0;
        int maxAttempts = 30;

        while (attempts < maxAttempts)
        {
            Vector3 offset = new Vector3(
                Random.Range(-maxDistance, maxDistance),
                0,
                Random.Range(-maxDistance, maxDistance)
            );

            Vector3 candidatePos = referencePos + offset;

            // Keep within room bounds
            candidatePos.x = Mathf.Clamp(candidatePos.x, 2, roomWidth - 2);
            candidatePos.z = Mathf.Clamp(candidatePos.z, 3, roomDepth - 3);

            if (IsPositionValid(candidatePos, 1.5f))
            {
                return candidatePos;
            }

            attempts++;
        }

        return referencePos + new Vector3(2, 0, 0); // Fallback position
    }

    bool IsPositionValid(Vector3 position, float minDistance)
    {
        // Check room boundaries
        if (position.x < 1 || position.x >= roomWidth - 1 ||
            position.z < 1 || position.z >= roomDepth - 1)
        {
            return false;
        }

        // Check distance from other objects
        foreach (Vector3 occupiedPos in occupiedPositions)
        {
            if (Vector3.Distance(position, occupiedPos) < minDistance)
            {
                return false;
            }
        }

        return true;
    }

    void BuildNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
    }

    void VerifyAccessibility()
    {
        if (playerStart == null || copyTable == null) return;

        NavMeshPath path = new NavMeshPath();
        bool pathToCopyTable = NavMesh.CalculatePath(
            playerStart.transform.position,
            copyTable.transform.position,
            NavMesh.AllAreas,
            path
        );


        if (!pathToCopyTable)
        {
            Debug.LogWarning("Path verification failed! Regenerating level...");
            // Regenerate if path is blocked
            GenerateClassroom();
        }
        else
        {
            Debug.Log("Level generation successful! All paths are accessible.");
        }
    }

}
