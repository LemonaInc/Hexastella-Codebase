using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFSCopyrightScreen : UIScreen
{
    private void OnEnable()
    {
        StartCoroutine(CopyrightLogoTime());
    }

    IEnumerator CopyrightLogoTime()
    {
        yield return new WaitForSecondsRealtime(3f);
        UIManager.instance.Show<MainScene>();
    }
}
