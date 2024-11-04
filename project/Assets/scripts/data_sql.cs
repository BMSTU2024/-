using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class data_sql : MonoBehaviour
{
    // Start is called before the first frame update
    public class player
    {
        public string login;
        public int password;
        public player(string lg, int ps)
        {
            login = lg;
            password = ps;
        }
    }
    public class chat
    {
        public string name;
    }
    public class lt_chat
    {
        public string ch;
        public string pl;
        public int id;
        public lt_chat(string ch, string pl, int id)
        {
            this.ch = ch;
            this.pl = pl;
            this.id = id;
        }
    }
    public class message
    {
        public message(int id,string login,int sender,string data,string text)
        {
            this.id = id;
            this.login = login;
            this.sender = sender;
            this.data = data;
            this.text = text;
        }
        public int id;
        public string login;
        public int sender;
        public string data;
        public string text;
    }

    public  static Queue<message> que_ms_now = new Queue<message>();
    //public Queue<message> que_ms_now_last = new Queue<message>();
    public static int id_chat_now;
    public static player player_now;
    //public static player chat_now;
    public bool exiting()
    {
        if (player_now != null)
        {
            StartCoroutine(status_account("http://localhost/DBUnity/logining.php", player_now.login, "offline",true));
            return false;
        }
        else
        {
            return true;
        }
            

    }
    public IEnumerator Get_proverka_account(string url, string lg, int ps)
    {
        using (UnityWebRequest web = UnityWebRequest.Get(url + "?php_login=" + lg + "&php_password=" + ps + "&php_status=offline"))
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
                    data_sql.player_now = lt_pl[0];
                    StartCoroutine(status_account("http://localhost/DBUnity/logining.php", lg, "online",false));

                }
                //



                //MatchCollection mc = Regex.Matches(web.downloadHandler.text, @"_");
                //string[] splitData = Regex.Split(dataText, @"_");
            }
            web.Dispose();
        }


    }
    public IEnumerator registrated_account(string url, string lg, int ps)
    {
        WWWForm form = new WWWForm();
        form.AddField("php_login", lg);
        form.AddField("php_password", ps);
        form.AddField("php_status", "online");
        using (UnityWebRequest web = UnityWebRequest.Post(url, form))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                string stt = web.downloadHandler.text;
                Debug.Log(stt);
                if (stt != "error")
                {
                    data_sql.player_now = new data_sql.player(lg, ps);
                    //StartCoroutine(status_account("http://localhost/DBUnity/logining.php", lg, "online"));
                    SceneManager.LoadScene("base menu");
                }
                //



                //MatchCollection mc = Regex.Matches(web.downloadHandler.text, @"_");
                //string[] splitData = Regex.Split(dataText, @"_");
            }
            web.Dispose();
        }


    }
    public IEnumerator status_account(string url, string login, string par,bool bl_exit)
    {
        WWWForm form = new WWWForm();
        Debug.Log("post " + login + " " + par);
        form.AddField("php_status", par);
        form.AddField("php_login", login);
        using (UnityWebRequest web = UnityWebRequest.Post(url, form))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                Debug.Log(web.downloadHandler.text);
                if(!bl_exit)
                    SceneManager.LoadScene("base menu");
                else
                {
                    player_now = null;
                    Application.Quit();
                }
                    
            }
            web.Dispose();
        }


    }
    public IEnumerator get_chats(string url,string name, string login,ui_chat ui_ch)
    {
        Debug.Log(name+" "+login);
        using (UnityWebRequest web = UnityWebRequest.Get(url+"?php_name="+name + "&php_login=" + login))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                //Debug.Log(web.downloadHandler.text);
                List<lt_chat> lt_ch = new List<lt_chat>();
                if (!(web.downloadHandler.text == "" || web.downloadHandler.text == null))
                {
                    string[] stt = web.downloadHandler.text.Split("_");
                    //Debug.Log("is truue " + stt.Length + " " );
                    
                    if (stt.Length > 0)
                    {
                       
                        for (int i = 0; i < stt.Length-1; i+=3)
                        {
                            //int ind_now = i / 2;
                            lt_ch.Add(new lt_chat(stt[i], stt[i + 1], int.Parse(stt[i+2])));
                        }
                        
                    }
                }
                ui_ch.print_chats(lt_ch);

                /*

                
                */
                //



                //MatchCollection mc = Regex.Matches(web.downloadHandler.text, @"_");
                //string[] splitData = Regex.Split(dataText, @"_");
            }
            web.Dispose();
        }


    }
    public IEnumerator get_message(string url, int id)
    {
        //Debug.Log(" " + id+"  "+ que_ms_now.Count);
        using (UnityWebRequest web = UnityWebRequest.Get(url + "?php_id=" + id+"&php_col="+que_ms_now.Count))
        {
            get_message_bl = true;
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                int col = que_ms_now.Count;
                //que_ms_now = new Queue<message>();
                //Debug.Log(web.downloadHandler.text);
                Queue<message> que_ms_now_last = new Queue<message>();
                //Debug.Log(web.downloadHandler.text+"  ");
                if (!(web.downloadHandler.text == "" || web.downloadHandler.text == null))
                {
                    string[] stt = web.downloadHandler.text.Split("_");
                    //Debug.Log(web.downloadHandler.text);

                    if (stt.Length > 0)
                    {

                        for (int i = 0; i < stt.Length - 1; i += 5)
                        {
                            //int ind_now = i / 2;
                            //Debug.Log(stt[i] + " " + stt[i + 1]);
                            message pl = new message(int.Parse(stt[i]), stt[i + 1], int.Parse(stt[i + 2]), stt[i + 3], stt[i+4]);

                            que_ms_now.Enqueue(pl);
                            que_ms_now_last.Enqueue(pl);
                        }


                    }

                }
                if (col==0)
                {
                    SceneManager.LoadScene("chat");
                }
                if (SceneManager.GetActiveScene().name == "chat")
                {
                    //StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().get_message("http://localhost/DBUnity/get_message.php", data_sql.id_chat_now, false, GameObject.Find("Canvas").GetComponent<ui_chat>()));
                    GameObject.Find("Canvas").GetComponent<ui_chat>().print_message(que_ms_now_last);
                    //bl_ch = true;

                }
                get_message_bl = false;
                //ui_ch.print_message(lt_ms);

                /*

                
                */
                //



                //MatchCollection mc = Regex.Matches(web.downloadHandler.text, @"_");
                //string[] splitData = Regex.Split(dataText, @"_");
            }
            web.Dispose();
        }


    }
    public IEnumerator post_message(string url, int id, string tx)
    {
        WWWForm form = new WWWForm();
        form.AddField("php_pl", id);
        form.AddField("php_tx", tx);
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
                //Queue<message> lt_ms = new Queue<message>();
                //Debug.Log(web.downloadHandler.text);

                //ui_ch.print_message(lt_ms);

                /*

                
                */
                //



                //MatchCollection mc = Regex.Matches(web.downloadHandler.text, @"_");
                //string[] splitData = Regex.Split(dataText, @"_");
            }
            web.Dispose();
        }


    }

    float timer = 0;
    static string date_now_second;
    bool bl_entry_chat = true;
    bool get_message_bl=false;
        [RuntimeInitializeOnLoadMethod]
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        Application.wantsToQuit += exiting;
        /*
        if (SceneManager.GetActiveScene().name == "chat")
        {
            StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().get_message("http://localhost/DBUnity/get_message.php", data_sql.id_chat_now, true, GameObject.Find("Canvas").GetComponent<ui_chat>()));
        }
        */
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "chat")
        {
            if (bl_entry_chat)
            {
                GameObject.Find("Canvas").GetComponent<ui_chat>().print_message(que_ms_now);
                bl_entry_chat =false;
            }
            else
            {
                //Debug.Log("TIMER ="+timer);
                if (timer < 1)
                    timer += Time.deltaTime;
                else if(!get_message_bl)
                {
                    timer = 0;
                    StartCoroutine(get_message("http://localhost/DBUnity/get_message.php", data_sql.id_chat_now));
                }
            }

        }

        /*
    if(SceneManager.GetActiveScene().name == "chat" && !bl_ch)
    {
        StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().get_message("http://localhost/DBUnity/get_message.php", data_sql.id_chat_now, false, GameObject.Find("Canvas").GetComponent<ui_chat>()));
        bl_ch = true;

    }

        */

        /*
        if (SceneManager.GetActiveScene().name == "chat" )
        {
            //StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().get_message("http://localhost/DBUnity/get_message.php", data_sql.id_chat_now, false, GameObject.Find("Canvas").GetComponent<ui_chat>()));
            GameObject.Find("Canvas").GetComponent<ui_chat>().print_message(que_ms_now);
            //bl_ch = true;

        }
        */
    }
}
