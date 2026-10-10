using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMovementUpdater : AbstractMovementUpdater
{
    [SerializeField]
    private List<ParticleSystem> engineParticles;
    private List<ParticleSystem.MainModule> engineParticles_main = new List<ParticleSystem.MainModule>();

    [SerializeField]
    private Gradient engineParticleColorProgression;
    [SerializeField]
    private AnimationCurve engineParticleSizeProgression;

    protected override void Awake()
    {
        base.Awake();
        foreach (var e in engineParticles)
            engineParticles_main.Add(e.main);
    }

    public override void UpdateObserver()
    {
        ControlEngineParticles(CalculateEngineThrust());
    }

    void ControlEngineParticles(float amount)
    {
        for(int i = 0; i < engineParticles_main.Count; i++)
        {
            var engine = engineParticles_main[i];
            engine.startColor = engineParticleColorProgression.Evaluate(amount);
            engine.startSizeMultiplier = engineParticleSizeProgression.Evaluate(amount);
        }
    }
}
