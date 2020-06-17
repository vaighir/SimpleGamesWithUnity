using System.Collections;
using System.Collections.Generic;

public class STetromino : Tetromino
{
    public STetromino()
    {
        this.id = 6;
        this.activeShape = Draw(0);

        this.shapes.Add(0, Draw(0));
        this.shapes.Add(1, Draw(1));
        this.shapes.Add(2, Draw(0));
        this.shapes.Add(3, Draw(1));
    }

    public override int[,] Fill(int[,] shape, int mode)
    {
        switch (mode)
        {
            case 0:
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (i > 0 && i < 3 && j == 0)
                        {
                            shape[i, j] = 1;
                        }
                        else if (i < 2 && j == 1)
                        {
                            shape[i, j] = 1;
                        }
                    }
                }
                break;
            case 1:
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (i == 1 && j < 2)
                        {
                            shape[i, j] = 1;
                        }
                        else if (i == 2 && j > 0 && j < 3)
                        {
                            shape[i, j] = 1;
                        }
                    }
                }
                break;
        }
        return shape;
    }
}