using UnityEngine;

public class EnemyIntroSoundTrigger : SoundTrigger
{
    private void OnTriggerEnter(Collider other)
    {
        VoiceOverSoundManager.instance.PlayEnemyVoiceMaybe("Play_Xenandros_Intro");
        Destroy(gameObject);
    }
}
