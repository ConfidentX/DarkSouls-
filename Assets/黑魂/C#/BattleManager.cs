using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 添加该脚本时加一个防御碰撞框
/// 战斗管理器
/// </summary>
[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : IActorManagerInterface {

    private CapsuleCollider defCol;
    //public ActorManager am;

    private void Start()
    {
        defCol = GetComponent<CapsuleCollider>();
        defCol.center = Vector3.up * 1.0f;
        defCol.height = 2.0f;
        defCol.radius = 0.5f;
        defCol.isTrigger = true;
    }
    private void OnTriggerEnter(Collider col)
    {
        WeaponController targetWc = col.GetComponentInParent<WeaponController>();
        GameObject attacker = targetWc.wm.am.gameObject;
        GameObject receiver = am.gameObject;
        Vector3 attackingDir = receiver.transform.position - attacker.transform.position;
        Vector3 counterDir = attacker.transform.position - receiver.transform.position;
        float attackingAngle = Vector3.Angle(attacker.transform.forward, attackingDir);
        float counterAngle_1 = Vector3.Angle(receiver.transform.forward, counterDir);
        float counterAngle_2 = Vector3.Angle(attacker.transform.forward, receiver.transform.forward);//should be closed to 180
        bool attackValid = (attackingAngle < 45);
        bool counterValid = (counterAngle_1 < 30 && Mathf.Abs(counterAngle_2 - 180) < 30);
        if (col.tag == "Weapon")
        {    
            
            am.TryDoDamage(targetWc,attackValid,counterValid);
                         
        }
    }
}
