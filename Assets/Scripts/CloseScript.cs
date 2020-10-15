using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void onClick()
    {
        Application.Quit();
        Debug.Log("Bye!");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
