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
	public GameObject uniBtn2;

	public bool pointerDown = false;
	private bool pointerUp = false;
	private float clickTimer = 0.7f;
	public bool endTimer = false;

	//[SyncVar]
	public static string text = "Start";

	// Start is called before the first frame update
	private void Start()
	{
		uniText2 = uniBtn2.GetComponentInChildren<TextMeshProUGUI>();
	}

	// Update is called once per frame
	private void Update()
	{
		uniText2.text = text;
		if (text == "stop")
			uniBtn2.GetComponent<Image>().color = Color.red;
		else
			uniBtn2.GetComponent<Image>().color = Color.green;

		if (pointerDown)
		{
			clickTimer -= Time.deltaTime;
			uniBtn2.GetComponent<Image>().fillAmount -= Time.deltaTime / clickTimer;
			if (clickTimer < 0)
			{
				pointerDown = false;
				clickTimer = 0.7f;
				endTimer = true;
				PlayerScript.triggerDisconnect = true;
			}
		}
		if (pointerUp)
		{
			clickTimer += Time.deltaTime;
			while (uniBtn2.GetComponent<Image>().fillAmount < 1)
			{
				uniBtn2.GetComponent<Image>().fillAmount += clickTimer;
			}

			if (clickTimer > 0.7)
			{
				pointerUp = false;
			}
		}
	}

	//This to change a player status from outside
	//Since directly triggering OpenVoiceComm from outside trigger an exception
	public void changeClickStatus()
	{
		PlayerScript.clicked = true;
	}

	public void OnPointerUp()
	{
		pointerUp = true;
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