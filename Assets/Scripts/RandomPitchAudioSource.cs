using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPitchAudioSource : MonoBehaviour
{
    [SerializeField]
    AudioSource source;

    [SerializeField]
    float minimum, maximum;

    private void Awake()
    {
        source.pitch = Random.Range(minimum, maximum);
    }
}
