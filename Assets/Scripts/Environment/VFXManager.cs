using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{

    [SerializeField] private TriggerVfx triggerVfxExplosion;
    [SerializeField] private TriggerVfx triggerVfxHit;
    [SerializeField] private TriggerVfx triggerVfxBonus;
    [SerializeField] private TriggerVfx triggerVfxDisc;
    [SerializeField] private TriggerVfx triggerVfxBroken;
    [SerializeField] private TriggerVfx triggerVfxCigarette;
    [SerializeField] private TriggerVfx triggerVfxFlicExclamation;

    [SerializeField] private GameObject vfxShoesL;
    [SerializeField] private GameObject vfxShoesR;
    [SerializeField] private GameObject vfxLoseOnFoot;
    [SerializeField] private GameObject vfxWeed;
    

    public void PlayVfxExplosion() {
        triggerVfxExplosion.PlayVfx();
    }

    public void PlayVfxHit() {
        triggerVfxHit.PlayVfx();
    }

    public void PlayVfxBonus() {
        triggerVfxBonus.PlayVfx();
    }

    public void PlayVfxDisc() {
        triggerVfxDisc.PlayVfx();
    }

    public void PlayVfxBroken() {
        triggerVfxBroken.PlayVfx();
    }

    public void PlayVfxCigarette() {
        triggerVfxCigarette.PlayVfx();
    }

    public void PlayVfxFlicExclamation() {
        triggerVfxFlicExclamation.PlayVfx();
    }

    public void SetActiveVfxShoes(bool isActive) {
        vfxShoesL.SetActive(isActive);
        vfxShoesR.SetActive(isActive);
    }

    public void SetActiveVfxLoseOnFoot(bool isActive) {
        vfxLoseOnFoot.SetActive(isActive);
    }

    public void SetActiveVfxWeed(bool isActive) {
        vfxWeed.SetActive(isActive);
    }
}
