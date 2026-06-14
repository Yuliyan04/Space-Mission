namespace SpaceMission.Models;

public class PathResult
{
    public string Id { get; }
    public Position Start { get; }
    public int PathLength { get; } // -1 = lost in space
    public IReadOnlyList<Position> Path { get; } // reconstructed route, empty if none

    public PathResult(string id, Position start, int pathLength, IReadOnlyList<Position> path)
    {
        Id = id;
        Start = start;
        PathLength = pathLength;
        Path = path;
    }
}
