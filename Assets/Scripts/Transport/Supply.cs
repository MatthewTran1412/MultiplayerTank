using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Supply : NetworkBehaviour
{
    public GameObject supply;
    private Rigidbody rb;
    public float speed;
    public float SpawnTime;
    public float timer;
    public float durationTime;
    // Start is called before the first frame update
    void Start()
    {
        SpawnTime=Random.Range(10,20);
        rb=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveMent();
        if(!IsServer)
            return;
        timer+=Time.deltaTime;
        if(timer>=SpawnTime)
        {
            SpawnSupplyClientRpc();
            timer=0;
        }
    }
    [ClientRpc]
    private void SpawnSupplyClientRpc()
    {
        Instantiate(supply,transform.position,transform.rotation);
        Destroy(gameObject,durationTime);
    }
    private void MoveMent()
    {
        Vector3 movement = transform.forward*speed*Time.deltaTime;
        rb.MovePosition(rb.position+movement);
    }
}
