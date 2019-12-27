using System;

public partial class Board
{
    private void GenerateMines()
    {
        Random random = new Random();
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                GenerateMines(random, i, j);
            }
        }
    }
    private void GenerateMines(Random random, int i, int j)
    {
        if (Equals(playerOnePosition, new Tuple<int, int>(i, j))
        || Equals(playerTwoPosition, new Tuple<int, int>(i, j)))
        {
            tiles[i, j].mine = false;
            return;
        }

        tiles[i, j].mine = (random.Next()%3) == 0;

        if (!tiles[i, j].mine) 
            return;

        if (i - 1 >= 0)
        {
            if (j - 1 >= 0)
            {
                tiles[i - 1, j - 1].number++;
            }
            
            tiles[i - 1, j    ].number++;
            
            if (j + 1 < tiles.GetLength(1))
            {
                tiles[i - 1, j + 1].number++;
            }
        }

        if (i + 1 < tiles.GetLength(0))
        {
            if (j - 1 >= 0)
            {
                tiles[i + 1, j - 1].number++;
            }

            tiles[i + 1, j    ].number++;
            
            if (j + 1 < tiles.GetLength(1))
            {
                tiles[i + 1, j + 1].number++;
            }
        }

        if (j - 1 >= 0)
        {
            tiles[i    , j - 1].number++;
        }

        if (j + 1 < tiles.GetLength(1))
        {
            tiles[i    , j + 1].number++;
        }
    }

    private void SetAllTextNumbers()
    {
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                NumberTextMeshes[i, j].text = tiles[i, j].number + (tiles[i, j].mine ? "m" : "");
            }
        }
    }
}