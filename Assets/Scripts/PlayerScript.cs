using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerScript : NetworkBehaviour
{
    public GameObject activateVoice;
    public Rigidbody2D rigidbody2d;
    public NewNetworkManager ntManager;

    // Start is called before the first frame update
    void Start()
    {
        ntManager = GameObject.Find("NetworkManager").GetComponent<NewNetworkManager>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            rigidbody2d.velocity = new Vector2(0, Input.GetAxisRaw("Vertical")) *
                                   (400 * Time.fixedDeltaTime);
        }
        
    }
    public override void OnStartClient()
    {
        //print("entered CLIENT START");
        base.OnStartClient();
        //CmdSpawn();
        
        transform.SetParent(GameObject.Find("ButtonPanel").transform);
        //transform.position = new Vector2(400, 500);
        //transform.position = ntManager.GetStartPosition().position;
    }




    //[Command]
    //void CmdSpawn()
    //{
    //    GameObject go = Instantiate(activateVoice, transform.position + new Vector3(400, 400, 0), Quaternion.identity);
    //    go.transform.parent = GameObject.Find("Canvas").transform;
    //    NetworkServer.Spawn(go, connectionToClient);
    //}
}
