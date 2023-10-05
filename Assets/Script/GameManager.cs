using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    enum CurrentPlayer
    {
        Player1,
        Player2
    }

    CurrentPlayer player;
    bool P1WinningShot = false;
    bool P2WinningShot = false;

    int player1BallsRemaining = 7;
    int player2BallsRemaining = 7;

    bool movement;
    bool willSwapPlayers;
    bool isGameOver;

    [SerializeField] float shotTimer = 3f;
    private float currentTimer;
    [SerializeField] float movementThreshold;

    [SerializeField] TextMeshProUGUI p1Text;
    [SerializeField] TextMeshProUGUI p2Text;
    [SerializeField] TextMeshProUGUI currentTurnText;
    [SerializeField] TextMeshProUGUI messageText;

    [SerializeField] GameObject restartButton;
    [SerializeField] Transform headPosition;

    [SerializeField] Camera cueStickCamera;
    [SerializeField] Camera overHeadCamera;
    Camera currentCamera;

    void Start()
    {
        player = CurrentPlayer.Player1;
        currentCamera = cueStickCamera;
    }

    private void Update()
    {
        bool allStopped = true;
        if (!movement && !isGameOver)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer > 0)
            {
                return;
            }
            foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
            {
                if (ball.GetComponent<Rigidbody>().velocity.magnitude >= movementThreshold)
                {
                    allStopped = false;
                    break;
                }
            }
        }
        if (allStopped)
        {
            movement = false;
            if (willSwapPlayers)
            {
                NextPlayerTurn();
            }
            else
            {
                SwitchCamera();
            }
            currentTimer = shotTimer;
        }
    }

    public void SwitchCamera()
    {
        if (currentCamera == cueStickCamera)
        {
            cueStickCamera.enabled = false;
            overHeadCamera.enabled = true;
            currentCamera = overHeadCamera;
            movement = true;
        }
        else
        {
            cueStickCamera.enabled = true;
            overHeadCamera.enabled = false;
            currentCamera = cueStickCamera;
            currentCamera.gameObject.GetComponent<CameraController>().ResetCamera();
        }
    }

    public void RestartTheGame()
    {
        SceneManager.LoadScene(0);
    }

    bool Scratch()
    {
        if(player == CurrentPlayer.Player1)
        {
            if (P1WinningShot)
            {
                ScratchOnWinningShot("Player 1");
                return true;
            }
        }
        else
        {
            if (P2WinningShot)
            {
                ScratchOnWinningShot("Player 2");
                return true;
            }
        }
        return false;
    }

    void EarlyEightball()
    {
        if(player == CurrentPlayer.Player1)
        {
            Lose("Early 8 ball sink: Player 1 Lose");
        }
        else
        {
            Lose("Early 8 ball sink: Player 2 Lose");
        }
    }

    void ScratchOnWinningShot(string playerName)
    {
        Lose(playerName + "Scratched on Final Shot and has lost");
    }

    bool CheckBall(Ball ball)
    {
        if (ball.IsCueBall())
        {
            if (Scratch())
            {
                return true;
            }
            else
            {
                return false;
            }
        }else if (ball.Is8Ball())
        {
            if(player == CurrentPlayer.Player1)
            {
                if (P1WinningShot)
                {
                    Win("Player 1");
                    return true;
                }
            }
            else
            {
                if (P2WinningShot)
                {
                    Win("Player 2");
                    return true;
                }
            }
            EarlyEightball();
        }
        else
        {
            if (ball.IsBallRed())
            {
                player1BallsRemaining--;
                p1Text.text = "Player 1 Balls: " + player1BallsRemaining;
                if(player1BallsRemaining >= 0)
                {
                    P1WinningShot = true;
                }
                if(player != CurrentPlayer.Player1)
                {
                    willSwapPlayers = true;
                }
            }
            else
            {
                player2BallsRemaining--;
                p2Text.text = "Player 2 Balls: " + player2BallsRemaining;
                if (player2BallsRemaining >= 0)
                {
                    P1WinningShot = true;
                }
                if (player != CurrentPlayer.Player2)
                {
                    willSwapPlayers = true;
                }
            }
        }
        return true;
    }

    void Lose(String message)
    {
        isGameOver = true;
        messageText.gameObject.SetActive(true);
        messageText.text = message;

        restartButton.gameObject.SetActive(true);
    }

    void Win(string player)
    {
        isGameOver = true;
        messageText.gameObject.SetActive(true);
        messageText.text = player + " Won";

        restartButton.gameObject.SetActive(true);
    }

    void NextPlayerTurn()
    {
        if (player == CurrentPlayer.Player1) 
        {
            player = CurrentPlayer.Player2;
            currentTurnText.text = "Player 2";
        }
        else
        {
            player = CurrentPlayer.Player1;
            currentTurnText.text = "Player 1";
        }

        willSwapPlayers = false;
        SwitchCamera();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("true");
        if(other.gameObject.tag == "Ball")
        {
            Debug.Log("ball");
            if (CheckBall(other.gameObject.GetComponent<Ball>()))
            {
                Destroy(other.gameObject);
            }
            else
            {
                Debug.Log("cueball");
                other.gameObject.transform.position = headPosition.position;
                other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
        
    }
}
