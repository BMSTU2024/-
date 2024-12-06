using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ui_chat : MonoBehaviour
{
    // Start is called before the first frame update
    public int type = 0;
    public IEnumerator action;
    public Queue<data_sql.message> messages = new Queue<data_sql.message>();
    public void print_chats(List<data_sql.lt_chat> lt_ch)
    {
        GameObject par = GameObject.Find("Scroll View").transform.Find("Viewport").Find("Content").gameObject;
        int ii = par.transform.childCount;
        for (int i = 0; i < ii; i++)
        {
            Debug.Log("is truue[1] " + i);
            Destroy(par.transform.GetChild(i).gameObject);
        }
        ii = 0;
        if (type == 0)
        {
            foreach (data_sql.lt_chat st in lt_ch)
            {
                Debug.Log("is truue[2] " + ii);
                GameObject obj = Instantiate(GameObject.Find("ignoring").transform.Find("bt chat").gameObject, par.transform);
                obj.transform.Find("name chat").GetComponent<Text>().text = st.ch;
                obj.GetComponent<ui_button>().type = st;
                obj.active = true;
                ii++;
            }
        }
        else if(type==1)
        {
            foreach (data_sql.lt_chat st in lt_ch)
            {
                Debug.Log("is truue[2] " + ii);
                GameObject obj = Instantiate(GameObject.Find("ignoring").transform.Find("bt add chat").gameObject, par.transform);
                obj.transform.Find("name chat").GetComponent<Text>().text = st.ch;
                obj.transform.Find("bt yes").GetComponent<ui_button>().type=st;
                obj.transform.Find("bt not").GetComponent<ui_button>().type = st;
                //obj.GetComponent<ui_button>().type = st;
                obj.active = true;
                ii++;
            }
        }

    }
    public void print_players(List<string> lt_ch)
    {
        GameObject par = GameObject.Find("Scroll View").transform.Find("Viewport").Find("Content").gameObject;
        int ii = par.transform.childCount;
        for (int i = 0; i < ii; i++)
        {
            Debug.Log("is truue[1] " + i);
            Destroy(par.transform.GetChild(i).gameObject);
        }
        ii = 0;
        foreach (string st in lt_ch)
        {
            Debug.Log("is truue[2] " + ii);
            GameObject obj = Instantiate(GameObject.Find("ignoring").transform.Find("bt player").gameObject, par.transform);
            obj.transform.Find("name player").GetComponent<Text>().text = st;
            obj.GetComponent<ui_button>().type = st;
            obj.active = true;
            ii++;
        }
    }
    public void print_message(Queue<data_sql.message> lt_ms)
    {

        messages = lt_ms;
        timer = 0;
        GameObject par = GameObject.Find("Scroll pn").transform.Find("Viewport").Find("Content").Find("pn").gameObject;
        int ii = par.transform.childCount;
        foreach (data_sql.message st in lt_ms)
        {
            GameObject obj = null;
            if (st.sender == data_sql.chat_now.id)
            {
                obj = Instantiate(GameObject.Find("ignoring").transform.Find("message post").gameObject, par.transform);
            }

            else
            {
                obj = Instantiate(GameObject.Find("ignoring").transform.Find("message get").gameObject, par.transform);
            }
            obj.transform.Find("tx login").GetComponent<Text>().text = st.login;
            obj.transform.Find("rm tx").Find("tx infa").GetComponent<Text>().text = st.text;
            obj.active = true;


            Scrollbar bar = GameObject.Find("Scrollbar Vertical").GetComponent<Scrollbar>();

            /*
            RectTransform re = GameObject.Find("Scroll pn").transform.Find("Viewport").Find("Content").GetComponent<RectTransform>();
            RectTransform re_pn = GameObject.Find("Scroll pn").GetComponent<RectTransform>();
            RectTransform rect_canv = GameObject.Find("Canvas").GetComponent<RectTransform>();
            float d_size = re.anchoredPosition.y-re.sizeDelta.y;


            if (d_size<rect_canv.sizeDelta.y/2)
            {
                Debug.Log("upd scroll" + (rect_canv.sizeDelta.y- re_pn.sizeDelta.y) +" "+ re.localPosition+"   "+ re.sizeDelta.y+"  "+ obj.GetComponent<RectTransform>().sizeDelta);
                //RectTransform re= GameObject.Find("Scroll pn").transform.Find("Viewport").Find("Content").GetComponent<RectTransform>();
                Vector2 v2 = re.anchoredPosition;
                v2.y = re.sizeDelta.y- (rect_canv.sizeDelta.y - re_pn.sizeDelta.y)+ obj.GetComponent<RectTransform>().sizeDelta.y;
                re.localPosition = v2;
                
        }
            */
            //bar.value = 0;
            /*
            Debug.Log("is truue[2] " + ii);
            GameObject obj = Instantiate(GameObject.Find("ignoring").transform.Find("bt chat").gameObject, par.transform);
            obj.transform.Find("name chat").GetComponent<Text>().text = st;
            obj.active = true;
            ii++;
            */
        }
        if (lt_ms.Count != 0)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(par.GetComponent<RectTransform>());

            RectTransform re = GameObject.Find("Scroll pn").transform.Find("Viewport").Find("Content").GetComponent<RectTransform>();
            RectTransform re_pn = GameObject.Find("Scroll pn").GetComponent<RectTransform>();
            RectTransform rect_canv = GameObject.Find("Canvas").GetComponent<RectTransform>();
            float d_size = re.sizeDelta.y - re.anchoredPosition.y;
            Debug.Log(d_size + " " + rect_canv.rect.height);
            if (d_size < rect_canv.rect.height)
            {
                Debug.Log(true);
                GameObject.Find("Scroll pn").GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
            }
        }
    }
    private void print_message_bl(Queue<data_sql.message> lt_ms)
    {
        GameObject par = GameObject.Find("Scroll pn").transform.Find("Viewport").Find("Content").Find("pn").gameObject;
        /*
        GameObject par = GameObject.Find("Scroll pn").transform.Find("Viewport").Find("Content").Find("pn").gameObject;
        int ii = par.transform.childCount;
        foreach (data_sql.message st in lt_ms)
        {
            GameObject obj = null;
            if (st.sender == data_sql.id_chat_now)
            {
                obj=Instantiate(GameObject.Find("ignoring").transform.Find("message post").gameObject, par.transform);
            }
                
            else
            {
                obj=Instantiate(GameObject.Find("ignoring").transform.Find("message get").gameObject, par.transform);
            }
            obj.transform.Find("tx login").GetComponent<Text>().text = st.login;
            obj.transform.Find("rm tx").Find("tx infa").GetComponent<Text>().text = st.text;
            obj.active = true;


            Scrollbar bar = GameObject.Find("Scrollbar Vertical").GetComponent<Scrollbar>();

            /*
            RectTransform re = GameObject.Find("Scroll pn").transform.Find("Viewport").Find("Content").GetComponent<RectTransform>();
            RectTransform re_pn = GameObject.Find("Scroll pn").GetComponent<RectTransform>();
            RectTransform rect_canv = GameObject.Find("Canvas").GetComponent<RectTransform>();
            float d_size = re.anchoredPosition.y-re.sizeDelta.y;


            if (d_size<rect_canv.sizeDelta.y/2)
            {
                Debug.Log("upd scroll" + (rect_canv.sizeDelta.y- re_pn.sizeDelta.y) +" "+ re.localPosition+"   "+ re.sizeDelta.y+"  "+ obj.GetComponent<RectTransform>().sizeDelta);
                //RectTransform re= GameObject.Find("Scroll pn").transform.Find("Viewport").Find("Content").GetComponent<RectTransform>();
                Vector2 v2 = re.anchoredPosition;
                v2.y = re.sizeDelta.y- (rect_canv.sizeDelta.y - re_pn.sizeDelta.y)+ obj.GetComponent<RectTransform>().sizeDelta.y;
                re.localPosition = v2;
                
        }
            */
        //bar.value = 0;
        /*
        Debug.Log("is truue[2] " + ii);
        GameObject obj = Instantiate(GameObject.Find("ignoring").transform.Find("bt chat").gameObject, par.transform);
        obj.transform.Find("name chat").GetComponent<Text>().text = st;
        obj.active = true;
        ii++;
        *//*
    }
*/
        if (lt_ms.Count != 0)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(par.GetComponent<RectTransform>());

            RectTransform re = GameObject.Find("Scroll pn").transform.Find("Viewport").Find("Content").GetComponent<RectTransform>();
            RectTransform re_pn = GameObject.Find("Scroll pn").GetComponent<RectTransform>();
            RectTransform rect_canv = GameObject.Find("Canvas").GetComponent<RectTransform>();
            float d_size = re.sizeDelta.y - re.anchoredPosition.y;
            Debug.Log(d_size + " " + rect_canv.rect.height);
            if (d_size < rect_canv.rect.height)
            {
                Debug.Log(true);
                GameObject.Find("Scroll pn").GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
            }
        }

        //GameObject.Find("Scroll pn").GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
    }
    void Start()
    {
        
    }
    public float timer = 0;
    // Update is called once per frame
    
    void Update()
    {
        /*
        if (timer < 1)
            timer += Time.deltaTime;
        if (messages != null && SceneManager.GetActiveScene().name=="chat" &&timer<1)
        {
            print_message_bl(messages);
            messages = null;
            //timer = 0;
        }
        */
    }
}
