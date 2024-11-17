using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class par_nature : NetworkBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    [SyncVar]
    public int hp;
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("CLICK");
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            //ui_move_mouse.start_selected = false;
            //GameObject.Find("select").GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            //if(!isServer)
            NetworkClient.Send(new func.struct_group_create { v2 = par_player.v2_in_world(Input.mousePosition), cel = gameObject });
            //else
            //par_mob.create_group(par_player.gm_selected, par_player.v2_in_world(Input.mousePosition), null);

        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
