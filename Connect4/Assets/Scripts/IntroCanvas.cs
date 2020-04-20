using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroCanvas : MonoBehaviour
{
    [SerializeField] private string[] m_Messages;
    [SerializeField] private GameObject m_Prompt;
    private Text m_promptText;

    void Awake()
    {
        m_promptText = m_Prompt.GetComponent<Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     // 0 AWAITING, 
     // 1 FETCHING_USER,
     // 2 ACCEPTED,
     // 3 ADDING_USER,
     // 4 USER_ADDED,
     // 5 REJECTED, 
     // 6 CONNECTING, 
     // 7 REFUSED, 
     // 8 CONNECTED,
     // 9 DISCONNECTED,
     //10 CONNECTION_COUNT
        if (ClientManager.Instance.ClientConnectionState == ConnectionState.AWAITING && m_promptText.text != m_Messages[0]) {
            m_promptText.text = m_Messages[0];
        }
        else if (ClientManager.Instance.ClientConnectionState == ConnectionState.FETCHING_USER && m_promptText.text != m_Messages[1]) {
            m_promptText.text = m_Messages[1];
        }
        else if (ClientManager.Instance.ClientConnectionState == ConnectionState.USER_FETCHED && m_promptText.text != m_Messages[2]) {
            m_promptText.text = m_Messages[2];
        }
        else if (ClientManager.Instance.ClientConnectionState == ConnectionState.ADDING_USER && m_promptText.text != m_Messages[3]) {
            m_promptText.text = m_Messages[3];
        }
        else if (ClientManager.Instance.ClientConnectionState == ConnectionState.USER_ADDED && m_promptText.text != m_Messages[4]) {
            m_promptText.text = m_Messages[4];
        }
        else if (ClientManager.Instance.ClientConnectionState == ConnectionState.REJECTED && m_promptText.text != m_Messages[5]) {
            m_promptText.text = m_Messages[5];
        }
        else if (ClientManager.Instance.ClientConnectionState == ConnectionState.CONNECTING && m_promptText.text != m_Messages[6]) {
            m_promptText.text = m_Messages[6];
        }
        else if (ClientManager.Instance.ClientConnectionState == ConnectionState.REFUSED && m_promptText.text != m_Messages[7]) {
            m_promptText.text = m_Messages[7];
        }
        else if (ClientManager.Instance.ClientConnectionState == ConnectionState.CONNECTED && m_promptText.text != m_Messages[8]) {
            m_promptText.text = m_Messages[8];
        }
        else if (ClientManager.Instance.ClientConnectionState == ConnectionState.DISCONNECTED && m_promptText.text != m_Messages[9]) {
            m_promptText.text = m_Messages[9];
        }
    }
}
