using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class par_build : NetworkBehaviour
{
    // Start is called before the first frame update

    [SyncVar]
    public int hp;
    [SyncVar]
    public par_player pl;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
