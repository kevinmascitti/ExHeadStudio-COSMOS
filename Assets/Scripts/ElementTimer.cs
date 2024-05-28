using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public enum TimerType
{
    Status,
    Effect
}

public class ElementTimer
{
    public int id;
    public Element element;
    public TimerType type;
    public double Interval;
    
    [NonSerialized] public bool Enabled;

    private double passedTime;

    static public EventHandler<ElementTimerArgs> Elapsed;
    
    public ElementTimer(double seconds, bool enabled, TimerType t, Element e, int i)
    {
        Interval = seconds;
        Enabled = enabled;
        type = t;
        element = e;
        id = i;
        PlayerCharacter.OnUpdate += Update;
    }

    public void Begin()
    {
        Enabled = false;
        passedTime = Interval;
        Enabled = true;
    }

    public void Resume()
    {
        Enabled = true;
    }

    public void Stop()
    {
        Enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Enabled)
        {
            passedTime -= Time.deltaTime;
            if (passedTime <= 0)
            {
                Elapsed?.Invoke(this, new ElementTimerArgs(type, element, id));
            }
        }
    }
}

public class ElementTimerArgs : EventArgs 
{
    public ElementTimerArgs(TimerType t, Element e, int i)
    {
        id = i;
        type = t;
        element = e;
    }

    public TimerType type;
    public Element element;
    public int id;
}