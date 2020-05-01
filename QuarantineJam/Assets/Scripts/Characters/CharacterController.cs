using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speedMove = 4;

    protected List<Room> currentRoom = new List<Room>();
    protected List<Stairs> stairsList = new List<Stairs>();


    public Room GetCurrentRoom()
    {
        return currentRoom[currentRoom.Count-1];
    }
    public void SetCurrentRoom(Room room)
    {
        currentRoom.Add(room);
    }
    public void RemoveRoom(Room room)
    {
        currentRoom.Remove(room);
    }


    public void SetStair(Stairs stairs)
    {
        stairsList.Add(stairs);
    }
    public void ExitStair(Stairs stairs)
    {
        stairsList.Remove(stairs);
    }
}
