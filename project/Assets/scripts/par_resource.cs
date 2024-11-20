using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class par_resource : NetworkBehaviour
{
    // Start is called before the first frame update
    public List<Sprite> lt_rd_sp = new List<Sprite>();
    void Start()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger");
        if (collision.gameObject.GetComponent<par_mob>() != null && isLocalPlayer)
        {
            collision.gameObject.GetComponent<par_mob>().pl.add_res(collision.gameObject.name);
            NetworkServer.Destroy(gameObject);
        }
        //if (isLocalPlayer)
        //pl.print_res();
    }
    private void Awake()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = lt_rd_sp[Random.Range(0,lt_rd_sp.Count-1)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
