using System.Collections;
using UnityEngine;

public class FiveAlarmLogoScreen : UIScreen
{
    private void OnEnable()
    {
        StartCoroutine(AlarmLogoTime());
    }

    IEnumerator AlarmLogoTime()
    {
        yield return new WaitForSecondsRealtime(3f);
        UIManager.instance.Show<VFSCopyrightScreen>();
    }
}
