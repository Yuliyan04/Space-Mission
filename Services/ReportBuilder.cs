using System.Text;
using SpaceMission.Models;

namespace SpaceMission.Services;

public class ReportBuilder
{
    private readonly MapRenderer renderer;

    public ReportBuilder(MapRenderer renderer)
    {
        this.renderer = renderer;
    }

    public string BuildReport(List<PathResult> results)
    {
        IEnumerable<PathResult> ordered = results.OrderBy(r => r.PathLength);

        StringBuilder report = new StringBuilder();

        foreach (PathResult result in ordered) {
            string currInfo = renderer.Render(result);
            report.Append(currInfo);
        }
        return report.ToString();
    }
}
