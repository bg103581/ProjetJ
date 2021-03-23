using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerVfx : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] vfxs;

    public void PlayVfx() {
        foreach (ParticleSystem vfx in vfxs) {
            vfx.Play();
        }
    }

    public void StopVfx() {
        foreach (ParticleSystem vfx in vfxs) {
            vfx.Stop();
        }
    }
}
