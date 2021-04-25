using UnityEngine;

public struct BoidData
{
    public Vector3 position;
    public Vector3 velocity;

    public BoidData(Vector3 position, Vector3 velocity)
    {
        this.position = position;
        this.velocity = velocity;
    }
}

[RequireComponent(typeof(Rigidbody))]
public class Boid : MonoBehaviour
{
    Rigidbody rigidbody;

    [SerializeField] private float neighbourhood = 10; // radius where other boids are visible

    [Range(1f, 20f)]
    [SerializeField] private float speed = 1;

    [Range(0.1f, 1f)]
    [SerializeField] private float forceOfCohesion = 1;
    [Range(0.1f, 1f)]
    [SerializeField] private float forceOfSeparation = 1;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public BoidData getBoidData()
    {
        return new BoidData(transform.position, rigidbody.velocity);
    }

    public void Align(BoidData[] boids)
    {
        Vector3 avg = new Vector3();
        int count = 0;

        foreach (BoidData other in boids) {

            // ignore self

            // if this other boid is within neighbourhood
            if (Vector3.Distance(transform.position, other.position) < neighbourhood)
            {
                //Vector3 dirToOther = other.position - transform.position;

                //// find angle between velocity and other boid
                //float theta = Mathf.Acos(Vector3.Dot(rigidbody.velocity, dirToOther));
                //Debug.Log(theta);

                avg += other.velocity * Random.value;
                count++;
            }
        }

        if (count != 0)
        {
            avg /= count;
            avg = avg.normalized * speed;
        }

        rigidbody.AddForce(avg);
    }

    public void Cohesion(BoidData[] boids)
    {
        Vector3 avg = new Vector3();
        int count = 0;

        foreach (BoidData other in boids)
        {

            // ignore self

            // if this other boid is within neighbourhood
            if (Vector3.Distance(transform.position, other.position) < neighbourhood)
            {
                avg += other.position;
                count++;
            }
        }

        if (count != 0)
        {
            avg /= count;
            rigidbody.AddForce((avg - transform.position).normalized * forceOfCohesion); 
        }
    }

    public void Separation(BoidData[] boids)
    {
        Vector3 avg = new Vector3();
        int count = 0;

        foreach (BoidData other in boids)
        {

            // ignore self

            // if this other boid is within neighbourhood
            if (Vector3.Distance(transform.position, other.position) < neighbourhood)
            {
                avg += other.position - transform.position;
                count++;
            }
        }

        if (count != 0)
        {
            avg /= count;
            rigidbody.AddForce(-avg.normalized * forceOfSeparation);
        }
    }

    private void FixedUpdate()
    {
        rigidbody.AddForce(-rigidbody.velocity); // cancel acceleration?

        float halfSize = BoidManager.spawnFieldRadius;

        Vector3 resolution = transform.position;
        if (transform.position.x > halfSize)
        {
            resolution.x = -halfSize;
        }
        else if (transform.position.x < -halfSize)
        {
            resolution.x = halfSize;
        }

        if (transform.position.y > halfSize)
        {
            resolution.y = -halfSize;
        }
        else if (transform.position.y < -halfSize)
        {
            resolution.y = halfSize;
        }

        if (transform.position.z > halfSize)
        {
            resolution.z = -halfSize;
        }
        else if (transform.position.z < -halfSize)
        {
            resolution.z = halfSize;
        }

        transform.position = resolution;
    }
}
