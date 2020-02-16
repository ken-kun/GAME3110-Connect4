/***************************************************************
 * Detector Script
 * By Hercules (HErC) Dias Campos (ID 101091070)
 * Created:         February 16, 2020
 * Last Modified:   February 16, 2020
 * 
 * Slot Detector class
 * Responsible for telling if a coin is currently in its slot,
 * and also setting its ID to that of the currently slotted coin
 * 
 *      ***WILL REQUIRE REFINEMENT***
 *      ***WILL ALSO REQUIRE BETTER MANAGEMENT***
 * 
 **************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class DetectorBehaviour : MonoBehaviour
{
    private int m_iOwnerId;

    void Awake() {
        m_iOwnerId = -1;
    }

    void OnTriggerEnter(Collider other) {
        CoinBehaviour coin = other.gameObject.GetComponent<CoinBehaviour>();
        if (coin) {
            m_iOwnerId = (int)coin.ParentId;
        }
        Debug.Log("Coin Entered...");
    }

    void OnTriggerExit(Collider other) {
        m_iOwnerId = -1;
        Debug.Log("Coin Exited...");
    }
}
