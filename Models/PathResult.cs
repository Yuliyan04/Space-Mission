namespace SpaceMission.Models;

public class AstronautPath
{
    public string AstronautId { get; }
    public Position Start { get; }
    public int PathLength { get; } // -1 = lost in space
    public IReadOnlyList<Position> Path { get; } // reconstructed route, empty if none

    public AstronautPath(string id, Position start, int pathLength, IReadOnlyList<Position> path)
    {
        AstronautId = id;
        Start = start;
        PathLength = pathLength;
        Path = path;
    }
}
