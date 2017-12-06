using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//blendshapes indexes
enum Idx
{
    //EB = EyeBrow
    //EL = Eyelid
    // l = left
    // r = right

    mouth_A,

    jaw_left,
    jaw_right,

    mouth_O,
    mouth_E,
    mouth_M,
    mouth_F,
    mouth_forward,

    EB_angry_l,
    EB_angry_r,

    EB_sad_l,
    EB_sad_r,

    EB_raised_l,
    EB_raised_r,

    EB_corner_l,
    EB_corner_r,

    EL_upper_up_l,
    EL_upper_up_r,

    EL_upper_closed_l,
    EL_upper_closed_r,

    EL_lower_l,
    EL_lower_r,

    Nose_up_l,
    Nose_up_r,

    smile_l,
    smile_r,

    sad_l,
    sad_r,

    Lip_up_l,
    Lip_up_r



}
public class EmotionControl : MonoBehaviour {

    // emotions in between are the sum of those feelings

    public SkinnedMeshRenderer head;
    

    // anger, happy, surprise, sad, fear and disgust
    [Range(0, 100)]
    public float ecstasy; // >  joy > serenity | opposite: grief
    // love
    [Range(0, 100)]
    public float admiration; // > trust > acceptance | opposite: loathing
    // submission
    [Range(0, 100)]
    public float terror; // > fear > apprehension | opposite: rage
    // awe
    [Range(0, 100)]
    public float amazement; // > surprise > distraction | opposite: vigilance
    // disapproval
    [Range(0, 100)]
    public float grief; // > sadness > pensiveness | opposite: ecstasy
    // remorse
    [Range(0, 100)]
    public float loathing; // > disgust > boredom | opposite: admiration
    //contempt
    [Range(0, 100)]
    public float rage; // > anger > annoyance | opposite: terror
    //agressiveness
    [Range(0, 100)]
    public float vigilance; // > anticipation > interest | opposite: amazement
    //optimisim

    // Use this for initialization
    void Start () {
        head.SetBlendShapeWeight((int)Idx.EB_angry_r, 100);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
