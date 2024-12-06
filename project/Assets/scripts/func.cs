using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using static par_mob;

public class func : NetworkManager
{
    //public static string url = ;
    public static int col_players = 0;
    //public func_sql f_sql;
    
    // Start is called before the first frame update
    public List<GameObject> lt_mobs = new List<GameObject>();
    public List<GameObject> lt_builds = new List<GameObject>();

    public List<par_mob.lt_mob_group> lt_mobs_group = new List< par_mob.lt_mob_group>();
    public List<Vector2> v2_start = new List<Vector2>();

    public List<GameObject> lt_prebafs = new List<GameObject>();
    public Dictionary<string,GameObject> base_obj = new Dictionary<string,GameObject>();
    public List<GameObject> obj_sql = new List<GameObject>();

    public Dictionary<NetworkConnectionToClient,par_player> lt_clients = new Dictionary<NetworkConnectionToClient, par_player>();
    public Dictionary<NetworkConnectionToClient, string> lt_clients_name = new Dictionary<NetworkConnectionToClient, string>();
    public par_player player_win = null;
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
            gm.name = "baza";
            gm.GetComponent<par_player>().id = con.connectionId;
            gm.GetComponent<par_player>().name = lt_clients_name[con];
            gm.GetComponent<par_build>().pl = gm.GetComponent<par_player>();
            lt_clients.Add(con, gm.GetComponent<par_player>());
            lt_builds.Add(gm);
            Vector2 v2= v2_start[Random.Range(0, v2_start.Count - 1)];
            gm.transform.position = v2;
            
            NetworkServer.AddPlayerForConnection(con, gm);
            create_object("worker",lt_clients[con],v2+Vector2.right);
            create_object("warrior", lt_clients[con], v2 + Vector2.up);
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
        //if(false)

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
                //bl = false;
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
    public struct stop_play_message : NetworkMessage
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
    public struct st_create_obj : NetworkMessage
    {
        public string name;
        public Vector2 v2;
        public par_player pl;
    }
    public struct st_res : NetworkMessage
    {
        public string type;
        public int znak;
        public par_player pl;
    }
    public struct st_mob_vis : NetworkMessage
    {
        public GameObject gm_cel;
        public par_mob mob_now;
    }
    public struct st_del_obj : NetworkMessage
    {
        public GameObject gm;
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        //NetworkServer.RegisterHandler<n_message>(create_player);
        NetworkServer.RegisterHandler<start_play_message>(connect_player);
        NetworkServer.RegisterHandler<struct_group_create>(create_group);
        NetworkServer.RegisterHandler<struct_group_delete>(delete_group);
        NetworkServer.RegisterHandler<st_create_obj>(create_obj_from);
        NetworkServer.RegisterHandler<st_res>(add_del_res);
        NetworkServer.RegisterHandler<st_mob_vis>(return_mob_vis);
        NetworkServer.RegisterHandler<st_del_obj>(del_obj);

