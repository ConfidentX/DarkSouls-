using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : IActorManagerInterface {
   // public ActorManager am;
    public float Hp = 15.0f;
    public float HPMax = 15.0f;
    [Header("1st order state flags")]
    public bool isGround;
    public bool isFall;
    public bool isRoll;
    public bool isJab;
    public bool isAttack;
    public bool isHit;
    public bool isDie;
    public bool isBlocked;
    public bool isDefense;
    public bool isCounterBack;       //当前的状态
    public bool isCounterBackEnable; //动画事件是否为真
    [Header("2nd order state flag")]
    public bool isAllowDefense;
    public bool isImmortal;
    public bool isCounterBackSuccess;
    public bool isCounterBackFailure;

    private void Start()
    {
        Hp = HPMax;
    }
    private void Update()
    {
        isGround = am.player.CheckState("Ground");
        isFall = am.player.CheckState("fall");
        isRoll = am.player.CheckState("roll");
        isJab = am.player.CheckState("jab");
        isAttack = am.player.CheckStateTag("attackR") || am.player.CheckStateTag("attackL");
        isHit = am.player.CheckState("hit");
        isDie = am.player.CheckState("die");
        isBlocked = am.player.CheckState("blocked");
        isCounterBack = am.player.CheckState("counterBack");
        isCounterBackSuccess = isCounterBackEnable;
        isCounterBackFailure = isCounterBack && !isCounterBackEnable;
       // isDefense = am.player.CheckState("defense", "defense");      
        isAllowDefense = isGround || isBlocked;
        isDefense = isAllowDefense && am.player.CheckState("defense", "defense");
        isImmortal = isRoll || isJab;
    }

  

    public void AddHP(float value)
    {
       Hp += value;
       Hp= Mathf.Clamp(Hp, 0, HPMax);//截断
       
    }
	
}
