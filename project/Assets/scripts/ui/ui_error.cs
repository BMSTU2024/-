using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ui_error : MonoBehaviour
{
    // Start is public called before the first frame update
    public Text tx;
    public float timer = 0;
    public void OnEnable()
    {
        timer = 0;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 5)
            timer += Time.deltaTime;
        else
            gameObject.SetActive(false);
    }
}
