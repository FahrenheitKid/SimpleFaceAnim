using System.Collections.Generic;
using UnityEngine;

//blendshapes indexes
public enum Idx
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

public enum EmotionIdx
{
    ecstasy,
    admiration,
    terror,
    amazement,
    grief,
    loathing,
    rage,
    vigilance
}

public struct Pose
{
    public float mouth_A;

    public float jaw_left;
    public float jaw_right;

    public float mouth_O;
    public float mouth_E;
    public float mouth_M;
    public float mouth_F;
    public float mouth_forward;

    public float EB_angry_l;
    public float EB_angry_r;

    public float EB_sad_l;
    public float EB_sad_r;

    public float EB_raised_l;
    public float EB_raised_r;

    public float EB_corner_l;
    public float EB_corner_r;

    public float EL_upper_up_l;
    public float EL_upper_up_r;

    public float EL_upper_closed_l;
    public float EL_upper_closed_r;

    public float EL_lower_l;
    public float EL_lower_r;

    public float Nose_up_l;
    public float Nose_up_r;

    public float smile_l;
    public float smile_r;

    public float sad_l;
    public float sad_r;

    public float Lip_up_l;
    public float Lip_up_r;

    public void init()
    {
        mouth_A = 0f;
        jaw_left = 0f;
        jaw_right = 0f;
        mouth_O = 0f;
        mouth_E = 0f;
        mouth_M = 0f;
        mouth_F = 0f;
        mouth_forward = 0f;
        EB_angry_l = 0;
        EB_angry_r = 0f;
        EB_sad_l = 0f;
        EB_sad_r = 0f;
        EB_raised_l = 0f;
        EB_raised_r = 0f;
        EB_corner_l = 0f;
        EB_corner_r = 0f;
        EL_upper_up_l = 0f;
        EL_upper_up_r = 0f;
        EL_upper_closed_l = 0f;
        EL_upper_closed_r = 0f;
        EL_lower_l = 0f;
        EL_lower_r = 0f;
        Nose_up_l = 0f;
        Nose_up_r = 0f;
        smile_l = 0f;
        smile_r = 0f;
        sad_l = 0f;
        sad_r = 0f;
        Lip_up_l = 0f;
        Lip_up_r = 0f;
    }
}

public class EmotionControl : MonoBehaviour
{
    // emotions in between are the sum of those feelings

    public SkinnedMeshRenderer head;

    public List<Pose> basic_poses_init;
    public List<Pose> basic_poses_end;
    public List<float> weights;

    public Pose ecstasy_pose = new Pose();
    public Pose joy_pose = new Pose();
    public Pose serenity_pose = new Pose();

    public Pose admiration_pose = new Pose();
    public Pose trust_pose = new Pose();
    public Pose acceptance_pose = new Pose();

    public Pose terror_pose = new Pose();
    public Pose fear_pose = new Pose();
    public Pose apprehension_pose = new Pose();

    public Pose amazement_pose = new Pose();
    public Pose surprise_pose = new Pose();
    public Pose distraction_pose = new Pose();

    public Pose grief_pose = new Pose();
    public Pose sadness_pose = new Pose();
    public Pose pensiveness_pose = new Pose();

    public Pose loathing_pose = new Pose();
    public Pose disgust_pose = new Pose();
    public Pose boredom_pose = new Pose();

    public Pose rage_pose = new Pose();
    public Pose anger_pose = new Pose();
    public Pose annoyance_pose = new Pose();

    public Pose vigilance_pose = new Pose();
    public Pose anticipation_pose = new Pose();
    public Pose interest_pose = new Pose();

    public Pose current_pose = new Pose();
    public Pose initial_Lerp = new Pose();
    public Pose final_Lerp = new Pose();

    // anger, happy, surprise, sad, fear and disgust
    [Range(0, 1)]
    public float ecstasy; // >  joy > serenity | opposite: grief

    // love
    [Range(0, 1)]
    public float admiration; // > trust > acceptance | opposite: loathing

    // submission
    [Range(0, 1)]
    public float terror; // > fear > apprehension | opposite: rage

    // awe
    [Range(0, 1)]
    public float amazement; // > surprise > distraction | opposite: vigilance

    // disapproval
    [Range(0, 1)]
    public float grief; // > sadness > pensiveness | opposite: ecstasy

    // remorse
    [Range(0, 1)]
    public float loathing; // > disgust > boredom | opposite: admiration

    //contempt
    [Range(0, 1)]
    public float rage; // > anger > annoyance | opposite: terror

    //agressiveness
    [Range(0, 1)]
    public float vigilance; // > anticipation > interest | opposite: amazement

    //optimisim

    // Use this for initialization
    private void Start()
    {
        current_pose = new Pose();
        current_pose.init();
        initPoses();

        setCurrentPose(serenity_pose);
        
        initial_Lerp.init();
        final_Lerp.init();

        // applyPose(final_Lerp);

        basic_poses_init = new List<Pose>(8);
        basic_poses_end = new List<Pose>(8);
        weights = new List<float>(8);

        for (int i = 0; i < 8; i++)
        {
            Pose p = new Pose();
            basic_poses_init.Add(p);
            basic_poses_end.Add(p);
            weights.Add(0);
        }
        
    }

