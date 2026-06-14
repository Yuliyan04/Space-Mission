namespace SpaceMission.Models;


public class Map
{
    public int Rows { get; }
    public int Cols { get; }
    private byte[,] Grid;
    public Position StationPosition { get; private set; }

    private List<Position> _astronautPositions;
    public IReadOnlyList<Position> AstronautPositions => _astronautPositions;


    public Map(int rows, int cols)
    {
        if (rows < 2 || rows > 100 || cols < 2 || cols > 100)
            throw new ArgumentException("Map dimensions must be between 2 and 100.");

        Rows = rows;
        Cols = cols;
        Grid = new byte[Rows, Cols];
        StationPosition = new Position(-1, -1);
        _astronautPositions = new List<Position>();
    }

    public void SetCell(int x, int y, byte value) => Grid[x, y] = value;
    public byte GetCell(int x, int y) => Grid[x,y];
    public void SetStationPosition(Position pos) => StationPosition = pos;
    public void AddAstronautPosition(Position pos) => _astronautPositions.Add(pos);
}