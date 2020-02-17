/***************************************************************
 * Detector Script
 * By Hercules (HErC) Dias Campos (ID 101091070)
 * Created:         February 16, 2020
 * Last Modified:   February 17, 2020
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

//[RequireComponent(typeof(SphereCollider))]
//Will keep the collider as is for now to aid visualization
public class DetectorBehaviour : MonoBehaviour
{
    //SERIALIZED FOR VISUALIZATION PURPOSES ONLY
    [SerializeField] private int m_iOwnerId; 
    public int OwnerId { get { return m_iOwnerId; } }
    //SERIALIZED FOR VISUALIZATION PURPOSES ONLY
    [SerializeField] private bool m_bHasOwner;
    public bool HasOwner { get { return m_bHasOwner; } }

    void Awake() {
        m_iOwnerId = -1;
        m_bHasOwner = false;
    }

    public bool SetOwner(int OwnerID) {
        if (m_iOwnerId < 0) {
            m_iOwnerId = OwnerID;
            m_bHasOwner = true;
        }
        return m_bHasOwner;
    }

    //Remnants of an old physics system...
    /*
    void OnTriggerEnter(Collider other) {
        CoinBehaviour coin = other.gameObject.GetComponent<CoinBehaviour>();
        if (coin) {
            m_iOwnerId = (int)coin.ParentId;
        }
    }

    void OnTriggerExit(Collider other) {
        m_iOwnerId = -1;
    }*/
}
