using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Collections;
using System.Text;
using C4NO;

public enum ConnectionState {AWAITING, 
                             FETCHING_USER,
                             USER_FETCHED,
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
    public ClientScript InnerClientScript { get; private set; }
    public static ClientManager Instance { get; private set; }

    [SerializeField] private string m_RetrievePlayerURL;
    [SerializeField] private string m_AddPlayerURL;

    void Awake() {
        ClientConnectionState = ConnectionState.AWAITING;
        if (Instance == null || Instance != this) { Instance = this; }
        //if (Instance != this) { Instance = this; }
        InnerClientScript = this.gameObject.GetComponent<ClientScript>();
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ClientConnectionState == ConnectionState.CONNECTED && SceneManager.GetActiveScene().buildIndex == 0) {
            SceneManager.LoadScene(1);
        }
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
        data.Username = GameObject.Find("Name").GetComponent<InputField>().text;
        data.Password = GameObject.Find("Password").GetComponent<InputField>().text;
        
        //converts data to Json, and then to byte array
        string classData = JsonUtility.ToJson(data);
        Debug.Log(classData);
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
            string result = "";
            if (req.isDone) {
                result = req.downloadHandler.text;
                Debug.Log(result);
                if (result == "Wrong Password") {
                    ClientConnectionState = ConnectionState.REJECTED;
                }
                else {
                    InnerClientScript.NetPlayer = JsonUtility.FromJson<C4NO.NetworkPlayer>(result);
                    ClientConnectionState = ConnectionState.USER_FETCHED;
                    InnerClientScript.ConnectToServer();
                    ClientConnectionState = ConnectionState.CONNECTING;
                }
            }
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
        data.Username = GameObject.Find("Name").GetComponent<Text>().text;
        data.Password = GameObject.Find("Password").GetComponent<Text>().text;

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

    public void exit() => Application.Quit();
}
