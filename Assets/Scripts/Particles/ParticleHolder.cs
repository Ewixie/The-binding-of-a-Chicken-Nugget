using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleHolder : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    public event Action ParticleSystemStopped;
    private void OnParticleSystemStopped()
    {
        ParticleSystemStopped?.Invoke();
    }
    public ParticleSystem GetParticleSystem()
    {
        if (particle is not null) return particle;
        //particle is null
        particle = GetComponent<ParticleSystem>();
        return particle;
    }
}
