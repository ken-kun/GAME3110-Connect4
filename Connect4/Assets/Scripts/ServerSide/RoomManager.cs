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
    private ServerScript m_serverScript; //needed for message exchange

    //Room attributes
    public string[] m_players; //has to be public so server knows. 
                               //   Will it use player names or ids?
    public int roomID;
    public int m_numPlayers;
    public int Capacity { get; private set; }
    public bool IsEmpty => m_numPlayers == 0;
    public bool HasRoom => m_numPlayers < Capacity;
    public bool IsFull => m_numPlayers == Capacity;

    //Game Attributes
    private bool m_bMatchStarted;
    private bool m_bGameSet;
    private int m_CurrentPlayerTurn; //player index

    //Board Attributes
    private string[] m_slots;

    private void Awake()
    {
        //gets reference to server to be able to send messages
        m_serverScript = this.gameObject.GetComponent<ServerScript>();

        //sets room and players
        Capacity = 2;
        m_numPlayers = 0;
        m_bMatchStarted = false;
        m_bGameSet = false;
        m_players = new string[2];
        m_CurrentPlayerTurn = -1; //initialized to -1 for safety purposes

        //Board setup
        m_slots = new string[42];
        for (int i = 0; i < m_slots.Length; ++i) {
            m_slots[i] = "";
        }
    }
    // Start is called before the first frame update
    void Start() {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (IsFull && !m_bMatchStarted) {
            StartMatch();
        }
    }
    void StartMatch() {

        m_CurrentPlayerTurn = Mathf.RoundToInt(Random.value);
        //send server update with match started
        m_bMatchStarted = true;
        UpdateTurn();
    }
    void UpdateBoard() {
        //for now, just sends the board's current state via server
    }
    void UpdateTurn() {
        if (m_CurrentPlayerTurn == 0) {
            //Send Update Turn message via server
            TurnUpdateMsg tmsg = new TurnUpdateMsg();
            tmsg.playerTurn = m_players[1];
        }
    }
    public int AddPlayer(string player) {
        if (m_players[0] == null) {
            m_players[0] = player;
            m_numPlayers++;
            return roomID;
        }
        else if (m_players[1] == null) {
            m_players[1] = player;
            m_numPlayers++;
            return roomID;
        }
        //it will be a full room
        return -1;
    }
    public bool SlotAvailable(int slot) {
        return m_slots[slot+35] == "";
    }
}
