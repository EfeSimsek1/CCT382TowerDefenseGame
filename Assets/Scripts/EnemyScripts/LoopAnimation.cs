using UnityEngine;

[RequireComponent(typeof(Animation))]
public class LoopAnimation : MonoBehaviour
{
    // Name of the animation clip to loop. If left empty, the default clip will be used.
    public string clipName;

    Animation anim;

    void Awake()
    {
        anim = GetComponent<Animation>();
        if (anim == null) return;

        if (!string.IsNullOrEmpty(clipName) && anim[clipName] != null)
        {
            anim[clipName].wrapMode = WrapMode.Loop;
            anim.Play(clipName);
        }
        else if (anim.clip != null)
        {
            anim[anim.clip.name].wrapMode = WrapMode.Loop;
            anim.Play(anim.clip.name);
        }
    }
}
