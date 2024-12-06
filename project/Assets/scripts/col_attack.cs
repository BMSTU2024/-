using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class col_attack : NetworkBehaviour
{
    // Start is called before the first frame update
    [SyncVar]
    public List<GameObject> gm_entered = new List<GameObject>();
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!gm_entered.Contains(collision.gameObject) &&
            ((collision.gameObject.GetComponent<par_mob>()!=null && collision.gameObject.GetComponent<par_mob>().pl!=gameObject.transform.parent.parent.gameObject.GetComponent<par_mob>().pl) ||
            (collision.gameObject.GetComponent<par_build>() != null && collision.gameObject.GetComponent<par_build>().pl != gameObject.transform.parent.parent.gameObject.GetComponent<par_mob>().pl))
            && isServer)
            gm_entered.Add(collision.gameObject);
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (gm_entered.Contains(collision.gameObject) && isServer)
            gm_entered.Remove(collision.gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
