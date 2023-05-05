using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class HapticsManager : MonoBehaviour
{
    static Dictionary<float, List<Vibration>> s_leftVibrations = new Dictionary<float, List<Vibration>>();
    static Dictionary<float, List<Vibration>> s_rightVibrations = new Dictionary<float, List<Vibration>>();

    public enum Hand
    {
        Left,
        Right
    }

    class Vibration
    {
        float _amplitude;
        float _duration;

        float _time;

        public Vibration(float amplitude, float duration)
        {
            _amplitude = amplitude;
            _duration = duration;
        }

        public float Amplitude { get { return _amplitude; } }
        public float Duration { get { return _duration; } }
        public float GetTime { get { return _duration; } }

        public void Elapse() { _time += Time.deltaTime; }

        public bool Complete() { return _time >= _duration; }
    }

    private void Update()
    {
        ManageVibrations(s_leftVibrations, OVRInput.Controller.LTouch);
        ManageVibrations(s_rightVibrations, OVRInput.Controller.RTouch);
    }
    public static void CancelVibration(Hand hand)
    {
        if (hand == Hand.Left)
        {
            s_leftVibrations.Clear();
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        } else
        {
            s_rightVibrations.Clear();
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        }
    }
    void ManageVibrations(Dictionary<float, List<Vibration>> handVibrations, OVRInput.Controller controller)
    {
        float[] priorities = handVibrations.Keys.ToArray();
        Array.Sort(priorities);

        // Play the highest-amplitude vibration from the highest-priority list
        if (priorities.Length > 0)
        {
            Vibration toPlay = handVibrations[priorities[0]][0];
            OVRInput.SetControllerVibration(0, toPlay.Amplitude, controller);
        }

        // Elapse time for all vibrations and remove completed vibrations
        foreach (float priority in priorities)
        {
            foreach (Vibration vibration in handVibrations[priority].ToArray())
            {
                vibration.Elapse();
                if (vibration.Complete()) handVibrations[priority].Remove(vibration);
            }
            if (handVibrations[priority].Count == 0)
            {
                handVibrations.Remove(priority);

                // Stop vibrating after last vibration is removed
                if (handVibrations.Count == 0)
                {
                    OVRInput.SetControllerVibration(0, 0, controller);
                }
            }
        }
    }

    public static void Vibrate(float amplitude, float duration, Hand hand, float priority = 0)
    {
        // Create new vibration
        Vibration newVibration = new Vibration(amplitude, duration);

        Dictionary<float, List<Vibration>> handVibrations = hand == Hand.Left ? s_leftVibrations : s_rightVibrations;

        if (!handVibrations.ContainsKey(priority)) handVibrations[priority] = new List<Vibration>();

        // Insert new vibration into sorted decending list (by amplitude)
        int index = handVibrations[priority].Count;
        for (int i = 0; i < index; i++)
        {
            // Same amplitude was found, check to update time and duration
            if (amplitude == handVibrations[priority][i].Amplitude)
            {
                if (handVibrations[priority][i].Duration - handVibrations[priority][i].GetTime < newVibration.Duration)
                {
                    handVibrations[priority][i] = newVibration;
                }
                return;
            }
            // Slot for new vibration was found
            if (amplitude > handVibrations[priority][i].Amplitude)
            {
                index = i;
                break;
            }
        }
        handVibrations[priority].Insert(index, newVibration);
    }
}
