using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class par_mob : NetworkBehaviour
{
    public class action
    {
        public enum condition
        {
            distanse,
            damage,
            enter
        }
        public float distanse;
        public condition con_action;
        public action(condition con, int dist)
        {
            con_action = con;
            distanse = dist;
        }
    }
    public class lt_mob_group
    {
        public List<GameObject> lt = new List<GameObject>();
        public Vector2 v2_wolk;
        public GameObject gm_cl;
        [Server]
        public void add_list(List<GameObject> lt_gm)
        {
            lt = lt_gm;
            foreach (GameObject gm in lt_gm)
            {
                par_mob mb = gm.GetComponent<par_mob>();
                //Debug.Log(v2);
                mb.v2_wolk = v2_wolk;
            }
        }
        public void add_list(List<GameObject> lt_gm, GameObject gm_cl)
        {
            lt = lt_gm;
            foreach (GameObject gm in lt_gm)
            {
                par_mob mb = gm.GetComponent<par_mob>();
                //Debug.Log(v2);
                mb.v2_wolk = v2_wolk;
                mb.cel = gm_cl;
            }
        }
        public lt_mob_group()
        {

        }
        public lt_mob_group(Vector2 v2)
        {
            v2_wolk = v2;
        }
        public lt_mob_group(Vector2 v2, GameObject gm)
        {
            v2_wolk = v2;
            gm_cl = gm;
        }
        public lt_mob_group(List<GameObject> lt_gm, Vector2 v2)
        {
            v2_wolk = v2;
            lt = lt_gm;
            foreach (GameObject gm in lt_gm)
            {
                par_mob mb = gm.GetComponent<par_mob>();
                //Debug.Log(v2);
                mb.v2_wolk = v2;
                mb.cel = gm_cl;
            }
            Dictionary<int, string> g = new Dictionary<int, string>();

        }
    }

    // Start is called before the first frame update
    [SyncVar]
    public par_player pl;
    public Collider2D col_mob;
    public col_attack col_atack;
    [SyncVar]
    public float mob_sin = 0;
    [SyncVar]
    public float mob_cos = 0;
    public GameObject gm_round;
    public float speed = 1;
    [SyncVar]
    public int r_find_mob = 6;


    [SyncVar]
    public int hp = 0;

    public Dictionary<int, action> lt_action = new Dictionary<int, action>();

    [SyncVar]
    public Vector2 v2_wolk;
    [SyncVar]
    public GameObject cel;
    public bool bl_cel = false;
    [SyncVar]
    public int action_now = 0;

    [SyncVar]
    public lt_mob_group group;
    //[SyncVar]
    public func func_now;

    public GameObject get_auto_cel()
    {
        //List < GameObject > list  = new List<GameObject>();
        func.st_mob_vis st = new func.st_mob_vis { mob_now=this};
        NetworkClient.Send(st);
        //Debug.Log(st.gm_cel);
        return st.gm_cel;
    }
    public void del_group(bool bl)
    {
        Debug.Log("del group");
        if (group != null)
        {
            
            group.lt.Remove(gameObject);
            if (group.lt.Count == 0 || bl)
            {
                NetworkClient.Send(new func.struct_group_delete { gr = group });
            }
            group = null;
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("enter");
        if (collision.gameObject == cel)
        {
            Debug.Log("ACTION");
            v2_wolk = Vector2.zero;
            gameObject.GetComponent<Animator>().SetBool("wolk", false);
            del_group(false);

            foreach (int i in lt_action.Keys)
            {
                Debug.Log(true);
                if (lt_action[i].con_action == action.condition.enter)
                {
                    start_action(i);
                }
            }
        }
        /*
        else if (collision.gameObject.GetComponent<par_resource>() != null && isLocalPlayer)
        {
            Debug.Log("enter 1");
            pl.add_res(collision.gameObject.name);
            NetworkServer.Destroy(collision.gameObject);
        }
        */
    }
    public void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("stay");
        if (collision.gameObject == cel && action_now == 0)
        {
            Debug.Log("ACTION");
            v2_wolk = Vector2.zero;
            gameObject.GetComponent<Animator>().SetBool("wolk", false);
            del_group(false);

            foreach (int i in lt_action.Keys)
            {
                Debug.Log(true);
                if (lt_action[i].con_action == action.condition.enter)
                {
                    start_action(i);
                }
            }
        }
        /*
        else if (collision.gameObject.GetComponent<par_resource>() != null && isLocalPlayer)
        {
            Debug.Log("enter 1");
            pl.add_res(collision.gameObject.name);
            NetworkServer.Destroy(collision.gameObject);
        }
        */
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger " + collision.gameObject.name);
        if (collision.gameObject.GetComponent<par_resource>() != null)
        {
            Debug.Log("trigger 1" + collision.gameObject.name);
            //if (isClient)
            //pl.add_res(collision.gameObject.name);
            //else
            //pl.add_res_ser(collision.gameObject.name);
            NetworkClient.Send(new func.st_res { znak = 1, type = collision.gameObject.name ,pl=this.pl});
            NetworkServer.Destroy(collision.gameObject);
        }
        //if (isLocalPlayer)
        //pl.print_res();
    }
    public void anim_action_activate()
    {
        if (isServer)
        {
            if (cel == null && action_now!=0)
            {
                end_action(action_now);
            }
            else if (action_now == 1 && gameObject.name == "worker" && cel != null)
            {
                par_nature nt = cel.GetComponent<par_nature>();
                nt.hp--;
                if (nt.hp <= 0)
                {
                    NetworkClient.Send(new func.st_create_obj { name = nt.drop, pl = this.pl, v2 = cel.transform.position });
                    //func_now.create_object(nt.drop,pl,cel.transform.position);
                    NetworkServer.Destroy(cel);

                    end_action(1);
                }
            }
            if (action_now == 1 && gameObject.name == "warrior" && cel != null)
            {
                foreach (GameObject gm in col_atack.gm_entered)
                {
                    if (gm.GetComponent<par_mob>() != null)
                    {
                        par_mob mb = gm.GetComponent<par_mob>();
                        mb.hp--;
                        if (mb.hp == 0)
                        {
                            NetworkClient.Send(new func.st_del_obj { gm = gm });
                        }
                    }
                    else
                    {
                        par_build bl = gm.GetComponent<par_build>();
                        bl.hp--;
                        if (bl.hp == 0)
                        {
                            NetworkClient.Send(new func.st_del_obj { gm = gm });
                        }
                    }

                }
                /*
                par_mob nt = cel.GetComponent<par_nature>();
                nt.hp--;
                if (nt.hp <= 0)
                {
                    NetworkClient.Send(new func.st_create_obj { name = nt.drop, pl = this.pl, v2 = cel.transform.position });
                    //func_now.create_object(nt.drop,pl,cel.transform.position);
                    NetworkServer.Destroy(cel);

                    end_action(1);
                }
                */
            }
        }
    }

    public void start_action(int i)
    {
        action_now = i;
        gameObject.GetComponent<Animator>().SetBool(i.ToString(), true);
    }
    public void end_action(int i)
    {
        action_now = 0;
        gameObject.GetComponent<Animator>().SetBool(i.ToString(), false);
    }

    void Start()
    {
        col_mob = gameObject.GetComponent<Collider2D>();
        if (gameObject.name == "worker")
        {
            lt_action.Add(1,new action(action.condition.enter,0));
        }
        if (gameObject.name == "warrior")
        {
            lt_action.Add(1, new action(action.condition.enter, 0));
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            //Debug.Log(lt_gm.Count);
            if (col_atack != null && (v2_wolk != Vector2.zero || cel != null))
            {
                float h = Screen.height;
                float w = Screen.width;
                //Vector2 v2_m = Input.mousePosition;
                Vector2 v2_m = v2_wolk;
                if (cel != null)
                    v2_m = cel.transform.position;
                v2_m.y -= transform.position.y;
                v2_m.x -= transform.position.x;
                float c = Mathf.Pow(v2_m.x * v2_m.x + v2_m.y * v2_m.y, 0.5f);
                float f_sin = v2_m.y / c;
                float f_cos = v2_m.x / c;
                mob_cos = f_cos;
                mob_sin = f_sin;
                float angle = 0;
                if (f_sin >= 0)
                {
                    angle = (-f_cos + 1) * 90;
                }
                else
                {
                    angle = (f_cos + 1) * 90 + 180;
                }


                //angle = -angle + 90;
                angle -= 90;
                Vector3 v3_r = new Vector3(0, 0, angle);
                gameObject.transform.Find("round comp").rotation = Quaternion.Euler(v3_r);
            }
            if (col_atack != null )
            {
                if(col_atack.gm_entered.Count > 0)
                {
                    if (v2_wolk != Vector2.zero)
                    {
                        v2_wolk = Vector2.zero;
                        gameObject.GetComponent<Animator>().SetBool("wolk", false);
                        del_group(false);
                        if (gameObject.name == "warrior")
                            start_action(1);
                    }
                }
                else
                {
                    if (gameObject.name == "warrior")
                        end_action(1);
                }
            }
            if (gameObject.name == "warrior" && action_now == 0 && group==null)
            {
                GameObject gm = get_auto_cel();
                
                if (gm != null)
                {
                    //cel = gm;
                    //v2_wolk = gm.transform.position;
                }
                
            }

            //Vector3 v3_r = new Vector3(0, angle, 0);
            //gameObject.transform.rotation = Quaternion.Euler(v3_r);


            if (v2_wolk != Vector2.zero )
            {
                gameObject.GetComponent<Animator>().SetBool("wolk", true);
                //Debug.Log("wolk");
                Vector2 v2_d = (v2_wolk - (Vector2)transform.position) / Vector2.Distance(v2_wolk, (Vector2)transform.position) * Time.deltaTime * speed;
                if (Vector2.Distance(v2_wolk, (Vector2)transform.position) <= v2_d.magnitude)
                {
                    v2_d = v2_wolk - (Vector2)transform.position;
                    v2_wolk = Vector2.zero;
                    del_group(true);
                    gameObject.GetComponent<Animator>().SetBool("wolk", false);

                }
                //gameObject.GetComponent<Rigidbody2D>().AddForce(v2_d*5, ForceMode2D.Impulse);
                Vector2 v2 = (Vector2)transform.position + v2_d;
                //if(isServer)
                transform.position = v2;

            }


        }




        //gameObject.transform.Find("Canvas m").Find("tx").gameObject.GetComponent<TMP_Text>().text = v2_wolk.x.ToString()+" " + v2_wolk.y.ToString();






    }
}
