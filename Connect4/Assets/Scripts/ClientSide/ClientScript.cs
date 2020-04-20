using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;
using System.Text;
using C4M;  //Network Messages
using C4NO; //Network Objects

public class ClientScript : MonoBehaviour
{
    public NetworkDriver m_Driver;
    public NetworkConnection m_Connection;
    public string m_ServerIP;
    public ushort m_ServerPort;
    public string m_InternalID;
    public C4NO.NetworkPlayer m_NetPlayer;
    public bool IsTurn { get; private set; }

    void Start()
    {

        m_Driver = NetworkDriver.Create();
        m_Connection = default(NetworkConnection);
        var endPoint = NetworkEndPoint.Parse(m_ServerIP, m_ServerPort);
        m_Connection = m_Driver.Connect(endPoint);

        //empty initialization
        m_InternalID = "";
        m_NetPlayer = default(C4NO.NetworkPlayer);
    }
    void SendToServer(string message)
    {
        var writer = m_Driver.BeginSend(m_Connection);
        NativeArray<byte> bytes = new NativeArray<byte>(Encoding.ASCII.GetBytes(message), Allocator.Temp);
        writer.WriteBytes(bytes);
        m_Driver.EndSend(writer);
    }
    void OnConnect()
    {
        //
    }
    void OnData(DataStreamReader stream)
    {
        NativeArray<byte> bytes = new NativeArray<byte>(stream.Length, Allocator.Temp);
        stream.ReadBytes(bytes);
        string recMsg = Encoding.ASCII.GetString(bytes.ToArray());
        MsgHeader header = JsonUtility.FromJson<MsgHeader>(recMsg);

        if ((header.cmd & Commands.SERVER_UPDATE) == Commands.SERVER_UPDATE)
        {
            ProcessServerMessage(recMsg);
        }
    }
    void Disconnect()
    {
        m_Connection.Disconnect(m_Driver);
        m_Connection = default(NetworkConnection);
    }
    void OnDisconnect()
    {
        m_Connection = default(NetworkConnection);
    }
    void OnDestroy()
    {
        m_Driver.Dispose();
    }
    void Update()
    {
        m_Driver.ScheduleUpdate().Complete();

        if (!m_Connection.IsCreated)
        {
            return;
        }

        DataStreamReader stream;
        NetworkEvent.Type cmd;
        cmd = m_Connection.PopEvent(m_Driver, out stream);
        while (cmd != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect) { OnConnect(); }
            else if (cmd == NetworkEvent.Type.Data) { OnData(stream); }
            else if (cmd == NetworkEvent.Type.Disconnect) { OnDisconnect(); }

            cmd = m_Connection.PopEvent(m_Driver, out stream);
        }

    }
    void ProcessServerMessage(string message)
    {
        MsgHeader header = JsonUtility.FromJson<MsgHeader>(message);
        switch (header.cmd ^ Commands.SERVER_UPDATE)
        {
            case Commands.ROOM_UPDATED:
                //Process room from added player
                break;
            case Commands.SLOT_REJECTED:
                //Continue playing
                break;
            case Commands.BOARD_UPDATE:
                //Update board
                break;
            case Commands.TURN_UPDATE:
                TurnUpdateMsg turnMsg = JsonUtility.FromJson<TurnUpdateMsg>(message);
                //decide what to use as a base
                IsTurn = turnMsg.playerTurn == m_NetPlayer.playerName;
                //update turn accordingly
                break;
            case Commands.GAME_SET:
                //Display Winner
                break;
        }
    }
    public void RequestSlot(int slot)
    {
        SlotRequestMsg msg = new SlotRequestMsg();
        msg.slot = slot;
        msg.player = m_NetPlayer;
        SendToServer(JsonUtility.ToJson(msg));
    }
}
