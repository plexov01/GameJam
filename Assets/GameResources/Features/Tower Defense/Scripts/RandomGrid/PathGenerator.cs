using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator
{
    private int width;
    private int height;
    private List<Vector2Int> pathCells;
    private List<Vector2Int> route;

    public int loopCount = 0;

    public PathGenerator(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public List<Vector2Int> GeneratePath(bool addLoops)
    {
        pathCells = new List<Vector2Int>();
        loopCount = 0;

        //int y = height / 2;
        int y = Random.Range(1, height - 1);
        int x = 1;

        while (x < width - 1)
        {
            pathCells.Add(new Vector2Int(x, y));

            bool validMove = false;

            while (!validMove)
            {
                int move = Random.Range(0, 3);

                if (move == 0 || x == 1 || x % 2 == 0 || x > (width - 2))
                {
                    x++;
                    validMove = true;
                }
                else if (move == 1 && CellIsEmpty(x, y + 1) && y < (height - 2))
                {
                    y++;
                    validMove = true;
                }

                else if (move == 2 && CellIsEmpty(x, y - 1) && y > 1)
                {
                    y--;
                    validMove = true;
                }
            }
        }

        if (addLoops)
        {
            AddLoops();
        }

        return pathCells;
    }

    public List<Vector2Int> GenerateRoute()
    {
        Vector2Int direction = Vector2Int.right;
        route = new List<Vector2Int>();
        Vector2Int currentCell = pathCells[0];

        while (currentCell.x < width - 1)
        {
            route.Add(new Vector2Int(currentCell.x, currentCell.y));

            if (CellIsTaken(currentCell + direction))
            {
                currentCell += direction;
            }
            else if (CellIsTaken(currentCell + Vector2Int.up) && direction != Vector2Int.down)
            {
                direction = Vector2Int.up;
                currentCell += direction;
            }
            else if (CellIsTaken(currentCell + Vector2Int.down) && direction != Vector2Int.up)
            {
                direction = Vector2Int.down;
                currentCell += direction;
            }
            else if (CellIsTaken(currentCell + Vector2Int.right) && direction != Vector2Int.left)
            {
                direction = Vector2Int.right;
                currentCell += direction;
            }
            else if (CellIsTaken(currentCell + Vector2Int.left) && direction != Vector2Int.right)
            {
                direction = Vector2Int.left;
                currentCell += direction;
            }
            else
            {
                // Nowhere to go. Returning the route
                //throw new System.Exception("HUH?");
                return route;
            }
        }

        return route;
    }

    public bool AddLoops_old()
    {
        for (int i = 0; i < pathCells.Count; i++)
        {
            Vector2Int pathCell = pathCells[i];

            if (pathCell.x > 3 && pathCell.x < width - 4 && pathCell.y > 2 && pathCell.y < height - 3)
            {
                if (CellIsEmpty(pathCell.x, pathCell.y + 3) && CellIsEmpty(pathCell.x + 1, pathCell.y + 3) && CellIsEmpty(pathCell.x + 2, pathCell.y + 3) &&
                CellIsEmpty(pathCell.x - 1, pathCell.y + 2) && CellIsEmpty(pathCell.x, pathCell.y + 2) && CellIsEmpty(pathCell.x + 1, pathCell.y + 2) && CellIsEmpty(pathCell.x + 2, pathCell.y + 2) && CellIsEmpty(pathCell.x + 3, pathCell.y + 2) &&
                CellIsEmpty(pathCell.x - 1, pathCell.y + 1) && CellIsEmpty(pathCell.x, pathCell.y + 1) && CellIsEmpty(pathCell.x + 1, pathCell.y + 1) && CellIsEmpty(pathCell.x + 2, pathCell.y + 1) && CellIsEmpty(pathCell.x + 3, pathCell.y + 1) &&
                CellIsEmpty(pathCell.x + 1, pathCell.y) && CellIsEmpty(pathCell.x + 2, pathCell.y) && CellIsEmpty(pathCell.x + 3, pathCell.y) &&
                CellIsEmpty(pathCell.x + 1, pathCell.y - 1) && CellIsEmpty(pathCell.x + 2, pathCell.y - 1))
                {
                    pathCells.InsertRange(i + 1, new List<Vector2Int> { new Vector2Int(pathCell.x + 1, pathCell.y),  new Vector2Int(pathCell.x + 2, pathCell.y),
                                                                    new Vector2Int(pathCell.x + 2, pathCell.y + 1), new Vector2Int(pathCell.x + 2, pathCell.y + 2),
                                                                    new Vector2Int(pathCell.x + 1, pathCell.y + 2), new Vector2Int(pathCell.x, pathCell.y + 2),
                                                                    new Vector2Int(pathCell.x, pathCell.y + 1)});
                    return true;
                }

                if (CellIsEmpty(pathCell.x + 1, pathCell.y + 1) && CellIsEmpty(pathCell.x + 2, pathCell.y + 1) &&
                CellIsEmpty(pathCell.x + 1, pathCell.y) && CellIsEmpty(pathCell.x + 2, pathCell.y) && CellIsEmpty(pathCell.x + 3, pathCell.y) &&
                CellIsEmpty(pathCell.x - 1, pathCell.y - 1) && CellIsEmpty(pathCell.x, pathCell.y - 1) && CellIsEmpty(pathCell.x + 1, pathCell.y - 1) && CellIsEmpty(pathCell.x + 2, pathCell.y - 1) && CellIsEmpty(pathCell.x + 3, pathCell.y - 1) &&
                CellIsEmpty(pathCell.x - 1, pathCell.y - 2) && CellIsEmpty(pathCell.x, pathCell.y - 2) && CellIsEmpty(pathCell.x + 1, pathCell.y - 2) && CellIsEmpty(pathCell.x + 2, pathCell.y - 2) && CellIsEmpty(pathCell.x + 3, pathCell.y - 2) &&
                CellIsEmpty(pathCell.x, pathCell.y - 3) && CellIsEmpty(pathCell.x + 1, pathCell.y - 3) && CellIsEmpty(pathCell.x + 2, pathCell.y - 3))
                {
                    pathCells.InsertRange(i + 1, new List<Vector2Int> { new Vector2Int(pathCell.x + 1, pathCell.y),  new Vector2Int(pathCell.x + 2, pathCell.y),
                                                                    new Vector2Int(pathCell.x + 2, pathCell.y - 1), new Vector2Int(pathCell.x + 2, pathCell.y - 2),
                                                                    new Vector2Int(pathCell.x + 1, pathCell.y - 2), new Vector2Int(pathCell.x, pathCell.y - 2),
                                                                    new Vector2Int(pathCell.x, pathCell.y - 1)});
                    return true;
                }
            }
        }

        return false;
    }

    public void AddLoops()
    {
        bool loopsGenerated = true;

        while (loopsGenerated)
        {
            loopsGenerated = false;

            for (int i = 0; i < pathCells.Count; i++)
            {
                List<Vector2Int> loop = IsLoopOption(i);

                if (loop.Count > 0)
                {
                    loopsGenerated = true;
                    pathCells.InsertRange(i + 1, loop);
                }
            }
        }
    }

    private List<Vector2Int> IsLoopOption(int i)
    {
        Vector2Int pathCell = pathCells[i];
        int x = pathCell.x;
        int y = pathCell.y;
        List<Vector2Int> returnPath = new List<Vector2Int>();

        // Top right (yellow)
        if (pathCell.x > 3 && pathCell.x < width - 4 && pathCell.y > 2 && pathCell.y < height - 3)
        {
            if (CellIsEmpty(x, y + 3) && CellIsEmpty(x + 1, y + 3) && CellIsEmpty(x + 2, y + 3) &&
            CellIsEmpty(x - 1, y + 2) && CellIsEmpty(x, y + 2) && CellIsEmpty(x + 1, y + 2) && CellIsEmpty(x + 2, y + 2) && CellIsEmpty(x + 3, y + 2) &&
            CellIsEmpty(x - 1, y + 1) && CellIsEmpty(x, y + 1) && CellIsEmpty(x + 1, y + 1) && CellIsEmpty(x + 2, y + 1) && CellIsEmpty(x + 3, y + 1) &&
            CellIsEmpty(x + 1, y) && CellIsEmpty(x + 2, y) && CellIsEmpty(x + 3, y) &&
            CellIsEmpty(x + 1, y - 1) && CellIsEmpty(x + 2, y - 1))
            {
                /*pathCells.InsertRange(i + 1, new List<Vector2Int> { new Vector2Int(x + 1, y),  new Vector2Int(x + 2, y),
                                                                    new Vector2Int(x + 2, y + 1), new Vector2Int(x + 2, y + 2),
                                                                    new Vector2Int(x + 1, y + 2), new Vector2Int(x, y + 2),
                                                                    new Vector2Int(x, y + 1)});*/
                returnPath = new List<Vector2Int> { new Vector2Int(x + 1, y),  new Vector2Int(x + 2, y),
                                                                    new Vector2Int(x + 2, y + 1), new Vector2Int(x + 2, y + 2),
                                                                    new Vector2Int(x + 1, y + 2), new Vector2Int(x, y + 2),
                                                                    new Vector2Int(x, y + 1)};
                loopCount++;
                Debug.Log("Top right at (" + x + ", " + y + ")");
            }

            // Bottom right (red)
            else if (CellIsEmpty(x + 1, y + 1) && CellIsEmpty(x + 2, y + 1) &&
            CellIsEmpty(x + 1, y) && CellIsEmpty(x + 2, y) && CellIsEmpty(x + 3, y) &&
            CellIsEmpty(x - 1, y - 1) && CellIsEmpty(x, y - 1) && CellIsEmpty(x + 1, y - 1) && CellIsEmpty(x + 2, y - 1) && CellIsEmpty(x + 3, y - 1) &&
            CellIsEmpty(x - 1, y - 2) && CellIsEmpty(x, y - 2) && CellIsEmpty(x + 1, y - 2) && CellIsEmpty(x + 2, y - 2) && CellIsEmpty(x + 3, y - 2) &&
            CellIsEmpty(x, y - 3) && CellIsEmpty(x + 1, y - 3) && CellIsEmpty(x + 2, y - 3))
            {
                /*pathCells.InsertRange(i + 1, new List<Vector2Int> { new Vector2Int(x + 1, y),  new Vector2Int(x + 2, y),
                                                                new Vector2Int(x + 2, y - 1), new Vector2Int(x + 2, y - 2),
                                                                new Vector2Int(x + 1, y - 2), new Vector2Int(x, y - 2),
                                                                new Vector2Int(x, y - 1)});*/
                returnPath = new List<Vector2Int> { new Vector2Int(x + 1, y),  new Vector2Int(x + 2, y),
                                                                new Vector2Int(x + 2, y - 1), new Vector2Int(x + 2, y - 2),
                                                                new Vector2Int(x + 1, y - 2), new Vector2Int(x, y - 2),
                                                                new Vector2Int(x, y - 1)};
                loopCount++;
                Debug.Log("Bottom right at (" + x + ", " + y + ")");
            }

            // Bottom left (brown)
            else if (CellIsEmpty(x - 2, y + 1) && CellIsEmpty(x - 1, y + 1) &&
            CellIsEmpty(x - 3, y) && CellIsEmpty(x - 2, y) && CellIsEmpty(x - 1, y) &&
            CellIsEmpty(x - 3, y - 1) && CellIsEmpty(x - 2, y - 1) && CellIsEmpty(x - 1, y - 1) && CellIsEmpty(x, y - 1) && CellIsEmpty(x + 1, y - 1) &&
            CellIsEmpty(x - 3, y - 2) && CellIsEmpty(x - 2, y - 2) && CellIsEmpty(x - 1, y - 2) && CellIsEmpty(x, y - 2) && CellIsEmpty(x + 1, y - 2) &&
            CellIsEmpty(x - 2, y - 3) && CellIsEmpty(x - 1, y - 3) && CellIsEmpty(x, y - 3))
            {
                /*pathCells.InsertRange(i + 1, new List<Vector2Int> { new Vector2Int(x, y - 1),  new Vector2Int(x, y - 2),
                                                                new Vector2Int(x - 1, y - 2), new Vector2Int(x - 2, y - 2),
                                                                new Vector2Int(x - 2, y - 1), new Vector2Int(x - 2, y),
                                                                new Vector2Int(x - 1, y)});*/
                returnPath = new List<Vector2Int> { new Vector2Int(x, y - 1),  new Vector2Int(x, y - 2),
                                                                new Vector2Int(x - 1, y - 2), new Vector2Int(x - 2, y - 2),
                                                                new Vector2Int(x - 2, y - 1), new Vector2Int(x - 2, y),
                                                                new Vector2Int(x - 1, y)};
                loopCount++;
                Debug.Log("Bottom left at (" + x + ", " + y + ")");
            }
            // Top left (blue)
            else if (CellIsEmpty(x - 2, y + 3) && CellIsEmpty(x - 1, y + 3) && CellIsEmpty(x, y + 3) &&
            CellIsEmpty(x - 3, y + 2) && CellIsEmpty(x - 2, y + 2) && CellIsEmpty(x - 1, y + 2) && CellIsEmpty(x, y + 2) && CellIsEmpty(x + 1, y + 2) &&
            CellIsEmpty(x - 3, y + 1) && CellIsEmpty(x - 2, y + 1) && CellIsEmpty(x - 1, y + 1) && CellIsEmpty(x, y + 1) && CellIsEmpty(x + 1, y + 1) &&
            CellIsEmpty(x - 3, y) && CellIsEmpty(x - 2, y) && CellIsEmpty(x - 1, y) &&
            CellIsEmpty(x - 2, y - 1) && CellIsEmpty(x - 1, y - 1))
            {
                /*pathCells.InsertRange(i + 1, new List<Vector2Int> { new Vector2Int(x, y + 1),  new Vector2Int(x, y + 2),
                                                                new Vector2Int(x - 1, y + 2), new Vector2Int(x - 2, y + 2),
                                                                new Vector2Int(x - 2, y + 1), new Vector2Int(x - 2, y),
                                                                new Vector2Int(x - 1, y)});*/
                returnPath = new List<Vector2Int> { new Vector2Int(x, y + 1),  new Vector2Int(x, y + 2),
                                                                new Vector2Int(x - 1, y + 2), new Vector2Int(x - 2, y + 2),
                                                                new Vector2Int(x - 2, y + 1), new Vector2Int(x - 2, y),
                                                                new Vector2Int(x - 1, y)};
                loopCount++;
                Debug.Log("Top left at (" + x + ", " + y + ")");
            }
        }

        return returnPath;
    }


    public bool CellIsEmpty(int x, int y)
    {
        return !pathCells.Contains(new Vector2Int(x, y));
    }

    public bool CellIsTaken(int x, int y)
    {
        return pathCells.Contains(new Vector2Int(x, y));
    }

    public bool CellIsTaken(Vector2Int cell)
    {
        //Debug.Log("Cell (" + cell.x + ", " + cell.y + ") is " + pathCells.Contains(cell));
        return pathCells.Contains(cell);
    }

    public int getCellNeighbourValue(int x, int y)
    {
        int returnValue = 0;

        if (CellIsTaken(x, y - 1))
        {
            returnValue += 1;
        }

        if (CellIsTaken(x - 1, y))
        {
            returnValue += 2;
        }

        if (CellIsTaken(x + 1, y))
        {
            returnValue += 4;
        }

        if (CellIsTaken(x, y + 1))
        {
            returnValue += 8;
        }

        return returnValue;
    }

}
