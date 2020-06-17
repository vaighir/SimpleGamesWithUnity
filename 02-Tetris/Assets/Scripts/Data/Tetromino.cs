using System.Collections;
using System.Collections.Generic;

public class Tetromino
{
    public int id;
    public int[,] activeShape;
    public int mode = 0;
    public Dictionary<int, int[,]> shapes = new Dictionary<int, int[,]>();
    public float xAnchor;
    public float yAnchor;

    public void ChangeActiveShape()
    {
        mode++;
        mode = mode % 4;
        activeShape = shapes[mode];
    }

    public int[,] Draw(int mode)
    {
        int[,] shape = new int[4, 4];

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                shape[i, j] = 0;
            }
        }

        shape = Fill(shape, mode);

        return shape;
    }

    public virtual int[,] Fill(int[,] shape, int mode)
    {
        return shape;
    }
}
