using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage all the different kind of spawn on the segment, probably the most reusable script in the project despite
/// Here all element has one in two chances to spawn. It's a value that work correctly for the game for each object.
/// </summary>
public class SegmentObstacleScript : MonoBehaviour
{
    [SerializeField] private List<Transform> obstacleSpawnList;
    [SerializeField] private List<Transform> coinSpawnList;
    [SerializeField] private List<Transform> fanSpawnList;
    [SerializeField] private List<Transform> tourniquetSpawnList;
    [SerializeField] private List<Transform> fanEnemySpawnList;
    [SerializeField] private GameObject wallObstacle;
    [SerializeField] private GameObject coinObject;
    [SerializeField] private GameObject tourniquetObject;
    [SerializeField] private List<GameObject> fanObjectList;
    [SerializeField] private List<GameObject> fanEnemyObjectList;

    // Start is called before the first frame update
    void Start()
    {
        //Obstacle
        foreach (var obstacleSpawn in obstacleSpawnList)
        {
            int canObstacleSpawn = Random.Range(0, 2);
            if (canObstacleSpawn == 1)
            {
                Instantiate(wallObstacle, obstacleSpawn.position, Quaternion.identity);
            }
        }

        //Coin
        foreach (var coinSpawn in coinSpawnList)
        {
            int canCoinSpawn = Random.Range(0, 2);
            if (canCoinSpawn == 1)
            {
                Instantiate(coinObject, coinSpawn.position, transform.rotation * Quaternion.Euler(0f, 90f, 90f));
            }
        }

        //Fan outside the road
        foreach (var fanSpawn in fanSpawnList)
        {
            int canFanSpawn = Random.Range(0, 2);
            if (canFanSpawn == 1)
            {
                int fanId = Random.Range(0, 3);
                switch (fanId)
                {
                    case 0: 
                        GameObject fanObject0 = Instantiate(fanObjectList[0], fanSpawn.position, Quaternion.identity);
                        fanObject0.transform.rotation = fanSpawn.rotation;
                        break;
                    case 1:
                        GameObject fanObject1 = Instantiate(fanObjectList[1], fanSpawn.position, Quaternion.identity);
                        fanObject1.transform.rotation = fanSpawn.rotation;
                        break;
                    case 2:
                        GameObject fanObject2 = Instantiate(fanObjectList[2], fanSpawn.position, Quaternion.identity);
                        fanObject2.transform.rotation = fanSpawn.rotation;
                        break;
                }
            }
        }

        //Tourniquet
        foreach (var tourniquetSpawn in tourniquetSpawnList)
        {
            int canCoinSpawn = Random.Range(0, 2);
            if (canCoinSpawn == 1)
            {
                Instantiate(tourniquetObject, tourniquetSpawn.position, Quaternion.identity);
            }
        }

        //Fan on the road
        foreach (var fanEnemySpawn in fanEnemySpawnList)
        {
            int canFanSpawn = Random.Range(0, 2);
            if (canFanSpawn == 1)
            {
                int fanId = Random.Range(0, 3);
                switch (fanId)
                {
                    case 0:
                        GameObject fanEnemyObject0 = Instantiate(fanEnemyObjectList[0], fanEnemySpawn.position, Quaternion.identity);
                        fanEnemyObject0.transform.rotation = fanEnemySpawn.rotation;
                        break;
                    case 1:
                        GameObject fanEnemyObject1 = Instantiate(fanEnemyObjectList[1], fanEnemySpawn.position, Quaternion.identity);
                        fanEnemyObject1.transform.rotation = fanEnemySpawn.rotation;
                        break;
                }
            }
        }
    }
}
