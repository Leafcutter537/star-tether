using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeteoriteMeshCreator
{
    
    public static Mesh Distend(Mesh mesh)
    {
        Mesh returnMesh = new Mesh();
        Vector3[] newVertices = new Vector3[mesh.vertices.Length];
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            newVertices[i] = mesh.vertices[i];
        }
        float[] heightRange = new float[] { 0.6f, 1.4f };
        float[] radiusRange = new float[] { 20, 80 };
        int humpCount = 6;
        System.Random rand = new System.Random();
        for (int i = 0; i < humpCount; i++)
        {
            float heightRatio = heightRange[0] + ((float)rand.NextDouble() * (heightRange[1] - heightRange[0]));
            float humpRadius = radiusRange[0] + ((float)rand.NextDouble() * (radiusRange[1] - radiusRange[0]));
            newVertices = AddHump(heightRatio, humpRadius, newVertices, GetRandomSpherePoint(rand));
        }
        returnMesh.vertices = newVertices;
        returnMesh.normals = mesh.normals;
        returnMesh.triangles = mesh.triangles;
        return returnMesh;
    }

    private static Vector3[] AddHump(float heightRatio, float humpRadius, Vector3[] originalPoints, Vector3 humpPeak)
    {
        Vector3[] returnArray = new Vector3[originalPoints.Length];
        for (int i = 0; i < originalPoints.Length; i++)
        {
            float change = 1;
            if (Vector3.Angle(originalPoints[i], humpPeak) < humpRadius)
            {
                change = (humpRadius - Vector3.Angle(originalPoints[i], humpPeak)) / humpRadius * Mathf.PI - Mathf.PI / 2;
                change = (Mathf.Sin(change) * ((heightRatio - 1.0f) / 2.0f)) + (heightRatio / 2.0f) + 0.5f;
            }
            returnArray[i] = originalPoints[i] * change;
        }
        return returnArray;
    }

    private static Vector3 GetRandomSpherePoint(System.Random rand)
    {
        float theta = (float) rand.NextDouble() * Mathf.PI * 2f;
        float z = (float)rand.NextDouble() * 2 - 1;
        float x = Mathf.Sqrt(1 - Mathf.Pow(z, 2)) * Mathf.Cos(theta);
        float y = Mathf.Sqrt(1 - Mathf.Pow(z, 2)) * Mathf.Sin(theta);
        return new Vector3(x, y, z);
    }



}
