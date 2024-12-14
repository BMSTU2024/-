using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ui_tx : MonoBehaviour
{
    public ui_chat ch1;
    //public IEnumerator action;
    public void sql_chats()
    {
        data_sql sqll = GameObject.Find("data_sql").GetComponent<data_sql>();
        string name = GameObject.Find("input tx chat").GetComponent<InputField>().text;
        if (ch1.type == 0)
        {
            StartCoroutine(sqll.get_chats("http://localhost/DBUnity/get_chats.php", name, data_sql.player_now.login, ch1));
        }
        else
        {
            StartCoroutine(sqll.get_chats_in_add("http://localhost/DBUnity/get_chats_in_add.php", name, data_sql.player_now.login, ch1));

        }
        
    }
    public void sql_player_set()
    {
        if (data_sql.type_chat_now == data_sql.type_chat_set.add)
            sql_players_add(ch1);
        else if (data_sql.type_chat_now == data_sql.type_chat_set.admin)
            sql_players_admin(ch1);
        if (data_sql.type_chat_now == data_sql.type_chat_set.del)
            sql_players_del(ch1);
    }
    public void sql_players_add(ui_chat ch)
    {
        data_sql sqll = GameObject.Find("data_sql").GetComponent<data_sql>();
        string name = GameObject.Find("input tx player").GetComponent<InputField>().text;
        StartCoroutine(sqll.get_chats_players_add("http://localhost/DBUnity/get_chat_player_add.php", name, data_sql.chat_now.ch, ch1));
        //SceneManager.LoadScene("entry");
    }
    public void sql_players_admin(ui_chat ch)
    {
        data_sql sqll = GameObject.Find("data_sql").GetComponent<data_sql>();
        string name = GameObject.Find("input tx player").GetComponent<InputField>().text;
        StartCoroutine(sqll.get_chats_players_admin("http://localhost/DBUnity/get_chat_player_admin.php", name, data_sql.chat_now.ch, ch1));
        //SceneManager.LoadScene("entry");
    }
    public void sql_players_del(ui_chat ch)
    {
        data_sql sqll = GameObject.Find("data_sql").GetComponent<data_sql>();
        string name = GameObject.Find("input tx player").GetComponent<InputField>().text;
        StartCoroutine(sqll.get_chats_players_del("http://localhost/DBUnity/get_chat_player_del.php", name, data_sql.chat_now.ch, ch1));
        //SceneManager.LoadScene("entry");
    }
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "menu chats")
            sql_chats();
        else if (SceneManager.GetActiveScene().name == "menu set chat")
        {
            if(data_sql.type_chat_now== data_sql.type_chat_set.add)
                sql_players_add(ch1);
            else if (data_sql.type_chat_now == data_sql.type_chat_set.admin)
                sql_players_admin(ch1);
            if (data_sql.type_chat_now == data_sql.type_chat_set.del)
                sql_players_del(ch1);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
