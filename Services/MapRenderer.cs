using System.Text;
using SpaceMission.Models;
using SpaceMission.Interfaces;

namespace SpaceMission.Services;

public class MapRenderer
{
    private readonly Map map;
    private readonly ISymbolEncoder encoder;

    public MapRenderer(Map map, ISymbolEncoder encoder)
    {
        this.map = map;
        this.encoder = encoder;
    }

    public string Render(AstronautPath result)
    {
        string id = result.AstronautId;

        string header;
        string mapText;

        if (result.Path.Count == 0) {
            header = $"Mission failed — Astronaut {id} lost in space!\n";
            mapText = "";
        }
        else {
            header = $"Astronaut {id} - Shortest path: {result.PathLength} steps\n";
            mapText = BuildMap(result.Path);
        }
        return header + mapText;
    }

    private string BuildMap(IReadOnlyList<Position> path)
    {
        bool[,] pathMarks = new bool[map.Rows, map.Cols];

        for (int i = 1; i < path.Count - 1; i++) {
            Position curr = path[i];
            pathMarks[curr.Row, curr.Col] = true;
        }

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < map.Rows; i++)
        {
            for (int j = 0; j < map.Cols; j++)
            {
                if (j > 0) 
                    sb.Append(' ');

                byte currByte = map.GetCell(i, j);
                string currSym = pathMarks[i, j] ? "*" : encoder.Decode(currByte);
                sb.Append(currSym);
            }
            sb.Append('\n');
        }
        return sb.ToString();
    }
}
