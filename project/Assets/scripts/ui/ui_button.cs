using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;

//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ui_button : MonoBehaviour
{
    // Start is called before the first frame update
    public object type;
    public par_player pl;
    //public func fn;
    public void perehod_scene(string st)
    {
        GameObject.Find("data_sql").GetComponent<data_sql>().st_perehod_scene(st);
    }
    public void sql_login()
    {
        data_sql sql = GameObject.Find("data_sql").GetComponent<data_sql>();
        string lg = GameObject.Find("input tx login").GetComponent<InputField>().text;
        int ps =int.Parse( GameObject.Find("input tx password").GetComponent<InputField>().text);
        StartCoroutine(sql.Get_proverka_account("http://localhost/DBUnity/get_player.php", lg,ps));

    }
    public void sql_registrated()
    {
        data_sql sql = GameObject.Find("data_sql").GetComponent<data_sql>();
        string lg = GameObject.Find("input tx login").GetComponent<InputField>().text;
        int ps = int.Parse(GameObject.Find("input tx password").GetComponent<InputField>().text);
        int ps1 = int.Parse(GameObject.Find("input tx password").GetComponent<InputField>().text);
        if(ps==ps1)
            StartCoroutine(sql.registrated_account("http://localhost/DBUnity/registrated.php", lg, ps));

    }

    public void sql_entry_chat()
    {
        data_sql.chat_now = (data_sql.lt_chat)type;
        GameObject.Find("data_sql").GetComponent<data_sql>().get_message_bl_now = true;
        GameObject.Find("data_sql").GetComponent<data_sql>().bl_entry_chat = true;
        StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().get_message("http://localhost/DBUnity/get_message.php", data_sql.chat_now.id));
        
    }
    public void sql_post_message(InputField tx)
    {
        StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().post_message("http://localhost/DBUnity/post_message.php", data_sql.chat_now.id, tx.text));
        tx.text = "";
    }
    public void sql_get_players_chat(bool bl)
    {
        StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().get_players_chat("http://localhost/DBUnity/get_players_chat.php", data_sql.chat_now,bl));
    }
    public void exit_login()
    {
        data_sql sqll= GameObject.Find("data_sql").GetComponent<data_sql>();
        Debug.Log(sqll);
        string lg = data_sql.player_now.login;
        data_sql.player_now = null;
        StartCoroutine(sqll.status_account("http://localhost/DBUnity/logining.php", lg, "offline",false));
        SceneManager.LoadScene("entry");
    }



    public void create_mob(string tp)
    {
        Debug.Log("click");
        if (tp == "worker" &&pl.col_res["res rock"]>=5)
            pl.create_mob(new func.st_create_obj { name = tp, pl = this.pl, v2 = pl.gameObject.transform.position});
        if (tp == "warrior" && pl.col_res["res ruda"] >= 2 && pl.col_res["res ruda"] >= 10)
            pl.create_mob(new func.st_create_obj { name = tp, pl = this.pl, v2 = pl.gameObject.transform.position });
    }
    /*
    public IEnumerator Get_proverka_account(string url, string lg, int ps)
    {
        using (UnityWebRequest web = UnityWebRequest.Get(url + "?php_login=" + lg + "&php_password=" + ps))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                string[] stt = web.downloadHandler.text.Split("_");
                Debug.Log(stt[0]);

                List<data_sql.player> lt_pl = new List<data_sql.player>();
                if (stt.Length > 0)
                {
                    for (int i = 0; i < stt.Length - 1; i += 2)
                    {
                        //int ind_now = i / 2;
                        Debug.Log(stt[i] + " " + stt[i + 1]);
                        data_sql.player pl = new data_sql.player(stt[i], int.Parse(stt[i + 1]));

                        lt_pl.Add(pl);
                    }
                }

                if (lt_pl.Count != 0)
                {
                    data_sql.player_now=lt_pl[0];
                    StartCoroutine(status_account("http://localhost/DBUnity/logining.php", lg,"online"));
                    
                }
                //



                //MatchCollection mc = Regex.Matches(web.downloadHandler.text, @"_");
                //string[] splitData = Regex.Split(dataText, @"_");
            }
            web.Dispose();
        }


    }
    public IEnumerator status_account(string url,string login,string par)
    {
        WWWForm form = new WWWForm();
        Debug.Log("post "+login+" "+par);
        form.AddField("php_status", par);
        form.AddField("php_login", login);
        using (UnityWebRequest web = UnityWebRequest.Post(url,form))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                Debug.Log(web.downloadHandler.text);
                SceneManager.LoadScene("base menu");
                /*
                string[] stt = web.downloadHandler.text.Split("_");
                Debug.Log(stt[0]);

                List<data_sql.player> lt_pl = new List<data_sql.player>();
                if (stt.Length > 0)
                {
                    for (int i = 0; i < stt.Length - 1; i += 2)
                    {
                        //int ind_now = i / 2;
                        Debug.Log(stt[i] + " " + stt[i + 1]);
                        data_sql.player pl = new data_sql.player(stt[i], int.Parse(stt[i + 1]));

                        lt_pl.Add(pl);
                    }
                }

                if (lt_pl.Count != 0)
                {
                    data_sql.player_now = lt_pl[0];

                    SceneManager.LoadScene("base menu");
                }
                
                //



                //MatchCollection mc = Regex.Matches(web.downloadHandler.text, @"_");
                //string[] splitData = Regex.Split(dataText, @"_");
            }
            web.Dispose();
        }

    
    }
    */

}
