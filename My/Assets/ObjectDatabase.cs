using UnityEngine;

public class ObjectDatabase : MonoBehaviour
{
    public GameObject[] objects;
    public string GetObject(int id)
    {
        return objects[id].name;
    }
}