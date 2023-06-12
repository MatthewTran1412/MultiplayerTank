using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TurnToItem : NetworkBehaviour
{
    public GameObject[] item;
    Vector3 newPosition;
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag=="Ground")
        {
            newPosition= new Vector3(transform.position.x,0,transform.position.z);
            TransformItemClientRpc();
        }
    }
    [ClientRpc]
    private void TransformItemClientRpc()
    {
        Instantiate(item[Random.Range(0,item.Length)],newPosition,transform.rotation);
        Destroy(gameObject);
    }
}
