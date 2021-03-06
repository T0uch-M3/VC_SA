﻿using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CloseScript : MonoBehaviour
{
	private GameObject CloseBtn;

	// Start is called before the first frame update
	private void Start()
	{
		CloseBtn = GameObject.FindWithTag("closeBtn");
		if (Application.platform == RuntimePlatform.Android)
		{
			CloseBtn.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
			CloseBtn.GetComponent<Image>().enabled = false;
			CloseBtn.GetComponent<Button>().enabled = false;
		}
	}

	public void onClick()
	{
		Application.Quit();
		Debug.Log("Bye!");
	}

	// Update is called once per frame
	private void Update()
	{
	}
}