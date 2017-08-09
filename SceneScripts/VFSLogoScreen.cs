using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFSLogoScreen : UIScreen
{
    private void OnEnable()
    {
        StartCoroutine(VFSLogoTime());
    }

    IEnumerator VFSLogoTime()
    {
        yield return new WaitForSecondsRealtime(3f);
        UIManager.instance.Show<FiveAlarmLogoScreen>();
    }
}
