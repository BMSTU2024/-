using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static data_sql;

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
        public string status;
        public lt_chat(string ch, string pl, int id,string status)
        {
            this.ch = ch;
            this.pl = pl;
            this.id = id;
            this.status = status;
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
    public class battle
    {
        public string status;
        public string lont_time;
        public string name;
        public battle(string long_time,string name,string status)
        {
            this.name = name;
            this.lont_time = long_time;
            this.status = status;
        }
    }
    public enum type_chat_set
    {
        none,
        add,
        admin,
        del
    }
    public static type_chat_set type_chat_now =type_chat_set.none;
    public static Queue<battle> que_bt_infa = new Queue<battle>();
    public  static Queue<message> que_ms_now = new Queue<message>();
    //public Queue<message> que_ms_now_last = new Queue<message>();

    public static List<lt_chat> pl_ch_now = new List<lt_chat>();
    public static lt_chat chat_now;
    public static player player_now;
    public static int id_battle_now = -1;
    //public static player chat_now;
    public void st_perehod_scene(string st)
    {
        if (st != "chat" && st!="infa chat" && st!="menu set chat")
        {
            data_sql.chat_now = null;
            data_sql.que_ms_now = new Queue<data_sql.message>();
            get_message_bl = true;
        }
        if (st == "chat")
        {
            bl_entry_chat = true;
            get_message_bl_now = true;
        }

        else
        {
            get_message_bl_now = false;
        }

        if (st != "menu set chat")
            type_chat_now = type_chat_set.none;
        if (st != "infa chat" && st!= "menu set chat")
            data_sql.pl_ch_now = null;
        else
        {
            bl_entry_infa_chat = true;
        }
        SceneManager.LoadScene(st);
    }

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
                else
                {
                    GameObject.Find("Canvas").transform.Find("pn error").Find("tx error").gameObject.GetComponent<Text>().text = "Ќеправильный логин или пароль";
                    GameObject.Find("Canvas").transform.Find("pn error").gameObject.SetActive(true);
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
                if (!bl_exit)
                    st_perehod_scene("base menu");
                    //SceneManager.LoadScene("base menu");
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
                List<lt_chat> lt_ch = new List<lt_chat>();
                if (!(web.downloadHandler.text == "" || web.downloadHandler.text == null))
                {
                    string[] stt = web.downloadHandler.text.Split("_");
                    if (stt.Length > 0)
                    {
                       
                        for (int i = 0; i < stt.Length-1; i+=4)
                        {

                            lt_ch.Add(new lt_chat(stt[i], stt[i + 1], int.Parse(stt[i + 2]), stt[i+3]));
                        }
                        
                    }
                }
                ui_ch.print_chats(lt_ch);
            }
            web.Dispose();
        }
    }
    public IEnumerator get_chats_in_add(string url, string name, string login, ui_chat ui_ch)
    {
        Debug.Log(name + " " + login);
        using (UnityWebRequest web = UnityWebRequest.Get(url + "?php_name=" + name + "&php_login=" + login))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                Debug.Log(web.downloadHandler.text);
                List<lt_chat> lt_ch = new List<lt_chat>();
                if (!(web.downloadHandler.text == "" || web.downloadHandler.text == null))
                {
                    string[] stt = web.downloadHandler.text.Split("_");
                    //Debug.Log("is truue " + stt.Length + " " );

                    if (stt.Length > 0)
                    {

                        for (int i = 0; i < stt.Length - 1; i += 4)
                        {
                            //int ind_now = i / 2;
                            lt_ch.Add(new lt_chat(stt[i], stt[i + 1], int.Parse(stt[i + 2]), stt[i + 3]));
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
    public IEnumerator get_chats_players_add(string url, string login, string chat, ui_chat ui_ch)
    {
        Debug.Log(chat + " " + login);
        using (UnityWebRequest web = UnityWebRequest.Get(url + "?php_chat=" + chat + "&php_player=" + login))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                Debug.Log(web.downloadHandler.text);
                List<string> lt_ch = new List<string>();
                if (!(web.downloadHandler.text == "" || web.downloadHandler.text == null))
                {
                    string[] stt = web.downloadHandler.text.Split("_");
                    //Debug.Log("is truue " + stt.Length + " " );

                    if (stt.Length > 0)
                    {

                        for (int i = 0; i < stt.Length - 1; i ++)
                        {
                            lt_ch.Add(stt[i]);
                            //int ind_now = i / 2;
                            //lt_ch.Add(new player(stt[i], stt[i + 1], int.Parse(stt[i + 2]), stt[i + 3]));
                        }

                    }
                }
                ui_ch.print_players(lt_ch);

                /*

                
                */
                //



                //MatchCollection mc = Regex.Matches(web.downloadHandler.text, @"_");
                //string[] splitData = Regex.Split(dataText, @"_");
            }
            web.Dispose();
        }


    }
    public IEnumerator get_chats_players_admin(string url, string login, string chat, ui_chat ui_ch)
    {
        Debug.Log(chat + " " + login);
        using (UnityWebRequest web = UnityWebRequest.Get(url + "?php_chat=" + chat + "&php_player=" + login))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                Debug.Log(web.downloadHandler.text);
                List<string> lt_ch = new List<string>();
                if (!(web.downloadHandler.text == "" || web.downloadHandler.text == null))
                {
                    string[] stt = web.downloadHandler.text.Split("_");
                    //Debug.Log("is truue " + stt.Length + " " );

                    if (stt.Length > 0)
                    {

                        for (int i = 0; i < stt.Length - 1; i++)
                        {
                            lt_ch.Add(stt[i]);
                            //int ind_now = i / 2;
                            //lt_ch.Add(new player(stt[i], stt[i + 1], int.Parse(stt[i + 2]), stt[i + 3]));
                        }

                    }
                }
                ui_ch.print_players(lt_ch);

                /*

                
                */
                //



                //MatchCollection mc = Regex.Matches(web.downloadHandler.text, @"_");
                //string[] splitData = Regex.Split(dataText, @"_");
            }
            web.Dispose();
        }


    }
    public IEnumerator get_chats_players_del(string url, string login, string chat, ui_chat ui_ch)
    {
        Debug.Log(chat + " " + login);
        using (UnityWebRequest web = UnityWebRequest.Get(url + "?php_chat=" + chat + "&php_player=" + login))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                Debug.Log(web.downloadHandler.text);
                List<string> lt_ch = new List<string>();
                if (!(web.downloadHandler.text == "" || web.downloadHandler.text == null))
                {
                    string[] stt = web.downloadHandler.text.Split("_");
                    //Debug.Log("is truue " + stt.Length + " " );

                    if (stt.Length > 0)
                    {

                        for (int i = 0; i < stt.Length - 1; i++)
                        {
                            lt_ch.Add(stt[i]);
                            //int ind_now = i / 2;
                            //lt_ch.Add(new player(stt[i], stt[i + 1], int.Parse(stt[i + 2]), stt[i + 3]));
                        }

                    }
                }
                ui_ch.print_players(lt_ch);

                /*

                
                */
                //



                //MatchCollection mc = Regex.Matches(web.downloadHandler.text, @"_");
                //string[] splitData = Regex.Split(dataText, @"_");
            }
            web.Dispose();
        }


    }

    public IEnumerator add_players_in_chat(string url, string login, string chat)
    {
        Debug.Log(chat + " " + login);
        using (UnityWebRequest web = UnityWebRequest.Get(url + "?php_chat=" + chat + "&php_player=" + login))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                Debug.Log(web.downloadHandler.text);
                List<string> lt_ch = new List<string>();
                st_perehod_scene("infa chat");
            }
            web.Dispose();
        }


    }
    public IEnumerator admin_players_in_chat(string url, string login,string login_now, string chat)
    {
        Debug.Log(chat + " " + login);
        using (UnityWebRequest web = UnityWebRequest.Get(url + "?php_chat=" + chat + "&php_player=" + login+ "&php_admin=" + login_now))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                Debug.Log(web.downloadHandler.text);
                List<string> lt_ch = new List<string>();
                data_sql.chat_now.status = "member";
                st_perehod_scene("infa chat");
            }
            web.Dispose();
        }


    }
    public IEnumerator del_players_in_chat(string url, string login, string chat)
    {
        Debug.Log(chat + " " + login);
        using (UnityWebRequest web = UnityWebRequest.Get(url + "?php_chat=" + chat + "&php_player=" + login))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                Debug.Log(web.downloadHandler.text);
                List<string> lt_ch = new List<string>();
                st_perehod_scene("infa chat");
            }
            web.Dispose();
        }


    }
    public IEnumerator exit_players_in_chat(string url, string login, string chat)
    {
        Debug.Log(chat + " " + login);
        using (UnityWebRequest web = UnityWebRequest.Get(url + "?php_chat=" + chat + "&php_player=" + login))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                Debug.Log(web.downloadHandler.text);
                List<string> lt_ch = new List<string>();
                st_perehod_scene("menu chats");
            }
            web.Dispose();
        }


    }

    public IEnumerator yes_add_player_chat(string url, string login, string chat,ui_tx tx)
    {
        Debug.Log(chat + " " + login);
        using (UnityWebRequest web = UnityWebRequest.Get(url + "?php_chat=" + chat + "&php_player=" + login))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                Debug.Log(web.downloadHandler.text);
                List<string> lt_ch = new List<string>();

                //st_perehod_scene("infa chat");
                /*
                if (!(web.downloadHandler.text == "" || web.downloadHandler.text == null))
                {
                    string[] stt = web.downloadHandler.text.Split("_");
                    //Debug.Log("is truue " + stt.Length + " " );

                    if (stt.Length > 0)
                    {

                        for (int i = 0; i < stt.Length - 1; i++)
                        {
                            lt_ch.Add(stt[i]);
                            //int ind_now = i / 2;
                            //lt_ch.Add(new player(stt[i], stt[i + 1], int.Parse(stt[i + 2]), stt[i + 3]));
                        }

                    }
                }
                */
                //ui_ch.print_players(lt_ch);

                /*

                
                */
                //
                tx.sql_chats();


                //MatchCollection mc = Regex.Matches(web.downloadHandler.text, @"_");
                //string[] splitData = Regex.Split(dataText, @"_");
            }
            web.Dispose();
        }


    }
    public IEnumerator set_admin_chat(string url, string login, string chat)
    {
        Debug.Log(chat + " " + login);
        WWWForm form = new WWWForm();
        form.AddField("php_chat", chat);
        form.AddField("php_player", login);
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
                List<string> lt_ch = new List<string>();

                //st_perehod_scene("infa chat");
                /*
                if (!(web.downloadHandler.text == "" || web.downloadHandler.text == null))
                {
                    string[] stt = web.downloadHandler.text.Split("_");
                    //Debug.Log("is truue " + stt.Length + " " );

                    if (stt.Length > 0)
                    {

                        for (int i = 0; i < stt.Length - 1; i++)
                        {
                            lt_ch.Add(stt[i]);
                            //int ind_now = i / 2;
                            //lt_ch.Add(new player(stt[i], stt[i + 1], int.Parse(stt[i + 2]), stt[i + 3]));
                        }

                    }
                }
                */
                //ui_ch.print_players(lt_ch);

                /*

                
                */
                //
                StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().get_players_chat("http://localhost/DBUnity/get_players_chat.php", data_sql.chat_now, false));


                //MatchCollection mc = Regex.Matches(web.downloadHandler.text, @"_");
                //string[] splitData = Regex.Split(dataText, @"_");
            }
            web.Dispose();
        }


    }
    public IEnumerator not_add_player_chat(string url, string login, string chat, ui_tx tx)
    {
        Debug.Log(chat + " " + login);
        using (UnityWebRequest web = UnityWebRequest.Get(url + "?php_chat=" + chat + "&php_player=" + login))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                Debug.Log(web.downloadHandler.text);
                List<string> lt_ch = new List<string>();
                
                //st_perehod_scene("infa chat");
                /*
                if (!(web.downloadHandler.text == "" || web.downloadHandler.text == null))
                {
                    string[] stt = web.downloadHandler.text.Split("_");
                    //Debug.Log("is truue " + stt.Length + " " );

                    if (stt.Length > 0)
                    {

                        for (int i = 0; i < stt.Length - 1; i++)
                        {
                            lt_ch.Add(stt[i]);
                            //int ind_now = i / 2;
                            //lt_ch.Add(new player(stt[i], stt[i + 1], int.Parse(stt[i + 2]), stt[i + 3]));
                        }

                    }
                }
                */
                //ui_ch.print_players(lt_ch);

                /*

                
                */
                //

                tx.sql_chats();

                //MatchCollection mc = Regex.Matches(web.downloadHandler.text, @"_");
                //string[] splitData = Regex.Split(dataText, @"_");
            }
            web.Dispose();
        }


    }


    public IEnumerator get_players_chat(string url, data_sql.lt_chat ch,bool bl)
    {
        Debug.Log(ch.ch);
        using (UnityWebRequest web = UnityWebRequest.Get(url + "?php_chat=" + ch.ch))
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
                Dictionary<lt_chat,string> st_chat = new Dictionary<lt_chat,string>();
                Debug.Log(web.downloadHandler.text);
                if (!(web.downloadHandler.text == "" || web.downloadHandler.text == null))
                {
                    string[] stt = web.downloadHandler.text.Split("_");
                    //Debug.Log("is truue " + stt.Length + " " );

                    if (stt.Length > 0)
                    {

                        for (int i = 0; i < stt.Length - 1; i += 5)
                        {
                            //int ind_now = i / 2;
                            lt_chat cc = new lt_chat(stt[i], stt[i + 1], int.Parse(stt[i + 2]), stt[i + 3]);
                            lt_ch.Add(cc);
                            if (stt[i + 4].Equals("online"))
                                st_chat.Add(cc, "в сети");
                            else
                                st_chat.Add(cc, "не в сети");
                        }

                    }
                }
                pl_ch_now = lt_ch;
                if (bl)
                {
                    st_perehod_scene("infa chat");
                    //SceneManager.LoadScene();
                }
                if (SceneManager.GetActiveScene().name == "infa chat")
                {
                    Debug.Log("LOAD PL");
                    int i_m = GameObject.Find("sc pn pl").transform.Find("Viewport").Find("pn pl").childCount;
                    for (int i = 0; i < i_m; i++)
                    {
                        Destroy(GameObject.Find("sc pn pl").transform.Find("Viewport").Find("pn pl").GetChild(i).gameObject);
                    }
                    foreach (lt_chat ch_now in st_chat.Keys)
                    {
                        GameObject gm = Instantiate(GameObject.Find("ignoring").transform.Find("pn infa").gameObject, GameObject.Find("sc pn pl").transform.Find("Viewport").Find("pn pl"));
                        gm.transform.Find("tx name").gameObject.GetComponent<Text>().text = ch_now.pl;
                        GameObject gm1 = gm.transform.Find("tx status").gameObject;
                        if (ch_now.status.Equals("admin"))
                        {
                            gm1.GetComponent<Text>().color = new Vector4(1, 0.88f, 0.4f,1);
                            gm1.GetComponent<Text>().text = "глава чата";
                        }
                            
                        else if (ch_now.status.Equals("member"))
                        {
                            gm1.GetComponent<Text>().color = new Vector4(1, 0.55f, 0,1);
                            gm1.GetComponent<Text>().text = "участник";
                        }

                        else
                        {
                            gm1.GetComponent<Text>().color = new Vector4(0.57f, 0.57f, 0.57f,1);
                            gm1.GetComponent<Text>().text = "приглашЄн";
                        }
                            
                        //gm1.GetComponent<Text>().text = ch_now.status;
                        GameObject gm2 = gm.transform.Find("tx set").gameObject;

                        if (st_chat[ch_now].Equals("в сети"))
                            gm2.GetComponent<Text>().color = Color.green;
                        else
                            gm2.GetComponent <Text>().color = Color.red;
                        gm2.GetComponent<Text>().text = st_chat[ch_now];
                    }
                    if (data_sql.chat_now.status.Equals("admin"))
                    {
                        GameObject.Find("pn settings").transform.Find("back chat").gameObject.active = false;
                        GameObject.Find("pn settings").transform.Find("bt add pl").gameObject.active = true;
                        GameObject.Find("pn settings").transform.Find("bt del pl").gameObject.active = true;
                        GameObject.Find("pn settings").transform.Find("bt ren pl").gameObject.active = true;
                    }
                    else
                    {
                        GameObject.Find("pn settings").transform.Find("back chat").gameObject.active = true;
                        GameObject.Find("pn settings").transform.Find("bt add pl").gameObject.active = false;
                        GameObject.Find("pn settings").transform.Find("bt del pl").gameObject.active = false;
                        GameObject.Find("pn settings").transform.Find("bt ren pl").gameObject.active = false;
                    }
                    //StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().get_message("http://localhost/DBUnity/get_message.php", data_sql.id_chat_now, false, GameObject.Find("Canvas").GetComponent<ui_chat>()));
                    //GameObject.Find("Canvas").GetComponent<ui_chat>().print_message(que_ms_now_last);
                    //bl_ch = true;

                }
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
            else if(get_message_bl_now)
            {
                int col = que_ms_now.Count;
                //que_ms_now = new Queue<message>();
                //Debug.Log(web.downloadHandler.text);
                Queue<message> que_ms_now_last = new Queue<message>();
                Debug.Log(web.downloadHandler.text+"  ");
                bool bl_er = false;
                if (!(web.downloadHandler.text == "" || web.downloadHandler.text == null))
                {
                    if (web.downloadHandler.text.Equals("none"))
                    {
                        bl_er = true;
                        st_perehod_scene("menu chats");
                        //get_message_bl=false;

                        //return;
                    }
                    else
                    {
                        string[] stt = web.downloadHandler.text.Split("_");
                        //Debug.Log(web.downloadHandler.text);

                        if (stt.Length > 0)
                        {

                            for (int i = 0; i < stt.Length - 1; i += 5)
                            {
                                //int ind_now = i / 2;
                                //Debug.Log(stt[i] + " " + stt[i + 1]);
                                message pl = new message(int.Parse(stt[i]), stt[i + 1], int.Parse(stt[i + 2]), stt[i + 3], stt[i + 4]);

                                que_ms_now.Enqueue(pl);
                                que_ms_now_last.Enqueue(pl);
                            }


                        }
                    }


                }
                if (!bl_er)
                {
                    if (col == 0 && bl_entry_chat)
                    {
                        st_perehod_scene("chat");
                        //SceneManager.LoadScene("chat");
                    }
                    if (SceneManager.GetActiveScene().name == "chat")
                    {
                        //StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().get_message("http://localhost/DBUnity/get_message.php", data_sql.id_chat_now, false, GameObject.Find("Canvas").GetComponent<ui_chat>()));
                        GameObject.Find("Canvas").GetComponent<ui_chat>().print_message(que_ms_now_last);
                        //bl_ch = true;

                    }
                }
                //col==0

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
    public IEnumerator create_chat(string url, string name)
    {
        //WWWForm form = new WWWForm();
        //form.AddField("php_pl", id);
        //form.AddField("php_tx", tx);
        using (UnityWebRequest web = UnityWebRequest.Get(url+ "?php_name="+name+"&php_player="+player_now.login))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                Debug.Log(web.downloadHandler.text);
                if(web.downloadHandler.text != null&&name!="" && web.downloadHandler.text.Equals("true") )
                {
                    //data_sql.player_now = lt_pl[0];
                    //StartCoroutine(status_account("http://localhost/DBUnity/logining.php", lg, "online", false));
                    st_perehod_scene("menu chats");
                }
                else
                {
                    //if (name == "")
                        //GameObject.Find("Canvas").transform.Find("pn error").Find("tx error").gameObject.GetComponent<Text>().text = "поле не может быть пустым";
                    //else
                    GameObject.Find("Canvas").transform.Find("pn error").Find("tx error").gameObject.GetComponent<Text>().text = "„ат с таким именем уже существует";
                    GameObject.Find("Canvas").transform.Find("pn error").gameObject.SetActive(true);
                }
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
    public IEnumerator post_battle_start(string url)
    {
        WWWForm form = new WWWForm();
        //form.AddField("php_bl", "false");
        using (UnityWebRequest web = UnityWebRequest.Get(url + "?php_bl=0") )
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                Debug.Log(web.downloadHandler.text);
                string[] stt = web.downloadHandler.text.Split("_");
                id_battle_now = int.Parse(stt[0]);
            }
            web.Dispose();
        }


    }
    public IEnumerator post_battle_end(string url,string win,string lose)
    {
        WWWForm form = new WWWForm();
        form.AddField("php_bl", "1");
        form.AddField("php_player_win", win);
        form.AddField("php_player_lose", lose);
        form.AddField("php_id", id_battle_now.ToString());
        using (UnityWebRequest web = UnityWebRequest.Post(url, form))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                //Debug.Log(web.downloadHandler.text);
                //string[] stt = web.downloadHandler.text.Split("_");
                id_battle_now = -1;
            }
            web.Dispose();
        }


    }
    public IEnumerator get_battle_infa(string url)
    {
        using (UnityWebRequest web = UnityWebRequest.Get(url+"?php_player="+player_now.login))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                Debug.Log(web.downloadHandler.text);
                string[] stt = web.downloadHandler.text.Split("_");
                if (!(web.downloadHandler.text == "" || web.downloadHandler.text == null))
                {
                    //string[] stt = web.downloadHandler.text.Split("_");
                    //Debug.Log(web.downloadHandler.text);

                    if (stt.Length > 0)
                    {

                        for (int i = 0; i < stt.Length - 1; i += 3)
                        {

                            int lg_time = int.Parse( stt[i + 1].Split(".")[0]);
                            string st_lg_time = (lg_time / 60).ToString() + ":" + (lg_time % 60).ToString();
                            battle bt = new battle(st_lg_time, stt[i], stt[i + 2]);
                            que_bt_infa.Enqueue(bt);
                            //int ind_now = i / 2;
                            //Debug.Log(stt[i] + " " + stt[i + 1]);
                            //message pl = new message(int.Parse(stt[i]), stt[i + 1], int.Parse(stt[i + 2]), stt[i + 3], stt[i + 4]);

                            //que_ms_now.Enqueue(pl);
                            //que_ms_now_last.Enqueue(pl);
                        }
                        foreach (battle bt in que_bt_infa)
                        {
                            GameObject gm = Instantiate(GameObject.Find("ignoring").transform.Find("pn infa").gameObject, GameObject.Find("Scroll View").transform.Find("Viewport").Find("Content"));
                            gm.transform.Find("tx name").gameObject.GetComponent<Text>().text = bt.name;
                            GameObject gm1 = gm.transform.Find("tx status").gameObject;


                            if (bt.status.Equals("win"))
                            {
                                gm1.GetComponent<Text>().color = Color.green;
                                gm1.GetComponent<Text>().text = "выигрыш";
                            }

                            else
                            {
                                gm1.GetComponent<Text>().color = Color.red;
                                gm1.GetComponent<Text>().text = "проигрыш";
                            }

                            //gm1.GetComponent<Text>().text = ch_now.status;
                            GameObject gm2 = gm.transform.Find("tx long").gameObject;
                            gm2.GetComponent<Text>().text = bt.lont_time;
                        }


                    }

                }
                //id_battle_now = -1;
            }
            web.Dispose();
        }


    }


    float timer = 0;
    static string date_now_second;
    public bool bl_entry_chat = true;
    public bool bl_entry_infa_chat = true;
    public bool get_message_bl_now = false;
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
                    StartCoroutine(get_message("http://localhost/DBUnity/get_message.php", data_sql.chat_now.id));
                }
            }

        }
        else if(SceneManager.GetActiveScene().name == "infa chat")
        {
            if (bl_entry_infa_chat)
            {
                StartCoroutine(get_players_chat("http://localhost/DBUnity/get_players_chat.php", data_sql.chat_now, false));
                bl_entry_infa_chat=false;
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
