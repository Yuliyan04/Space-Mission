using SpaceMission.Models;
using SpaceMission.Interfaces;

namespace SpaceMission.Services;

public class BFSPathfinder : IPathFinder
{
    private static readonly Position Up = new Position(-1, 0);
    private static readonly Position Down = new Position(1, 0);
    private static readonly Position Left = new Position(0, -1);
    private static readonly Position Right = new Position(0, 1);
    private static readonly Position[] Directions = { Up, Down, Left, Right };
    private const int OpenSpaceCost = 1;
    private const int DebrisCost = 2;
    private const int InvalidPathValue = -1;
    private readonly ISymbolEncoder Encoder;


    public BFSPathfinder(ISymbolEncoder encoder)
    {
        Encoder = encoder;
    }


    public List<AstronautPath> FindPaths(Map map)
    {
        (Position? parentPos, int dist)[,] parentPosAndDist = BuildParentMatrix(map);

        List<AstronautPath> results = new List<AstronautPath>();
        foreach (Position pos in map.AstronautPositions)
        {
            int dist = parentPosAndDist[pos.Row, pos.Col].dist;
            List<Position> currPath;
            AstronautPath currPathRes;
            string id = Encoder.Decode(map.GetCell(pos.Row, pos.Col));

            if (dist == int.MaxValue) {
                currPath = new List<Position>();
                currPathRes = new AstronautPath(id, pos, InvalidPathValue, currPath);
            } 
            else {
                currPath = ReconstructPath(pos, parentPosAndDist);
                currPathRes = new AstronautPath(id, pos, dist, currPath);
            }
            results.Add(currPathRes);
        }
        return results;
    }

    private List<Position> ReconstructPath(Position start, (Position? parentPos, int dist)[,] matrix)
    {
        List<Position> path = new List<Position>();
        Position? current = start;

        while (current != null)
        {
            path.Add(current);
            current = matrix[current.Row, current.Col].parentPos;
        }
        return path;
    }

    private (Position? parentPosition, int distance)[,] BuildParentMatrix(Map map)
    {
        (Position? parentPos, int dist)[,] res = new (Position? parentPos, int dist)[map.Rows, map.Cols];
        for (int i = 0; i < map.Rows; i++) {
            for (int j = 0; j < map.Cols; j++) {
                res[i, j] = (null, int.MaxValue);
            }
        }

        LinkedList<Position> deque = new LinkedList<Position>();
        Position start = map.StationPosition;

        deque.AddFirst(start);
        res[start.Row, start.Col] = (null, 0);

        while(deque.Count != 0)
        {
            Position curr = deque.First!.Value;
            deque.RemoveFirst();

            foreach (Position dir in Directions)
            {
                Position neighborPos = new Position(curr.Row + dir.Row, curr.Col + dir.Col);
                if (!IsValidPosition(map, neighborPos)) continue;

                byte cell = map.GetCell(neighborPos.Row, neighborPos.Col);
                
                if (cell == Encoder.Asteroid) continue;

                int cost = cell == Encoder.Debris ? DebrisCost : OpenSpaceCost; //if it is astronaut cost still one I guess.
                int newDist = cost + res[curr.Row, curr.Col].dist;
                int neighborDist = res[neighborPos.Row, neighborPos.Col].dist;
                if ( newDist < neighborDist)
                {
                    res[neighborPos.Row, neighborPos.Col].dist = newDist;

                    if (cost == OpenSpaceCost)
                        deque.AddFirst(neighborPos);
                    else
                        deque.AddLast(neighborPos);
                    
                    res[neighborPos.Row, neighborPos.Col].parentPos = curr;
                }
            }
        }
        return res;
    }

    private bool IsValidPosition(Map map, Position pos)
    {
        return pos.Row >= 0 && pos.Row < map.Rows && 
               pos.Col >= 0 && pos.Col < map.Cols;
    }
}