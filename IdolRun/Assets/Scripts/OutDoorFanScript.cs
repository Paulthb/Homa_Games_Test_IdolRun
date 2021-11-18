using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the animation of the fan outside the road
/// </summary>
public class OutDoorFanScript : MonoBehaviour
{
    private Animator anim;
    private Transform idolTargetPos;

    //to make the crowd outside the road more alive, each fan has several type of animation that is choose randomly when they are created
    void Awake()
    {
        anim = GetComponent<Animator>();

        int resultAnim = Random.Range(1, 5);
        anim.SetInteger("AnimChoice", resultAnim);

        idolTargetPos = PlayerScript.Instance.transform;
    }
}
