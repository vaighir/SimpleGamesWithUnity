using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject ball;
    [SerializeField] GameObject topLeft;
    [SerializeField] GameObject bottomRight;
    [SerializeField] GameObject cameraMin;
    [SerializeField] GameObject cameraMax;
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    [SerializeField] Text Score;
    [SerializeField] AudioSource bounce, miss;
    private float playerXSize, playerYSize;
    private Vector3 position;
    private float xSpeed, ySpeed;
    private bool upOk, downOk, bounceBack;
    private int player1Score, player2Score;
    private float xMin, xMax, xCheck, yMin, yMax, yCheck;

    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();
        Center();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayScore();
        Move();
    }

    private void InitializeVariables()
    {
        position = transform.position;

        xMin = -6.0f;
        xMax = 6.0f;
        xCheck = 3.5f;
        yMin = -2.0f;
        yMax = 2.0f;
        yCheck = 0.5f;

        InitializeSpeed();

        playerXSize = 0.2f;
        playerYSize = 2.5f;
        player1Score = 0;
        player2Score = 0;
    }

    private void Center()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    private void InitializeSpeed()
    {
        xSpeed = Random.Range(xMin, xMax);
        ySpeed = Random.Range(yMin, yMax);

        if (xSpeed < xCheck && xSpeed > -xCheck || ySpeed < yCheck && ySpeed > -yCheck)
        {
            InitializeSpeed();
        }
    }

    private void DisplayScore()
    {
        Score.text = player1Score + "       -       " + player2Score;
    }

    private void Move()
    {
        transform.position = transform.position + new Vector3(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, 0);

        if (topLeft.transform.position.y >= cameraMin.transform.position.y || bottomRight.transform.position.y <= cameraMax.transform.position.y)
        {
            ySpeed = -ySpeed;
        }

        if (topLeft.transform.position.x < player1.transform.position.x + playerXSize)
        {
            BounceBack(player1);
        }
        else if (bottomRight.transform.position.x > player2.transform.position.x - playerXSize)
        {
            BounceBack(player2);
        }
    }

    private void BounceBack(GameObject player)
    {
        float playerTopPosition = player.transform.position.y + 0.5f * playerYSize;
        float playerBottomPosition = player.transform.position.y - 0.5f * playerYSize;

        if (transform.position.y < playerTopPosition && transform.position.y > playerBottomPosition)
        {
            xSpeed = -xSpeed;
            xSpeed = SpeedUp(xSpeed);
            ySpeed = SpeedUp(ySpeed);

            bounce.Play(0);

        } else
        {
            if (transform.position.x < 0)
            {
                player2Score += 1;
            } else
            {
                player1Score += 1;
            }

            miss.Play(0);

            Clean();
            InitializeSpeed();
            Center();
        } 
    }

    private float SpeedUp(float CurrentSpeed)
    {
        if (CurrentSpeed > 0)
        {
            CurrentSpeed += 0.2f;
        } else
        {
            CurrentSpeed -= 0.2f;
        }

        return CurrentSpeed;
    }

    private void Clean()
    {
        if (player1Score > 9 || player2Score > 9)
        {
            player1Score = 0;
            player2Score = 0;
        }

        if (xSpeed > 0)
        {
            xMin = -6.0f;
            xMax = 0;
        } else
        {
            xMin = 0;
            xMax = 6.0f;
        }
    }
}
