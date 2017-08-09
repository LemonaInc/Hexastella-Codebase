using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{
    public GameObject spinDamageBubble;
    public GameObject tentacleAttackCube;
    public GameObject laserTriggerCollider;
    public ParticleSystem spinSwoosh;
    public LaserParticleSystem laserfromEye;

    private uint spinSound;

    internal bool tentacleSlamActive = false;
    internal bool isSpinning = false;

    public void PlaySoundEffect(string soundName)
    {
        AkSoundEngine.PostEvent(soundName, gameObject);
    }

    //Animation functions
    public void AnimationAttackActive()
    {
        tentacleSlamActive = true;
        tentacleAttackCube.SetActive(true);
    }

    public void AnimationAttackInactive()
    {
        tentacleSlamActive = false;
        tentacleAttackCube.SetActive(false);
    }

    public void SpinAttackActive()
    {
        isSpinning = true;
        spinDamageBubble.SetActive(true);
        spinSound = AkSoundEngine.PostEvent("Play_SFX_XEN_Spin", gameObject);
    }

    public void SpinAttackInactive()
    {
        isSpinning = false;
        spinDamageBubble.SetActive(false);
        AkSoundEngine.StopPlayingID(spinSound, 2);
    }

    public void SpinParticleStart()
    {
        spinSwoosh.Play();
    }

    public void SpinParticleStop()
    {
        spinSwoosh.Stop();
    }

    //Sound effects
    public void ChargeLaser()
    {
        PlaySoundEffect("Play_Alien_Lazer_Charge");
        laserfromEye.Play();
    }

    public void ShootLaser()
    {
        PlaySoundEffect("Play_Alien_Lazer_Shot");
        laserTriggerCollider.SetActive(true);
    }

    public void StopLaser()
    {
        PlaySoundEffect("Stop_Alien_Lazer_Loop");
        laserTriggerCollider.SetActive(false);
    }

    public void ChargeAOE()
    {
        PlaySoundEffect("Play_SFX_ALN_MEC6");
    }

    public void ShootAOE()
    {
        PlaySoundEffect("Play_SFX_ALN_MEC2");
    }
    
    public void SlamWomp()
    {
        AkSoundEngine.SetSwitch("Physical_Attack_Type", "Slam", gameObject);
        uint feetEvent = AkSoundEngine.PostEvent("Play_Alien_Physical_Attacks", gameObject);
    }
}
