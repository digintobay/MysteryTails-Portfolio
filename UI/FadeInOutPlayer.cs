using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutPlayer : MonoBehaviour
{
    public ChapterOne fadeInOutEvents;
    private Animation fadeAnim;

    private void Awake()
    {
        fadeAnim = GetComponent<Animation>();

        fadeInOutEvents = fadeInOutEvents.GetComponent<ChapterOne>();
        fadeInOutEvents.FadeInOutPlay += Test_OnFadeStart; //±¸µ¶
    }

    public void Test_OnFadeStart(object sender, EventArgs eventArgs)
    {
       fadeAnim.Play();
    }
}
