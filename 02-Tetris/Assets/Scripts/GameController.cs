using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int[,] dataGrid;
    private SpriteRenderer[,] displayGrid, nextDisplayGrid;
    private int height, width, spawnAnchorX, spawnAnchorY, activeAnchorX, activeAnchorY, activePrefix, settledPrefix, uniqueTetrominos, score, r, g, b;
    private float gameSpeed, moveSpeed, maxSpeed, speedUpdateIntervals, lastFallUpdateTime, lastMoveUpdateTime, lastSpeedUpdateTime;
    private bool gameOver, activeTetrominoAvailable;
    private Tetromino activeTetromino, nextTetromino;

    [SerializeField] Sprite backgroundSprite, activeSprite;
    [SerializeField] Text scoreText, gameOverText;
    [SerializeField] Button playAgain;
    [SerializeField] AudioSource clear, land, move;

    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();
        InitializeGrids();
        SpawnTetromino();
    }

    private void InitializeVariables()
    {
        height = 2 * (int)Camera.main.orthographicSize;
        width = (int)(0.6 * height);

        uniqueTetrominos = 7;
        score = 0;
        r = 0;
        g = 0;
        b = 0;

        TetrominoFactory(Random.Range(0, uniqueTetrominos));

        gameOverText.text = "";
        playAgain.gameObject.SetActive(false);

        gameOver = false;
        activeTetrominoAvailable = false;

        lastFallUpdateTime = Time.time;
        lastMoveUpdateTime = Time.time;
        lastSpeedUpdateTime = Time.time;

        gameSpeed = 0.5f;
        maxSpeed = 0.1f;
        moveSpeed = 0.05f;
        speedUpdateIntervals = 30;

        activePrefix = 20;
        settledPrefix = 10;

        spawnAnchorX = 4;
        spawnAnchorY = 0;
    }

    private void InitializeGrids()
    {
        dataGrid = new int[width, height];
        displayGrid = new SpriteRenderer[width, height];

        float offsetX = -(0.5f * width - 0.5f);
        float offsetY = 0.5f * height - 0.5f;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                dataGrid[i, j] = 0;
                SpawnBlocks(i, j, offsetX, offsetY, displayGrid);
            }
        }

        nextDisplayGrid = new SpriteRenderer[4, 4];

        offsetX = 0.5f * width + 1.5f;
        offsetY = 0.5f * height - 1.5f;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                SpawnBlocks(i, j, offsetX, offsetY, nextDisplayGrid);
            }
        }
    }

    private void SpawnBlocks(int x, int y, float offsetX, float offsetY, SpriteRenderer[,] grid)
    {
        GameObject g = new GameObject("X: " + x + "Y: " + y);
        g.transform.position = new Vector3(x + offsetX, -y + offsetY, 0);
        var s = g.AddComponent<SpriteRenderer>();
        s.sprite = backgroundSprite;
        s.color = new Color(0, 0, 0, 1);
        grid[x, y] = s;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            gameOverText.text = "GAME OVER";
            playAgain.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        if (Time.time - lastFallUpdateTime > gameSpeed)
        {
            FallDown();
            ClearRow();
            lastFallUpdateTime = Time.time;
        }

        if(Time.time - lastMoveUpdateTime > moveSpeed)
        {
            MoveTetromino();
            lastMoveUpdateTime = Time.time;
        }

        if(Time.time - lastSpeedUpdateTime > speedUpdateIntervals && gameSpeed > maxSpeed)
        {
            gameSpeed -= 0.05f;
            lastSpeedUpdateTime = Time.time;
        }

        if (Input.GetKeyDown("w"))
        {
            TurnTetromino();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        scoreText.text = score.ToString();

        RemoveActiveTetrominoFromGrid();
        AddActiveTetrominoToGrid(activePrefix);
        DisplayBlocks();
    }

    private void SpawnTetromino()
    {
        activeAnchorX = spawnAnchorX;
        activeAnchorY = spawnAnchorY;

        activeTetromino = nextTetromino;

        TetrominoFactory(Random.Range(0, uniqueTetrominos));

        if (CheckCollision(activeAnchorX, activeAnchorY, activeTetromino.activeShape))
        {
            gameOver = true;
        }
        else
        {
            AddActiveTetrominoToGrid(activePrefix);
            activeTetrominoAvailable = true;
        }

    }

    private void TetrominoFactory(int id)
    {
        switch (id)
        {
            case 1:
                nextTetromino = new ITetromino();
                break;
            case 2:
                nextTetromino = new TTetromino();
                break;
            case 3:
                nextTetromino = new LTetromino();
                break;
            case 4:
                nextTetromino = new JTetromino();
                break;
            case 5:
                nextTetromino = new ZTetromino();
                break;
            case 6:
                nextTetromino = new STetromino();
                break;
            default:
                nextTetromino = new OTetromino();
                break;
        }

    }

    private void FallDown()
    {
        if(CheckCollision(activeAnchorX, activeAnchorY + 1, activeTetromino.activeShape))
        {
            land.Play(0);
            AddActiveTetrominoToGrid(settledPrefix);
            activeTetrominoAvailable = false;
            SpawnTetromino();
        }
        else
        {
            activeAnchorY++;
        }
    }

    private void MoveTetromino()
    {
        if (Input.GetKey("a") && !CheckCollision(activeAnchorX - 1, activeAnchorY, activeTetromino.activeShape))
        {
            activeAnchorX--;
        }
        else if (Input.GetKey("d") && !CheckCollision(activeAnchorX + 1, activeAnchorY, activeTetromino.activeShape))
        {
            activeAnchorX++;
        }
        else if (Input.GetKey("s"))
        {
            FallDown();
        }
    }

    private void TurnTetromino()
    {
        int mode = activeTetromino.mode;
        int[,] newShape = activeTetromino.shapes[(mode + 1) % 4];
        if(!CheckCollision(activeAnchorX, activeAnchorY, newShape))
        {
            move.Play(0);
            activeTetromino.ChangeActiveShape();
        }
    }

    private void RemoveActiveTetrominoFromGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (dataGrid[i, j] >= activePrefix)
                {
                    dataGrid[i, j] = 0;
                }
            }
        }
    }

    private void AddActiveTetrominoToGrid(int prefix)
    {
        int[,] activeShape = activeTetromino.activeShape;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (activeShape[i, j] == 1)
                {
                    dataGrid[activeAnchorX + i, activeAnchorY + j] = prefix + activeTetromino.id;
                }
            }
        }
    }

    private bool CheckCollision(int newAnchorX, int newAnchorY, int[,] newShape)
    {
        bool status = false;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (newShape[i,j] == 1)
                {
                    if (newAnchorX + i >= width || newAnchorX + i < 0 || newAnchorY + j >= height)
                    {
                        status = true;
                    } else if (dataGrid[newAnchorX + i, newAnchorY + j] > 0 && dataGrid[newAnchorX + i, newAnchorY + j] < activePrefix)
                    {
                        status = true;
                    }
                }
            }
        }

        return status;
    }

    private void ClearRow()
    {
        for (int j = 0; j < height; j++)
        {
            int count = 0;
            for (int i = 0; i < width; i++)
            {
                if (dataGrid[i, j] >= settledPrefix && dataGrid[i,j] < activePrefix)
                {
                    count++;
                    Debug.Log(count);
                }
            }
            if (count == width)
            {
                clear.Play(0);
                score++;
                for (int i = 0; i < width; i++)
                {
                    dataGrid[i, j] = 0;
                }
                ShiftDown(j);
            }
        }
        
    }

    private void ShiftDown(int y)
    {
        for(int j = y; j > 0; j--)
        {
            for (int i = 0; i < width; i++)
            {
                dataGrid[i, j] = dataGrid[i, j - 1];
            }
        }
        for (int i = 0; i < width; i++)
        {
            dataGrid[i, 0] = 0; 
        }
    }

    private void DisplayBlocks()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (dataGrid[i, j] == 0)
                {
                    displayGrid[i, j].sprite = backgroundSprite;
                    displayGrid[i, j].color = new Color(0, 0, 0, 1);
                } else
                {
                    displayGrid[i, j].sprite = activeSprite;
                    int value = dataGrid[i, j] % 10;
                    GetColorValue(value);
                    displayGrid[i, j].color = new Color(r, g, b, 1);
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (nextTetromino.activeShape[i,j] == 0)
                {
                    nextDisplayGrid[i, j].sprite = backgroundSprite;
                    nextDisplayGrid[i, j].color = new Color(0, 0, 0, 1);
                }
                else
                {
                    nextDisplayGrid[i, j].sprite = activeSprite;
                    int value = nextTetromino.id;
                    GetColorValue(value);
                    nextDisplayGrid[i, j].color = new Color(r, g, b, 1);
                }
            }
        }
    }

    private void GetColorValue(int value)
    {
        r = 0;
        g = 0;
        b = 0;

        switch (value)
        {
            case 1:
                b = 1;
                break;
            case 2:
                g = 1;
                break;
            case 3:
                b = 1;
                g = 1;
                break;
            case 4:
                r = 1;
                break;
            case 5:
                r = 1;
                b = 1;
                break;
            case 6:
                r = 1;
                g = 1;
                break;
            default:
                r = 1;
                g = 1;
                b = 1;
                break;
        }
    }

    public void PlayAgain()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
