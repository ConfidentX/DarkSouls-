using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//第三段攻击的位移信号
public class RootMotionControl : MonoBehaviour {

    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpdateRM",(object)anim.deltaPosition);
    }
}
