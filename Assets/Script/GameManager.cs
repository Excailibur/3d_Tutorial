using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
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

    [SerializeField] TextMeshProUGUI p1Text;
    [SerializeField] TextMeshProUGUI p2Text;
    [SerializeField] TextMeshProUGUI currentTurnText;
    [SerializeField] TextMeshProUGUI messageText;

    [SerializeField] GameObject restartButton;
    [SerializeField] Transform headPosition;

    void Start()
    {
        player = CurrentPlayer.Player1;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    void NoMoreBalls(CurrentPlayer currentPlayer)
    {
        if(currentPlayer == CurrentPlayer.Player1)
        {
            P1WinningShot = true;
        }
        else if(currentPlayer == CurrentPlayer.Player2)
        {
            P2WinningShot = true;
        }
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
                    NextPlayerTurn();                
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
                    NextPlayerTurn();
                }
            }
        }
        return true;
    }

    void Lose(String message)
    {
        messageText.gameObject.SetActive(true);
        messageText.text = message;

        restartButton.gameObject.SetActive(true);
    }

    void Win(string player)
    {
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
