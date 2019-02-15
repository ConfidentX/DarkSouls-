using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public GameObject model;
    public CameraControl camcon;
    private Animator anim;
    public UserInput pi;
    private Rigidbody rigid;
    //玩家的移动量
    private Vector3 movingVec;
    //冲量向量
    private Vector3 thrustVec;
    //判断是否锁死地面的速度 
    private bool lockPlanar = false;
    //追踪角色方向
    private bool trackDir = false;
    private CapsuleCollider col;
    private float lerpTarget;//attack的目标权重（0，1）
    private Vector3 deltaPos;
    //private float run1Velocity;
    //private float run2Velocity;





    public float walkSpeed=1.0f;
    public float runSpeed;
    public float jumpVelocity;
    public float rollVelocity;
    public float jabVelocity;
    private bool canAttack;
    [Header("=====  Friction Settings  =====")]//材质设置
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;



    public  bool leftIsShield = true;

    // Use this for initialization
    void Awake () {
        anim = model.GetComponent<Animator>();
        pi = GetComponent<UserInput>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
	}
	
	// Update is called once per frame
	void Update ()// 1/60
    {
        float targetRunMulti = ((pi.run) ? 2.0f : 1.0f);
        if (pi.lockon)
        {
            camcon.LockUnlock();
        }
        if (camcon.lockState == false)
        {
            anim.SetFloat("Forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("Forward"), targetRunMulti, 0.5f));
            anim.SetFloat("right", 0);
        }
        else
        {
            Vector3 localDvec = transform.InverseTransformVector(pi.Dvec);
            anim.SetFloat("Forward", localDvec.z * targetRunMulti);
            anim.SetFloat("right", localDvec.x * targetRunMulti);
            anim.SetFloat("move", Mathf.Abs(localDvec.z) + Mathf.Abs(localDvec.x));
        }              
       
        if (pi.roll||rigid.velocity.magnitude>7f)//rigid.velocity.magnitudefan返回速度值
        {
            anim.SetTrigger("Roll");
            canAttack = false;
        }
        if ((pi.rb||pi.lb) && canAttack && (CheckState("Ground") || CheckStateTag("attackR")|| CheckStateTag("attackL"))) 
        {
            if (pi.rb)
            {
                anim.SetBool("mirror", false);
                anim.SetTrigger("Attack");
            }
            else if (pi.lb&&!leftIsShield)
            {
                anim.SetBool("mirror", true);
                anim.SetTrigger("Attack");
            }
            
        }
        if((pi.rt||pi.lt)&&canAttack&&(CheckState("Ground")|| CheckStateTag("attackR") || CheckStateTag("attackL")))
        {
            if (pi.rt)
            {
                //do right heavy attack
            }
            else
            {
                if (!leftIsShield)
                {
                    //do left heavy attack
                }
                else
                {
                    anim.SetTrigger("counterBack");
                }
            }
        }
        if (leftIsShield)
        {
            if (CheckState("Ground")||CheckState("blocked"))
            {
                anim.SetBool("defense", pi.defense);
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), 1);
            }
            else
            {
                anim.SetBool("defense", false);
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
            }
        }
        else
        {
            anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
        }


        if (camcon.lockState == false)
        {
            //缓慢获取运动方向
            if (pi.Dmag > 0.1f)
            {
                //将角色运动方向缓慢转到方向向量
                Vector3 targetForward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);
                //将目标方向赋给角色
                model.transform.forward = targetForward;
            }
            //获取运动速度
            if (lockPlanar == false)
            {
                movingVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run) ? 2.0f : 1.0f);
            }
        }
        else
        {
            if (trackDir == false)
            {
                model.transform.forward = transform.forward;
            }
            else
            {
                model.transform.forward = movingVec.normalized;
            }
            if (lockPlanar == false)
            {
                movingVec = pi.Dvec * walkSpeed * ((pi.run) ? 2.0f : 1.0f);
            }
            
        }
        if (pi.jump)
        {
            anim.SetTrigger("Jump");
            canAttack = false;
        }
        
        
	}
    private void FixedUpdate()// 1/50
    {
        //rigid不能在update里运行
        rigid.position += deltaPos;
        // rigid.position += movingVec * Time.fixedDeltaTime;
        //角色位移，指派movingVec的x，z方向的速度
        rigid.velocity = new Vector3(movingVec.x, rigid.velocity.y, movingVec.z)+thrustVec;
        //动作冲力位移清0
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }
   public  bool CheckState(string stateName,string layerName="Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName);
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
        return result;
    }
    public  bool CheckStateTag(string tagName, string layerName = "Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName);
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsTag(tagName);
        return result;
    }









    /// <summary>
    /// Message processing block,接受处理信息
    /// </summary>
    public void OnJumpEnter()
    {
        // print("onjump");
        pi.inputEnabled = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, jumpVelocity, 0);
        trackDir = true;
    }
    //public void OnJumpExit()
    //{
    //    pi.inputEnabled = true;
    //    lockPlanar = false;
    //}
    public void IsGround()
    {
        //print("is on ground");
        anim.SetBool("IsGround", true);
    }
    public void IsNotGround()
    {
        //print("is not on ground");
        anim.SetBool("IsGround", false);
    }
    public void OnGroundEnter()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
        canAttack = true;
        col.material = frictionOne;
        trackDir = false;
    }
    public void OnGroundExit()
    {
        col.material = frictionZero;
    }
    public void OnFallEnter()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
    }
    public void OnRollEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, rollVelocity, 0);
        trackDir = true;
    }
    public void OnJabEnter()
    {
        thrustVec = model.transform.forward * -jabVelocity;
        pi.inputEnabled = false;
        lockPlanar = true;
    }
    public void OnJabUpdate()
    {
       
        //print(anim.GetFloat("JabVelocity"));
    }

    public void OnAttackEnter1()
    {
        //lockPlanar = true;
        pi.inputEnabled = false;
       // lerpTarget = 1.0f;
        
    }
    public void OnStunnedEnter()
    {
        pi.inputEnabled = false;
        movingVec = Vector3.zero;
    }
    public void OnCounterBackEnter()
    {
        pi.inputEnabled = false;
        movingVec = Vector3.zero;
    }
    public void OnBlockedEnter()
    {
        pi.inputEnabled = false;
    }
    public void OnDieEnter()
    {
        pi.inputEnabled = false;
        movingVec = Vector3.zero;
        model.SendMessage("WeaponDisable");
    }
    public void OnAttackUpdate1()
    {
        thrustVec = model.transform.forward * anim.GetFloat("AttackVelocity");
        //float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("attack"));
        //currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.4f);
        //anim.SetLayerWeight(anim.GetLayerIndex("attack"),currentWeight);

    }
    //public void OnAttackIdleEnter()
    //{
    //    pi.inputEnabled = true;
    //   // lerpTarget = 0;
    //    //lockPlanar = false;
    //    ;//getlayerIndex找到动画的层
    //}
    //public void OnAttackIdleUpdate()
    //{
    //    //float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("attack"));
    //    //currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.4f);
    //    //anim.SetLayerWeight(anim.GetLayerIndex("attack"), currentWeight);
    //}
    public void OnUpdateRM(object _deltaPos)
    {
        if (CheckState("attack3"))
        {
            //让第三段攻击动画的位移显的柔和，对晕3D友好
            deltaPos += (0.8f*deltaPos+0.2f*(Vector3)_deltaPos)/1.0f;
        }
        

    }
    public void OnHitEnter()
    {
        pi.inputEnabled = false;
        movingVec = Vector3.zero;
        model.SendMessage("WeaponDisable");

    }
    public void IssueTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }
    public void OnAttackExit()
    {
        model.SendMessage("WeaponDisable");
    }
}
