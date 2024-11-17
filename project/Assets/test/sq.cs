using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class sq : NetworkBehaviour
{
    // Start is called before the first frame update
    [SyncVar]
    public Vector2 v2_wolk;
    [SyncVar]
    public player pl;
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        if(v2_wolk!= Vector2.zero)
        {
            Vector2 v2_d = (v2_wolk - (Vector2)transform.position) / Vector2.Distance(v2_wolk, (Vector2)transform.position) * Time.deltaTime * 1;
            if (Vector2.Distance(v2_wolk, (Vector2)transform.position) <= v2_d.magnitude)
            {
                v2_d = v2_wolk - (Vector2)transform.position;
                v2_wolk = Vector2.zero;
                //delete_group(group);
                //gameObject.GetComponent<Animator>().SetBool("wolk", false);

            }
            Vector2 v2 = (Vector2)transform.position + v2_d;
            transform.position = v2;
        }
        if (Input.GetMouseButtonDown(1))
        {
                pl.set_client_v2_wolk(isServer,gameObject,Camera.main.ScreenToWorldPoint(Input.mousePosition));

        }

    }
}
