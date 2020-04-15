/***************************************************************
 * Server Script
 * By Hercules (HErC) Dias Campos (ID 101091070)
 * Created:         April 08, 2020
 * Last Modified:   April 08, 2020
 * 
 * Overall Server Manager class
 * Manages server, connections and room management.
 * 
 * On player connection (hopefully implemented correctly by the
 * matchmaker server), the server script creates a room, or 
 * redirects a player to a room where a suitable opponent is
 * waiting.
 * 
 * To that end, it keeps tabs on how many players are connected 
 * and how many rooms it has.
 * 
 **************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerScript : MonoBehaviour
{

    private List<GameObject> m_Rooms;
    private List<C4NO.NetworkPlayer> m_networkPlayers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddRoom() {
        m_Rooms.Add(new GameObject());
        m_Rooms[m_Rooms.Count - 1].AddComponent<RoomManager>();
    }
}
