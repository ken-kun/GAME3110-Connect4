using System; //so we don't have to write System.Serializable every time
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Build.Player;
using UnityEngine;

namespace C4M { //Connect4 Messages

    [Flags] //Setting flags helps the system identify complex messasges
    public enum Commands {
        //Message's main type: Whether it comes from server or player
        NONE = 0,
        SERVER_UPDATE = 1,
        PLAYER_UPDATE = SERVER_UPDATE << 1,
        //Room Messages: for use by the server
        //Update examples include player added or removed.
        //The broadcast enables players to get a room ID
        ROOM_CREATED = PLAYER_UPDATE << 1, //what's the use of this, specifically?
        ROOM_UPDATED = PLAYER_UPDATE << 2,
        //Slot Management messages:
        //  player requests slot, game accepts or rejects it
        //  The game should always accept or reject a request 
        //  before updating the board
        SLOT_REQUESTED = ROOM_UPDATED << 1,
        SLOT_ACCEPTED = ROOM_UPDATED << 2,//may not be necessary
        SLOT_REJECTED = ROOM_UPDATED << 3,
        //Board Management messages: For use by the server
        TURN_UPDATE = SLOT_REJECTED << 1,
        BOARD_UPDATE = SLOT_REJECTED << 2,
        GAME_SET = SLOT_REJECTED << 3,
    }

    [Serializable] public class MsgHeader { public Commands cmd; }

    [Serializable]
    public class ServerMsg : MsgHeader {
        public ServerMsg() {
            cmd = Commands.SERVER_UPDATE;
        }
    }

    [Serializable]
    public class RoomCreatedMsg : ServerMsg {
        public int roomID;
        public RoomCreatedMsg() {
            cmd |= Commands.ROOM_CREATED;
        }
    }
    [Serializable]
    public class RoomUpdatedMsg : ServerMsg {
        public int roomID;
        public int capacity;
        public int occupancy;
        public C4NO.NetworkPlayer[] players;
        public RoomUpdatedMsg()
        {
            cmd |= Commands.ROOM_UPDATED;
            players = new C4NO.NetworkPlayer[2];
        }
    }
    [Serializable]
    public class SlotAcceptedMsg : ServerMsg {
        public SlotAcceptedMsg() {
            cmd |= Commands.SLOT_ACCEPTED;
        }
    }
    [Serializable]
    public class SlotRejectedMsg : ServerMsg {
        public SlotRejectedMsg()
        {
            cmd |= Commands.SLOT_REJECTED;
        }
    }
    [Serializable]
    public class BoardUpdateMsg : ServerMsg {
        public int[] slots;
        public BoardUpdateMsg() {
            cmd |= Commands.BOARD_UPDATE;
        }
    }
    [Serializable]
    public class TurnUpdateMsg : ServerMsg {
        //Basically, tells whose turn it is
        public string playerTurn;
        public TurnUpdateMsg() {
            cmd |= Commands.TURN_UPDATE;
        }
    }
    public class GameSetMsg : ServerMsg {
        public string winner;
        public GameSetMsg() {
            cmd |= Commands.GAME_SET;
        }
    }
    [Serializable]
    public class PlayerMsg : MsgHeader {
        public PlayerMsg() {
            cmd = Commands.PLAYER_UPDATE;
        }
    }

    public class SlotRequestMsg : PlayerMsg {
        public C4NO.NetworkPlayer player;
        public int slot;
        public SlotRequestMsg() {
            cmd |= Commands.SLOT_REQUESTED;
        }
    }
}

namespace C4NO { //Connect4 Network Objects

    [Serializable] public class NetworkObjects {
        public string id;
    }

    [Serializable] public class NetworkPlayer : NetworkObjects {
        //Database variables
        public int userID;
        public string Username;
        public int playerSlot;
        public int PlayerLevel;
        public float SkillLv;
        public int MatchesWon;
        public int MatchesLost;
        public int MatchesDrawn;

        //Room variables
        public bool isInRoom;
        public int playerRoom;

        public NetworkPlayer() {
            userID = 0;
            Username = "";
            playerSlot = -1;
            PlayerLevel = 0;
            SkillLv = 0;
            MatchesWon = 0;
            MatchesLost = 0;
            MatchesDrawn = 0;

            isInRoom = false;
            playerRoom = -1;
        }
    }
    [Serializable]
    public class PlayerLoginInfo {
        public string Username;
        public string Password;
    }
}