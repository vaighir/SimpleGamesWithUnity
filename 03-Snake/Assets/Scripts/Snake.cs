using System.Collections;
using System.Collections.Generic;

public class Snake
{
    public int lenght { get; set; }
    public SnakeBlock head { get; set; }
    public List<SnakeBlock> blocks { get; set; }

    public Snake(int lenght, int x, int y)
    {
        this.lenght = lenght;
        this.head = new SnakeBlock(x, y);
        DrawSnake();
    }

    private void DrawSnake()
    {
        blocks = new List<SnakeBlock>();
        blocks.Add(head);

        SnakeBlock lastBlock = this.head;
        SnakeBlock currentBlock;

        for (int i = 0; i < lenght - 1; i++)
        {
            currentBlock = new SnakeBlock(lastBlock.x, lastBlock.y + 1);
            blocks.Add(currentBlock);
            lastBlock = currentBlock;
        }
    }

    public SnakeBlock GetNewHead(int x, int y)
    {
        SnakeBlock newHead = new SnakeBlock(head.x + x, head.y + y);
        return newHead;
    }

    public void MoveSnake(int x, int y)
    {
        head = GetNewHead(x, y);
        blocks.Insert(0, head);
        blocks.RemoveAt(lenght);
    }

    public void Eat(SnakeBlock food)
    {
        blocks.Add(food);
        lenght++;
    }

    public SnakeBlock GetTail()
    {
        return blocks[lenght - 1];
    }
}
