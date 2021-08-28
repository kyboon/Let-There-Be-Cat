using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public List<GameObject> levelPrefabs = new List<GameObject>();

    public List<GameObject> existingLevels = new List<GameObject>();

    public float spawnRange = 40f;
    public float despawnRange = 60f;

    public HumanSpawner humanSpawner;

    private Transform playerTransform;
    // Start is called before the first frame update

    void Start()
    {
        playerTransform = PlayerManager.instance.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (playerTransform == null || existingLevels.Count == 0)
            return;
        // check left

        //Debug.Log(playerTransform.position.y - existingLevels[0].transform.position.x);
        //return;
        if (playerTransform.position.x - existingLevels[0].transform.position.x > despawnRange)
        {
            Destroy(existingLevels[0]);
            existingLevels.RemoveAt(0);
        } else if (playerTransform.position.x - existingLevels[0].transform.position.x < spawnRange)
        {
            // spawn one level left to the leftmost level
            int randomIndex = Random.Range(0, levelPrefabs.Count);
            Vector3 spawnPosition = existingLevels[0].transform.position - new Vector3(20, 0, 0);
            GameObject newLevel = Instantiate(levelPrefabs[randomIndex], spawnPosition , transform.rotation);
            float humanSpawnOffset = Random.Range(-8, 8);
            humanSpawner.trySpawnHuman(spawnPosition + new Vector3(humanSpawnOffset, 2, 0));
            existingLevels.Insert(0, newLevel);
        }

        if (existingLevels.Count == 0)
            return;
        int lastLevelIndex = existingLevels.Count - 1;
        if (existingLevels[lastLevelIndex].transform.position.x - playerTransform.position.x> despawnRange)
        {
            Destroy(existingLevels[lastLevelIndex]);
            existingLevels.RemoveAt(lastLevelIndex);
        } else if (existingLevels[lastLevelIndex].transform.position.x - playerTransform.position.x < spawnRange)
        {
            // spawn one level right to the rightmost level
            int randomIndex = Random.Range(0, levelPrefabs.Count);
            Vector3 spawnPosition = existingLevels[lastLevelIndex].transform.position + new Vector3(20, 0, 0);
            GameObject newLevel = Instantiate(levelPrefabs[randomIndex], spawnPosition, transform.rotation);

            float humanSpawnOffset = Random.Range(-8, 8);
            humanSpawner.trySpawnHuman(spawnPosition + new Vector3(humanSpawnOffset, 2, 0));
            existingLevels.Add(newLevel);
        }
    }
}
