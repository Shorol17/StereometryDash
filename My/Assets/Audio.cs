using UnityEngine;

public class Audio : MonoBehaviour
{
    AudioSource WalkAudio;
    bool IsPlaying;
    float timer = 0.5f;

    void Awake()
    {
        WalkAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && !IsPlaying)
        {
            IsPlaying = true;
            WalkAudio.Play();
        }
        if (timer < 0)
        {
            IsPlaying = false;
            timer = 0.5f;
        }
    }
}
