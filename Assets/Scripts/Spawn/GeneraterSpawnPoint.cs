using System.Collections.Generic;
using UnityEngine;

public static class GeneraterSpawnPoint
{
    public static List<Vector3> GeneratePoints(float radius, Vector3 offset, Vector2 sampleRegionSize, int numberSamplesBeforeRejection = 30)
    {
        int multiplier = 2;
        Vector2 startPoint = sampleRegionSize / 2;

        float cellSize = radius / Mathf.Sqrt(multiplier);

        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];
        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();

        spawnPoints.Add(startPoint);

        while (spawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCenter = spawnPoints[spawnIndex];
            bool isCandidateAccepted = false;

            for (int i = 0; i < numberSamplesBeforeRejection; i++)
            {
                float angle = Random.value * Mathf.PI * multiplier;
                Vector2 direction = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 candidate = spawnCenter + direction * Random.Range(radius, multiplier * radius);

                if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
                    isCandidateAccepted = true;
                    break;
                }
            }

            if (isCandidateAccepted == false)
                spawnPoints.RemoveAt(spawnIndex);
        }

        return ConvertToVector3(points, offset);
    }

    private static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid)
    {
        if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)
        {
            int countCellsToCheck = 2;
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);

            int searchStartX = Mathf.Max(0, cellX - countCellsToCheck);
            int searchEndX = Mathf.Min(cellX + countCellsToCheck, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, cellY - countCellsToCheck);
            int searchEndY = Mathf.Min(cellY + countCellsToCheck, grid.GetLength(1) - 1);

            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    int pointIndex = grid[x, y] - 1;

                    if (pointIndex != -1)
                    {
                        float distance = (candidate - points[pointIndex]).magnitude;

                        if (distance < radius)
                            return false;
                    }
                }
            }

            return true;
        }

        return false;
    }

    private static List<Vector3> ConvertToVector3(List<Vector2> points, Vector3 offset)
    {
        List<Vector3> convertedPoints = new List<Vector3>();

        foreach (Vector3 point in points)
            convertedPoints.Add(new Vector3(point.x, 0, point.y) + offset);

        return convertedPoints;
    }
}
