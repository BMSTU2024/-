using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class par_player : NetworkBehaviour
{
    // Start is called before the first frame update
    public static int col_players = 0;
    public List<int> lt_fr_id = new List<int>();
    [SyncVar]
    public int id = 0;

    public int count;


    public float speed_move = 1;

    public Vector2 v2_start;
    public static bool start_selected = false;
    [SyncVar]
    public List<GameObject> gm_selected = new List<GameObject>();
    public List<par_mob> gm_selected_pr = new List<par_mob>();

    
    [Command]
    public void set_gm_selected(List<GameObject> lt)
    {
        gm_selected= lt;
        Debug.Log(gm_selected.Count);
    }
    
    void Start()
    {
        //id = (int)netId;
    }

    public static Vector2 v2_in_world(Vector3 v3)
    {
        return Camera.main.ScreenToWorldPoint(v3);
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


            }
            if (start_selected)
            {
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
                set_gm_selected( new List<GameObject>());
                foreach (Collider2D c in ms_coll)
                {
                    //
                    if (c.gameObject.GetComponent<par_mob>() != null)
                    {
                        //Debug.Log(c.gameObject.GetComponent<par_mob>().pl + "   " + id + "   select id");
                        if (c.gameObject.GetComponent<par_mob>().pl == this && !gm_selected.Contains(c.gameObject))

                            gm_selected.Add(c.gameObject);
                        //gm_selected_pr.Add(c.gameObject.GetComponent<par_mob>());
                    }

                }
                set_gm_selected(gm_selected);
            }
            if (Input.GetMouseButtonUp(0) && start_selected)
            {
                start_selected = false;
                GameObject.Find("select").GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                //v2_start = Input.mousePosition;
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
