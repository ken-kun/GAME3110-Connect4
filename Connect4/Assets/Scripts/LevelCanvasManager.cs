/***************************************************************
 * Board Manager Script
 * By Hercules (HErC) Dias Campos (ID 101091070)
 * Created:         February 17, 2020
 * Last Modified:   February 17, 2020
 * 
 * Level Canvas (UI) manager class
 * Responsible for prompting player turn and displaying winner
 *  
 * **Consider turning this into a Singleton in the future**
 * 
 **************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCanvasManager : MonoBehaviour
{
    private Canvas m_canvas;

    [SerializeField] private Text m_waitingText;
    [SerializeField] private Text m_turnText;
    [SerializeField] private Text m_gameSetText;

    void Awake() {
        m_canvas = this.gameObject.GetComponent<Canvas>();
        m_waitingText.gameObject.SetActive(false);
        m_turnText.gameObject.SetActive(true);
        m_gameSetText.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (ClientManager.Instance.ClientConnectionState == ConnectionState.AWAITING_P2) {
                m_waitingText.gameObject.SetActive(true);
        }
        if (ClientManager.Instance.ClientConnectionState != ConnectionState.AWAITING_P2) {
                m_waitingText.gameObject.SetActive(false);
        }
    }
    public void UpdateTurnText(string player) {
        m_turnText.text = player + "'s turn";
    }

    public void UpdateSetText(string player) {
        if (m_turnText.isActiveAndEnabled) { m_turnText.gameObject.SetActive(false); }

        m_gameSetText.gameObject.SetActive(true);
        m_gameSetText.text = "Game Set!\n" + player + " wins";
    }
}