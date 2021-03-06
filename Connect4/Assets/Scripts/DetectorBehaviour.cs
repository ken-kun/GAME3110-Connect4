﻿/***************************************************************
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
    public bool HasOwner { get { return m_iOwnerId != -1; } }

    void Awake() {
        m_iOwnerId = -1;
    }

    public bool SetOwner(int ID) {
        if (m_iOwnerId < 0) {
            m_iOwnerId = ID;
        }
        return HasOwner;
    }
}
