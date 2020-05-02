using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speedMove = 4;

    protected List<Room> currentRoom = new List<Room>();
    protected List<Stairs> stairsList = new List<Stairs>();

    protected int currentLevel = 0;

    protected SpriteRenderer display;
    protected Animator anim;

    protected virtual void Awake()
    {
        display = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    #region Room
    public Room GetCurrentRoom()
    {
        if (currentRoom.Count > 0)
        {
            return currentRoom[currentRoom.Count - 1];
        }
        return null;
    }
    public void SetCurrentRoom(Room room)
    {
        currentLevel = room.level;
        currentRoom.Add(room);
    }
    public void RemoveRoom(Room room)
    {
        currentRoom.Remove(room);
    }
    #endregion

    #region Stairs
    public Stairs GetStairs()
    {
        for (int i = stairsList.Count; i-- > 0;)
        {
            if (stairsList[i].level != currentLevel)
            {
                stairsList.RemoveAt(i);
            }
        }
        if (stairsList.Count > 0)
        {
            return stairsList[stairsList.Count - 1];
        }
        return null;
    }
    public void SetStair(Stairs stairs)
    {
        if (stairs.level != currentLevel || stairsList.Contains(stairs))
        {
            return;
        }
        stairsList.Add(stairs);
    }
    public void ExitStair(Stairs stairs)
    {
        stairsList.Remove(stairs);
    }
    #endregion
}
