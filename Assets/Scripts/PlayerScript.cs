using System;
using Dissonance;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerScript : NetworkBehaviour
{
    public GameObject activateVoice;
    public Rigidbody2D rigidbody2d;
    public NewNetworkManager ntManager;

    public static bool triggered = false;
    public bool triggerStatus = false;
    private RoomChannels chanCol;
    public RoomChannel roomChan;
    private DissonanceComms disCom;
    public Text statusText;
    public static bool clicked = false;


    // Start is called before the first frame update
    void Start()
    {
        ntManager = GameObject.Find("NetworkManager").GetComponent<NewNetworkManager>();
        disCom = GameObject.Find("DissonanceSetup").GetComponent<DissonanceComms>();
        //channels collection
        chanCol = disCom.RoomChannels;
    }

    [SyncVar(hook = nameof(OnStatusChange))]
    public string statusData;



    void OnStatusChange(string oldStatusData, string newStatusData)
    {
        print("OnStatusChange");
        statusText.text = newStatusData;
        if (newStatusData == "open")
        {
            BtnScript.text = "stop";
        }
        else
        {
            BtnScript.text = "start";
        }
    }


    [Command]
    void CmdUpdateStatus(string value)
    {
        //print("CmdUpdateStatus");
        statusData = value;
        //BtnScript.text = triggered ? "Stop" : "Start";

        //statusData = Random.Range(100, 999).ToString();
    }

    /* This another working method for sending messages from client to server
    public void UpdateStatus()
    {
        if (isServer)
        {
            statusData = Random.Range(100, 999).ToString();
        }
        else
        {
            CmdUpdateStatus();
        }
    }

    [Command]
    void CmdUpdateStatus()
    {
        UpdateStatus();
    }
    */



    //[Command]
    //void CloseClientSideChannels()
    //{

    //}


    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            rigidbody2d.velocity = new Vector2(0, Input.GetAxisRaw("Vertical")) *
                                   (700 * Time.fixedDeltaTime);

        }
        //InvokeRepeating(nameof(CmdUpdateStatus), 1, 1);
        if (Input.GetKeyDown(KeyCode.X))
        {
            print(chanCol.Count);
            print("TRIGGERED: " + triggered);

        }

        if (!isLocalPlayer) return;
        CmdUpdateStatus(chanCol.Count > 0 ? "open" : "close");

        //clicked change is triggered from inside BtnScript.cs
        if (clicked)
        {
            OpenVoiceComm();
            clicked = false;
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }

    public override void OnStartClient()
    {
        //CloseOpenedChannels();
        //print("entered CLIENT START");
        base.OnStartClient();

        transform.SetParent(GameObject.Find("ButtonPanel").transform);


        //transform.position = new Vector2(400, 500);
        //transform.position = ntManager.GetStartPosition().position;
    }
    //The calling of CmdUpdateStatus below is counting on object authority to 
    //prevent unwanted access to the object when not "isLocalPl yer"
    public void OpenVoiceComm()
    {
        if (!triggered)
        {
            if (BtnScript.text == "stop")
                return;
            print("clicked");
            if (isLocalPlayer)
                roomChan = chanCol.Open("Global", true);
            //CmdUpdateStatus();
            triggered = true;
            //triggerStatus = true;
            //clicked = false;
        }
        else
        {
            if (BtnScript.text == "start")
                return;
            print("!clicked");
            if (isLocalPlayer)
                roomChan.Dispose();
            //CmdUpdateStatus();
            triggered = false;
            //clicked = false;
        }
    }
    /// TODO/ Check the number of opened channel before closing from outside
    /// TODO/ the class, print it the count out and see, so we know whether we   
    /// TODO/ are checking the right object/script or not
    //public void CloseOpenedChannels()
    //{
    //    //try
    //    //{
    //    if (isServer)
    //    {
    //        print("CLOSE FROM SERVER");
    //        //print(chanCol.Count);
    //        roomChan.Dispose();
    //    }
    //    else
    //    {
    //        print("CLOSE FROM CLIENT");
    //        //print(chanCol.Count);
    //        roomChan.Dispose();
    //    }
    //    //}
    //    //catch (NullReferenceException ex)
    //    //{
    //    //    print(ex.Message);
    //    //}
    //}
}