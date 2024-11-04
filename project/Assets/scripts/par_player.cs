using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class par_player : NetworkBehaviour
{
    // Start is called before the first frame update
    public static int col_players = 0;
    public List<int> lt_fr_id = new List<int>();
    public int id = 0;

    public int count;


    void Start()
    {
        id = col_players;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
