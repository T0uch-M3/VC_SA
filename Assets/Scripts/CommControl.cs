using System.Collections;
using System.Collections.Generic;
using Dissonance;
using Mirror;
using UnityEngine;

public class CommControl : NetworkBehaviour
{
    public bool triggered = false;
    public bool triggerStatus = false;
    private RoomChannels channel;
    public RoomChannel chan;
    private DissonanceComms dc;


    //[SyncVar] private bool status;

   
    // Start is called before the first frame update
    void Start()
    {
        dc = GameObject.Find("DissonanceSetup").GetComponent<DissonanceComms>();
        channel = dc.RoomChannels;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    OpenVoiceComm();
        //}
    }

    public void OpenVoiceComm()
    {
        if (!triggered)
        {
            chan = channel.Open("Global", true);
            triggered = true;
            triggerStatus = true;
            CmdupdateStatusValue();
        }
        else
        {
            chan.Dispose();
            triggered = false;
            CmdupdateStatusValue();
        }
    }
    [Command]
    void CmdupdateStatusValue()
    {
        if (triggerStatus)
        {
            if (chan.IsOpen)
            {
                //NetworkIdentity ni = NetworkClient.connection.identity;
                //PlayerController pc = ni.GetComponent<PlayerController>();
                //pc.currentTarget = gameObject;
                print("checkpoint reached");
                Status s = GameObject.Find("Status").GetComponent<Status>();
                s.textValue = "Status = On";
                //status = true;
            }
            else
            {
                //status = false;
                Status s = GameObject.Find("Status").GetComponent<Status>();
                s.textValue = "Status = Off";
            }
        }
    }
}
