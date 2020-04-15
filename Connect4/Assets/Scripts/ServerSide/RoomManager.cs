/***************************************************************
 * Server Script
 * By Hercules (HErC) Dias Campos (ID 101091070)
 * Created:         April 08, 2020
 * Last Modified:   April 08, 2020
 * 
 * Overall Room Manager class
 * Manages the room/board (most of the BoardManager functionality
 * has been transferred here).
 * 
 * Has two players, manages things on a match level
 * 
 **************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C4M;
using C4NO;

public class RoomManager : MonoBehaviour
{
    public C4NO.NetworkPlayer[] m_players;
    public bool IsEmpty { get { return m_players[0] == null && m_players[1] == null; } }
    public bool HasRoom { get { return m_players[0] == null ^ m_players[1] == null; } }
    public bool IsFull { get { return m_players[0] != null && m_players[1] != null; } }
    
    private void Awake()
    {
        m_players = new C4NO.NetworkPlayer[2];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool AddPlayer(C4NO.NetworkPlayer player) {
        if (m_players[0] == null)
        {
            m_players[0] = player;
            return true;
        }
        else if (m_players[1] == null) {
            m_players[1] = player;
            return true;
        }
        //it will be a full room
        return false;
    }
}
