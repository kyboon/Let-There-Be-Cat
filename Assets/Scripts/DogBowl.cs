using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBowl : InteractableProp
{
    // Start is called before the first frame update

    public GameObject dogPrefab;

    public SpriteRenderer sp;
    public Sprite noBoneSprite;

    public CutSceneManager cutScene;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        base.Interact();

        cutScene = GameObject.Find("CutSceneManager").GetComponent<CutSceneManager>();
        bool shouldTransferCam = cutScene.PlayCutScene(4);

        if (shouldTransferCam)
            PlayerManager.instance.setInvincible(true);
        AudioManager.instance.PlaySound(15);
        StartCoroutine(spawnDog(shouldTransferCam));
    }

    IEnumerator spawnDog(bool shouldTransferCam)
    {
        yield return new WaitForSeconds(0.1f);
        GameObject dog = Instantiate(dogPrefab, transform.position + new Vector3(-20, 0), transform.rotation);
        Dog dogScript = dog.GetComponent<Dog>();
        dogScript.bowl = this;
        dogScript.shouldTransferCam = shouldTransferCam;
    }

    public void takeBone()
    {
        sp.sprite = noBoneSprite;
    }
}
