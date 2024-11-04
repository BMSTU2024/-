using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ui_tx : MonoBehaviour
{
    public void sql_chats(ui_chat ch)
    {
        data_sql sqll = GameObject.Find("data_sql").GetComponent<data_sql>();
        string name = GameObject.Find("input tx chat").GetComponent<InputField>().text;
        StartCoroutine(sqll.get_chats("http://localhost/DBUnity/get_chats.php", name,data_sql.player_now.login, ch));
        //SceneManager.LoadScene("entry");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
