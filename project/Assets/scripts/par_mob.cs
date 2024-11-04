using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class par_mob : NetworkBehaviour
{
    // Start is called before the first frame update
    public par_player pr_pl;
    public Collider2D col_mob;
    public Collider2D col_atack;
    public int mob_sin=0;
    public int mob_cos=0;
    public GameObject gm_round;

    public int r_find_mob = 6;

    void Start()
    {
        col_mob = gameObject.GetComponent<Collider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Screen.height;
        float w = Screen.width;
        Vector2 v2_m = Input.mousePosition;
        v2_m.y -= h / 2;
        v2_m.x -= w / 2;
        float c = Mathf.Pow(v2_m.x * v2_m.x + v2_m.y * v2_m.y, 0.5f);
        float f_sin = v2_m.y / c;
        float f_cos = v2_m.x / c;
        float angle = 0;
        if (f_sin >= 0)
        {
            angle = (-f_cos + 1) * 90;
        }
        else
        {
            angle = (f_cos + 1) * 90 + 180;
        }
        angle = -angle + 90;
        Vector3 v3_r = new Vector3(0, angle, 0);
        gameObject.transform.rotation = Quaternion.Euler(v3_r);


    }
}
