using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : IActorManagerInterface {
    public Collider weaponColL;
    public Collider weaponColR;
    public GameObject whL;
    public GameObject whR;
    public WeaponController wcL;
    public WeaponController wcR;

   // public ActorManager am;

    private void Start()
    {
        whL = transform.DeepFind("weaponHandleL").gameObject;
        whR = transform.DeepFind("weaponHandleR").gameObject;

        wcL = BindWeaponController(whL);
        wcR = BindWeaponController(whR);


        weaponColL = whL.GetComponentInChildren<Collider>();
        weaponColR = whR.GetComponentInChildren<Collider>();
    }
    public WeaponController BindWeaponController(GameObject targetObj)
    {
        WeaponController tempWc;
        tempWc = targetObj.GetComponent<WeaponController>();
        if (tempWc == null)
        {
           tempWc=targetObj.AddComponent<WeaponController>();
        }
        tempWc.wm = this;

        return tempWc;
    }


    public void WeaponEnable()
    {
        if (am.player.CheckStateTag("attackL"))
        {
            weaponColL.enabled = true;
        }
        else
        {
            weaponColR.enabled = true;
        }
        
    }
    public void WeaponDisable()
    {
        weaponColL.enabled = false;
        weaponColR.enabled = false;
    }
    public void CounterBackEnable()
    {
        am.SetIsCounterBack(true);
    }
    public void CounterBackDisable()
    {
        am.SetIsCounterBack(false);
    }
}
