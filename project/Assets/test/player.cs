using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : NetworkBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> lt_gm;
    public int id;
    void Start()
    {
        //id = connectionToClient.connectionId;
    }
    [Server]
    public void set_v2_wolk(GameObject gm,Vector2 v2)
    {
        Debug.Log("SERVER");
        gm.GetComponent<sq>().v2_wolk = v2;
    }
    [Command]
    public void Cmd_v2_wolk(GameObject gm, Vector2 v2)
    {
        
        set_v2_wolk(gm,v2);
    }
    public void set_client_v2_wolk(bool bl,GameObject gm, Vector2 v2)
    {
        Debug.Log("COMMAND "+bl+" "+isOwned);
        if (bl)
            set_v2_wolk(gm, v2);
        else
            Cmd_v2_wolk(gm, v2);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isLocalPlayer)
        {
            NetworkClient.Send(new fn.mes { v2 = Input.mousePosition ,pl = this});


        }

    }
}
