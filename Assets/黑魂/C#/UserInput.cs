using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//无法实例化（instance）
public abstract  class UserInput : MonoBehaviour {

    [Header("===== Output signals =====")]
    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Dvec;//旋转
    public float JUp;
    public float JRight;
    public bool run;
    public bool jump;
    protected bool lastJump;
   // public bool attack;
    public bool roll;
    public bool defense;
    public bool lockon;//锁定
    public bool lb;
    public bool lt;
    public bool rb;
    public bool rt;
    protected bool lastAttack;


    [Header("===== Others =====")]
    public bool inputEnabled = true;//判断按键是否按下
    protected float targetDup;
    protected float targetDright;
    protected float velocityDup;//内存空间，无意义
    protected float velocityDright;//内存空间，无意义    
    //方形坐标转换成圆形坐标
    protected Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }
    protected void UpdateDmagDvec(float Dup,float Dright)
    {
        Dmag = Mathf.Sqrt(Dup * Dup + Dright * Dright);
        Dvec = Dright * transform.right + Dup * transform.forward;
    }
}
