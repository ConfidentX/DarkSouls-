using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour {
    public CapsuleCollider capcol;//胶囊体
    private Vector3 point1;//脚底
    private Vector3 point2;//头顶
    private float radius;//半径

    // Use this for initialization
    void Awake () {
        radius = capcol.radius;
        
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        point1 = transform.position + transform.up * radius;
        point2 = transform.position + transform.up * capcol.height - transform.up * radius;
        Collider[] outputCols = Physics.OverlapCapsule(point1, point2, radius,LayerMask.GetMask("Ground"));
        if (outputCols.Length!=0)
        {
            SendMessageUpwards("IsGround");
        }
        else
        {
            SendMessageUpwards("IsNotGround");
        }

    }
}
