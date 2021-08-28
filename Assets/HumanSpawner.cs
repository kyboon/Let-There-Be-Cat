using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : MonoBehaviour
{
    public int maxHumanCount = 15;

    public List<GameObject> humanPrefabs;
    public List<GameObject> humans = new List<GameObject>();
    public float humanDespawnRange = 100f;
    public float humanDespawnCheckRate = 1f;
    public float despawnCheckCountdown = 0f;
    private bool forceSpawnNewUnit = false;

    public int difficulty = 0;
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
    public void setDifficulty(int difficulty)
    {
        this.difficulty = difficulty;
        forceSpawnNewUnit = true;
    }
    private void FixedUpdate()
    {
        despawnCheckCountdown -= Time.fixedDeltaTime;

        if (despawnCheckCountdown <= 0)
        {
            despawnCheckCountdown = humanDespawnCheckRate;

            List<GameObject> humansToBeDespawn = new List<GameObject>();

            foreach(GameObject human in humans)
            {
                if (Mathf.Abs(human.transform.position.x - playerTransform.position.x) > humanDespawnRange)
                {
                    humansToBeDespawn.Add(human);
                }
            }

            foreach(GameObject humanToBeDespawn in humansToBeDespawn)
            {
                Destroy(humanToBeDespawn);
                humans.Remove(humanToBeDespawn);
            }
        }
    }
    public void trySpawnHuman(Vector3 position)
    {
        if (humans.Count < maxHumanCount)
        {
            int spawnIndex = 0;

            if (difficulty == 1)
            {
                // 30% chance spawn AC1
                if (forceSpawnNewUnit || Random.Range(0, 1f) > 0.7)
                {
                    spawnIndex = 1;
                }
            } else if (difficulty == 2)
            {
                // 15% chance spawn AC2, 45% chance spawn AC1
                float randomNum = Random.Range(0, 1f);
                if (forceSpawnNewUnit || randomNum > 0.85)
                {
                    spawnIndex = 2;
                } else if (randomNum > 0.4)
                {
                    spawnIndex = 1;
                }
            }
            forceSpawnNewUnit = false;

            GameObject newHuman = Instantiate(humanPrefabs[spawnIndex], position, transform.rotation);
            humans.Add(newHuman);
        }
    }
}
