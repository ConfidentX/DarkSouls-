using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//IK
public class LeftArmAnimFix : MonoBehaviour {
    private Animator anim;
    public Vector3 a;
    private PlayerController player;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponentInParent<PlayerController>();
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (player.leftIsShield)
        {
            //没有举盾的时候进行修正
            if (anim.GetBool("defense") == false)
            {
                //抓取得左下臂的骨骼
                Transform leftArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
                leftArm.localEulerAngles += a;
                anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftArm.localEulerAngles));
            }
        }
       
        
    }
}
