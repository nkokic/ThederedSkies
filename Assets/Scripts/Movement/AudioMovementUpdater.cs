using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMovementUpdater : AbstractMovementUpdater
{
    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AnimationCurve engineSoundSpeedProgression;

    public override void UpdateObserver()
    {
        ControlEngineSound(CalculateEngineThrust());
    }

    void ControlEngineSound(float amount)
    {
        audioSource.pitch = engineSoundSpeedProgression.Evaluate(amount);
    }
}
