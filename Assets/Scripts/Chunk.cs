using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Vector3Int position;
    public byte[,,] blocks = new byte[16,16,16];
    public GameObject chunkObject;
}
