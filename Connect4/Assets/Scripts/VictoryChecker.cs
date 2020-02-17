/***************************************************************
 * Victory Checker Script
 * By Hercules (HErC) Dias Campos (ID 101091070)
 * Created:         February 16, 2020
 * Last Modified:   February 16, 2020
 * 
 * Victory Detector Static Class
 * Responsible for telling if the coins are aligned 
 *  and who the winning player is.
 * 
 **************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VictoryChecker
{
    private static Color m_WinColor = new Color(1.0f, 1.0f, 0.0f);
    public static bool CheckHorizontalWin(int playerId, int index, GameObject[] grid) {

        if (grid[index].GetComponent<DetectorBehaviour>().OwnerId   == playerId &&
            grid[index+1].GetComponent<DetectorBehaviour>().OwnerId == playerId &&
            grid[index+2].GetComponent<DetectorBehaviour>().OwnerId == playerId &&
            grid[index+3].GetComponent<DetectorBehaviour>().OwnerId == playerId) {
            Debug.Log("Player " + playerId + " wins with match from index" + index + " to index " + (index+3));
            return true;
        }
        return false;
    }

    public static bool CheckVerticalWin(int playerId, int index, GameObject[] grid) {

        if (grid[index].GetComponent<DetectorBehaviour>().OwnerId      == playerId &&
            grid[index +  7].GetComponent<DetectorBehaviour>().OwnerId == playerId &&
            grid[index + 14].GetComponent<DetectorBehaviour>().OwnerId == playerId &&
            grid[index + 21].GetComponent<DetectorBehaviour>().OwnerId == playerId) {
            Debug.Log("Player " + playerId + " wins with match from index" + index + " to index " + (index + 21));
            return true;
        }

        return false;
    }

    public static bool CheckUpRightWin(int playerId, int index, GameObject[] grid) {

        if (grid[index].GetComponent<DetectorBehaviour>().OwnerId      == playerId &&
            grid[index +  8].GetComponent<DetectorBehaviour>().OwnerId == playerId &&
            grid[index + 16].GetComponent<DetectorBehaviour>().OwnerId == playerId &&
            grid[index + 24].GetComponent<DetectorBehaviour>().OwnerId == playerId) {
            Debug.Log("Player " + playerId + " wins with match from index" + index + " to index " + (index + 24));
            return true;
        }
        return false;
    }

    public static bool CheckUpLeftWin(int playerId, int index, GameObject[] grid) {

        if (grid[index].GetComponent<DetectorBehaviour>().OwnerId      == playerId &&
            grid[index +  6].GetComponent<DetectorBehaviour>().OwnerId == playerId &&
            grid[index + 12].GetComponent<DetectorBehaviour>().OwnerId == playerId &&
            grid[index + 18].GetComponent<DetectorBehaviour>().OwnerId == playerId) {
            Debug.Log("Player " + playerId + " wins with match from index" + index + " to index " + (index + 18));
            return true;
        }
        return false;
    }
}
