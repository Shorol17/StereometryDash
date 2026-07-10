using UnityEngine;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    public Dictionary<Vector3Int, Chunk> chunks = new();
    public Material material;
    public GameObject player;
    public int RenderDistance;

    private Vector3 CurrentChunk;
    private HashSet<Vector3Int> oldset = new();

    static readonly int[,] Edges =
    {
        {0,1},
        {1,3},
        {3,2},
        {2,0},

        {4,5},
        {5,7},
        {7,6},
        {6,4},

        {0,4},
        {1,5},
        {2,6},
        {3,7}
    };

    public void Awake()
    {
        CurrentChunk = new Vector3Int(Mathf.FloorToInt(player.transform.position.x/16), Mathf.FloorToInt(player.transform.position.y/16), Mathf.FloorToInt(player.transform.position.z/16));
        for (int x = Mathf.FloorToInt(CurrentChunk.x) - RenderDistance; x < CurrentChunk.x + RenderDistance; x++)
        for (int y = Mathf.FloorToInt(CurrentChunk.y) - RenderDistance; y < CurrentChunk.y + RenderDistance; y++)
        for (int z = Mathf.FloorToInt(CurrentChunk.z) - RenderDistance; z < CurrentChunk.z + RenderDistance; z++)
                {
                    Vector3Int ChunkPos = new Vector3Int(x,y,z);
                    Chunk chunk = new Chunk();
                    chunk.position = ChunkPos;
                    GenerateChunk(chunk);
                    chunks.Add(ChunkPos, chunk);
                    RenderChunk(chunks[ChunkPos]);
                    oldset.Add(ChunkPos);
                }
        player.transform.Translate(0,Mathf.PerlinNoise(0,0)*30,0);
    }

    public void Update()
    {
        Vector3 SuperCurrentChunk = new Vector3(Mathf.FloorToInt(player.transform.position.x/16), Mathf.FloorToInt(player.transform.position.y/16), Mathf.FloorToInt(player.transform.position.z/16));
        if (CurrentChunk != SuperCurrentChunk)
        {
            HashSet<Vector3Int> newset = new();
            HashSet<Vector3Int> newset1 = new();
            for (int x = Mathf.FloorToInt(SuperCurrentChunk.x) - RenderDistance; x < SuperCurrentChunk.x + RenderDistance; x++)
            for (int y = Mathf.FloorToInt(SuperCurrentChunk.y) - RenderDistance; y < SuperCurrentChunk.y + RenderDistance; y++)
            for (int z = Mathf.FloorToInt(SuperCurrentChunk.z) - RenderDistance; z < SuperCurrentChunk.z + RenderDistance; z++){
                newset.Add(new Vector3Int(x,y,z));
                newset1.Add(new Vector3Int(x,y,z));
            }
            newset1.ExceptWith(oldset);
            oldset.ExceptWith(newset);
            foreach (var i in oldset)
                GameObject.Find("Chunk"+i).GetComponent<MeshRenderer>().enabled = false;
            foreach (var i in newset1)
            {
                if (chunks.ContainsKey(i))
                {
                    GameObject.Find("Chunk"+i).GetComponent<MeshRenderer>().enabled = true;
                }
                else
                {
                    Chunk chunk = new Chunk();
                    chunk.position = i;
                    GenerateChunk(chunk);
                    chunks.Add(i, chunk);
                    RenderChunk(chunks[i]);
                }
            }
            CurrentChunk = SuperCurrentChunk;
            oldset = newset;
        }
    }

    public void RenderChunk(Chunk chunk)
    {
        GameObject parent = new GameObject("Chunk"+chunk.position);

        parent.transform.position = chunk.position * 16;

        Mesh mesh = BuildMesh(chunk);

        Instantiate(mesh, parent.transform.position, Quaternion.identity);

        chunk.chunkObject = parent;
        parent.AddComponent<MeshRenderer>();
        parent.GetComponent<MeshRenderer>().material = material;
        parent.AddComponent<MeshFilter>();
        parent.GetComponent<MeshFilter>().mesh = mesh;
        parent.AddComponent<MeshCollider>();
    }

    private Chunk GenerateChunk(Chunk chunk)
    {
        int x = chunk.position.x*16;
        int y = chunk.position.y*16;
        int z = chunk.position.z*16;
        for (int x1 = 0; x1 < 16; x1++)
        for (int z1 = 0; z1 < 16; z1++){
            float height = Mathf.PerlinNoise((x+x1)*0.01f, (z+z1)*0.01f) * 30 + Mathf.PerlinNoise((x+x1)*0.001f, (z+z1)*0.001f) * 300;
        for (int y1 = 0; y1 < 16; y1++)
            if (y1+y > height)
                chunk.blocks[x1,y1,z1] = 0;
            else
                chunk.blocks[x1,y1,z1] = 1;
        }
        return chunk;
    }

    private byte[,,] GenerateChunkPlus(Chunk chunk)
    {
        byte[,,] Chunkplus = new byte[20,20,20];

        int x = chunk.position.x*16;
        int y = chunk.position.y*16;
        int z = chunk.position.z*16;
        for (int x1 = 0; x1 < 20; x1++)
        for (int z1 = 0; z1 < 20; z1++){
            float height = Mathf.PerlinNoise((x+x1)*0.01f, (z+z1)*0.01f) * 30 + Mathf.PerlinNoise((x+x1)*0.001f, (z+z1)*0.001f) * 300;
        for (int y1 = 0; y1 < 20; y1++)
            if (y1+y > height)
                Chunkplus[x1,y1,z1] = 0;
            else
                Chunkplus[x1,y1,z1] = 1;
        }
        return Chunkplus;
    }
