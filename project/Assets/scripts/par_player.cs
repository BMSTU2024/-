using Mirror;
using Org.BouncyCastle.Asn1;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class par_player : NetworkBehaviour
{
    // Start is called before the first frame update
    public static int col_players = 0;
    public List<int> lt_fr_id = new List<int>();
    [SyncVar]
    public int id = 0;
    [SyncVar]
    public string name;

    public int count;


    public float speed_move = 1;

    public Vector2 v2_start;
    public static bool start_selected = false;
    [SyncVar]
    public List<GameObject> gm_selected = new List<GameObject>();
    public List<par_mob> gm_selected_pr = new List<par_mob>();

    [SyncVar]
    public SyncDictionary<string,int> col_res = new SyncDictionary<string,int>();

    
    [Command]
    public void set_gm_selected(List<GameObject> lt)
    {
            gm_selected= lt;
        
    }
    [Command]
    public void add_gm_selected(GameObject gm)
    {
            gm_selected.Add(gm);

    }
    public void add_res(string st)
    {
            add_res_ser(st);
        //print_res();
    }
    [Command]
    public void add_res_ser(string st)
    {
        if (col_res.ContainsKey(st))
        {
            col_res[st]++;

        }
        else
        {
            Debug.Log("add res " + st);
            //col_res.Add(st, 1);
        }
    }
    public void print_res()
    {
        TMP_Text tx = null;
        tx = GameObject.Find("pn res").transform.Find("col res rock").transform.Find("tx res").gameObject.GetComponent<TMP_Text>();
        tx.text = col_res["res rock"].ToString();
        tx = GameObject.Find("pn res").transform.Find("col res ruda").transform.Find("tx res").gameObject.GetComponent<TMP_Text>();
        tx.text = col_res["res ruda"].ToString();
    }

    [Command]
    public void del_res(string st,int col)
    {
        if (col_res.ContainsKey(st))
        {
            col_res[st]-=col;
        }
    }

    void Start()
    {
        if (isServer)
        {

            col_res.Add("res ruda", 0);
            col_res.Add("res rock", 0);
        }
        if (isLocalPlayer)
        {
            GameObject.Find("Canvas").transform.Find("pn").Find("pn mobs").Find("button mob create worker").Find("bt").gameObject.GetComponent<ui_button>().pl = this;
            GameObject.Find("Canvas").transform.Find("pn").Find("pn mobs").Find("button mob create warrior").Find("bt").gameObject.GetComponent<ui_button>().pl = this;
            //LayoutRebuilder.ForceRebuildLayoutImmediate(GameObject.Find("Canvas").transform.Find("pn").Find("pn mobs").gameObject.GetComponent<RectTransform>());
        }
        //id = (int)netId;
    }

    public static Vector2 v2_in_world(Vector3 v3)
    {
        return Camera.main.ScreenToWorldPoint(v3);
    }
    [TargetRpc]
    public void proverka_win(NetworkConnectionToClient con, bool bl)
    {
        if (bl)
        {
            GameObject.Find("Canvas").transform.Find("pn win").gameObject.active = true;
        }
        else
            GameObject.Find("Canvas").transform.Find("pn lose").gameObject.active = true;
        //OnStopClient();
    }
    public void create_mob(func.st_create_obj obj)
    {
        if (obj.name == "warrior")
        {
            //del_res("res rock", 10);
            //del_res("res ruda", 2);
            NetworkClient.Send(new func.st_res { znak = -10, type = "res rock",pl = obj.pl });
            NetworkClient.Send(new func.st_res { znak = -2, type = "res ruda", pl = obj.pl });
        }
        else if (obj.name == "worker")
        {
            NetworkClient.Send(new func.st_res { znak = -5, type = "res rock", pl = obj.pl });
        }
        NetworkClient.Send(obj);
    }
    // Update is called once per frame
    void Update()
    {
        

        if (isLocalPlayer)
        {

            if (Input.GetMouseButtonDown(0))
            {
                start_selected = true;
                v2_start = Input.mousePosition;
                Debug.Log("set lt -1 " +id);
                set_gm_selected(new List<GameObject>());
                Debug.Log("click mouse");
            }
            if (start_selected)
            {
                Debug.Log("sel id= " + id);
                //Vector2 v2_c = v2_in_world(Input.mousePosition) - v2_in_world(v2_start);
                RectTransform r_select = GameObject.Find("select").GetComponent<RectTransform>();


                r_select.anchoredPosition = (Vector2)Input.mousePosition - ((Vector2)Input.mousePosition - v2_start) / 2;
                Vector2 v2_sd = (Vector2)Input.mousePosition - v2_start;

                if (v2_sd.x < 0)
                    v2_sd.x *= -1;
                if (v2_sd.y < 0)
                    v2_sd.y *= -1;
                //Debug.Log(v2_sd);
                r_select.sizeDelta = v2_sd;
                //Collider[] ms_coll = Physics.OverlapBox(v3_c, new Vector3(r, r, r));
                Collider2D[] ms_coll = Physics2D.OverlapAreaAll(v2_in_world(v2_start), v2_in_world(Input.mousePosition));
                //ms_coll = Physics2D.OverlapAreaAll(new Vector2(0, 0), new Vector2(1, 1));

                //Debug.Log(NetworkConnectionToClient.LocalConnectionId);
                Debug.Log("set lt 0");
                //set_gm_selected( new List<GameObject>());
                foreach (Collider2D c in ms_coll)
                {
                    //
                    if (c.gameObject.GetComponent<par_mob>() != null)
                    {
                        //Debug.Log(c.gameObject.GetComponent<par_mob>().pl + "   " + id + "   select id");
                        if (c.gameObject.GetComponent<par_mob>().pl == this && !gm_selected.Contains(c.gameObject))
                        {
                            Debug.Log("set lt 0-1");
                            //gm_selected.Add(c.gameObject);
                            add_gm_selected(c.gameObject);
                        }

                            
                        //gm_selected_pr.Add(c.gameObject.GetComponent<par_mob>());
                    }

                }
                Debug.Log("set lt 1");
                //set_gm_selected(gm_selected);
            }
            if (Input.GetMouseButtonUp(0) && start_selected)
            {
                start_selected = false;
                GameObject.Find("select").GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                v2_start = Input.mousePosition;
                //set_gm_selected(new List<GameObject>());
            }
            /*
            Debug.Log(Input.GetMouseButtonDown(1) + " " + gm_selected.Count);
            if(Input.GetMouseButtonDown(1) && gm_selected.Count != 0)
            {
                Debug.Log("start move");
                par_mob.create_group(gm_selected,v2_in_world(Input.mousePosition));

            }
            */
            /*
            float h = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta.y;
            float w = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta.x;
            float dx = 0;
            float dy = 0;
            float dy_ = h / 100 * 10;
            float dx_ = w / 100 * 10;
            Vector2 v2 = Input.mousePosition;
            if (v2.x >= w - dx_)
                dx = (v2.x - (w - dx_)) / dx_;
            else if (v2.x <= dx_)
                dx = (v2.x - dx_) / dx_;
            if (v2.y >= h - dy_)
                dy = (v2.y - (h - dy_)) / dy_;
            else if (v2.y <= dy_)
                dy = (v2.y - dy_) / dy_;
            */
            if (isLocalPlayer)
            {
                Vector3 v3 = Camera.main.transform.position;
                v3.x += Input.GetAxis("Horizontal") * speed_move * Time.deltaTime;
                v3.y += Input.GetAxis("Vertical") * speed_move * Time.deltaTime;
                Camera.main.transform.position = v3;

                print_res();
            }
            
            //Camera.main.transform.position = v3;
            /*
            if (gm_selected.Count > 0)
            {
                Vector2 v2_t = gm_selected[0].transform.position;
                v2_t.x += Input.GetAxis("Horizontal");
                v2_t.y += Input.GetAxis("Vertical");
                gm_selected[0].transform.position = v2_t;
            }
            */
        }


    }
}
