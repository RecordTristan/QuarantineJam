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
        if (player.GrabOjbect() && player.GetCurrentRoom() == devil.GetCurrentRoom())
        {
            Debug.Log("Loose");
            player.Loose();
        }
    }

    public void CheckWin(List<GrabObject> objectInSuitCase)
    {
        List<GrabObject> compareToListFinish = winItems.Where(i => objectInSuitCase.Contains(i)).ToList();

        if (compareToListFinish.Count == winItems.Count)
        {
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
