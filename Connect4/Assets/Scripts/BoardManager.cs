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
    [SerializeField] GameObject m_detectorTemplate;
    //Will have to work on flattened array
    [SerializeField] private GameObject[] m_detectors;
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
        //1. This is semi-hardcoded. I don't like it, but well...
        //2. Row height changes only when row is done; therefore,
        //   the x loop (row filler) has to be nested inside the y loop
        //3. This creates the rows FROM LEFT TO RIGHT, BOTTOM TO TOP
        for (int y = 0; y < 6; ++y) {
            for (int x = 0; x < 7; ++x) {
                Vector3 detectorPosition = new Vector3(-2.75f + (float)x, 1.0f + (float)y, 0.0f);
                m_detectors[x + (x * y)] = GameObject.Instantiate( m_detectorTemplate, 
                            this.gameObject.transform.position + detectorPosition,
                            this.gameObject.transform.rotation, 
                            this.gameObject.transform);
            }
        }
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
        foreach (PlayerBehaviour behaviour in m_Behaviours) {
            behaviour.SetTurn(false);
            behaviour.enabled = false;
        }
        if (playerID == 0) {
            m_Behaviours[1].enabled = true;
            m_Behaviours[1].SetTurn(true);
        }
        else {
            m_Behaviours[0].enabled = true;
            m_Behaviours[0].SetTurn(true);
        }
        
    }
}
