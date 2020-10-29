using System;
using Dissonance;
using Dissonance.Audio;
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
    public Slider volSlider;

    public bool triggered = false;
    public bool triggerStatus = false;
    private RoomChannels chanCol;
    public RoomChannel roomChan;
    private DissonanceComms disCom;
    public Text statusText;
    public static bool clicked = false;
    public static bool triggerDisconnect = false;
    public float vol = 0;


    // Start is called before the first frame update
    void Start()
    {
        ntManager = GameObject.Find("NetworkManager").GetComponent<NewNetworkManager>();
        disCom = GameObject.Find("DissonanceSetup").GetComponent<DissonanceComms>();
        volSlider = GameObject.Find("VolSlider").GetComponent<Slider>();
        //channels collection
        chanCol = disCom.RoomChannels;
        //vol = disCom.RemoteVoiceVolume;
        //vol = roomChan.Volume;

        volSlider.onValueChanged.AddListener(SliderValChange);
    }

    [SyncVar(hook = nameof(OnStatusChange))]
    public string statusData;

    [SyncVar(hook = nameof(OnVolChange))]
    public float volData;



    void OnStatusChange(string oldStatusData, string newStatusData)
    {
        print("OnStatusChange");
        statusText.text = newStatusData;
        if (newStatusData == "open")
        {
            BtnScript.text = "stop";
            if (volSlider != null)
                volSlider.interactable = true;
        }
        else
        {
            BtnScript.text = "start";
            if (volSlider != null)
                volSlider.interactable = false;
        }
    }

    void OnVolChange(float oldVol, float newVol)
    {
        print("OnVolChange");
        volSlider.value = newVol;
    }


    [Command]
    void CmdUpdateStatus(string value)
    {
        //print("CmdUpdateStatus");
        statusData = value;
        //statusData = Random.Range(100, 999).ToString();
    }

    [Command]
    void CmdUpdateVol(float val)
    {
        volData = val;
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
            ntManager.StopHost();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            print(triggered.ToString());
        }

        if (!isLocalPlayer) return;
        if (NetworkClient.active)
        {
            CmdUpdateStatus(chanCol.Count > 0 ? "open" : "close");
            ///the slider only worked on one side, using it on the other side
            ///only triggered the animation, with no volume change.
            ///But after bringing CmdUpdateVol to here from under if(triggered)
            ///it started working on both side.
            CmdUpdateVol(vol);
        }



        //clicked change is triggered from inside BtnScript.cs
        if (clicked)
        {
            OpenVoiceComm();
            clicked = false;
        }

        if (triggerDisconnect)
        {
            ntManager.StopHost();
            triggerDisconnect = false;///the secret ingredient
        }

        if (triggered)
        {
            roomChan.Volume = vol;
        }
    }

    public void SliderValChange(float val)
    {
        vol = volSlider.value;
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
    }

    //The calling of CmdUpdateStatus below is counting on object authority to 
    //prevent unwanted access to the object when not "isLocalPlayer"
    public void OpenVoiceComm()
    {
        print("CLICK " + triggered.ToString());
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
            vol = roomChan.Volume;
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



}