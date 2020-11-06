using Dissonance;
using Mirror.Examples.Additive;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorTransition : MonoBehaviour
{
    public float timeLeft = 5f;
    public float every = 0;
    private bool toZero = false;
    private bool toOne = true;
    public bool interpolating = false;
    private Color targetColor = Color.grey;
    private TextMeshProUGUI topText;
    private Color green = new Color(0f, 1f, 0);
    private Color red = new Color(1f, 0, 0);

    // Start is called before the first frame update
    private void Start()
    {
        //print("shader name "+gameObject.GetComponent<TextMeshPro>().color);
        //Debug.Log("shader");
        topText = GameObject.FindGameObjectWithTag("Headertext").GetComponent<TextMeshProUGUI>();
        //topText = gameObject.GetComponent<TextMeshPro>();
        //topText.faceColor = red;
        targetColor = green;
    }

    // Update is called once per frame
    private void Update()
    {
        //print("shader name " + gameObject.GetComponent<Material>());

        //topText.faceColor = Color.Lerp(green, SecGreen, every);

        //if (toOne)
        //{
        //    every += 0.0025f;
        //    if (every >= 1)
        //    {
        //        toOne = false;
        //        toZero = true;
        //        //targetColor = new Color(Random.value, Random.value, Random.value);
        //    }
        //}
        //if (toZero)
        //{
        //    every -= 0.0025f;
        //    if (every <= 0)
        //    {
        //        toOne = true;
        //        toZero = false;
        //    }
        //}

        //if (timeLeft <= Time.deltaTime)
        //{
        //    topText.faceColor = targetColor;

        //    targetColor = new Color(Random.value, Random.value, Random.value);
        //    timeLeft = 5f;
        //}
        //else
        //{
        //    //topText.faceColor = Color.Lerp(topText.faceColor, targetColor, 0.025f);
        //    topText.faceColor = Color.Lerp(topText.faceColor, targetColor, Time.deltaTime / timeLeft);

        //    timeLeft -= Time.deltaTime;
        //}

        //topText.faceColor = Color.Lerp(green, red, every);
        //topText.faceColor = Color.Lerp(green, SecGreen, every);

        topText.faceColor = Color.Lerp(topText.faceColor, targetColor, every);
        ///start "every" from 1
        if (toOne)
        {
            every += 0.0025f;
            if (every >= 5)
            {
                toOne = false;
                toZero = true;
                //topText.faceColor = targetColor;
            }
        }
        if (toZero)
        {
            every -= 0.0025f;
            if (every <= 0)
            {
                toOne = true;
                toZero = false;
                targetColor = new Color(Random.value, Random.value, Random.value);
            }
            //topText.faceColor = Color.Lerp(topText.faceColor, targetColor, every);
        }
        if (interpolating)
        {
            targetColor = new Color(Random.value, Random.value, Random.value);
            interpolating = false;
        }
    }
}