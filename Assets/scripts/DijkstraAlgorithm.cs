using System.Collections.Generic;
using UnityEngine;

public class DijkstraAlgorithm
{
    // EnkisaMesafeyiHesapla metodu, en kısa mesafeleri hesaplar ve sonuçları döndürür.
    public Dictionary<GameObject, float> EnkisaMesafeyiHesapla(GameObject startNode, GameObject[] targetNodes)
    {
        // Algoritmanın iç ayrıntıları gizlenir.
        return DijkstraHelper.EnkisaMesafeyiHesapla(startNode, targetNodes);
    }

    // Yardımcı sınıf, algoritmanın detaylarını içerir ve soyutlama sağlar.
    private static class DijkstraHelper
    {
        public static Dictionary<GameObject, float> EnkisaMesafeyiHesapla(GameObject startNode, GameObject[] targetNodes)
        {
            // En kısa mesafeleri depolamak için bir Dictionary oluşturulur.
            Dictionary<GameObject, float> shortestDistances = new Dictionary<GameObject, float>();
            HashSet<GameObject> visited = new HashSet<GameObject>();

            shortestDistances[startNode] = 0;
            foreach (GameObject node in targetNodes)
            {
                if (node != startNode)
                    shortestDistances[node] = Mathf.Infinity;
            }

            while (visited.Count < targetNodes.Length)
            {
                GameObject current = null;
                float shortest = Mathf.Infinity;

                foreach (GameObject node in targetNodes)
                {
                    if (!visited.Contains(node) && shortestDistances[node] < shortest)
                    {
                        shortest = shortestDistances[node];
                        current = node;
                    }
                }

                visited.Add(current);

                foreach (GameObject neighbor in KomşuBul(current))
                {
                    float distance = Vector3.Distance(current.transform.position, neighbor.transform.position);

                    if (shortestDistances[current] + distance < shortestDistances[neighbor])
                    {
                        shortestDistances[neighbor] = shortestDistances[current] + distance;
                    }
                }
            }

            return shortestDistances;
        }

        // Verilen bir düğümün komşularını bulan yardımcı metot.
        private static List<GameObject> KomşuBul(GameObject node)
        {
            List<GameObject> neighbors = new List<GameObject>();

            Collider2D[] colliders = Physics2D.OverlapCircleAll(node.transform.position, 1.0f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.CompareTag("SandikPrefab"))
                {
                    neighbors.Add(collider.gameObject);
                }
            }

            return neighbors;
        }
    }
}

