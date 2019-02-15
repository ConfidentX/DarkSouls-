using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 角色管理器
/// </summary>
public class ActorManager : MonoBehaviour {
    public PlayerController player;
    public BattleManager bm;
    public WeaponManager wm;
    public StateManager sm;
	void Awake () {
        player = GetComponent<PlayerController>();
        //找到sensor子物体
        GameObject sensor = transform.Find("sensor").gameObject;
        GameObject model = player.model;
        bm = Bind<BattleManager>(sensor);
        wm = Bind<WeaponManager>(model);
        sm = Bind<StateManager>(gameObject);
        
	}
    private T Bind<T>(GameObject go)where T : IActorManagerInterface
    {
        T tempInstance;
        tempInstance = go.GetComponent<T>();
        if (tempInstance == null)
        {
            tempInstance = go.AddComponent<T>();
        }
        tempInstance.am = this;
        return tempInstance;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetIsCounterBack(bool value)
    {
        sm.isCounterBackEnable = value;
    }

    public void TryDoDamage(WeaponController targetWc,bool attackValid,bool counterValid)
    {
        if (sm.isCounterBackSuccess)
        {
            if (counterValid)
            {
                targetWc.wm.am.Stunned();
            }
            
        }
        else if (sm.isCounterBackFailure)
        {
            if (attackValid)
            {
                HitorDie(false);
            }
            
        }
        else if (sm.isImmortal)//无敌
        {
            //Do Nothing;
        }

        else if (sm.isDefense)
        {
            Blocked();
        }
        else
        {
            if (attackValid)
            {
                HitorDie(true);
            }
           
        }       
    }
    public void HitorDie(bool doHitAnimation)
    {
        if (sm.Hp <= 0)
        {

        }
        else
        {
            sm.AddHP(-5);
            if (sm.Hp > 0)
            {
                if (doHitAnimation)
                {
                    Hit();
                }
                //do some VFX,like blood....
            }
            else
            {
                Die();
            }
        }
    }
    public void Stunned()
    {
        player.IssueTrigger("stunned");
    }
    public void Blocked()
    {
        player.IssueTrigger("blocked");
    }
    public void Hit()
    {
        player.IssueTrigger("hit");
    }
    public void Die()
    {
        player.IssueTrigger("die");
        player.pi.inputEnabled = false;
        if (player.camcon.lockState == true)
        {           
            player.camcon.LockUnlock();           
        }
        player.camcon.enabled = false;
        
    }
}
