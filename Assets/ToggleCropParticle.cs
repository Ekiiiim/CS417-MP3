using UnityEngine;

public class ToggleCropParticle : MonoBehaviour
{
    // Actually, not toggle, because the particle system is a one-time burst of emission
    // However, we want to control the count of particles bursted.
    public ParticleSystem cropParticle;

    public void TriggerParticle(int count)
    {
        var emission = cropParticle.emission;
        emission.SetBurst(0, new ParticleSystem.Burst(0f, (short)count));
        cropParticle.Play();
    }
}
