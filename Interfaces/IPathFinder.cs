using SpaceMission.Models;

namespace SpaceMission.Interfaces;


public interface IPathFinder
{
    List<AstronautPath> FindPaths(Map map);
}