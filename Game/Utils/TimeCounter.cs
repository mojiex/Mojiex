using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  单位是秒
/// </summary>
public class TimeCounter
{
    public float MaxTime { get; private set; }
    public float CurrentTime { get; private set; }
    public float LeftTime => MaxTime - CurrentTime;
    public float CurrentTimePercent => Mathf.Clamp01(CurrentTime / MaxTime);
    public float LeftTimePercent => Mathf.Clamp01(LeftTime / MaxTime);
    public bool IsFinish => CurrentTime >= MaxTime;
    public TimeCounter() { }
    public TimeCounter(float maxTime, float currentTime = 0)
    {
        MaxTime = maxTime;
        CurrentTime = currentTime;
    }

    public void SetMaxTime(float max)
    {
        MaxTime = max;
    }

    public void Step(float step)
    {
        CurrentTime += step;
    }

    public void SetCurrentTime(float time)
    {
        CurrentTime = time;
    }

    public void Reset()
    {
        SetCurrentTime(0);
    }
}
