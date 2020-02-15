/***************************************************************
 * Coin Behaviour Script
 * By Hercules (HErC) Dias Campos (ID 101091070)
 * Created on February 19, 2020
 * Last Modified on February 20, 2020
 * 
 * Behaviour implemented to control Coin Physics
 * Also tracks who the parent was (needed for win/loss condition
 *     detection)
 * Worth noting that this script requires (as of now) no Update()
 * 
 ***************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CoinBehaviour : MonoBehaviour
{
    [SerializeField] private uint m_iCoinParent;

    public Rigidbody m_rb;

    void Awake() {

        m_rb = this.gameObject.GetComponent<Rigidbody>();

        if (m_rb){

            m_rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else {

            Debug.Log("Where's my Rigidbody???");
        }
    }

    public void SetPlayerOwner(uint playerID) {
        m_iCoinParent = playerID;
    }

    public void OnDetach() {

        this.gameObject.transform.parent = null;
        m_rb.constraints = RigidbodyConstraints.None;
        m_rb.constraints = RigidbodyConstraints.FreezeRotation  | 
                           RigidbodyConstraints.FreezePositionZ |
                           RigidbodyConstraints.FreezePositionX;
    }
}