    // Update is called once per frame
    private void Update()
    {
        //head.SetBlendShapeWeight((int)Idx.mouth_A, 80);

        handleInitEndPoses();
        handleSlidersValues();
        
        initial_Lerp = getAveragePose(basic_poses_init, weights);
        final_Lerp = getAveragePose(basic_poses_end, weights);

        
        float weightsum = 0;
        for (int i = 0; i < weights.Count; i++)
        {
            weightsum += weights[i];
        }
        applyPose(getLerpPose(initial_Lerp, final_Lerp, weightsum));
    }

    //seta a variavel current pose com uma nova pose
    public void setCurrentPose(Pose p)
    {
        current_pose = p;
    }

    //aplica a variavel current pose na mesh em si
    public void applyCurrentPose()
    {
        head.SetBlendShapeWeight((int)Idx.mouth_A, current_pose.mouth_A);
        head.SetBlendShapeWeight((int)Idx.jaw_left, current_pose.jaw_left);
        head.SetBlendShapeWeight((int)Idx.jaw_right, current_pose.jaw_right);
        head.SetBlendShapeWeight((int)Idx.mouth_O, current_pose.mouth_O);
        head.SetBlendShapeWeight((int)Idx.mouth_E, current_pose.mouth_E);
        head.SetBlendShapeWeight((int)Idx.mouth_M, current_pose.mouth_M);
        head.SetBlendShapeWeight((int)Idx.mouth_F, current_pose.mouth_F);
        head.SetBlendShapeWeight((int)Idx.mouth_forward, current_pose.mouth_forward);
        head.SetBlendShapeWeight((int)Idx.EB_angry_l, current_pose.EB_angry_l);
        head.SetBlendShapeWeight((int)Idx.EB_angry_r, current_pose.EB_angry_r);
        head.SetBlendShapeWeight((int)Idx.EB_sad_l, current_pose.EB_sad_l);
        head.SetBlendShapeWeight((int)Idx.EB_sad_r, current_pose.EB_sad_r);
        head.SetBlendShapeWeight((int)Idx.EB_raised_l, current_pose.EB_raised_l);
        head.SetBlendShapeWeight((int)Idx.EB_raised_r, current_pose.EB_raised_r);
        head.SetBlendShapeWeight((int)Idx.EB_corner_l, current_pose.EB_corner_l);
        head.SetBlendShapeWeight((int)Idx.EB_corner_r, current_pose.EB_corner_r);
        head.SetBlendShapeWeight((int)Idx.EL_upper_up_l, current_pose.EL_upper_up_l);
        head.SetBlendShapeWeight((int)Idx.EL_upper_up_r, current_pose.EL_upper_up_r);
        head.SetBlendShapeWeight((int)Idx.EL_upper_closed_l, current_pose.EL_upper_closed_l);
        head.SetBlendShapeWeight((int)Idx.EL_upper_closed_r, current_pose.EL_upper_closed_r);
        head.SetBlendShapeWeight((int)Idx.EL_lower_l, current_pose.EL_lower_l);
        head.SetBlendShapeWeight((int)Idx.EL_lower_r, current_pose.EL_lower_r);
        head.SetBlendShapeWeight((int)Idx.Nose_up_l, current_pose.Nose_up_l);
        head.SetBlendShapeWeight((int)Idx.Nose_up_r, current_pose.Nose_up_r);
        head.SetBlendShapeWeight((int)Idx.smile_l, current_pose.smile_l);
        head.SetBlendShapeWeight((int)Idx.smile_r, current_pose.smile_r);
        head.SetBlendShapeWeight((int)Idx.sad_l, current_pose.sad_l);
        head.SetBlendShapeWeight((int)Idx.sad_r, current_pose.sad_r);
        head.SetBlendShapeWeight((int)Idx.Lip_up_l, current_pose.Lip_up_l);
        head.SetBlendShapeWeight((int)Idx.Lip_up_r, current_pose.Lip_up_r);
    }

    public void applyPose(Pose p)
    {
        head.SetBlendShapeWeight((int)Idx.mouth_A, p.mouth_A);
        head.SetBlendShapeWeight((int)Idx.jaw_left, p.jaw_left);
        head.SetBlendShapeWeight((int)Idx.jaw_right, p.jaw_right);
        head.SetBlendShapeWeight((int)Idx.mouth_O, p.mouth_O);
        head.SetBlendShapeWeight((int)Idx.mouth_E, p.mouth_E);
        head.SetBlendShapeWeight((int)Idx.mouth_M, p.mouth_M);
        head.SetBlendShapeWeight((int)Idx.mouth_F, p.mouth_F);
        head.SetBlendShapeWeight((int)Idx.mouth_forward, p.mouth_forward);
        head.SetBlendShapeWeight((int)Idx.EB_angry_l, p.EB_angry_l);
        head.SetBlendShapeWeight((int)Idx.EB_angry_r, p.EB_angry_r);
        head.SetBlendShapeWeight((int)Idx.EB_sad_l, p.EB_sad_l);
        head.SetBlendShapeWeight((int)Idx.EB_sad_r, p.EB_sad_r);
        head.SetBlendShapeWeight((int)Idx.EB_raised_l, p.EB_raised_l);
        head.SetBlendShapeWeight((int)Idx.EB_raised_r, p.EB_raised_r);
        head.SetBlendShapeWeight((int)Idx.EB_corner_l, p.EB_corner_l);
        head.SetBlendShapeWeight((int)Idx.EB_corner_r, p.EB_corner_r);
        head.SetBlendShapeWeight((int)Idx.EL_upper_up_l, p.EL_upper_up_l);
        head.SetBlendShapeWeight((int)Idx.EL_upper_up_r, p.EL_upper_up_r);
        head.SetBlendShapeWeight((int)Idx.EL_upper_closed_l, p.EL_upper_closed_l);
        head.SetBlendShapeWeight((int)Idx.EL_upper_closed_r, p.EL_upper_closed_r);
        head.SetBlendShapeWeight((int)Idx.EL_lower_l, p.EL_lower_l);
        head.SetBlendShapeWeight((int)Idx.EL_lower_r, p.EL_lower_r);
        head.SetBlendShapeWeight((int)Idx.Nose_up_l, p.Nose_up_l);
        head.SetBlendShapeWeight((int)Idx.Nose_up_r, p.Nose_up_r);
        head.SetBlendShapeWeight((int)Idx.smile_l, p.smile_l);
        head.SetBlendShapeWeight((int)Idx.smile_r, p.smile_r);
        head.SetBlendShapeWeight((int)Idx.sad_l, p.sad_l);
        head.SetBlendShapeWeight((int)Idx.sad_r, p.sad_r);
        head.SetBlendShapeWeight((int)Idx.Lip_up_l, p.Lip_up_l);
        head.SetBlendShapeWeight((int)Idx.Lip_up_r, p.Lip_up_r);
    }

