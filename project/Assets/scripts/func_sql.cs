using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class func_sql : MonoBehaviour
{
    public GameObject sql_prebaf;
    /*
    // Start is called before the first frame update

    public List<player> lt_pl = new List<player>();
    public List<player> get_players()
    {

        StartCoroutine(Getting("http://localhost/DBUnity/get_accounts.php"));
        return lt_pl;
    }
    public List<player> get_players(string lg,int ps)
    {
        lt_pl = null;
       // StartCoroutine(Get_proverka());
        return lt_pl;
    }

    IEnumerator Getting(string url)
    {
        using (UnityWebRequest web = UnityWebRequest.Get(url))
        {
            yield return web.SendWebRequest();

            Debug.Log(web == null);
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                string[] stt = web.downloadHandler.text.Split("_");
                //Debug.Log(stt.Length);
                if (stt.Length > 0)
                {
                    for (int i = 0; i < stt.Length - 1; i += 2)
                    {
                        //int ind_now = i / 2;
                        player pl = new player(stt[i], int.Parse(stt[i + 1]));
                        //Debug.Log(pl);
                        lt_pl.Add(pl);
                    }
                }


                //
                


                //MatchCollection mc = Regex.Matches(web.downloadHandler.text, @"_");
                //string[] splitData = Regex.Split(dataText, @"_");
            }
            web.Dispose();
        }


    }



    // Update is called once per frame
    void Update()
    {
        
    }
    */
    void Start()
    {
        if (GameObject.Find("data_sql") == null)
        {
            Instantiate(sql_prebaf).name= "data_sql";
        }
        /*
        else if (SceneManager.GetActiveScene().name == "chat")
        {
            StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().get_message("http://localhost/DBUnity/logining.php",data_sql.id_chat_now,true ,GameObject.Find("Canvas").GetComponent<ui_chat>()));
        }
        */
    }
}
