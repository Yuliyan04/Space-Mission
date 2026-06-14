using SpaceMission.Interfaces;

namespace SpaceMission.Services;

public class SymbolEncoder : ISymbolEncoder
{
    public byte Open => 0;
    public byte Asteroid => 1;
    public byte Debris => 2;
    public byte Station => 3;

    // Astronauts occupy a contiguous range so identity is preserved in the grid:
    // S1 => 4, S2 => 5, S3 => 6.
    private const byte AstronautBase = 4;
    private const int MaxAstronauts = 3;

    public bool IsAstronaut(byte value) =>
        value >= AstronautBase && value < AstronautBase + MaxAstronauts;

    public byte Encode(string symbol)
    {
        return symbol switch
        {
            "0" or "O" => Open,
            "X" => Asteroid,
            "D" => Debris,
            "F" => Station,
            "S1" => (byte)(AstronautBase + 0),
            "S2" => (byte)(AstronautBase + 1),
            "S3" => (byte)(AstronautBase + 2),
            _ => throw new ArgumentException($"Unknown symbol: {symbol}")
        };
    }

    public string Decode(byte value)
    {
        if (value == Open) return "0";
        if (value == Asteroid) return "X";
        if (value == Debris) return "D";
        if (value == Station) return "F";
        if (IsAstronaut(value)) return "S" + (value - AstronautBase + 1);
        throw new ArgumentException($"Unknown value: {value}");
    }
}
