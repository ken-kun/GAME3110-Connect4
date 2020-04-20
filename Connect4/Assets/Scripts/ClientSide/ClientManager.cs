using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Collections;
using System.Text;

public enum ConnectionState {AWAITING, 
                             FETCHING_USER,
                             ACCEPTED,
                             ADDING_USER,
                             USER_ADDED,
                             REJECTED, 
                             CONNECTING, 
                             REFUSED, 
                             CONNECTED,
                             DISCONNECTED,
                             CONNECTION_COUNT }
public class ClientManager : MonoBehaviour
{ 
    public ConnectionState ClientConnectionState { get; private set; }
    public ClientScript ClientScript { get; private set; }
    public static ClientManager Instance { get; private set; }

    [SerializeField] private string m_RetrievePlayerURL;
    [SerializeField] private string m_AddPlayerURL;

    void Awake() {
        ClientConnectionState = ConnectionState.AWAITING;
        if (Instance == null || Instance != this) { Instance = this; }
        //if (Instance != this) { Instance = this; }
        this.gameObject.GetComponent<ClientScript>();
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FetchUser() {
        if (ClientConnectionState == ConnectionState.AWAITING   || 
            ClientConnectionState == ConnectionState.USER_ADDED ||
            ClientConnectionState == ConnectionState.REJECTED) {
            StartCoroutine(RetrieveUser());
        }
    }
    IEnumerator RetrieveUser() {
        //creates data to send
        C4NO.PlayerLoginInfo data = new C4NO.PlayerLoginInfo();
        data.Username = GameObject.Find("NameField").GetComponent<Text>().text;
        data.Password = GameObject.Find("PasswordField").GetComponent<Text>().text;
        
        //converts data to Json, and then to byte array
        string classData = JsonUtility.ToJson(data);
        UTF8Encoding temp = new UTF8Encoding();
        byte[] bytes = temp.GetBytes(classData);
        
        //prepares web request
        UnityWebRequest req = UnityWebRequest.Put(m_RetrievePlayerURL, bytes);
        req.SetRequestHeader("Content-Type", "application/json");
        ClientConnectionState = ConnectionState.FETCHING_USER;
        
        //waits for answer
        yield return req.SendWebRequest();
        
        //outcome
        if (req.isNetworkError || req.isHttpError) {
            Debug.Log(req.error);
            ClientConnectionState = ConnectionState.REJECTED;
        }
        else {
            ClientConnectionState = ConnectionState.ACCEPTED;
            //This worked. Encode retrieved player into internal player
        }
    }
    public void NewUser() {
        if (ClientConnectionState == ConnectionState.AWAITING ||
            ClientConnectionState == ConnectionState.REJECTED) {
            StartCoroutine(AddUser());
        }
        
    }
    IEnumerator AddUser() {
        //creates data to send
        C4NO.PlayerLoginInfo data = new C4NO.PlayerLoginInfo();
        data.Username = GameObject.Find("NameField").GetComponent<Text>().text;
        data.Password = GameObject.Find("PasswordField").GetComponent<Text>().text;

        //converts data to Json, and then to byte array
        string classData = JsonUtility.ToJson(data);
        UTF8Encoding temp = new UTF8Encoding();
        byte[] bytes = temp.GetBytes(classData);

        //prepares web request
        UnityWebRequest req = UnityWebRequest.Put(m_AddPlayerURL, bytes);
        req.SetRequestHeader("Content-Type", "application/json");
        ClientConnectionState = ConnectionState.ADDING_USER;

        //waits for answer
        yield return req.SendWebRequest();

        //outcome
        if (req.isNetworkError || req.isHttpError) {
            Debug.Log(req.error);
            ClientConnectionState = ConnectionState.REJECTED;
        }
        else {
            ClientConnectionState = ConnectionState.USER_ADDED;
            //This worked. Encode retrieved player into internal player
        }
        
    }
}