/*
    private Mesh BuildMesh(Chunk chunk)
    {
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for(int x = 0; x < 16; x++)
        for(int y = 0; y < 16; y++)
        for(int z = 0; z < 16; z++)
                {
                    if (chunk.blocks[x,y,z] == 0)
                        continue;
                    else
                    {
                        if (x == 15 || chunk.blocks[x+1,y,z] == 0)
                        {
                            int vertexIndex = vertices.Count;

                            vertices.Add(new Vector3(x+1, y+1, z+1));
                            vertices.Add(new Vector3(x+1, y+1, z));
                            vertices.Add(new Vector3(x+1, y, z+1));
                            vertices.Add(new Vector3(x+1, y, z));

                            triangles.Add(vertexIndex);
                            triangles.Add(vertexIndex+2);
                            triangles.Add(vertexIndex+1);

                            triangles.Add(vertexIndex+1);
                            triangles.Add(vertexIndex+2);
                            triangles.Add(vertexIndex+3);
                        }
                        if (x == 0 || chunk.blocks[x-1,y,z] == 0)
                        {
                            int vertexIndex = vertices.Count;

                            vertices.Add(new Vector3(x, y+1, z+1));
                            vertices.Add(new Vector3(x, y+1, z));
                            vertices.Add(new Vector3(x, y, z+1));
                            vertices.Add(new Vector3(x, y, z));

                            triangles.Add(vertexIndex);
                            triangles.Add(vertexIndex+1);
                            triangles.Add(vertexIndex+2);

                            triangles.Add(vertexIndex+1);
                            triangles.Add(vertexIndex+3);
                            triangles.Add(vertexIndex+2);
                        }
                        if (y == 15 || chunk.blocks[x,y+1,z] == 0)
                        {
                            int vertexIndex = vertices.Count;

                            vertices.Add(new Vector3(x+1, y+1, z+1));
                            vertices.Add(new Vector3(x+1, y+1, z));
                            vertices.Add(new Vector3(x, y+1, z+1));
                            vertices.Add(new Vector3(x, y+1, z));

                            triangles.Add(vertexIndex);
                            triangles.Add(vertexIndex+1);
                            triangles.Add(vertexIndex+2);

                            triangles.Add(vertexIndex+1);
                            triangles.Add(vertexIndex+3);
                            triangles.Add(vertexIndex+2);
                        }
                        if (y == 0 || chunk.blocks[x,y-1,z] == 0)
                        {
                            int vertexIndex = vertices.Count;

                            vertices.Add(new Vector3(x+1, y, z+1));
                            vertices.Add(new Vector3(x+1, y, z));
                            vertices.Add(new Vector3(x, y, z+1));
                            vertices.Add(new Vector3(x, y, z));

                            triangles.Add(vertexIndex);
                            triangles.Add(vertexIndex+2);
                            triangles.Add(vertexIndex+1);

                            triangles.Add(vertexIndex+1);
                            triangles.Add(vertexIndex+2);
                            triangles.Add(vertexIndex+3);
                        }
                        if (z == 15 || chunk.blocks[x,y,z+1] == 0)
                        {
                            int vertexIndex = vertices.Count;

                            vertices.Add(new Vector3(x+1, y+1, z+1));
                            vertices.Add(new Vector3(x+1, y, z+1));
                            vertices.Add(new Vector3(x, y+1, z+1));
                            vertices.Add(new Vector3(x, y, z+1));

                            triangles.Add(vertexIndex);
                            triangles.Add(vertexIndex+2);
                            triangles.Add(vertexIndex+1);

                            triangles.Add(vertexIndex+1);
                            triangles.Add(vertexIndex+2);
                            triangles.Add(vertexIndex+3);
                        }
                        if (z == 0 || chunk.blocks[x,y,z-1] == 0)
                        {
                            int vertexIndex = vertices.Count;

                            vertices.Add(new Vector3(x+1, y+1, z));
                            vertices.Add(new Vector3(x+1, y, z));
                            vertices.Add(new Vector3(x, y+1, z));
                            vertices.Add(new Vector3(x, y, z));

                            triangles.Add(vertexIndex);
                            triangles.Add(vertexIndex+1);
                            triangles.Add(vertexIndex+2);

                            triangles.Add(vertexIndex+1);
                            triangles.Add(vertexIndex+3);
                            triangles.Add(vertexIndex+2);
                        }
                    }
                }
        
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();

        return mesh;
    }*/

    private Mesh BuildMesh(Chunk chunk)
    {
        Mesh mesh = new Mesh();

        byte[,,] Chunkplus = GenerateChunkPlus(chunk);
        float[,,] density = new float[19,19,19];
        
        System.Random rnd = new System.Random();

        for (int x = 0; x < 19 ; x++)
        for (int y = 0; y < 19 ; y++)
        for (int z = 0; z < 19 ; z++){
            density[x,y,z] = (Chunkplus[x,y,z] + Chunkplus[x+1,y,z] + Chunkplus[x,y+1,z] + Chunkplus[x,y,z+1] +
                            Chunkplus[x+1,y+1,z] + Chunkplus[x+1,y,z+1] + Chunkplus[x,y+1,z+1] + Chunkplus[x+1,y+1,z+1])/4 - 1;
            if (density[x,y,z] > 0)
                density[x,y,z] += Mathf.PerlinNoise((chunk.position.x*16+x+10)*0.01f, (chunk.position.z*16+z+10)*0.01f)*0.5f;
            else
                density[x,y,z] -= Mathf.PerlinNoise((chunk.position.x*16+x+10)*0.01f, (chunk.position.z*16+z+10)*0.01f)*0.5f;
        }

        List<Vector3> points = new List<Vector3>();
        int[,,] vertexIndex = new int[18,18,18];
        for(int x=0;x<18;x++)
        for(int y=0;y<18;y++)
        for(int z=0;z<18;z++)
            vertexIndex[x,y,z] = -1;
        int CurrentVertex = 0;

        for (int x = 0; x < 18 ; x++)
        for (int y = 0; y < 18 ; y++)
        for (int z = 0; z < 18 ; z++)
                {
                    float[] cube = new float[8];

                    cube[0] = density[x,     y,     z];
                    cube[1] = density[x + 1, y,     z];
                    cube[2] = density[x,     y + 1, z];
                    cube[3] = density[x + 1, y + 1, z];

                    cube[4] = density[x,     y,     z + 1];
                    cube[5] = density[x + 1, y,     z + 1];
                    cube[6] = density[x,     y + 1, z + 1];
                    cube[7] = density[x + 1, y + 1, z + 1];

                    bool hasSurface = false;

                    bool firstSign = cube[0] > 0;

                    for(int i = 1; i < 8; i++)
                    {
                        if((cube[i] > 0) != firstSign)
                        {
                            hasSurface = true;
                            break;
                        }
                    }

                    if (hasSurface)
                    {
                        List<Vector3> intersections = new();

                        Vector3[] verts ={
                            new(x,     y,     z),
                            new(x + 1, y,     z),
                            new(x,     y + 1, z),
                            new(x + 1, y + 1, z),
                            new(x,     y,     z + 1),
                            new(x + 1, y,     z + 1),
                            new(x,     y + 1, z + 1),
                            new(x + 1, y + 1, z + 1)};

                        for(int e = 0; e < 12; e++)
                        {
                            int a = Edges[e,0];
                            int b = Edges[e,1];

                            if((cube[a] > 0) == (cube[b] > 0))
                                continue;

                            Vector3 point = Vector3.Lerp(
                                verts[a],
                                verts[b],
                                cube[a] / (cube[a] - cube[b])
                            );
                            intersections.Add(point);
                        }
                        Vector3 vertex = Vector3.zero;

                        foreach(Vector3 p in intersections)
                            vertex += p;

                        vertex /= intersections.Count;

                        points.Add(vertex);

                        vertexIndex[x,y,z] = CurrentVertex;
                        CurrentVertex++;
                    }
                }
        
        List<int> triangles = new List<int>();

        for (int x = 1; x < 17 ; x++)
        for (int y = 1; y < 18 ; y++)
        for (int z = 1; z < 18 ; z++){
            int a = vertexIndex[x,y,z];
            int b = vertexIndex[x,y,z-1];
            int c = vertexIndex[x,y-1,z];
            int d = vertexIndex[x,y-1,z-1];
            if (a < 0 || b < 0 || c < 0 || d < 0)
                continue;
            if (density[x,y,z] > density [x+1,y,z]){
            triangles.Add(a);
            triangles.Add(c);
            triangles.Add(b);

            triangles.Add(c);
            triangles.Add(d);
            triangles.Add(b);
            }
            else{
            triangles.Add(a);
            triangles.Add(b);
            triangles.Add(c);

            triangles.Add(c);
            triangles.Add(b);
            triangles.Add(d);
            }
        }

        for (int x = 1; x < 18 ; x++)
        for (int y = 1; y < 17 ; y++)
        for (int z = 1; z < 18 ; z++){
            int a = vertexIndex[x,y,z];
            int b = vertexIndex[x-1,y,z];
            int c = vertexIndex[x,y,z-1];
            int d = vertexIndex[x-1,y,z-1];
            if (a < 0 || b < 0 || c < 0 || d < 0)
                continue;
            if (density[x,y,z] > density [x,y+1,z]){
            triangles.Add(a);
            triangles.Add(c);
            triangles.Add(b);

            triangles.Add(c);
            triangles.Add(d);
            triangles.Add(b);
            }
            else{
            triangles.Add(a);
            triangles.Add(b);
            triangles.Add(c);

            triangles.Add(c);
            triangles.Add(b);
            triangles.Add(d);
            }
        }

        for (int x = 1; x < 18 ; x++)
        for (int y = 1; y < 18 ; y++)
        for (int z = 1; z < 17 ; z++){
            int a = vertexIndex[x,y,z];
            int b = vertexIndex[x-1,y,z];
            int c = vertexIndex[x,y-1,z];
            int d = vertexIndex[x-1,y-1,z];
            if (a < 0 || b < 0 || c < 0 || d < 0)
                continue;
            if (density[x,y,z] > density [x,y,z+1]){
                triangles.Add(a);
                triangles.Add(b);
                triangles.Add(c);

                triangles.Add(c);
                triangles.Add(b);
                triangles.Add(d);
            }
            else{
                triangles.Add(a);
                triangles.Add(c);
                triangles.Add(b);

                triangles.Add(c);
                triangles.Add(d);
                triangles.Add(b);
            }
        }

        mesh.vertices = points.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();

        return mesh;
    }
}