        foreach (GameObject gm in spawnPrefabs)
        {
            Debug.Log(gm.name);
            base_obj.Add(gm.name, gm);
        }
        //NetworkServer.Spawn();
    }
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        //lt_clients.Remove()
    }
    public override void OnStopServer()
    {
        base.OnStopServer();


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
        lt_clients_name.Add(conn, mes.name);
        col_players++;
        if (col_players == 2)
        {
            StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().post_battle_start("http://localhost/DBUnity/set_battle.php"));
            generate();

        }
           
        //NetworkServer.AddPlayerForConnection(conn, gm);
    }
    public void create_obj_from(NetworkConnectionToClient conn, st_create_obj mes)
    {
        par_player pl = mes.pl;
        if (pl == null)
            pl = lt_clients[conn];
        Vector2 v2 = mes.v2;
        if(v2==new Vector2(-1,-1))
            v2 = (Vector2)pl.gameObject.transform.position + Vector2.right;  
        if(mes.pl)
        create_object(mes.name, mes.pl, mes.v2);
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
        
        if (gm.GetComponent<par_mob>() != null && pl != null)
        {
            gm.GetComponent<par_mob>().pl = pl;
            //gm.GetComponent<par_mob>().func_now = this;
            lt_mobs.Add(gm);
        }
        else if(gm.GetComponent<par_build>() != null)
        {
            gm.GetComponent<par_build>().pl = pl;
            //gm.GetComponent<par_mob>().func_now = this;
            lt_builds.Add(gm);
        }
            NetworkServer.Spawn(gm);

    }
    public void del_obj(NetworkConnectionToClient con, st_del_obj mb)
    {
        if (mb.gm.GetComponent<par_mob>() != null)
        {
            lt_mobs.Remove(mb.gm);
            mb.gm.GetComponent<par_mob>().del_group(false);
        }
        else if (mb.gm.GetComponent<par_build>() != null)
        {
            lt_builds.Remove(mb.gm);
            //mb.gm.GetComponent<par_build>().del_group(false);
        }
        if(mb.gm.name!="baza")
            NetworkServer.Destroy(mb.gm);
        else
        {
            List<par_player> lt_pl = new List<par_player> ();
            string pl_win = null;
            string pl_lose = null;
            foreach (NetworkConnectionToClient con1 in lt_clients.Keys)
            {
                if (mb.gm.GetComponent<par_build>().pl != lt_clients[con1])
                {
                    lt_clients[con1].proverka_win(con1, true);
                    pl_win = lt_clients_name[con1];
                }
                    
                else
                {
                    lt_clients[con1].proverka_win(con1, false);
                    pl_lose = lt_clients_name[con1];
                }
            }

            lt_mobs.Clear();
            lt_builds.Clear();
            StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().post_battle_end("http://localhost/DBUnity/set_battle.php",pl_win,pl_lose));
            try
            {
                //foreach (NetworkConnectionToClient con_ in lt_clients.Keys)
                //{
                    //OnClientDisconnect();
                //}
                //StopClient();
                //StopServer();
            }
            catch (System.Exception)
            {

                //throw;
            }
            //
            //pl_lose.proverka_win(false);
        }
    }
    [Server]
    public void add_del_res(NetworkConnectionToClient con, st_res gr)
    {
        Debug.Log(gr.pl);
        gr.pl.col_res[gr.type]+=gr.znak;
        //print_res();
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

    [Server]
    public void return_mob_vis(NetworkConnectionToClient con, st_mob_vis st)
    {
        GameObject gm_now = null;
        //Debug.Log(lt_mobs.Count);
        foreach (GameObject gm in lt_mobs)
        {
            if (gm_now!=null && st.mob_now.gameObject.name=="warrior")
            {
                //Debug.Log(Vector2.Distance(gm.transform.position, st.mob_now.gameObject.transform.position)+"  distance");
            }
            if (gm.GetComponent<par_mob>().pl!= st.mob_now.pl)
                if ( (gm_now==null || Vector2.Distance(gm.transform.position, st.mob_now.gameObject.transform.position) < Vector2.Distance(gm_now.transform.position, st.mob_now.gameObject.transform.position)   ) && Vector2.Distance(gm.transform.position, st.mob_now.gameObject.transform.position)<st.mob_now.r_find_mob   )
                    gm_now = gm;
        }
        foreach (GameObject gm in lt_builds)
        {
            if (gm.GetComponent<par_build>().pl != st.mob_now.pl)
                if ((gm_now == null || Vector2.Distance(gm.transform.position, st.mob_now.gameObject.transform.position) < Vector2.Distance(gm_now.transform.position, st.mob_now.gameObject.transform.position)) && Vector2.Distance(gm.transform.position, st.mob_now.gameObject.transform.position) < st.mob_now.r_find_mob)
                    gm_now = gm;
        }
        if (gm_now != null)
        {
            st.gm_cel = gm_now;
            st.mob_now.cel = gm_now;
            st.mob_now.v2_wolk = gm_now.transform.position;
        }

        //Debug.Log(gm_now);

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
