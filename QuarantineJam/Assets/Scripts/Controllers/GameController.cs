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
        if (player.GrabOjbect() && player.GetCurrentRoom() == devil.GetCurrentRoom() && !isEnd)
        {
            Debug.Log("Loose");
            player.Loose();
            isEnd = true;
            SoundController.instance.PlaySFX(gameOver);
        }
    }

    public void CheckWin()
    {
        List<GrabObject> compareToListFinish = winItems.Where(i => ObjectManager.instance.suitcase.objets.Contains(i)).ToList();

        Debug.Log(compareToListFinish.Count+"   "+ winItems.Count);
        if (compareToListFinish.Count == winItems.Count)
        {
            player.Win();
            Debug.Log("Win");
            //chekWin;
        }
    }

    public void GetRandomObjects()
    {
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

    }

}
