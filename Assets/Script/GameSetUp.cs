using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetUp : MonoBehaviour
{
    int redBallRemaining = 7;
    int blueBallRemaining = 7;

    float ballRadius;
    float ballDiameter;
    float ballDiameteWithBuffer;

    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform cueBallPosition;
    [SerializeField] Transform headBallPosition;
    void Start()
    {
        ballRadius = ballPrefab.GetComponent<SphereCollider>().radius = 100f;
        ballDiameter = ballRadius * 2;
        PlaceAllBalls();
    }

    void Update()
    {

    }

    private void PlaceAllBalls()
    {
        PlaceCueBall();
        PlaceRandomBalls();
    }

    private void PlaceCueBall()
    {
        GameObject ball = Instantiate(ballPrefab, cueBallPosition.position, Quaternion.identity);
        ball.GetComponent<Ball>().SetCueBall();
    }
    private void PlaceEightBall(Vector3 position)
    {
        GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
        ball.GetComponent<Ball>().SetEightBall();
    }

    private void PlaceRandomBalls()
    {
        int NumInThisRow = 1;
        int Rand;

        Vector3 firstInRowPosition = headBallPosition.position;
        Vector3 currentPositon = firstInRowPosition;

        void PlaceRedBall(Vector3 position)
        {
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
            ball.GetComponent<Ball>().BallSetUp(true);
            redBallRemaining--;
        }

        void PlaceBlueBall(Vector3 position)
        {
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
            ball.GetComponent<Ball>().BallSetUp(false);
            blueBallRemaining--;
        }

        //Outerloop
        for (int row = 0; row < 5; row++)
        {
            //InnerLoop
            for (int col = 0; col < NumInThisRow; col++)
            {
                //Place Eight ball in center
                if (row == 2 && col == 1)
                {
                    PlaceEightBall(currentPositon);
                    continue;
                }
                //Randomly place red or blue ball
                else if (redBallRemaining > 0 && blueBallRemaining > 0)
                {
                    Rand = UnityEngine.Random.Range(0, 2);
                    if (Rand == 0)
                    {
                        PlaceRedBall(currentPositon);
                        continue;
                    }
                    else
                    {
                        PlaceBlueBall(currentPositon);
                        continue;
                    }
                }
                //If only 
                else if (redBallRemaining > 0)
                {
                    PlaceRedBall(currentPositon);

                }
                else
                {
                    PlaceBlueBall(currentPositon);
                }

                currentPositon += new Vector3(1, 0, 0).normalized * ballDiameter;
            }
            firstInRowPosition += new Vector3(-1, 0, -1).normalized * ballDiameter;
            currentPositon = firstInRowPosition;
            NumInThisRow++;
        }
    }
}
