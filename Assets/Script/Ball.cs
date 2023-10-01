using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool isRed;
    private bool is8Ball = false;
    private bool isCueBall = false;

    private void Start()
    {
        
    }

    public bool IsBallRed() { return isRed; }
    public bool IsCueBall() {  return isCueBall; }
    public bool Is8Ball() { return is8Ball; }

    public void BallSetUp(bool red)
    {
        isRed = red;
        if (isRed)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    public void SetCueBall() {
        isCueBall = true;
    }

    public void SetEightBall() { 
        is8Ball = true;
        GetComponent<Renderer>().material.color = Color.black;
    }
}
