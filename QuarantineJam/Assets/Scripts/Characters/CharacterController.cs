using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    protected List<Room> currentRoom = new List<Room>();

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
}
