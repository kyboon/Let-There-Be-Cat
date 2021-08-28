using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour, FallListener
{
    FallableProp fallableProp;
    // Start is called before the first frame update
    void Start()
    {
        fallableProp = GetComponent<FallableProp>();
        fallableProp.listener = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10) // if touched human
        {
            // slip human
            Human human = collision.gameObject.GetComponent<Human>();
            if (human != null && human.TrySlip())
            {
                PlayerManager.instance.addScore(3);
                Destroy(gameObject);
            }
        }
    }

    public void Broken(GameObject prop)
    {
        gameObject.layer = 11; // change layer to sabotage prop
    }
}
