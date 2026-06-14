using SpaceMission.Models;
using SpaceMission.Interfaces;
namespace SpaceMission.Services;
public class MissionControl
{
    private readonly ISymbolEncoder symEncoder;
    private readonly IMapGenerator mapGenerator;
    private readonly IPathFinder pathFinder;

    private string report = "";

    public MissionControl(
        ISymbolEncoder encoder,
        IMapGenerator generator,
        IPathFinder finder)
    {
        symEncoder = encoder;
        mapGenerator = generator;
        pathFinder = finder;
    }

    public void completeMission()
    {
        Map map = mapGenerator.GenerateMap();

        List<AstronautPath> pathResults = pathFinder.FindPaths(map);

        MapRenderer mapRenderer = new MapRenderer(map, symEncoder);
        ReportBuilder reportBuilder = new ReportBuilder(mapRenderer);

        report = reportBuilder.BuildReport(pathResults);

        Console.Write(report);
    }

    public void sendReport(IReportSender sender)
    {
        if (string.IsNullOrEmpty(report))
            throw new InvalidOperationException("No report to send - run the mission first.");

        sender.Send(report);
    }
}