using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TipsHandling : MonoBehaviour
{
    public float clickTimer = 0.7f;
    bool startCD = false; 
    float coolDown = 2;
    public bool clickDown = false;
    bool clickUp = false;
    bool goLink = false;
    string oldText;

    // Update is called once per frame
    void Update()
    {
        if (clickDown && !startCD)
        {

            clickTimer -= Time.deltaTime;
            if (clickTimer < 0)
            {
                startCD = true;
                clickDown = false;
                clickTimer = 0.7f;
                oldText = gameObject.GetComponent<TextMeshProUGUI>().text;
                gameObject.GetComponent<TextMeshProUGUI>().text = "https://github.com/T0uchM3";
            }

        }
        if (startCD)
        {
            coolDown -= Time.deltaTime;
            if (coolDown < 0)
            {
                coolDown = 2;
                gameObject.GetComponent<TextMeshProUGUI>().text = oldText;
                startCD = false;
            }
            if (clickDown)
            {
                Application.OpenURL("https://github.com/T0uchM3");
                clickDown = false;
            }
        }

    }
    public void OnPointerDown()
    {
        clickDown = true;

    }
    public void OnPointerUp()
    {
        clickUp = true;
        clickTimer = 0.7f;
        clickDown = false;

    }
}