    public Pose getLerpPose(Pose a, Pose b, float t)
    {
        Pose result;

        result.mouth_A = Mathf.Lerp(a.mouth_A, b.mouth_A, t);
        result.jaw_left = Mathf.Lerp(a.jaw_left, b.jaw_left, t);
        result.jaw_right = Mathf.Lerp(a.jaw_right, b.jaw_right, t);
        result.mouth_O = Mathf.Lerp(a.mouth_O, b.mouth_O, t);
        result.mouth_E = Mathf.Lerp(a.mouth_E, b.mouth_E, t);
        result.mouth_M = Mathf.Lerp(a.mouth_M, b.mouth_M, t);
        result.mouth_F = Mathf.Lerp(a.mouth_F, b.mouth_F, t);
        result.mouth_forward = Mathf.Lerp(a.mouth_forward, b.mouth_forward, t);
        result.EB_angry_l = Mathf.Lerp(a.EB_angry_l, b.EB_angry_l, t);
        result.EB_angry_r = Mathf.Lerp(a.EB_angry_r, b.EB_angry_r, t);
        result.EB_sad_l = Mathf.Lerp(a.EB_sad_l, b.EB_sad_l, t);
        result.EB_sad_r = Mathf.Lerp(a.EB_sad_r, b.EB_sad_r, t);
        result.EB_raised_l = Mathf.Lerp(a.EB_raised_l, b.EB_raised_l, t);
        result.EB_raised_r = Mathf.Lerp(a.EB_raised_r, b.EB_raised_r, t);
        result.EB_corner_l = Mathf.Lerp(a.EB_corner_l, b.EB_corner_l, t);
        result.EB_corner_r = Mathf.Lerp(a.EB_corner_r, b.EB_corner_r, t);
        result.EL_upper_up_l = Mathf.Lerp(a.EL_upper_up_l, b.EL_upper_up_l, t);
        result.EL_upper_up_r = Mathf.Lerp(a.EL_upper_up_r, b.EL_upper_up_r, t);
        result.EL_upper_closed_l = Mathf.Lerp(a.EL_upper_closed_l, b.EL_upper_closed_l, t);
        result.EL_upper_closed_r = Mathf.Lerp(a.EL_upper_closed_r, b.EL_upper_closed_r, t);
        result.EL_lower_l = Mathf.Lerp(a.EL_lower_l, b.EL_lower_l, t);
        result.EL_lower_r = Mathf.Lerp(a.EL_lower_r, b.EL_lower_r, t);
        result.Nose_up_l = Mathf.Lerp(a.Nose_up_l, b.Nose_up_l, t);
        result.Nose_up_r = Mathf.Lerp(a.Nose_up_r, b.Nose_up_r, t);
        result.smile_l = Mathf.Lerp(a.smile_l, b.smile_l, t);
        result.smile_r = Mathf.Lerp(a.smile_r, b.smile_r, t);
        result.sad_l = Mathf.Lerp(a.sad_l, b.sad_l, t);
        result.sad_r = Mathf.Lerp(a.sad_r, b.sad_r, t);
        result.Lip_up_l = Mathf.Lerp(a.Lip_up_l, b.Lip_up_l, t);
        result.Lip_up_r = Mathf.Lerp(a.Lip_up_r, b.Lip_up_r, t);

        return result;
    }

