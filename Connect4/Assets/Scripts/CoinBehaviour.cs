/***************************************************************
 * Coin Behaviour Script
 * By Hercules (HErC) Dias Campos (ID 101091070)
 * Created:         February 15, 2020
 * Last Modified:   February 17, 2020
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
    public uint ParentId { get { return m_iCoinParent; } }

    public Rigidbody m_rb;

    void Awake() {

        m_rb = this.gameObject.GetComponent<Rigidbody>();
        m_rb.constraints = RigidbodyConstraints.FreezeAll;
        
    }

    public void SetPlayerOwner(uint playerID) {
        m_iCoinParent = playerID;
    }

    public void OnDetach() {

        this.gameObject.GetComponent<SphereCollider>().enabled = true;
        this.gameObject.transform.parent = null;
        m_rb.constraints = RigidbodyConstraints.None;
        m_rb.constraints = RigidbodyConstraints.FreezeRotation  | 
                           RigidbodyConstraints.FreezePositionZ |
                           RigidbodyConstraints.FreezePositionX;
    }
}