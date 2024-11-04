using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class func : NetworkManager
{
    //public static string url = ;
    public static int col_players = 0;
    //public func_sql f_sql;
    
    // Start is called before the first frame update
    public static List<GameObject> lt_mobs = new List<GameObject>();
    public List<Vector2> v2_start = new List<Vector2>();

    public List<GameObject> lt_prebafs = new List<GameObject>();
    public Dictionary<string,GameObject> base_obj = new Dictionary<string,GameObject>();
    public List<GameObject> obj_sql = new List<GameObject>();

    public List<NetworkConnectionToClient> lt_clients = new List<NetworkConnectionToClient>();
    public static int size_global = 20;
    [Server]
    public void generate(List<NetworkConnectionToClient> lt)
    {
        Debug.Log("start generate");
        foreach (NetworkConnectionToClient con in lt)
        {
            Debug.Log("create player");
            GameObject gm = Instantiate(playerPrefab);
            Vector2 v2= v2_start[Random.Range(0, v2_start.Count - 1)];
            gm.transform.position = v2;
            NetworkServer.AddPlayerForConnection(con, gm);
            v2_start.Remove(v2);
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


    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<n_message>(create_player);
        NetworkServer.RegisterHandler<start_play_message>(connect_player);

        foreach (GameObject gm in spawnPrefabs)
        {
            Debug.Log(gm.name);
            base_obj.Add(gm.name, gm);
        }
        //NetworkServer.Spawn();
    }
    public void create_player(NetworkConnectionToClient conn,n_message mes)
    {
        Debug.Log("create player");
        GameObject gm = Instantiate(playerPrefab);
        gm.transform.position = mes.v2;
        NetworkServer.AddPlayerForConnection(conn, gm);
    }
    public void connect_player(NetworkConnectionToClient conn, start_play_message mes)
    {
        Debug.Log("connect player " + mes.name);
        lt_clients.Add(conn);
        col_players++;
        if(col_players==2)
            generate(lt_clients);
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
        GameObject gm = Instantiate(base_obj[name]);
        gm.transform.position = v2;
        NetworkServer.Spawn(gm);
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
    [Server]
    void Update()
    {
        //Debug.Log(count);
    }
}
