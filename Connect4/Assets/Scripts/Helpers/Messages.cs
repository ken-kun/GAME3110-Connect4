using System; //so we don't have to write System.Serializable every time
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace C4M { //Connect4 Messages

    [Flags] //Setting flags helps the system identify complex messasges
    public enum Commands {
        //Message's main type: Whether it comes from server or player
        SERVER_UPDATE   = 0,
        PLAYER_UPDATE   = 1,
        //Room Messages: for use by the server
        ROOM_CREATED    = PLAYER_UPDATE << 1,
        PLAYER_ADDED    = PLAYER_UPDATE << 2,
        PLAYER_REMOVED  = PLAYER_UPDATE << 3,
        ROOM_UPDATED    = PLAYER_UPDATE << 4,
        //Slot Management messages:
        //  player requests slot, game accepts or rejects it
        SLOT_REQUESTED  = ROOM_UPDATED << 1,
        SLOT_ACCEPTED   = ROOM_UPDATED << 2,
        SLOT_REJECTED   = ROOM_UPDATED << 3,
        //Board Management messages: For use by the server
        TURN_UPDATE     = SLOT_REJECTED << 1,
        BOARD_UPDATE    = SLOT_REJECTED << 2,
        GAME_SET        = SLOT_REJECTED << 3,
    }

    [Serializable] public class MsgHeader { public Commands cmd; }



    [Serializable] public class InputMsg : MsgHeader{
        public C4NO.NetworkPlayer player;
        InputMsg() {
            cmd = Commands.PLAYER_UPDATE;
            player = new C4NO.NetworkPlayer();
        }
    }

    [Serializable] public class TurnMsg : MsgHeader {
        public bool currentTurn;
        TurnMsg() {
            cmd = Commands.SERVER_UPDATE;
            currentTurn = false;
        }
    }

    [Serializable] public class BoardUpdateMsg : MsgHeader {
        //Board Updates can also be used to set player victory
        public int[] boardState;
        BoardUpdateMsg() {
            cmd = Commands.SERVER_UPDATE;
        }
    }
}

namespace C4NO { //Connect4 Network Objects

    [Serializable] public class NetworkObjects {
        public string id;
    }

    [Serializable] public class NetworkPlayer {
        public int playerRoom;
        public string playerName;
        public int playerSlot;
    }
}