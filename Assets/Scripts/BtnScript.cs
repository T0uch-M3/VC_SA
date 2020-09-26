using Mirror;
using Mirror.Examples.Basic;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BtnScript : MonoBehaviour
{
    public TextMeshProUGUI uniText2;
    public bool pointerDown = false;
    float clickTimer = 0.7f;
    public bool endTimer = false;

    public GameObject uniBtn2;
    //[SyncVar]
    public static string text = "Start";
    // Start is called before the first frame update
    void Start()
    {
        uniText2 = uniBtn2.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        uniText2.text = text;
        if (text == "stop")
            uniBtn2.GetComponent<Image>().color = Color.red;
        else
            uniBtn2.GetComponent<Image>().color = Color.green;

        if (pointerDown)
        {
            clickTimer -= Time.deltaTime;
            if (clickTimer < 0)
            {
                pointerDown = false;
                clickTimer = 0.7f;
                endTimer = true;
                PlayerScript.triggerDisconnect = true;
            }

        }
    }
    //This to change a player status from outside
    //Since directly triggering OpenVoiceComm from outside trigger an exception
    public void changeClickStatus()
    {
        //PlayerScript.clicked = PlayerScript.clicked != true;
        PlayerScript.clicked = true;
    }
    public void OnPointerUp()
    {
        pointerDown = false;
        if (!endTimer)
        {
            clickTimer = 0.7f;
        }
    }
    public void OnPointerDown()
    {
        pointerDown = true;
    }
}
