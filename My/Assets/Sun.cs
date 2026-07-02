using UnityEngine;

public class Sun : MonoBehaviour
{
    public float TimeOf24Hours;
    void Update()
    {
        float EarthRotationSpeed = 360/TimeOf24Hours;
        transform.Rotate(EarthRotationSpeed*Time.deltaTime,0f,0f);
    }
}