using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public PlayerController player;
    public DevilController devil;

    [Header("WinCondition")]
    public List<GrabObject> winItems = new List<GrabObject>();
    public int nbrObjectsToRecup;
    public bool isEnd = false;
    public int score;


    [Header("Sounds")]
    public AudioClip gameOver;

    void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;
    }
    void Start()
    {
        GetRandomObjects();
    }

    public void CheckDefeat()
    {
        if (player.GrabOjbect() && player.GetObjectInHand().canBeSelect && player.GetCurrentRoom() == devil.GetCurrentRoom() && !isEnd)
        {
            Debug.Log("Loose");
            player.Loose();
            isEnd = true;
            SoundController.instance.PlaySFX(gameOver);
            UIController.instance.Loose();
        }
    }

    public void CheckWin()
    {
        List<GrabObject> compareToListFinish = winItems.Where(i => ObjectManager.instance.suitcase.objets.Contains(i)).ToList();

        Debug.Log(compareToListFinish.Count+"   "+ winItems.Count);
        if (compareToListFinish.Count == winItems.Count)
        {
            player.Win();
            UIController.instance.Win();
            Debug.Log("Win");
            //chekWin;
        }
    }

    public void GetRandomObjects()
    {
        int difficulty = PlayerPrefs.GetInt("Difficulty",0);
        if (difficulty < 3)
        {
            nbrObjectsToRecup = GameDesigner.instance.difficulty[difficulty];
        }
        else
        {
            nbrObjectsToRecup = UnityEngine.Random.Range(2, 7);
        }

        List<int> rands = new List<int>();
        while (rands.Count < nbrObjectsToRecup )
        {
            int rand = UnityEngine.Random.Range(0, ObjectManager.instance.GetNumberOfObject());
            if (!rands.Contains(rand))
            {
                rands.Add(rand);
                winItems.Add(ObjectManager.instance.GetInteractObject(rand));
            }
        }
        UIController.instance.SetList(winItems);
    }

    public void AddObject(GrabObject grabObject)
    {
        if (winItems.Contains(grabObject))
        {
            score += (int)(grabObject.weight * 100);
        }
        else
        {
            score += (int)(grabObject.weight * 20);
        }
    }

}
