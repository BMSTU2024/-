using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class fn : NetworkManager
{
    public struct mes:NetworkMessage
    {
        public Vector2 v2;
        public GameObject gm;
        public player pl;
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<mes>(create_mob);
    }
    public void create_mob(NetworkConnectionToClient con, mes ms)
    {
        if (ms.gm == null)
        {
            Debug.Log("GM IS NULL");
            //return;
        }
        
        Vector2 v2 = Camera.main.ScreenToWorldPoint(ms.v2);
        GameObject gm = Instantiate(spawnPrefabs[0]);
        ms.pl.lt_gm.Add(gm);
        gm.GetComponent<sq>().pl = ms.pl;
        gm.transform.position = v2;
        NetworkServer.Spawn(gm);

        StopHost();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