    public Pose getAveragePose(List<Pose> poses, List<float> weights)
    {
        Pose result = new Pose();

        float weitghsum = 0;
        for (int i = 0; i < weights.Count; i++)
            weitghsum += weights[i];

        if (weitghsum == 0)
        {
            result.init();
            return result;
        }
        float sum = 0;

        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].mouth_A * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.mouth_A = 0;
                else result.mouth_A = sum / weitghsum;
                sum = 0;
            }
        }

        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].jaw_left * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.jaw_left = 0;
                else result.jaw_left = sum / weitghsum;
                sum = 0;
            }
        }

        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].jaw_right * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.jaw_right = 0;
                else result.jaw_right = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].mouth_O * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.mouth_O = 0;
                else result.mouth_O = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].mouth_E * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.mouth_E = 0;
                else result.mouth_E = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].mouth_M * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.mouth_M = 0;
                else result.mouth_M = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].mouth_F * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.mouth_F = 0;
                else result.mouth_F = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].mouth_forward * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.mouth_forward = 0;
                else result.mouth_forward = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].EB_angry_l * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.EB_angry_l = 0;
                else result.EB_angry_l = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].EB_angry_r * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.EB_angry_r = 0;
                else result.EB_angry_r = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].EB_sad_l * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.EB_sad_l = 0;
                else result.EB_sad_l = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].EB_sad_r * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.EB_sad_r = 0;
                else result.EB_sad_r = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].EB_raised_l * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.EB_raised_l = 0;
                else result.EB_raised_l = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].EB_raised_r * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.EB_raised_r = 0;
                else result.EB_raised_r = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].EB_corner_l * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.EB_corner_l = 0;
                else result.EB_corner_l = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].EB_corner_r * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.EB_corner_r = 0;
                else result.EB_corner_r = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].EL_upper_up_l * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.EL_upper_up_l = 0;
                else result.EL_upper_up_l = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].EL_upper_up_r * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.EL_upper_up_r = 0;
                else result.EL_upper_up_r = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].EL_upper_closed_l * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.EL_upper_closed_l = 0;
                else result.EL_upper_closed_l = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].EL_upper_closed_r * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.EL_upper_closed_r = 0;
                else result.EL_upper_closed_r = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].EL_lower_l * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.EL_lower_l = 0;
                else result.EL_lower_l = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].EL_lower_r * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.EL_lower_r = 0;
                else result.EL_lower_r = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].Nose_up_l * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.Nose_up_l = 0;
                else result.Nose_up_l = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].Nose_up_r * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.Nose_up_r = 0;
                else result.Nose_up_r = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].smile_l * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.smile_l = 0;
                else result.smile_l = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].smile_r * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.smile_r = 0;
                else result.smile_r = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].sad_l * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.sad_l = 0;
                else result.sad_l = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].sad_r * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.sad_r = 0;
                else result.sad_r = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].Lip_up_l * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.Lip_up_l = 0;
                else result.Lip_up_l = sum / weitghsum;
                sum = 0;
            }
        }
        for (int i = 0; i < poses.Count; i++)
        {
            sum += (poses[i].Lip_up_r * weights[i]);
            if (i == poses.Count - 1) // ultima iteração
            {
                if (sum == 0) result.Lip_up_r = 0;
                else result.Lip_up_r = sum / weitghsum;
                sum = 0;
            }
        }

        return result;
    }

    // soma dos pesos deve ser = 1.0f
    public Pose getAveragePose(Pose a, Pose b, float a_weight, float b_weight)
    {
        Pose result = new Pose();
        if (a_weight + b_weight == 0)
        {
            result.init();
            return result;
        }

        float sum = (a.mouth_A * a_weight + b.mouth_A * b_weight);
        if (sum == 0) result.mouth_A = 0; else result.mouth_A = sum / (a_weight + b_weight);

        sum = (a.jaw_left * a_weight + b.jaw_left * b_weight);
        if (sum == 0) result.jaw_left = 0; else result.jaw_left = sum / (a_weight + b_weight);

        sum = (a.jaw_right * a_weight + b.jaw_right * b_weight);
        if (sum == 0) result.jaw_right = 0; else result.jaw_right = sum / (a_weight + b_weight);

        sum = (a.mouth_O * a_weight + b.mouth_O * b_weight);
        if (sum == 0) result.mouth_O = 0; else result.mouth_O = sum / (a_weight + b_weight);

        sum = (a.mouth_E * a_weight + b.mouth_E * b_weight);
        if (sum == 0) result.mouth_E = 0; else result.mouth_E = sum / (a_weight + b_weight);

        sum = (a.mouth_M * a_weight + b.mouth_M * b_weight);
        if (sum == 0) result.mouth_M = 0; else result.mouth_M = sum / (a_weight + b_weight);

        sum = (a.mouth_F * a_weight + b.mouth_F * b_weight);
        if (sum == 0) result.mouth_F = 0; else result.mouth_F = sum / (a_weight + b_weight);

        sum = (a.mouth_forward * a_weight + b.mouth_forward * b_weight);
        if (sum == 0) result.mouth_forward = 0; else result.mouth_forward = sum / (a_weight + b_weight);

        sum = (a.EB_angry_l * a_weight + b.EB_angry_l * b_weight);
        if (sum == 0) result.EB_angry_l = 0; else result.EB_angry_l = sum / (a_weight + b_weight);

        sum = (a.EB_angry_r * a_weight + b.EB_angry_r * b_weight);
        if (sum == 0) result.EB_angry_r = 0; else result.EB_angry_r = sum / (a_weight + b_weight);

        sum = (a.EB_sad_l * a_weight + b.EB_sad_l * b_weight);
        if (sum == 0) result.EB_sad_l = 0; else result.EB_sad_l = sum / (a_weight + b_weight);

        sum = (a.EB_sad_r * a_weight + b.EB_sad_r * b_weight);
        if (sum == 0) result.EB_sad_r = 0; else result.EB_sad_r = sum / (a_weight + b_weight);

        sum = (a.EB_raised_l * a_weight + b.EB_raised_l * b_weight);
        if (sum == 0) result.EB_raised_l = 0; else result.EB_raised_l = sum / (a_weight + b_weight);

        sum = (a.EB_raised_r * a_weight + b.EB_raised_r * b_weight);
        if (sum == 0) result.EB_raised_r = 0; else result.EB_raised_r = sum / (a_weight + b_weight);

        sum = (a.EB_corner_l * a_weight + b.EB_corner_l * b_weight);
        if (sum == 0) result.EB_corner_l = 0; else result.EB_corner_l = sum / (a_weight + b_weight);

        sum = (a.EB_corner_r * a_weight + b.EB_corner_r * b_weight);
        if (sum == 0) result.EB_corner_r = 0; else result.EB_corner_r = sum / (a_weight + b_weight);

        sum = (a.EL_upper_up_l * a_weight + b.EL_upper_up_l * b_weight);
        if (sum == 0) result.EL_upper_up_l = 0; else result.EL_upper_up_l = sum / (a_weight + b_weight);

        sum = (a.EL_upper_up_r * a_weight + b.EL_upper_up_r * b_weight);
        if (sum == 0) result.EL_upper_up_r = 0; else result.EL_upper_up_r = sum / (a_weight + b_weight);

        sum = (a.EL_upper_closed_l * a_weight + b.EL_upper_closed_l * b_weight);
        if (sum == 0) result.EL_upper_closed_l = 0; else result.EL_upper_closed_l = sum / (a_weight + b_weight);

        sum = (a.EL_upper_closed_r * a_weight + b.EL_upper_closed_r * b_weight);
        if (sum == 0) result.EL_upper_closed_r = 0; else result.EL_upper_closed_r = sum / (a_weight + b_weight);

        sum = (a.EL_lower_l * a_weight + b.EL_lower_l * b_weight);
        if (sum == 0) result.EL_lower_l = 0; else result.EL_lower_l = sum / (a_weight + b_weight);

        sum = (a.EL_lower_r * a_weight + b.EL_lower_r * b_weight);
        if (sum == 0) result.EL_lower_r = 0; else result.EL_lower_r = sum / (a_weight + b_weight);

        sum = (a.Nose_up_l * a_weight + b.Nose_up_l * b_weight);
        if (sum == 0) result.Nose_up_l = 0; else result.Nose_up_l = sum / (a_weight + b_weight);

        sum = (a.Nose_up_r * a_weight + b.Nose_up_r * b_weight);
        if (sum == 0) result.Nose_up_r = 0; else result.Nose_up_r = sum / (a_weight + b_weight);

        sum = (a.smile_l * a_weight + b.smile_l * b_weight);
        if (sum == 0) result.smile_l = 0; else result.smile_l = sum / (a_weight + b_weight);

        sum = (a.smile_r * a_weight + b.smile_r * b_weight);
        if (sum == 0) result.smile_r = 0; else result.smile_r = sum / (a_weight + b_weight);

        sum = (a.sad_l * a_weight + b.sad_l * b_weight);
        if (sum == 0) result.sad_l = 0; else result.sad_l = sum / (a_weight + b_weight);

        sum = (a.sad_r * a_weight + b.sad_r * b_weight);
        if (sum == 0) result.sad_r = 0; else result.sad_r = sum / (a_weight + b_weight);

        sum = (a.Lip_up_l * a_weight + b.Lip_up_l * b_weight);
        if (sum == 0) result.Lip_up_l = 0; else result.Lip_up_l = sum / (a_weight + b_weight);

        sum = (a.Lip_up_r * a_weight + b.Lip_up_r * b_weight);
        if (sum == 0) result.Lip_up_r = 0; else result.Lip_up_r = sum / (a_weight + b_weight);

        return result;
    }

    private void handleInitEndPoses()
    {
        Pose p = new Pose();

        // ECSTASY
        if (ecstasy <= 0.3f)
        {
            p.init();
            basic_poses_init[(int)EmotionIdx.ecstasy] = p;
            basic_poses_end[(int)EmotionIdx.ecstasy] = serenity_pose;
        }
        else if (ecstasy > 0.3f && ecstasy <= 0.6f)
        {
            basic_poses_init[(int)EmotionIdx.ecstasy] = serenity_pose;
            basic_poses_end[(int)EmotionIdx.ecstasy] = joy_pose;
        }
        else if (ecstasy > 0.6f)
        {
            basic_poses_init[(int)EmotionIdx.ecstasy] = joy_pose;
            basic_poses_end[(int)EmotionIdx.ecstasy] = ecstasy_pose;
        }

        //ADMIRATION
        if (admiration <= 0.3f)
        {
            p.init();
            basic_poses_init[(int)EmotionIdx.admiration] = p;
            basic_poses_end[(int)EmotionIdx.admiration] = acceptance_pose;
        }
        else if (admiration > 0.3f && admiration <= 0.6f)
        {
            basic_poses_init[(int)EmotionIdx.admiration] = acceptance_pose;
            basic_poses_end[(int)EmotionIdx.admiration] = trust_pose;
        }
        else if (admiration > 0.6f)
        {
            basic_poses_init[(int)EmotionIdx.admiration] = trust_pose;
            basic_poses_end[(int)EmotionIdx.admiration] = admiration_pose;
        }

        // TERROR
        if (terror <= 0.3f)
        {
            p.init();
            basic_poses_init[(int)EmotionIdx.terror] = p;
            basic_poses_end[(int)EmotionIdx.terror] = apprehension_pose;
        }
        else if (terror > 0.3f && terror <= 0.6f)
        {
            basic_poses_init[(int)EmotionIdx.terror] = apprehension_pose;
            basic_poses_end[(int)EmotionIdx.terror] = fear_pose;
        }
        else if (terror > 0.6f)
        {
            basic_poses_init[(int)EmotionIdx.terror] = fear_pose;
            basic_poses_end[(int)EmotionIdx.terror] = terror_pose;
        }

        //AMAZEMENT
        if (amazement <= 0.3f)
        {
            p.init();
            basic_poses_init[(int)EmotionIdx.amazement] = p;
            basic_poses_end[(int)EmotionIdx.amazement] = distraction_pose;
        }
        else if (amazement > 0.3f && amazement <= 0.6f)
        {
            basic_poses_init[(int)EmotionIdx.amazement] = distraction_pose;
            basic_poses_end[(int)EmotionIdx.amazement] = surprise_pose;
        }
        else if (amazement > 0.6f)
        {
            basic_poses_init[(int)EmotionIdx.amazement] = surprise_pose;
            basic_poses_end[(int)EmotionIdx.amazement] = amazement_pose;
        }

        // GRIEF
        if (grief <= 0.3f)
        {
            p.init();
            basic_poses_init[(int)EmotionIdx.grief] = p;
            basic_poses_end[(int)EmotionIdx.grief] = pensiveness_pose;
        }
        else if (grief > 0.3f && grief <= 0.6f)
        {
            basic_poses_init[(int)EmotionIdx.grief] = pensiveness_pose;
            basic_poses_end[(int)EmotionIdx.grief] = sadness_pose;
        }
        else if (grief > 0.6f)
        {
            basic_poses_init[(int)EmotionIdx.grief] = sadness_pose;
            basic_poses_end[(int)EmotionIdx.grief] = grief_pose;
        }

        //LOATHING
        if (loathing <= 0.3f)
        {
            p.init();
            basic_poses_init[(int)EmotionIdx.loathing] = p;
            basic_poses_end[(int)EmotionIdx.loathing] = boredom_pose;
        }
        else if (loathing > 0.3f && loathing <= 0.6f)
        {
            basic_poses_init[(int)EmotionIdx.loathing] = boredom_pose;
            basic_poses_end[(int)EmotionIdx.loathing] = disgust_pose;
        }
        else if (loathing > 0.6f)
        {
            basic_poses_init[(int)EmotionIdx.loathing] = disgust_pose;
            basic_poses_end[(int)EmotionIdx.loathing] = loathing_pose;
        }

        // RAGE
        if (rage <= 0.3f)
        {
            p.init();
            basic_poses_init[(int)EmotionIdx.rage] = p;
            basic_poses_end[(int)EmotionIdx.rage] = annoyance_pose;
        }
        else if (rage > 0.3f && rage <= 0.6f)
        {
            basic_poses_init[(int)EmotionIdx.rage] = annoyance_pose;
            basic_poses_end[(int)EmotionIdx.rage] = anger_pose;
        }
        else if (rage > 0.6f)
        {
            basic_poses_init[(int)EmotionIdx.rage] = anger_pose;
            basic_poses_end[(int)EmotionIdx.rage] = rage_pose;
        }

        //VIGILANCE
        if (vigilance <= 0.3f)
        {
            p.init();
            basic_poses_init[(int)EmotionIdx.vigilance] = p;
            basic_poses_end[(int)EmotionIdx.vigilance] = interest_pose;
        }
        else if (vigilance > 0.3f && vigilance <= 0.6f)
        {
            basic_poses_init[(int)EmotionIdx.vigilance] = interest_pose;
            basic_poses_end[(int)EmotionIdx.vigilance] = anticipation_pose;
        }
        else if (vigilance > 0.6f)
        {
            basic_poses_init[(int)EmotionIdx.vigilance] = anticipation_pose;
            basic_poses_end[(int)EmotionIdx.vigilance] = vigilance_pose;
        }
    }

    private void handleSlidersValues()
    {
        weights[(int)EmotionIdx.ecstasy] = ecstasy;
        weights[(int)EmotionIdx.admiration] = admiration;
        weights[(int)EmotionIdx.terror] = terror;
        weights[(int)EmotionIdx.amazement] = amazement;
        weights[(int)EmotionIdx.grief] = grief;
        weights[(int)EmotionIdx.loathing] = loathing;
        weights[(int)EmotionIdx.rage] = rage;
        weights[(int)EmotionIdx.vigilance] = vigilance;
    }

    private void initPoses()
    {
        current_pose.init();

        serenity_pose.init();

        serenity_pose.mouth_E = 20f;
        serenity_pose.EL_upper_closed_l = 80f;
        serenity_pose.EL_upper_closed_r = 80f;
        serenity_pose.smile_l = 50f;
        serenity_pose.smile_r = 50f;

        joy_pose.init();

        joy_pose.mouth_O = 30f;
        joy_pose.EB_raised_l = 30f;
        joy_pose.EB_raised_r = 30f;
        joy_pose.EL_lower_l = 30f;
        joy_pose.EL_lower_r = 30f;
        joy_pose.smile_l = 100f;
        joy_pose.smile_r = 100f;

        ecstasy_pose.init();

        ecstasy_pose.mouth_O = 100f;
        ecstasy_pose.mouth_E = 100f;
        ecstasy_pose.EB_raised_l = 30f;
        ecstasy_pose.EB_raised_r = 30f;
        ecstasy_pose.EB_corner_l = 20f;
        ecstasy_pose.EB_corner_r = 20f;
        ecstasy_pose.EL_lower_l = 100f;
        ecstasy_pose.EL_lower_r = 100f;
        ecstasy_pose.smile_l = 100f;
        ecstasy_pose.smile_r = 100f;

        acceptance_pose.init();

        acceptance_pose.mouth_E = 80f;
        acceptance_pose.EL_lower_l = 30f;
        acceptance_pose.EL_lower_r = 30f;
        acceptance_pose.smile_l = 20f;
        acceptance_pose.smile_r = 20f;

        trust_pose.init();

        trust_pose.EB_raised_l = 10f;
        trust_pose.EB_raised_r = 10f;
        trust_pose.EL_lower_l = 33f;
        trust_pose.EL_lower_r = 33f;
        trust_pose.smile_l = 33f;
        trust_pose.smile_r = 33f;

        admiration_pose.init();

        admiration_pose.mouth_O = 60f;
        admiration_pose.mouth_E = 30f;
        admiration_pose.mouth_forward = 80f;
        admiration_pose.EB_raised_l = 15f;
        admiration_pose.EB_raised_r = 15f;
        admiration_pose.EL_lower_l = 33f;
        admiration_pose.EL_lower_r = 33f;
        admiration_pose.smile_l = 60f;
        admiration_pose.smile_r = 60f;
        admiration_pose.EL_upper_up_l = 80f;
        admiration_pose.EL_upper_up_r = 80f;
        admiration_pose.EB_corner_l = 10f;
        admiration_pose.EB_corner_r = 10f;

        terror_pose.init();

        terror_pose.mouth_A = 100f;
        terror_pose.EB_raised_l = 100f;
        terror_pose.EB_raised_r = 100f;
        terror_pose.Nose_up_l = 30f;
        terror_pose.Nose_up_r = 30f;
        terror_pose.sad_l = 30f;
        terror_pose.sad_r = 30f;

        fear_pose.init();

        fear_pose.mouth_A = 30f;
        fear_pose.mouth_F = 50f;
        fear_pose.EB_raised_l = 50f;
        fear_pose.EB_raised_r = 50f;
        fear_pose.Nose_up_l = 10f;
        fear_pose.Nose_up_r = 10f;
        fear_pose.sad_l = 40f;
        fear_pose.sad_r = 40f;

        apprehension_pose.init();

        apprehension_pose.mouth_F = 50f;
        apprehension_pose.EB_raised_l = 30f;
        apprehension_pose.EB_raised_r = 30f;
        apprehension_pose.sad_l = 40f;
        apprehension_pose.sad_r = 40f;

        amazement_pose.init();

        amazement_pose.mouth_O = 70f;
        amazement_pose.mouth_E = 20f;
        amazement_pose.EB_raised_l = 20f;
        amazement_pose.EB_raised_r = 20f;
        amazement_pose.EB_corner_l = 20f;
        amazement_pose.EB_corner_r = 20f;
        amazement_pose.EL_upper_up_l = 50f;
        amazement_pose.EL_upper_up_r = 50f;
        amazement_pose.smile_l = 100f;
        amazement_pose.smile_r = 100f;

        surprise_pose.init();

        surprise_pose.mouth_O = 70f;
        surprise_pose.mouth_E = 20f;
        surprise_pose.EB_raised_l = 20f;
        surprise_pose.EB_raised_r = 20f;
        surprise_pose.EB_corner_l = 30f;
        surprise_pose.EB_corner_r = 30f;
        surprise_pose.EL_upper_up_l = 50f;
        surprise_pose.EL_upper_up_r = 50f;
        surprise_pose.smile_l = 20f;
        surprise_pose.smile_r = 20f;

        distraction_pose.init();

        distraction_pose.EB_raised_l = 80f;
        distraction_pose.EB_raised_r = 30f;

        pensiveness_pose.init();

        pensiveness_pose.jaw_right = 50f;
        pensiveness_pose.EB_raised_l = 20f;
        pensiveness_pose.EB_raised_r = 20f;
        pensiveness_pose.EB_corner_l = 40f;
        pensiveness_pose.EL_lower_r = 100f;

        sadness_pose.init();

        sadness_pose.EB_sad_l = 60f;
        sadness_pose.EB_sad_r = 60f;
        sadness_pose.sad_l = 60f;
        sadness_pose.sad_r = 60f;

        grief_pose.init();

        grief_pose.mouth_A = 60f;
        grief_pose.mouth_forward = 50f;
        grief_pose.EB_sad_l = 100f;
        grief_pose.EB_sad_r = 100f;
        grief_pose.sad_l = 100f;
        grief_pose.sad_r = 100f;

        boredom_pose.init();

        boredom_pose.jaw_left = 70f;
        boredom_pose.mouth_M = 20f;
        boredom_pose.mouth_F = 20f;
        boredom_pose.EB_sad_l = 10f;
        boredom_pose.EB_sad_r = 10f;
        boredom_pose.EB_raised_l = 20f;
        boredom_pose.EL_upper_closed_l = 10f;
        boredom_pose.EL_upper_closed_r = 10f;
        boredom_pose.sad_l = 20f;
        boredom_pose.sad_r = 20f;

        disgust_pose.init();

        disgust_pose.EB_raised_l = 10f;
        disgust_pose.EB_raised_r = 10f;
        disgust_pose.EL_upper_closed_l = 10f;
        disgust_pose.EL_lower_l = 50f;
        disgust_pose.EL_lower_r = 50f;
        disgust_pose.Nose_up_l = 80f;
        disgust_pose.Lip_up_l = 70f;

        loathing_pose.init();

        loathing_pose.mouth_A = 30f;
        loathing_pose.mouth_F = 60f;
        loathing_pose.EB_raised_l = 30f;
        loathing_pose.EB_raised_r = 30f;
        loathing_pose.EB_corner_l = 50f;
        loathing_pose.EL_upper_closed_l = 10f;
        loathing_pose.Nose_up_l = 80f;
        loathing_pose.Nose_up_r = 50f;
        loathing_pose.Lip_up_l = 100f;
        loathing_pose.Lip_up_r = 100f;

        annoyance_pose.init();

        annoyance_pose.jaw_left = 50f;
        annoyance_pose.mouth_E = 40f;
        annoyance_pose.mouth_M = 50f;
        annoyance_pose.mouth_F = 50f;
        annoyance_pose.EB_angry_l = 50f;
        annoyance_pose.EB_angry_r = 50f;
        annoyance_pose.EB_raised_l = 10f;
        annoyance_pose.EB_raised_r = 10f;
        annoyance_pose.EB_corner_l = 50f;
        annoyance_pose.EB_corner_r = 10f;
        annoyance_pose.EL_upper_up_l = 30f;
        annoyance_pose.EL_upper_up_r = 30f;
        annoyance_pose.EL_upper_closed_r = 10f;
        annoyance_pose.EL_lower_l = 20f;
        annoyance_pose.EL_lower_r = 20f;
        annoyance_pose.Nose_up_l = 50f;
        annoyance_pose.Nose_up_r = 50f;
        annoyance_pose.sad_l = 50f;
        annoyance_pose.sad_r = 50f;
        annoyance_pose.Lip_up_l = 80f;

        anger_pose.init();

        anger_pose.jaw_left = 20f;
        anger_pose.mouth_E = 40f;
        anger_pose.mouth_M = 50f;
        anger_pose.mouth_F = 50f;
        anger_pose.EB_angry_l = 50f;
        anger_pose.EB_angry_r = 50f;
        anger_pose.EB_corner_l = 30f;
        anger_pose.EB_corner_r = 30f;
        anger_pose.EL_upper_up_l = 60f;
        anger_pose.EL_upper_up_r = 60f;
        anger_pose.Nose_up_l = 100f;
        anger_pose.Nose_up_r = 100f;
        anger_pose.sad_l = 50f;
        anger_pose.sad_r = 50f;
        anger_pose.Lip_up_l = 50f;
        anger_pose.Lip_up_r = 50f;

        rage_pose.init();

        rage_pose.mouth_A = 50f;
        rage_pose.mouth_E = 50f;
        rage_pose.mouth_F = 100f;
        rage_pose.EB_angry_l = 100f;
        rage_pose.EB_angry_r = 100f;
        rage_pose.EB_corner_l = 30f;
        rage_pose.EB_corner_r = 30f;
        rage_pose.EL_upper_up_l = 100f;
        rage_pose.EL_upper_up_r = 100f;
        rage_pose.Nose_up_l = 100f;
        rage_pose.Nose_up_r = 100f;
        rage_pose.sad_l = 50f;
        rage_pose.sad_r = 50f;
        rage_pose.Lip_up_l = 30f;
        rage_pose.Lip_up_r = 30f;

        interest_pose.init();

        interest_pose.mouth_A = 5f;
        interest_pose.mouth_O = 10f;
        interest_pose.mouth_E = 10f;
        interest_pose.mouth_F = 50f;
        interest_pose.mouth_forward = 10f;
        interest_pose.EB_angry_l = 20f;
        interest_pose.EB_angry_r = 20f;
        interest_pose.EB_raised_l = 45f;
        interest_pose.EB_raised_r = 55f;
        interest_pose.EB_corner_l = 10f;
        interest_pose.EB_corner_r = 10f;
        interest_pose.EL_upper_up_l = 20f;
        interest_pose.EL_upper_up_r = 20f;
        interest_pose.smile_l = 20f;
        interest_pose.smile_l = 20f;
        interest_pose.sad_l = 20f;
        interest_pose.sad_r = 20f;

        anticipation_pose.init();

        anticipation_pose.jaw_right = 50f;
        anticipation_pose.mouth_M = 50f;
        anticipation_pose.mouth_F = 100f;
        anticipation_pose.mouth_forward = 10f;
        anticipation_pose.EB_angry_l = 10f;
        anticipation_pose.EB_angry_r = 10f;
        anticipation_pose.EB_corner_l = 10f;
        anticipation_pose.EB_corner_r = 10f;
        anticipation_pose.EB_raised_l = 10f;
        anticipation_pose.EB_raised_r = 10f;
        anticipation_pose.EB_sad_l = 10f;
        anticipation_pose.EB_sad_r = 10f;
        anticipation_pose.smile_l = 20f;
        anticipation_pose.smile_r = 20f;
        anticipation_pose.sad_l = 20f;
        anticipation_pose.sad_r = 20f;

        vigilance_pose.init();

        vigilance_pose.mouth_M = 50f;
        vigilance_pose.mouth_F = 100f;
        vigilance_pose.mouth_forward = 50f;
        vigilance_pose.EB_angry_l = 10f;
        vigilance_pose.EB_angry_r = 10f;
        vigilance_pose.EB_corner_l = 10f;
        vigilance_pose.EB_corner_r = 10f;
        vigilance_pose.EL_lower_l = 80f;
        vigilance_pose.EL_lower_r = 80f;
    }
}