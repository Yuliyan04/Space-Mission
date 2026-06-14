using SpaceMission.Models;

namespace SpaceMission.Interfaces;


public interface IPathFinder
{
    List<PathResult> FindPaths(Map map);
}