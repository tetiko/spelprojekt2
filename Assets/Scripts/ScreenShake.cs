using DG.Tweening;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    //duration The duration of the shake.
    [SerializeField] float duration = 0.3f;
    //strength The shake strength. Using a Vector3 instead of a float lets you choose the strength for each axis.
    [SerializeField] float strength = 0.8f;
    //vibrato Indicates how much the shake will vibrate.
    [SerializeField] int vibrato = 30;
    //randomness Indicates how much the shake will be random (0 to 180 - values higher than 90 kind of suck, so beware). Setting it to 0 will shake along a single direction.
    [SerializeField] float randomness = 50f;
    //snapping If TRUE the tween will smoothly snap all values to integers.
    [SerializeField] bool snapping = false;
    //fadeOut (default: true) If TRUE the shake will automatically fadeOut smoothly within the tween's duration, otherwise it will not.
    [SerializeField] bool fadeOut = true;
    //randomnessMode(default: Full) The type of randomness to apply, Full (fully random) or Harmonic(more balanced and visually more pleasant). 
    [SerializeField] ShakeRandomnessMode RandomnessMode;
 
    private void Start()
    {
    }

    public void Shake()
    {
        //DOShakePosition(float duration, float/Vector3 strength, int vibrato, float randomness, bool snapping, bool fadeOut, ShakeRandomnessMode randomnessMode);
        transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut, RandomnessMode);
        print("shake");
    }
}
