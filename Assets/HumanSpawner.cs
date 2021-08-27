using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : MonoBehaviour
{
    public int maxHumanCount = 15;

    public GameObject humanPrefab;
    public List<GameObject> humans = new List<GameObject>();
    public float humanDespawnRange = 100f;
    public float humanDespawnCheckRate = 1f;
    public float despawnCheckCountdown = 0f;
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
            GameObject newHuman = Instantiate(humanPrefab, position, transform.rotation);
            humans.Add(newHuman);
        }
    }
}
