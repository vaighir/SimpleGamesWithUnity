using System.Collections;
using System.Collections.Generic;

public class OTetromino : Tetromino
{
   public OTetromino()
    {
        this.id = 0;
        this.activeShape = DrawO();

        this.shapes.Add(0, DrawO());
        this.shapes.Add(1, DrawO());
        this.shapes.Add(2, DrawO());
        this.shapes.Add(3, DrawO());
    }

    private int[,] DrawO()
    {
        int[,] shape = new int[4, 4];

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (j < 2 && i > 0 && i < 3)
                {
                    shape[i, j] = 1;
                } else
                {
                    shape[i, j] = 0;
                }
            }
        }

        return shape;
    }
}
