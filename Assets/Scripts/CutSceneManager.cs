using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject cutScenePanel;
    public float cutSceneLength = 3f;
    public Text cutSceneText;
    private int currentIndex;
    public Image talkerImage;
    public Image catImage;

    public List<Sprite> talkerSprites = new List<Sprite>();

    private Color opaqueColor = new Color(1, 1, 1, 1);
    private Color semiTransColor = new Color(1, 1, 1, 0.5f);

    private bool playedDogScene = false;

    private List<string> cutSceneTexts = new List<string>()
    {
        "That's enough! I'm calling animal control.",
        "This cat is slippery. I need backup!",
        "It's impossible! Bring in the net gun!",
        "Hello NASA, I think we have an alien cat here.",
        "Woof woof! Woof!"
    }; 
    
    private List<string> catTexts = new List<string>()
    {
        "MEOW",
        "MEOW?",
        "MEOW!?",
        "MEOW :)",
        "MEOW MEOW"
    };
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool PlayCutScene(int index)
    {
        if (PlayerPrefs.GetInt("ShowCutscene", 1) == 1)
        {

            if (index == 4)
            {
                if (playedDogScene)
                    return false;
                playedDogScene = true;
            }

            currentIndex = index;
            StartCutScene();
            return true;
        }
        return false;
    }

    void StartCutScene()
    {
        AudioManager.instance.PlaySound(1);
        cutSceneText.text = cutSceneTexts[currentIndex];
        talkerImage.sprite = talkerSprites[currentIndex];
        talkerImage.color = opaqueColor;
        catImage.color = semiTransColor;
        Time.timeScale = 0;
        cutScenePanel.SetActive(true);
        StartCoroutine(WaitCutScene());
    }

    IEnumerator WaitCutScene()
    {
        yield return new WaitForSecondsRealtime(cutSceneLength);
        AudioManager.instance.PlaySound(0);
        cutSceneText.text = catTexts[currentIndex];
        talkerImage.color = semiTransColor;
        catImage.color = opaqueColor;
        StartCoroutine(WaitEndScene());
    }

    IEnumerator WaitEndScene()
    {
        yield return new WaitForSecondsRealtime(1);
        EndCutScene();
    }
    void EndCutScene()
    {
        Time.timeScale = 1;
        cutScenePanel.SetActive(false);
    }
}
