using System.Collections;
using System.Collections.Generic;
using Dissonance;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : NetworkBehaviour
{
    public GameObject activateVoice;
    public Rigidbody2D rigidbody2d;
    public NewNetworkManager ntManager;

    public bool triggered = false;
    public bool triggerStatus = false;
    private RoomChannels channel;
    public RoomChannel chan;
    private DissonanceComms dc;
    public Text statusText;


    // Start is called before the first frame update
    void Start()
    {
        ntManager = GameObject.Find("NetworkManager").GetComponent<NewNetworkManager>();

    }

    [SyncVar(hook = nameof(OnStatusChange))]
    public string statusData;

    void OnStatusChange(string oldStatusData, string newStatusData)
    {
        print("OnStatusChange");
        //TODO: I could change the statusText object from the server with it's id??
        statusText.text = newStatusData;
    }


    [Command]
    void CmdUpdateStatus()
    {
        print("CmdUpdateStatus");
        statusData = Random.Range(100, 999).ToString();
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            rigidbody2d.velocity = new Vector2(0, Input.GetAxisRaw("Vertical")) *
                                   (700 * Time.fixedDeltaTime);

        }
        //InvokeRepeating(nameof(CmdUpdateStatus), 1, 1);


    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }

    public override void OnStartClient()
    {
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
            print("clicked");
            //chan = channel.Open("Global", true);
            CmdUpdateStatus();
            triggered = true;
            //triggerStatus = true;
        }
        else
        {
            print("!clicked");
            //chan.Dispose();
            CmdUpdateStatus();
            triggered = false;
        }
    }
}
