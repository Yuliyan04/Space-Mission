using SpaceMission.Models;
using SpaceMission.Interfaces;
namespace SpaceMission.Services;


public class ConsoleMapReader : IMapGenerator
{
    private readonly ISymbolEncoder _encoder;

    public ConsoleMapReader(ISymbolEncoder encoder)
    {
        _encoder = encoder;
    }

    public Map GenerateMap()
    {
        int m = ConsoleInput.ReadInt("Map rows: ", 2, 100);
        int n = ConsoleInput.ReadInt("Map columns: ", 2, 100);

        Map map = new Map(m, n);

        Console.WriteLine("Cosmic map:");

        int stationCount = 0;
        for (int i = 0; i < m; i++)
        {
            string[] tokens = ReadRow(i, n);
            for (int j = 0; j < n; j++)
            {
                byte currSym = _encoder.Encode(tokens[j]); // throws on unknown symbol
                map.SetCell(i, j, currSym);

                if (currSym == _encoder.Station)
                {
                    stationCount++;
                    map.SetStationPosition(new Position(i, j));
                }
                else if (_encoder.IsAstronaut(currSym))
                {
                    map.AddAstronautPosition(new Position(i, j));
                }
            }
        }

        ValidateMap(map, stationCount);
        return map;
    }

    private string[] ReadRow(int rowIndex, int n)
    {
        string? line = Console.ReadLine();
        if (line == null)
            throw new ArgumentException($"Missing input for map row {rowIndex + 1}.");

        string[] tokens = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length != n)
            throw new ArgumentException(
                $"Map row {rowIndex + 1} must have exactly {n} cells but had {tokens.Length}.");

        return tokens;
    }

    private void ValidateMap(Map map, int stationCount)
    {
        if (stationCount != 1)
            throw new ArgumentException(
                $"The map must contain exactly one station 'F' but found {stationCount}.");

        int astronautCount = map.AstronautPositions.Count;
        if (astronautCount < 1 || astronautCount > 3)
            throw new ArgumentException(
                $"The map must contain 1 to 3 astronauts but found {astronautCount}.");
    }
}
