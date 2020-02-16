/***************************************************************
 * Board Manager Script
 * By Hercules (HErC) Dias Campos (ID 101091070)
 * Created:         February 16, 2020
 * Last Modified:   February 16, 2020
 * 
 * Board (level) manager class
 * Responsible for instantiating players and managing turns
 * 
 *  ***ALWAYS BEGINS WITH P1***
 *  ***WILL REQUIRE REFINEMENT***
 *  
 *  TODO: Check whether deactivating the player's scripts
 *          works better as an alternative to turns
 * 
 **************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject[] m_players;
    //Not the preferred implementation, but this'll have to do
    [SerializeField] private PlayerBehaviour[] m_Behaviours; 
    private uint currentPlayerTurn;
    private bool gameWon;

    void Awake() {
        for (uint i = 0; i < m_players.Length; ++i) {
            m_players[i].transform.parent = this.gameObject.transform;
            m_Behaviours[i] = m_players[i].GetComponent<PlayerBehaviour>();
            m_Behaviours[i].SetTurn(false);
            m_Behaviours[i].SetPlayerId(i);
        }
        currentPlayerTurn = 0;
        
        gameWon = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Behaviours[0].SetTurn(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEndTurn(uint playerID) {
        m_Behaviours[playerID].SetTurn(false);
        if (playerID == 0) {
            m_Behaviours[1].SetTurn(true);
        }
        else {
            m_Behaviours[0].SetTurn(true);
        }
        
    }
}
