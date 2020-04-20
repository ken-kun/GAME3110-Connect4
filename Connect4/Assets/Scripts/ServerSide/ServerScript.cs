/***************************************************************
 * Server Script
 * By Hercules (HErC) Dias Campos (ID 101091070)
 * Created:         April 08, 2020
 * Last Modified:   April 08, 2020
 * 
 * Overall Server Manager class
 * Manages server, connections and room management.
 * 
 * On player connection (hopefully implemented correctly by the
 * matchmaker server), the server script creates a room, or 
 * redirects a player to a room where a suitable opponent is
 * waiting.
 * 
 * To that end, it keeps tabs on how many players are connected 
 * and how many rooms it has.
 * 
 **************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Collections;
using Unity.Networking.Transport;
using C4M; //Connect4 Messages
using C4NO; //Connect4 Network Objects
using System.Text;
using System.Threading;
using UnityEngine.Networking;

public class ServerScript : MonoBehaviour
{
    public NetworkDriver m_Driver;
    public ushort m_ServerPort;
    private NativeList<NetworkConnection> m_Connections;

    private List<C4NO.NetworkPlayer> m_NetworkPlayers;
    [SerializeField, Tooltip("Max skill variance tolerance for players " +
                             "to join the same room")]
    private float m_tolerance; 
    private List<RoomManager> m_Rooms;

    void Awake()
    {
        m_Driver = NetworkDriver.Create();
        var endpoint = NetworkEndPoint.AnyIpv4;
        endpoint.Port = m_ServerPort;
        if (m_Driver.Bind(endpoint) != 0) {
            //flag error
        }
        else {
            m_Driver.Listen();
        }
        m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);
        m_NetworkPlayers = new List<C4NO.NetworkPlayer>(16);
    }
    // Start is called before the first frame update
    void Start() {
    
    }
    void Update()
    {
        m_Driver.ScheduleUpdate().Complete();

        //connection cleanup
        for (int i = 0; i < m_Connections.Length; ++i) {
            if (!m_Connections[i].IsCreated) {
                m_Connections.RemoveAtSwapBack(i);
                --i;
            }
        }

        //accepting new connections
        NetworkConnection c = m_Driver.Accept();
        while (c!= default(NetworkConnection)) {
            OnConnect(c);
            c = m_Driver.Accept();
        }

        //Incoming messages reading
        DataStreamReader stream;
        for (int i = 0; i < m_Connections.Length; ++i) {
            Assert.IsTrue(m_Connections[i].IsCreated);

            NetworkEvent.Type eventType;
            eventType = m_Driver.PopEventForConnection(m_Connections[i], out stream);
            while (eventType != NetworkEvent.Type.Empty) {
                if (eventType == NetworkEvent.Type.Data) {
                    OnData(stream, i);
                }
                else if (eventType == NetworkEvent.Type.Disconnect) {
                    OnDisconnect(i);
                }
                eventType = m_Driver.PopEventForConnection(m_Connections[i], out stream);
            }
        }
    }
    //Room Management Functions:
    void AddRoom() {
        m_Rooms.Add(new RoomManager());
        m_Rooms[m_Rooms.Count - 1].roomID = m_Rooms.Count - 1;
    }

    bool AddPlayerToRoom(C4NO.NetworkPlayer player, RoomManager room) {

        if (room.IsEmpty) {
            //Shorthand: adding a player to a room returns an int,
            //which can be assigned back to the playerRoom attribute
            return ((player.playerRoom = room.AddPlayer(player.id)) != -1);
        }
        else if (room.HasRoom) {
            foreach (C4NO.NetworkPlayer netPlayer in m_NetworkPlayers) {
                if (netPlayer.playerRoom == room.roomID) {
                    if (Mathf.Abs(player.SkillLv - netPlayer.SkillLv) < m_tolerance) {
                        return ((player.playerRoom = room.AddPlayer(player.id)) != -1);
                    }
                }
            }
        }
        return false;
    }
    //Message & Network Management Functions
    void SendToClient(string message, NetworkConnection c) {
        var writer = m_Driver.BeginSend(NetworkPipeline.Null, c);
        NativeArray<byte> bytes = new NativeArray<byte>(Encoding.ASCII.GetBytes(message), Allocator.Temp);
        writer.WriteBytes(bytes);
        m_Driver.EndSend(writer);
    }
    public void OnDestroy() {
        m_Driver.Dispose();
        m_NetworkPlayers.Clear();
        m_Connections.Dispose();
    }
    void OnConnect(NetworkConnection c) {
        m_Connections.Add(c);
        m_NetworkPlayers.Add(new C4NO.NetworkPlayer());
        //REQUIRES LAMBDA: RETRIEVE PLAYER INFO
            //DEFINE STUFF here
        foreach (RoomManager room in m_Rooms) {
            //Shorthand again. If the player is added to a room, the attribute changes
            //this means that the player has a room. This is enough to break out of the loop
            if ((m_NetworkPlayers[m_NetworkPlayers.Count-1].isInRoom = AddPlayerToRoom(m_NetworkPlayers[m_NetworkPlayers.Count - 1], room)) == true) {
                //Create server update message
                break;
            }
        }
        if (!m_NetworkPlayers[m_NetworkPlayers.Count - 1].isInRoom) {
            AddRoom();
            m_NetworkPlayers[m_NetworkPlayers.Count - 1].isInRoom = AddPlayerToRoom(m_NetworkPlayers[m_NetworkPlayers.Count - 1], m_Rooms[m_Rooms.Count - 1]);
            //create server update message
        }
    }
    void OnData(DataStreamReader stream, int i) {
        NativeArray<byte> bytes = new NativeArray<byte>(stream.Length, Allocator.Temp);
        stream.ReadBytes(bytes);
        string message = Encoding.ASCII.GetString(bytes.ToArray());
        MsgHeader header = JsonUtility.FromJson<MsgHeader>(message);

        if ((header.cmd & Commands.PLAYER_UPDATE) == Commands.PLAYER_UPDATE) {
            ProcessPlayerMessage(message);
        }
    }
    void OnDisconnect(int i) {
        ProcessDisconnect(i);
        m_Connections[i] = default(NetworkConnection);
    }
    void ProcessPlayerMessage(string msg) {
        MsgHeader header = JsonUtility.FromJson<MsgHeader>(msg);

        switch (header.cmd ^ Commands.PLAYER_UPDATE) {
            case Commands.SLOT_REQUESTED:

                break;
        }
    }
    void ProcessSlotRequest(SlotRequestMsg msg) {
        
    }
    void ProcessDisconnect(int i) {
        //cleanup
    }
}
