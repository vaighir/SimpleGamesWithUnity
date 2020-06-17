using System.Collections;
using System.Collections.Generic;

public class ITetromino : Tetromino
{
    public ITetromino()
    {
        this.id = 1;
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
                        if (i == 1)
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
                        if (j == 1)
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