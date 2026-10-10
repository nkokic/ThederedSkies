using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHealthUpdater : AbstractHealthUpdater
{
    [SerializeField]
    private ParticleSystem particleSystem;

    [SerializeField]
    Gradient colourOverHealth;

    ParticleSystem.MainModule particleSystemModule;

    protected override void Awake()
    {
        base.Awake();
        particleSystemModule = particleSystem.main;
    }

    public override void UpdateObserver()
    {
        if(health.GetHealth / health.GetMaxHealth > 0.9f)
        {
            particleSystem.Stop();
            return;
        }
        particleSystem.Play();
        particleSystemModule.startColor = colourOverHealth.Evaluate(health.GetHealth / health.GetMaxHealth);
    }
    public override void StopObserver()
    {
        transform.SetParent(null);
        particleSystemModule.stopAction = ParticleSystemStopAction.Destroy;
        particleSystem.Stop();
    }
}
