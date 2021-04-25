using UnityEngine;

using System.Collections.Generic;

public class BoidManager : MonoBehaviour
{
    [SerializeField]
    private GameObject boidPrefab;

    [SerializeField] private int boidCount = 100;
    public static int spawnFieldRadius = 75;

    private Boid[] boids;
    private Octree spatialTree;

    private void Start()
    {
        boids = new Boid[boidCount];

        for (int i = 0; i < boidCount; i++)
        {
            GameObject instBoid = Instantiate(boidPrefab, Random.insideUnitSphere * spawnFieldRadius, transform.rotation, transform);
            boids[i] = instBoid.GetComponent<Boid>();

            instBoid.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere;
        }

        List<BoidData> boidList = new List<BoidData>();
        foreach (Boid boid in boids)
        {
            boidList.Add(boid.getBoidData());
        }
        spatialTree = new Octree(new Region(Vector3.zero, spawnFieldRadius*2), boidList);
    }

    private void OnDrawGizmos()
    {
        if(spatialTree != null)
            spatialTree.Show();

        
    }

    private void Update()
    {
        BoidData[] snapshot = new BoidData[boidCount];

        for (int i = 0; i < boidCount; i++)
        {
            snapshot[i] = boids[i].getBoidData();
        }

        foreach (Boid boid in boids)
        {
            //boid.Align(snapshot);
            //boid.Cohesion(snapshot);
        }

        //spatialTree.UpdateTree();
    }
}
