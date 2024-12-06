using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_battle_infa : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        data_sql sqll = GameObject.Find("data_sql").GetComponent<data_sql>();
        StartCoroutine(sqll.get_battle_infa("http://localhost/DBUnity/get_battles.php"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
