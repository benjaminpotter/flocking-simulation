using UnityEngine;

using System.Collections.Generic;

public class Region
{
    public Vector3 center;
    public float size;

    private float half;
    private float quarter;

    public Region(Vector3 center, float size)
    {
        this.center = center;
        this.size = size;

        half = size / 2;
        quarter = size / 4;
    }

    // create eight new subregions
    public Region[] Split()
    {
        Region[] subregions = new Region[8];

        Vector3 tar;

        tar = new Vector3(
            center.x + quarter,
            center.y + quarter,
            center.z + quarter
            );
        subregions[0] = new Region(tar, half);

        tar = new Vector3(
            center.x - quarter,
            center.y + quarter,
            center.z + quarter
            );
        subregions[1] = new Region(tar, half);

        tar = new Vector3(
            center.x + quarter,
            center.y - quarter,
            center.z + quarter
            );
        subregions[2] = new Region(tar, half);

        tar = new Vector3(
            center.x - quarter,
            center.y - quarter,
            center.z + quarter
            );
        subregions[3] = new Region(tar, half);


        tar = new Vector3(
            center.x + quarter,
            center.y + quarter,
            center.z - quarter
            );
        subregions[4] = new Region(tar, half);

        tar = new Vector3(
            center.x - quarter,
            center.y + quarter,
            center.z - quarter
            );
        subregions[5] = new Region(tar, half);

        tar = new Vector3(
            center.x + quarter,
            center.y - quarter,
            center.z - quarter
            );
        subregions[6] = new Region(tar, half);

        tar = new Vector3(
            center.x - quarter,
            center.y - quarter,
            center.z - quarter
            );
        subregions[7] = new Region(tar, half);

        return subregions;
    }

    // is this point inside this region
    public bool Contains(Vector3 point)
    {
        return
            point.x >= center.x - half && point.x <= center.x + half &&
            point.y >= center.y - half && point.y <= center.y + half &&
            point.z >= center.z - half && point.z <= center.z + half;
    }

    public bool exceedsMinSize(int min)
    {
        return size >= min;
    }
}

public class Octree
{
    // size of the world -- or the container of octree
    Region worldSpace;

    // min size of a node
    public static int MIN_SIZE = 1;

    // every boid in the game world
    List<BoidData> boids;

    // root node
    OctreeNode head;

    public Octree(Region worldSpace, List<BoidData> boids)
    {
        this.worldSpace = worldSpace;
        this.boids = boids;

        BuildTree();

        Debug.Log(worldSpace.Contains(new Vector3(75, 0, 0)));
    }

    public void Show()
    {
        Gizmos.color = Color.green;
        head.Show();
    }

    // construct the tree
    void BuildTree() {

        // create the head of the list
        head = new OctreeNode(null, worldSpace, boids);
        head.UpdateTree();
    }

    public void UpdateTree()
    {
        head.UpdateTree();
    }
}

class OctreeNode
{
    // section of space this node occupies
    Region region;

    OctreeNode parent;

    // sub-branches
    OctreeNode[] children = new OctreeNode[8];

    // all the boids in this region
    List<BoidData> inRegion = new List<BoidData>();


    public OctreeNode(OctreeNode parent, Region region, List<BoidData> inRegion)
    {
        this.parent = parent;
        this.region = region;
        this.inRegion = inRegion;
    }

    // update node and all children
    public void UpdateTree()
    {
        // check if we are a leaf
        if (!region.exceedsMinSize(Octree.MIN_SIZE) || inRegion.Count <= 1)
            return; // no tree to update if so

        Region[] subregions = region.Split();

        // create all subnodes
        for (int i = 0; i < 8; i++)
        {
            // pass an empty list by default
            children[i] = new OctreeNode(this, subregions[i], new List<BoidData>());
        }

        List<BoidData> delist = new List<BoidData>();
        foreach (BoidData boid in inRegion)
        {
            for (int i = 0; i < 8; i++)
            {
                if (subregions[i].Contains(boid.position))
                {
                    // add the boid to the corresponding node
                    children[i].inRegion.Add(boid);
                    delist.Add(boid); // remove from this node
                    break; // can only be in one region
                }
            }
        }

        foreach(BoidData boid in delist)
        {
            inRegion.Remove(boid);
        }

        foreach (OctreeNode node in children)
        {
            // ask children to update themselves 
            node.UpdateTree();
        }
    }

    public void Show()
    {
        if(inRegion.Count > 0)
            Gizmos.DrawWireCube(region.center, Vector3.one * region.size);

        foreach(OctreeNode child in children)
        {
            if(child != null)
                child.Show();
        }
    }
}
