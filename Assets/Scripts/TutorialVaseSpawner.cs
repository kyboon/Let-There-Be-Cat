using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialVaseSpawner : MonoBehaviour, FallListener
{
    // Start is called before the first frame update

    public List<GameObject> prefabs = new List<GameObject>();

    private GameObject leftGO;
    private GameObject middleGO;
    private GameObject rightGO;

    public float spacing = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (leftGO == null)
        {
            leftGO = Instantiate(randomPrefab(), transform.position + new Vector3(-1 * spacing, 0), transform.rotation);
            FallableProp prop = leftGO.GetComponent<FallableProp>();
            if (prop != null)
                prop.listener = this;
        }
        if (rightGO == null)
        {
            rightGO = Instantiate(randomPrefab(), transform.position + new Vector3(spacing, 0), transform.rotation);
            FallableProp prop = rightGO.GetComponent<FallableProp>();
            if (prop != null)
                prop.listener = this;
        }
        if (middleGO == null)
        {
            middleGO = Instantiate(randomPrefab(), transform.position, transform.rotation);
            FallableProp prop = middleGO.GetComponent<FallableProp>();
            if (prop != null)
                prop.listener = this;
        }
    }

    GameObject randomPrefab()
    {
        if (prefabs.Count > 0)
        {
            int index = Random.Range(0, prefabs.Count);
            return prefabs[index];
        }
        return null;
    }

    public void Broken(GameObject prop)
    {
        if (prop == leftGO)
        {
            leftGO = null;
        } else if (prop == middleGO)
        {
            middleGO = null;
        } else if (prop == rightGO)
        {
            rightGO = null;
        }
        Destroy(prop, 10);
    }
}