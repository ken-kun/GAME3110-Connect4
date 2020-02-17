/***************************************************************
 * Player Behaviour Script
 * By Hercules (HErC) Dias Campos (101091070)
 * Created:         February 15, 2020
 * Last Modified:   February 15, 2020
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
    [SerializeField] private GameObject m_coinTemplate;
    private GameObject m_coin;
    [SerializeField] private uint m_iPlayerId;
    [SerializeField] private uint m_iPlayerSlot;
    public uint CurrentPlayerSlot { get { return m_iPlayerSlot; } }
    [SerializeField] private float m_fClampMin;
    [SerializeField] private float m_fClampMax;

    private BoardManager m_boardManager;
    private bool m_bIsTurn;
    public bool IsTurn { get { return m_bIsTurn; } }

    void Awake() {

        m_boardManager =  this.gameObject.transform.parent.gameObject.GetComponent<BoardManager>();
        m_bIsTurn = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //CreateCoin();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bIsTurn) {

            if (Input.GetKeyDown(KeyCode.Space)) { ReleaseCoin(); }

            if (Input.GetKeyDown(KeyCode.RightArrow) && this.gameObject.transform.position.x < m_fClampMax) {

                this.gameObject.transform.Translate(Vector3.right);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && this.gameObject.transform.position.x > m_fClampMin) {

                this.gameObject.transform.Translate(Vector3.right * -1);
            }
        }
        else {
            if (m_coin) {
                ClearCoin();
            }
        }
    }

    public void SetPlayerId(uint id) {
        m_iPlayerId = id;
    }

    public void SetTurn(bool turn) {

        m_bIsTurn = turn;
        if (m_bIsTurn) {
            CreateCoin();
        }
    }

    void ReleaseCoin() {

        m_coin.GetComponent<CoinBehaviour>().OnDetach();
        m_coin = null;
        m_boardManager.OnEndTurn(m_iPlayerNumber, m_iPlayerSlot);
        //Send Message to server, informing that the player made a move
    }

    private void CreateCoin() {

        m_coin = GameObject.Instantiate(m_coinTemplate, this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
        m_coin.GetComponent<CoinBehaviour>().SetPlayerOwner(m_iPlayerId);
        m_coin.GetComponent<SphereCollider>().enabled = false;
    }

    private void ClearCoin() {
        Destroy(m_coin);
    }
}
