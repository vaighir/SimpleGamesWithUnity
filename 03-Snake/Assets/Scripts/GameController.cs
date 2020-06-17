using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private int[,] dataGrid;
    private SpriteRenderer[,] displayGrid;
    private int height, width, snakeLength, startX, startY, foodX, foodY;
    private float offsetX, offsetY;
    private Snake snake;

    [SerializeField] Sprite backgroundSprite, activeSprite;

    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();
        InitializeGrids();
        SpawnSnake();
        SpawnFood();
    }

    private void InitializeVariables()
    {
        height = 2 * (int)Camera.main.orthographicSize;
        width = (int)(height * Screen.width / Screen.height);
        offsetX = -(0.5f * width - 0.5f);
        offsetY = 0.5f * height - 0.5f;

        snakeLength = 4;
        startX = (int)(width / 2);
        startY = (int)((height - snakeLength) / 2);
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
        snake = new Snake(snakeLength, startX, startY);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDataGrid();
        DisplayGrid();
    }

    private void SpawnFood()
    {
        //TODO add checking if snake is not already there
        foodX = Random.Range(0, width);
        foodY = Random.Range(0, height);
    }

    private void UpdateDataGrid()
    {
        dataGrid[foodX, foodY] = 2;

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
