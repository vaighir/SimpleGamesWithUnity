using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private int[,] dataGrid;
    private SpriteRenderer[,] displayGrid;
    private int height, width, snakeLength, moveX, moveY, eatingCounter;
    private float offsetX, offsetY, lastMoveTime, gameSpeed;
    private Snake snake;
    private SnakeBlock start, food, newTail;
    private bool gameOver, foodAvailable, changingDirection, eating;
    private string direction;

    [SerializeField] Sprite backgroundSprite, activeSprite;

    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();
        InitializeGrids();
        SpawnSnake();
    }

    private void InitializeVariables()
    {
        height = 2 * (int)Camera.main.orthographicSize;
        width = (int)(height * Screen.width / Screen.height);
        offsetX = -(0.5f * width - 0.5f);
        offsetY = 0.5f * height - 0.5f;

        snakeLength = 4;
        start = new SnakeBlock((int)(width / 2), (int)((height - snakeLength) / 2));

        gameOver = false;
        foodAvailable = false;
        changingDirection = false;
        eating = false;

        lastMoveTime = Time.time;
        gameSpeed = 0.5f;
        direction = "up";
        eatingCounter = 0;
    }

    private void InitializeGrids()
    {
        dataGrid = new int[width, height];
        displayGrid = new SpriteRenderer[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                dataGrid[i, j] = 0;
                SpawnBlocks(i, j);
            }
        }
    }

    private void SpawnBlocks(int x, int y)
    {
        GameObject g = new GameObject("X: " + x + "Y: " + y);
        g.transform.position = new Vector3(x + offsetX, -y + offsetY, 0);
        var s = g.AddComponent<SpriteRenderer>();
        s.sprite = backgroundSprite;
        s.color = new Color(0, 0, 0, 1);
        displayGrid[x, y] = s;
    }

    private void SpawnSnake()
    {
        snake = new Snake(snakeLength, start.x, start.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            Time.timeScale = 0;
        }
        else
        {
            if (!foodAvailable)
            {
                SpawnFood();
            }

            if (!changingDirection)
            {
                ChangeDirection();
            }
            
            if (Time.time - lastMoveTime > gameSpeed)
            {
                SetMovementCoordinates();
                Move();
                lastMoveTime = Time.time;
            }

            if (eating && eatingCounter == 0)
            {
                Debug.Log("eaten!");
                snake.Eat(newTail);
                snakeLength++;
                eating = false;
            }

            UpdateDataGrid();
            DisplayGrid();
        }

    }

    private void SpawnFood()
    {
        int x = Random.Range(0, width);
        int y = Random.Range(0, height);

        if(dataGrid[x, y] != 0)
        {
            SpawnFood();
        }

        food = new SnakeBlock(x, y);
        foodAvailable = true;
    }

    private void SetMovementCoordinates()
    {
        if(direction == "up")
        {
            moveX = 0;
            moveY = -1;
        }
        else if(direction == "down")
        {
            moveX = 0;
            moveY = 1;
        }
        else if (direction == "left")
        {
            moveX = -1;
            moveY = 0;
        }
        else if (direction == "right")
        {
            moveX = 1;
            moveY = 0;
        }
        else
        {
            moveX = 0;
            moveY = 0;
        }
    }

    private void Move()
    {
        SnakeBlock newHead = snake.GetNewHead(moveX, moveY);

        if (!CheckCollision(newHead))
        {
            if (eating)
            {
                eatingCounter--;
            }
            snake.MoveSnake(moveX, moveY);
            if(snake.head.x == food.x && snake.head.y == food.y)
            {
                Debug.Log("eating");
                newTail = food;
                Debug.Log(newTail.x);
                Debug.Log(newTail.y);
                eating = true;
                
                eatingCounter = snakeLength;
                foodAvailable = false;
            }
            
        }

        if (changingDirection)
        {
            changingDirection = false;
        }
    }

    private void ChangeDirection()
    {
        if(Input.GetKey("w") && direction != "down")
        {
            direction = "up";
            changingDirection = true;
        } else if(Input.GetKey("s") && direction != "up")
        {
            direction = "down";
            changingDirection = true;
        } else if(Input.GetKey("a") && direction != "right")
        {
            direction = "left";
            changingDirection = true;
        } else if(Input.GetKey("d") && direction != "left")
        {
            direction = "right";
            changingDirection = true;
        }
    }

    private bool CheckCollision(SnakeBlock newHead)
    {
        bool status = false;

        if (newHead.x < 0 || newHead.x >= width || newHead.y < 0 || newHead.y >= height)
        {
            Debug.Log("you hit the wall");
            status = true;
        } else if (dataGrid[newHead.x, newHead.y] == 1)
        {
            Debug.Log("you hit yourself");
            status = true;
        }

        return status;
    }

    private void UpdateDataGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                dataGrid[i, j] = 0;
            }
        }

        dataGrid[food.x, food.y] = 2;

        for(int i = 0; i < snakeLength; i++)
        {
            SnakeBlock currentBlock = snake.blocks[i];
            dataGrid[currentBlock.x, currentBlock.y] = 1;
        }
    }

    private void DisplayGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (dataGrid[i, j] == 0)
                {
                    displayGrid[i, j].sprite = backgroundSprite;
                    displayGrid[i, j].color = new Color(0, 0, 0, 1);
                }
                else
                {
                    displayGrid[i, j].sprite = activeSprite;
                    displayGrid[i, j].color = new Color(0, 0, 1, 1);
                }
            }
        }
    }
}
