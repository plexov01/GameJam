using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PathGenerator
{
    private int width;
    private int height;
    public List<Vector2Int> pathCells = new();
    private List<Vector2Int> route;
    private List<Vector2Int> routeDirection;
    private HashSet<Vector2Int> path;
    //private List<Vector2Int> pathCells;

    private Vector2Int[] directions = new Vector2Int[]
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    public int loopCount = 0;

    public PathGenerator(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    /*    public List<Vector2Int> GeneratePath(bool addLoops, int minLoops, int maxLoops)
        {
            pathCells = new List<Vector2Int>();
            loopCount = 0;

            //int y = height / 2;
            int y = Random.Range(1, height - 1);
            int x = 1;
            int loops = Random.Range(minLoops, maxLoops + 1);

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
                AddLoops(loops);
            }

            return pathCells;
        }*/

    public List<Vector2Int> GeneratePath(int maxAttempts, int minPathLength, int maxPathLength)
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            path = new HashSet<Vector2Int>();
            List<Vector2Int> pathList = new();

            // Стартовая точка
            Vector2Int current = new Vector2Int(1, Random.Range(1, height - 1));
            path.Add(current);
            pathList.Add(current);

            // Первый шаг ВПРАВО
            Vector2Int firstStep = current + Vector2Int.right;
            path.Add(firstStep);
            pathList.Add(firstStep);
            current = firstStep;

            // Основной цикл
            while (current.x < width - 2)
            {
                List<Vector2Int> options = GetSafeDirections(current);

                if (options.Count == 0)
                {
                    ///Debug.Log("Тупик, остановка пути");
                    break;
                }

                Vector2Int chosen = ChooseBiasedDirection(options);
                current += chosen;

                path.Add(current);
                pathList.Add(current);
            }

            int pathSize = pathList.Count;

            // Проверяем, достиг ли правого края и входит ли длина в допустимый диапазон
            if (current.x >= width - 2 && pathSize >= minPathLength && pathSize <= maxPathLength)
            {
                Debug.Log($"Успешно сгенерирован путь длиной {pathSize} на попытке {attempt + 1}");
                return pathList;
            }
            /*else
            {
                Debug.Log($"Попытка {attempt + 1}: путь не подходит (X={current.x}, длина={pathSize}), пробуем снова.");
            }*/
        }

        Debug.LogWarning("Не удалось сгенерировать путь, достигающий правого края за максимальное количество попыток.");
        return new List<Vector2Int>(); // Возвращаем пустой путь
    }

    private List<Vector2Int> GetSafeDirections(Vector2Int current)
    {
        List<Vector2Int> dirs = new()
        {
            Vector2Int.right,
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left
        };

        List<Vector2Int> result = new();

        foreach (var dir in dirs)
        {
            Vector2Int next = current + dir;

            if (!InBounds(next) || path.Contains(next) || HasAdjacentPath(next, current))
                continue;

            if (HasFutureOptions(next))
                result.Add(dir);
            /*else
                Debug.Log($"Отклонено: {next} — нет будущих опций");*/
        }

        /*if (result.Count == 0)
            Debug.Log($"Тупик в точке: {current}");*/

        return result;
    }

    private bool InBounds(Vector2Int cell)
    {
        bool inside = cell.x >= 1 && cell.x < width - 1 && cell.y >= 1 && cell.y < height - 1;
        /*if (!inside)
            Debug.Log($"{cell} — вне границ поля");*/
        return inside;
    }

    private bool HasAdjacentPath(Vector2Int cell, Vector2Int previousCell)
    {
        foreach (var dir in directions)
        {
            Vector2Int neighbor = cell + dir;
            if (!InBounds(neighbor)) continue;
            if (path.Contains(neighbor))
            {
                if (neighbor == previousCell) continue; // разрешаем соседство только с предыдущей клеткой
                return true;
            }
        }
        return false;
    }

    private bool HasFutureOptions(Vector2Int next)
    {
        //Debug.Log($"Проверка будущих ходов из клетки: {next}");

        foreach (var dir in directions)
        {
            Vector2Int neighbor = next + dir;

            if (!InBounds(neighbor))
            {
                //Debug.Log($"{neighbor} — вне границ");
                continue;
            }

            if (path.Contains(neighbor))
            {
                //Debug.Log($"{neighbor} — уже входит в путь");
                continue;
            }

            if (HasAdjacentPath(neighbor, next))
            {
                //Debug.Log($"{neighbor} — слишком близко к существующему пути");
                continue;
            }

            //Debug.Log($"{neighbor} — допустимый следующий шаг");
            return true;
        }

        //Debug.Log("Нет допустимых направлений — тупик!");
        return false;
    }


    private Vector2Int ChooseBiasedDirection(List<Vector2Int> options)
    {
        List<(Vector2Int dir, float weight)> weighted = new();

        foreach (var dir in options)
        {
            float weight = dir == Vector2Int.right ? 0.6f :
                           dir == Vector2Int.left ? 0.05f :
                           0.175f;

            weighted.Add((dir, weight));
        }

        return WeightedRandomChoice(weighted);
    }

    private Vector2Int WeightedRandomChoice(List<(Vector2Int dir, float weight)> options)
    {
        float total = options.Sum(o => o.weight);
        float r = Random.Range(0f, total);
        float cumulative = 0f;

        foreach (var option in options)
        {
            cumulative += option.weight;
            if (r <= cumulative)
                return option.dir;
        }

        return options.Last().dir;
    }


    public Tuple<List<Vector2Int>, List<Vector2Int>> GenerateRoute()
    {
        Vector2Int direction = Vector2Int.right;
        route = new List<Vector2Int>();
        Vector2Int currentCell = pathCells[0];
        routeDirection = new List<Vector2Int>();

        while (currentCell.x < width - 1)
        {
            route.Add(new Vector2Int(currentCell.x, currentCell.y));
            routeDirection.Add(direction);

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
                currentCell += Vector2Int.right;
                route.Add(new Vector2Int(currentCell.x, currentCell.y));
                currentCell += Vector2Int.right;
                route.Add(new Vector2Int(currentCell.x, currentCell.y));
                return new Tuple<List<Vector2Int>, List<Vector2Int>>(route, routeDirection);
            }
        }

        return new Tuple<List<Vector2Int>, List<Vector2Int>>(route, routeDirection);
    }

    public void AddLoops(int loops)
    {
        bool loopGenerated = true;

        while (loopGenerated)
        {
            loopGenerated = false;

            for (int i = 0; i < pathCells.Count; i++)
            {
                if (Random.Range(0f, 1f) < 0.1f && loopCount != loops)
                {
                    List<Vector2Int> loop = IsLoopAnOption(i);

                    if (loop.Count > 0)
                    {
                        loopGenerated = true;
                        loopCount++;
                        pathCells.InsertRange(i + 1, loop);
                    }
                }
            }
        }
    }

    private List<Vector2Int> IsLoopAnOption(int i)
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
                returnPath = new List<Vector2Int> { new Vector2Int(x + 1, y),  new Vector2Int(x + 2, y),
                                                                    new Vector2Int(x + 2, y + 1), new Vector2Int(x + 2, y + 2),
                                                                    new Vector2Int(x + 1, y + 2), new Vector2Int(x, y + 2),
                                                                    new Vector2Int(x, y + 1)};
                //Debug.Log("Top right at (" + x + ", " + y + ")");
            }

            // Bottom right (red)
            else if (CellIsEmpty(x + 1, y + 1) && CellIsEmpty(x + 2, y + 1) &&
            CellIsEmpty(x + 1, y) && CellIsEmpty(x + 2, y) && CellIsEmpty(x + 3, y) &&
            CellIsEmpty(x - 1, y - 1) && CellIsEmpty(x, y - 1) && CellIsEmpty(x + 1, y - 1) && CellIsEmpty(x + 2, y - 1) && CellIsEmpty(x + 3, y - 1) &&
            CellIsEmpty(x - 1, y - 2) && CellIsEmpty(x, y - 2) && CellIsEmpty(x + 1, y - 2) && CellIsEmpty(x + 2, y - 2) && CellIsEmpty(x + 3, y - 2) &&
            CellIsEmpty(x, y - 3) && CellIsEmpty(x + 1, y - 3) && CellIsEmpty(x + 2, y - 3))
            {
                returnPath = new List<Vector2Int> { new Vector2Int(x + 1, y),  new Vector2Int(x + 2, y),
                                                                new Vector2Int(x + 2, y - 1), new Vector2Int(x + 2, y - 2),
                                                                new Vector2Int(x + 1, y - 2), new Vector2Int(x, y - 2),
                                                                new Vector2Int(x, y - 1)};
                //Debug.Log("Bottom right at (" + x + ", " + y + ")");
            }

            // Bottom left (brown)
            else if (CellIsEmpty(x - 2, y + 1) && CellIsEmpty(x - 1, y + 1) &&
            CellIsEmpty(x - 3, y) && CellIsEmpty(x - 2, y) && CellIsEmpty(x - 1, y) &&
            CellIsEmpty(x - 3, y - 1) && CellIsEmpty(x - 2, y - 1) && CellIsEmpty(x - 1, y - 1) && CellIsEmpty(x, y - 1) && CellIsEmpty(x + 1, y - 1) &&
            CellIsEmpty(x - 3, y - 2) && CellIsEmpty(x - 2, y - 2) && CellIsEmpty(x - 1, y - 2) && CellIsEmpty(x, y - 2) && CellIsEmpty(x + 1, y - 2) &&
            CellIsEmpty(x - 2, y - 3) && CellIsEmpty(x - 1, y - 3) && CellIsEmpty(x, y - 3))
            {
                returnPath = new List<Vector2Int> { new Vector2Int(x, y - 1),  new Vector2Int(x, y - 2),
                                                                new Vector2Int(x - 1, y - 2), new Vector2Int(x - 2, y - 2),
                                                                new Vector2Int(x - 2, y - 1), new Vector2Int(x - 2, y),
                                                                new Vector2Int(x - 1, y)};
                //Debug.Log("Bottom left at (" + x + ", " + y + ")");
            }
            // Top left (blue)
            else if (CellIsEmpty(x - 2, y + 3) && CellIsEmpty(x - 1, y + 3) && CellIsEmpty(x, y + 3) &&
            CellIsEmpty(x - 3, y + 2) && CellIsEmpty(x - 2, y + 2) && CellIsEmpty(x - 1, y + 2) && CellIsEmpty(x, y + 2) && CellIsEmpty(x + 1, y + 2) &&
            CellIsEmpty(x - 3, y + 1) && CellIsEmpty(x - 2, y + 1) && CellIsEmpty(x - 1, y + 1) && CellIsEmpty(x, y + 1) && CellIsEmpty(x + 1, y + 1) &&
            CellIsEmpty(x - 3, y) && CellIsEmpty(x - 2, y) && CellIsEmpty(x - 1, y) &&
            CellIsEmpty(x - 2, y - 1) && CellIsEmpty(x - 1, y - 1))
            {
                returnPath = new List<Vector2Int> { new Vector2Int(x, y + 1),  new Vector2Int(x, y + 2),
                                                                new Vector2Int(x - 1, y + 2), new Vector2Int(x - 2, y + 2),
                                                                new Vector2Int(x - 2, y + 1), new Vector2Int(x - 2, y),
                                                                new Vector2Int(x - 1, y)};
                //Debug.Log("Top left at (" + x + ", " + y + ")");
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

    public int GetCellNeighbourValue(int x, int y)
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
