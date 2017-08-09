using UnityEngine;

public class PlayerIntroSoundTrigger : SoundTrigger
{
    private void OnTriggerEnter(Collider other)
    {
        VoiceOverSoundManager.instance.PlayPlayerVoiceMaybe("Play_AAAP_Enter");
        Destroy(gameObject);
    }
}
