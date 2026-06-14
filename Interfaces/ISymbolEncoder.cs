namespace SpaceMission.Interfaces;

public interface ISymbolEncoder
{
    byte Open { get; }
    byte Asteroid { get; }
    byte Debris { get; }
    byte Station { get; }

    bool IsAstronaut(byte value);
    byte Encode(string symbol);
    string Decode(byte value);
}
