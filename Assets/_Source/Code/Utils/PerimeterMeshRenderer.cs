using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Client_.Scripts.Utils
{
    public class PerimeterMeshRenderer : MonoBehaviour
    {
        [Button]
        public void CalculatePerimeter()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
        
            if (meshFilter != null)
            {
                Mesh mesh = meshFilter.mesh;

                Vector3[] vertices = mesh.vertices;

                float perimeter = 0f;

                for (int i = 0; i < mesh.triangles.Length; i += 3)
                {
                    Vector3 vertex1 = vertices[mesh.triangles[i]];
                    Vector3 vertex2 = vertices[mesh.triangles[i + 1]];
                    Vector3 vertex3 = vertices[mesh.triangles[i + 2]];

                    float edge1 = Vector3.Distance(vertex1, vertex2);
                    float edge2 = Vector3.Distance(vertex2, vertex3);
                    float edge3 = Vector3.Distance(vertex3, vertex1);

                    perimeter += edge1 + edge2 + edge3;
                }

                Debug.Log("Периметр меша: " + perimeter);
            }
        }

        [Button]
        public void DrawPerimeter()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] vertices = mesh.vertices;

            GameObject lineObj = new GameObject( name+" Perimeter");
            LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

            lineRenderer.positionCount = mesh.vertices.Length + 1;
            lineRenderer.useWorldSpace = false;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;

            List<Vector3> points = new();
        
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                Vector3 scaledVertex = Vector3.Scale(vertices[i], transform.localScale);
                scaledVertex.y = 0;

                if (i > 0)
                {
                    if(points.Contains(scaledVertex))
                    {
                        continue;
                    }
                    else
                    {
                        points.Add(scaledVertex);
                    }
                }
            }

            Vector3 firstVertex = Vector3.Scale(vertices[0], transform.localScale);
            firstVertex.y = 0;
            points.Add(firstVertex);
        
            for (int i = 0; i < points.Count; i++)
            {
                var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                obj.transform.position = points[i];
                obj.transform.localScale = Vector3.one * 0.15f;
                obj.transform.SetParent(lineObj.transform);
                lineRenderer.SetPosition(i, points[i]);
            }
        
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;

            lineObj.transform.position = transform.position;
            lineObj.transform.rotation = transform.rotation;
        }

        private bool FindClosest(Vector3 target, Vector3[] vertices,float tolerance)
        {
            foreach (var vertex in vertices)
            {
                var tempVertex = Vector3.Scale(vertex, transform.localScale);;
                tempVertex.y = 0;

                if (Vector3.Distance(target, tempVertex) < tolerance)
                {
                    return true;
                }
            }

            return false;
        }
    }
}



