using System;
using Dissonance;
using Mirror;
using Mirror.Websocket;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerScript : NetworkBehaviour
{
    public GameObject activateVoice;
    public Rigidbody2D rigidbody2d;
    public NewNetworkManager ntManager;

    public bool triggered = false;
    public bool triggerStatus = false;
    private RoomChannels chanCol;
    public RoomChannel roomChan;
    private DissonanceComms disCom;
    public Text statusText;
    public static bool clicked = false;
    public static bool triggerDisconnect = false;


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
            //ntManager.StopClient();
            
            

            ntManager.StopHost();
            //ntManager.OnApplicationQuit();



            //ntManager.StopServer();
            //print(chanCol.Count);
            //print("TRIGGERED: " + triggered);

        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //print("NBRPALYER  " + ntManager.numPlayers.ToString());
            //Destroy(GameObject.Find("NetworkManager"));

            //ntManager.StopHost();
            print(triggered.ToString());
            //print("TRIGGERED: " + triggered);

        }

        if (!isLocalPlayer) return;
        if (NetworkClient.active)
            CmdUpdateStatus(chanCol.Count > 0 ? "open" : "close");

        //clicked change is triggered from inside BtnScript.cs
        if (clicked)
        {
            OpenVoiceComm();
            clicked = false;
        }
        if (triggerDisconnect)
        {
                ntManager.StopHost();
            triggerDisconnect = false;//the secret ingredient
        }
        //print("TRIGGER DISCONNECT");
        //if(isServer)
        //    print("NBRPALYER  " + ntManager.numPlayers.ToString());
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

    //public void stopHost()
    //{
    //    if (isServer )
    //    {
    //        print("inside stopHost" );
    //        ntManager.StopHost();
    //    }
    //}

    //The calling of CmdUpdateStatus below is counting on object authority to 
    //prevent unwanted access to the object when not "isLocalPl yer"
    public void OpenVoiceComm()
    {
        print("CLICK "+triggered.ToString());
        if (!triggered)
        {
            if (isServer && BtnScript.text == "stop")
            {
                //print("NBRPALYER" + ntManager.numPlayers.ToString());
                ntManager.StopHost();
                triggered = true;
                return;
            }
            if (isClient && BtnScript.text == "stop")
            {
                ntManager.StopHost();
                triggered = true;
                return;
            }
            //IgnoranceThreaded.activeTransport.Shutdown();

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