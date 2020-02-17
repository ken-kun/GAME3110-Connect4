﻿/***************************************************************
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
    //Top Slot Config Variables
    //Serialization for visualization
    private Vector3[] m_vTopSlots;

    private uint m_iMinSlot;
    public uint MinSlot { get { return m_iMinSlot; } }

    private uint m_iMaxSlot;
    public uint MaxSlot { get { return m_iMaxSlot; } }

    //Player-related variables
    [SerializeField] private GameObject[] m_players;
    //Not the preferred implementation, but this'll have to do
    [SerializeField] private PlayerBehaviour[] m_Behaviours;

    //Detector slots
    [SerializeField] GameObject m_detectorTemplate;
    //Will have to work on flattened array
    [SerializeField] private GameObject[] m_detectors;

    //Gameplay variables
    private uint currentPlayerTurn;
    private bool gameWon;
    public bool GameOver { get { return gameWon; } }

    void Awake() {
        
        currentPlayerTurn = 0;
        
        gameWon = false;
        /*
         *1. This is semi-hardcoded. I don't like it, but well...
         *2. Row height changes only when row is done; therefore,
         *   the x loop (row filler) has to be nested inside the y loop
         *3. This creates the rows FROM LEFT TO RIGHT, BOTTOM TO TOP
         *4. This method uses a flattened array, so the index is incremented
         *   every loop, from 0 to 41
         */
        
        int index = 0;
        for (int y = 0; y < 6; ++y) {
            for (int x = 0; x < 7; ++x) {
                Vector3 detectorPosition = new Vector3(-2.75f + (float)x, 1.0f + (float)y, 0.0f);
                m_detectors[index++] = GameObject.Instantiate( m_detectorTemplate, 
                            this.gameObject.transform.position + detectorPosition,
                            this.gameObject.transform.rotation, 
                            this.gameObject.transform);
            }
        }

        m_iMinSlot = 0;
        m_iMaxSlot = 6;
        m_vTopSlots = new Vector3[7];
        for (int i = 0; i < m_vTopSlots.Length; ++i) {
            Vector3 topSlot = new Vector3(-2.75f + (float)i, 8.0f, 0.0f);
            m_vTopSlots[i] = this.gameObject.transform.position + topSlot;
        }

        for (uint i = 0; i < m_players.Length; ++i) {
            m_players[i].transform.parent = this.gameObject.transform;
            m_Behaviours[i] = m_players[i].GetComponent<PlayerBehaviour>();
            m_Behaviours[i].SetTurn(false);
            m_Behaviours[i].SetPlayerId(i);
            m_Behaviours[i].SetPlayerSlot(i*6);
            m_Behaviours[i].transform.position = GetSlotPosition(i * 6);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //TODO: Randomize
        m_Behaviours[0].SetTurn(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEndTurn(uint playerID, uint playerSlot) {

        foreach (PlayerBehaviour behaviour in m_Behaviours)
        {
            behaviour.SetTurn(false);
            behaviour.enabled = false;
        }

        uint slot = playerSlot;
        for (int i = (int)playerSlot; i < 42; ++i) {
            if (!m_detectors[i].GetComponent<DetectorBehaviour>().HasOwner)
            {
                m_detectors[i].GetComponent<DetectorBehaviour>().SetOwner((int)currentPlayerTurn);
                break;
            }
            else {
                i += 6;
            }
        }

        CheckWin();

        if (!gameWon) {
            if (playerID == 0) {
                currentPlayerTurn = 1;
                m_Behaviours[1].enabled = true;
                m_Behaviours[1].SetTurn(true);
            }
            else {
                currentPlayerTurn = 0;
                m_Behaviours[0].enabled = true;
                m_Behaviours[0].SetTurn(true);
            }
        }
    }

    public Vector3 GetSlotPosition(uint slotIndex) {
        if (slotIndex <= m_iMaxSlot) { return m_vTopSlots[slotIndex]; }
        else {
            Debug.Log("Invalid Slot");
            return m_vTopSlots[m_iMaxSlot];
        }
    }

    public bool IsSlotFull(uint slot) {

        return m_detectors[35 + slot].GetComponent<DetectorBehaviour>().HasOwner;
    }

    private bool CheckWin() {
        for (int p = 0; p < 2; ++p) { //for number of players
            for (int i = 0; i < 38; ++i) { //for number of relevant slots
                if (i < 21) {
                    gameWon = VictoryChecker.CheckVerticalWin(p, i, m_detectors);
                    if (i % 7 < 4) {
                        gameWon = VictoryChecker.CheckUpRightWin(p, i, m_detectors);
                    }
                    if (i % 7 > 2) {
                        gameWon = VictoryChecker.CheckUpLeftWin(p, i, m_detectors);
                    }

                    if (gameWon) {
                        break;
                    }
                }
                if (i % 7 < 4) {
                    VictoryChecker.CheckHorizontalWin(p, i, m_detectors);
                    if (gameWon) {
                        break;
                    }
                }
                if (gameWon) {
                    break;
                }
            }
            if (gameWon) {
                break;
            }
        }
        return gameWon;
    }
}
