using SpaceMission.Models;
using SpaceMission.Interfaces;

namespace SpaceMission.Services;

public class RandomMapGenerator : IMapGenerator
{
    private readonly ISymbolEncoder encoder;
    private readonly int rows;
    private readonly int cols;
    private readonly int asteroidCount;
    private readonly int astronautCount;
    private readonly Random rng;

    public RandomMapGenerator(
        ISymbolEncoder encoder,
        int rows,
        int cols,
        int asteroidCount,
        int astronautCount = 1)
    {
        if (astronautCount < 1 || astronautCount > 3)
            throw new ArgumentException("Astronaut count must be between 1 and 3.");

        if (asteroidCount < 0)
            throw new ArgumentException("Asteroid count cannot be negative.");

        int needed = asteroidCount + astronautCount + 1;
        if (needed > rows * cols)
            throw new ArgumentException("Too many asteroids/astronauts for the given map size.");

        this.encoder = encoder;
        this.rows = rows;
        this.cols = cols;
        this.asteroidCount = asteroidCount;
        this.astronautCount = astronautCount;
        this.rng = new Random();
    }

    public Map GenerateMap()
    {
        Map map = new Map(rows, cols);

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                map.SetCell(i, j, encoder.Open);
            }
        }

        List<Position> cells = new List<Position>();
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                Position curr = new Position(i, j);
                cells.Add(curr);
            }
        }
        
        Shuffle(cells);
        int index = 0;

        Position stationPos = cells[index++];
        map.SetCell(stationPos.Row, stationPos.Col, encoder.Station);
        map.SetStationPosition(stationPos);

        for (int i = 1; i <= astronautCount; i++)
        {
            Position astroPos = cells[index++];
            byte curr = encoder.Encode("S" + i);
            map.SetCell(astroPos.Row, astroPos.Col, curr);
            map.AddAstronautPosition(astroPos);
        }

        for (int k = 0; k < asteroidCount; k++)
        {
            Position rock = cells[index++];
            map.SetCell(rock.Row, rock.Col, encoder.Asteroid);
        }
        return map;
    }

    private void Shuffle(List<Position> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
