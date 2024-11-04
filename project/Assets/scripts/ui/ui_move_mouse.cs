using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ui_move_mouse : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed_move = 1;

    public Vector2 v2_start;
    public bool start_selected = false;
    void Start()
    {

    }
    public static Vector2 v2_in_world(Vector3 v3)
    {
        return Camera.main.ScreenToWorldPoint(v3);
    }
    // Update is called once per frame
    void Update()
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

            
            r_select.anchoredPosition = (Vector2)Input.mousePosition -((Vector2)Input.mousePosition - v2_start)/2 ;
            Vector2 v2_sd = (Vector2)Input.mousePosition - v2_start;

            if (v2_sd.x < 0)
                v2_sd.x *= -1;
            if (v2_sd.y < 0)
                v2_sd.y *= -1;
            Debug.Log(v2_sd);
            r_select.sizeDelta = v2_sd;
            //Collider[] ms_coll = Physics.OverlapBox(v3_c, new Vector3(r, r, r));
            Collider2D[] ms_coll = Physics2D.OverlapAreaAll(v2_in_world(v2_start), v2_in_world(Input.mousePosition));
            //ms_coll = Physics2D.OverlapAreaAll(new Vector2(0, 0), new Vector2(1, 1));
            foreach (Collider2D c in ms_coll)
            {
                Debug.Log(c.gameObject.name);
            }
        }
        if (Input.GetMouseButtonUp(0) && start_selected)
        {
            start_selected = false;
            GameObject.Find("select").GetComponent<RectTransform>().sizeDelta = Vector2.zero;   
            //v2_start = Input.mousePosition;
        }
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

        Vector3 v3 = gameObject.transform.position;
        v3.x += dx * speed_move * Time.deltaTime;
        v3.y += dy * speed_move * Time.deltaTime;
        gameObject.transform.position = v3;



        //Debug.Log(dx+" "+dy);
    }
}
