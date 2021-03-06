﻿/***************************************************************
 * Player Behaviour Script
 * By Hercules (HErC) Dias Campos (101091070)
 * Created:         February 15, 2020
 * Last Modified:   February 17, 2020
 * 
 * Class implemented basically to control player input
 * As it is, it's not suitable for the proposed gameplay
 * What it (currently) does:
 *      - Press Right arrow to move player right
 *      - Press Left arrow to move player left
 *      - If the player has no coin, press spacebar
 *          to create one
 *      - If the player has a coin, press spacebar
 *          to release it
 * 
 **************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    //Network "interface"
    private ClientScript m_netPlayer; //will change

    [SerializeField] private GameObject m_coinTemplate;
    private GameObject m_coin;
    [SerializeField] private int m_iPlayerId;
    [SerializeField] private int m_iCurrentSlot;
    public int CurrentPlayerSlot { get { return m_iCurrentSlot; } }

    private BoardManager m_boardManager;
    private bool m_bIsTurn;
    public bool IsTurn { get { return m_bIsTurn; } }

    void Awake() {

        m_boardManager =  this.gameObject.transform.parent.gameObject.GetComponent<BoardManager>();
        //The board is doing this for us...
        //this.gameObject.transform.position = m_boardManager.GetSlotPosition();
        m_bIsTurn = false;
        m_netPlayer = this.gameObject.transform.parent.gameObject.GetComponent<ClientScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //m_bIsTurn = m_netPlayer.IsTurn;
        if (!m_bIsTurn) {
            if (m_coin) { ClearCoin(); }
            return;
        }
        //this should prevent players from stacking coins over the top of the board
        if (Input.GetKeyDown(KeyCode.Space)){ m_netPlayer.RequestSlot(CurrentPlayerSlot); }

        //implemented wraparound
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            ++m_iCurrentSlot;
            m_iCurrentSlot %= 7;
            this.gameObject.transform.position = m_boardManager.GetSlotPosition(m_iCurrentSlot);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            m_iCurrentSlot = m_iCurrentSlot == 0 ? 6 : --m_iCurrentSlot;
            this.gameObject.transform.position = m_boardManager.GetSlotPosition(m_iCurrentSlot);
        }
    }

    public void SetPlayerSlot(int slot) {

        m_iCurrentSlot = slot % 7; //wraparound
    }

    public void SetPlayerId(int id) {
        m_iPlayerId = id;
    }

    public void SetTurn(bool turn) {

        m_bIsTurn = turn;
        if (m_bIsTurn) {
            CreateCoin();
        }
    }

    void ReleaseCoin() {
        if (m_coin) {
            m_coin.GetComponent<CoinBehaviour>().OnDetach();
            m_coin = null;
            m_boardManager.OnEndTurn(m_iPlayerId, m_iCurrentSlot);
            //Send Message to server, informing that the player made a move
        }
    }

    private void CreateCoin() {

        m_coin = GameObject.Instantiate(m_coinTemplate, this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
        m_coin.GetComponent<CoinBehaviour>().SetPlayerOwner(m_iPlayerId);
        m_coin.GetComponent<SphereCollider>().enabled = false;
    }

    private void ClearCoin() { //may benefit from pooling...
        Destroy(m_coin);
    }
}
