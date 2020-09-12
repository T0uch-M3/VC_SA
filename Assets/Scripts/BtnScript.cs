using Mirror;
using Mirror.Examples.Basic;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BtnScript : MonoBehaviour
{
    public TextMeshProUGUI uniText2;

    public GameObject uniBtn2;
    //[SyncVar]
    public static string text = "Start" ;
    // Start is called before the first frame update
    void Start()
    {
        uniText2 = uniBtn2.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        uniText2.text = text;
        //if (NewNetworkManager.clientLoaded)
        //{
        //    uniText2.text = text;
        //}
        //if (PlayerScript.triggered)
        //{
        //    uniText2.text = "Stop";
        //}
    }
    //This to change a player status from outside
    //Since directly triggering OpenVoiceComm from outside trigger an exception
    public void changeClickStatus()
    {
        PlayerScript.clicked = PlayerScript.clicked != true;
    }
}
