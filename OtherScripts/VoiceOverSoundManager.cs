using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOverSoundManager : MonoBehaviour
{
    uint PlayerSoundEventCurrentlyPlaying;
    uint BossSoundEventCurrentlyPlaying;

    PlayerController player;
    public EnemyAttackController enemy;

    //Singleton
    public static VoiceOverSoundManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        player = GlobalUtils.Player;
    }

    public uint PlayPlayerVoiceMaybe(string wwiseEventName, float probabilityThreshold = 100f)
    {
        float random = UnityEngine.Random.value;

        if (random * 100f < probabilityThreshold)
        {
            AkSoundEngine.StopPlayingID(PlayerSoundEventCurrentlyPlaying);

            uint playID = AkSoundEngine.PostEvent(wwiseEventName, player.gameObject);
            PlayerSoundEventCurrentlyPlaying = playID;

            return playID;
        }
        else
            return 0;
    }

    public uint PlayEnemyVoiceMaybe(string wwiseEventName, float probabilityThreshold = 100f)
    {
        float random = UnityEngine.Random.value;

        if (random * 100f < probabilityThreshold)
        {
            AkSoundEngine.StopPlayingID(BossSoundEventCurrentlyPlaying);

            uint playID = AkSoundEngine.PostEvent(wwiseEventName, enemy.gameObject);
            BossSoundEventCurrentlyPlaying = playID;

            return playID;
        }
        else
            return 0;
    }

}
