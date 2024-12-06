using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ui_dropdown : MonoBehaviour
{
    // Start is called before the first frame update
    public void select_type_chat(ui_chat ch)
    {
        ch.type = gameObject.GetComponent<TMP_Dropdown>().value;
        data_sql sqll = GameObject.Find("data_sql").GetComponent<data_sql>();
        string name = GameObject.Find("input tx chat").GetComponent<InputField>().text;
        //string urll = null;
        if (ch.type == 0)
        {
            StartCoroutine(sqll.get_chats("http://localhost/DBUnity/get_chats.php", name, data_sql.player_now.login, ch));

            //ch.action = sqll.get_chats("http://localhost/DBUnity/get_chats.php", name, data_sql.player_now.login, ch);
        }
        else if (ch.type == 1)
        {
            StartCoroutine(sqll.get_chats_in_add("http://localhost/DBUnity/get_chats_in_add.php", name, data_sql.player_now.login, ch));

            //ch.action = sqll.get_chats_in_add("http://localhost/DBUnity/get_chats_in_add.php", name, data_sql.player_now.login, ch);
        }
        //StartCoroutine(ch.action);
        //Debug.Log();
        //gameObject.GetComponent<Dropdown>().value = 1;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
