using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using static par_mob;

public class func : NetworkManager
{
    //public static string url = ;
    public static int col_players = 0;
    //public func_sql f_sql;
    
    // Start is called before the first frame update
    public static List<GameObject> lt_mobs = new List<GameObject>();
    
    public List<par_mob.lt_mob_group> lt_mobs_group = new List< par_mob.lt_mob_group>();
    public List<Vector2> v2_start = new List<Vector2>();

    public List<GameObject> lt_prebafs = new List<GameObject>();
    public Dictionary<string,GameObject> base_obj = new Dictionary<string,GameObject>();
    public List<GameObject> obj_sql = new List<GameObject>();

    public Dictionary<NetworkConnectionToClient,par_player> lt_clients = new Dictionary<NetworkConnectionToClient, par_player>();
    public List<NetworkConnectionToClient>lt_con = new List<NetworkConnectionToClient>();
    public static int size_global = 20;
    [Server]
    public void generate()
    {
        Debug.Log("start generate");
        List<Vector2> lt_st= new List<Vector2>();
        int id = 0;
        foreach (NetworkConnectionToClient con in lt_con)
        {

            Debug.Log("create player");
            GameObject gm = Instantiate(playerPrefab);
            gm.GetComponent<par_player>().id = con.connectionId;
            lt_clients.Add(con, gm.GetComponent<par_player>());
            Vector2 v2= v2_start[Random.Range(0, v2_start.Count - 1)];
            gm.transform.position = v2;
            
            NetworkServer.AddPlayerForConnection(con, gm);
            create_object("worker",lt_clients[con],v2+Vector2.right);
            //create_object("worker", lt_clients[con], v2 + Vector2.down);
            lt_st.Add(v2);
            v2_start.Remove(v2);
            //id++;
        }
        for (int i = 0; i < size_global-1; i++)
        {
            create_object("rock", new Vector2(i,0));
            create_object("rock", new Vector2(0, size_global-i-1));
            create_object("rock", new Vector2(size_global - i-1, size_global-1));
            create_object("rock", new Vector2(size_global-1,  i));
        }
        for (int i = 1; i < size_global-1; i++)
        {
            for (int j = 1; j < size_global-1; j++)
            {
                create_object("eath", new Vector2(j, i));
                bool bl = true;
                foreach (Vector2 v2 in lt_st)
                {
                    int x = (int)Mathf.Abs(v2.x - j);
                    int y=(int)Mathf.Abs(v2.y - i);
                    if (x<3 && y<3)
                    {
                        bl = false;
                        break;
                    }
                }
                if (bl)
                {
                    if(Random.Range(0,10)!=0)
                        create_object("rock", new Vector2(j, i));
                    else
                        create_object("ruda", new Vector2(j, i));
                }
            }
        }
    }
    public struct n_message: NetworkMessage
    {
        public Vector2 v2;
    }
    public struct start_play_message : NetworkMessage
    {
        public string name;
    }
    public struct struct_group_create : NetworkMessage
    {
        public List<GameObject> lt_gm;
        public GameObject cel;
        public Vector2 v2;
    }
    public struct struct_group_delete : NetworkMessage
    {
        public par_mob.lt_mob_group gr;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        //NetworkServer.RegisterHandler<n_message>(create_player);
        NetworkServer.RegisterHandler<start_play_message>(connect_player);
        NetworkServer.RegisterHandler<struct_group_create>(create_group);
        NetworkServer.RegisterHandler<struct_group_delete>(delete_group);

        foreach (GameObject gm in spawnPrefabs)
        {
            Debug.Log(gm.name);
            base_obj.Add(gm.name, gm);
        }
        //NetworkServer.Spawn();
    }
    /*
    public void create_player(NetworkConnectionToClient conn,n_message mes)
    {
        Debug.Log("create player");
        GameObject gm = Instantiate(playerPrefab);
        gm.transform.position = mes.v2;
        gm.GetComponent<par_player>().id = col_players;
        NetworkServer.AddPlayerForConnection(conn, gm);
    }
    */
    public void connect_player(NetworkConnectionToClient conn, start_play_message mes)
    {
        Debug.Log("connect player " + mes.name);
        lt_con.Add(conn);
        col_players++;
        if(col_players==2)
            generate();
        //NetworkServer.AddPlayerForConnection(conn, gm);
    }
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        NetworkClient.Send(new start_play_message() {name= data_sql.player_now.login });
        //NetworkClient.Send(new n_message() { v2= Vector2.zero});
        //GameObject gm = Instantiate(playerPrefab);
        //NetworkServer.AddPlayerForConnection(conn, gm);
    }
    [Server]
    public void create_object(string name,Vector2 v2)
    {
        create_object(name, null, v2);
    }
    [Server]
    public void create_object(string name,par_player pl, Vector2 v2)
    {
        GameObject gm = Instantiate(base_obj[name]);
        gm.name = name;
        gm.transform.position = v2;
        NetworkServer.Spawn(gm);
        if (gm.GetComponent<par_mob>() != null && pl!=null)
            gm.GetComponent<par_mob>().pl = pl;

    }
    [Server]
    public void create_group(NetworkConnectionToClient con, struct_group_create gr)
    {
        Debug.Log("create group");
        if (lt_clients[con].gm_selected.Count != 0)
        {
            Debug.Log("create group 1");
            List<par_mob> lt_mbs = new List<par_mob>();
            lt_mob_group gr_now = new lt_mob_group(gr.v2, gr.cel);
            foreach (GameObject gm in lt_clients[con].gm_selected)
            {
                par_mob mb = gm.GetComponent<par_mob>();
                lt_mbs.Add(mb);
                mb.group = gr_now;
                mb.cel = gr.cel;

            }
            gr_now.add_list(new List<GameObject>(lt_clients[con].gm_selected));
            lt_mobs_group.Add(gr_now);
        }
        
    }
    [Server]
    public void delete_group(NetworkConnectionToClient con, struct_group_delete gr_del)
    {

        foreach (GameObject gm in gr_del.gr.lt)
        {
            par_mob mb = gm.GetComponent<par_mob>();
            mb.v2_wolk = Vector2.zero;
            mb.group = null;
        }
        lt_mobs_group.Remove(gr_del.gr);
    }
    /*
    [Server]
    public void Start()
    {
        foreach (GameObject gm in spawnPrefabs)
        {
            Debug.Log(gm.name);
            base_obj.Add(gm.name, gm);
        }
    }*/
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(count);
    }
}
