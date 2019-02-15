using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardInput : UserInput {
    //按键
    [Header("===== Key Settings=====")]
    public string KeyUP;
    public string KeyDown;
    public string KeyRight;
    public string KeyLeft;
    public string keyA;//space信号
    public string keyB;
    public string keyLT;//右手攻击
    public string keyLB;//左手防御
    public string keyE;//lock信号
    public string keyRT;//左手攻击
    public string keyRB;
    public string keyJRight;
    public string keyJLeft;
    public string keyJUp;
    public string keyJDown;

    public MyButton buttonE = new MyButton();
    public MyButton buttonA = new MyButton();
    public MyButton buttonB = new MyButton();
    public MyButton buttonLT = new MyButton();
    public MyButton buttonLB = new MyButton();
    
    public MyButton buttonRT = new MyButton();
    public MyButton buttonRB = new MyButton();
    
    [Header("===== Mouse Settings =====")]
    public bool mouseEnable = false;
    //鼠标X轴的灵敏度
    public float mouseSensitivityX = 1.0f;
    //鼠标Y轴的灵敏度
    public float mouseSensitivityY = 1.0f;

   //[Header("===== Output signals =====")]
   // public float Dup;
   // public float Dright;
   // public float Dmag;
   // public Vector3 Dvec;//旋转
   // public float JUp;
   // public float JRight;   
   // public bool run;
   // public bool jump;
   // private bool lastJump;
   // public bool attack;
   // private bool lastAttack;

   // [Header("===== Others =====")]
   // public bool inputEnabled = true;//判断按键是否按下
   // private float targetDup;
   // private float targetDright;
   // private float velocityDup;//内存空间，无意义
   // private float velocityDright;//内存空间，无意义    
    


    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        buttonA.Tick(Input.GetKey(keyA));
        buttonB.Tick(Input.GetKey(keyB));
        buttonE.Tick(Input.GetKey(keyE));
        buttonRT.Tick(Input.GetKey(keyRT));
        buttonLT.Tick(Input.GetKey(keyLT));
        buttonRB.Tick(Input.GetKey(keyRB));
        buttonLB.Tick(Input.GetKey(keyLB));


        ///<summary>    
        ///信号的处理
        ///</summary>
        //将数位键值转化为模拟信号（上下左右）
        targetDup = (Input.GetKey(KeyUP) ? 1.0f : 0) - (Input.GetKey(KeyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(KeyRight) ? 1.0f : 0) - (Input.GetKey(KeyLeft) ? 1.0f : 0);
        //如果没有按键，信号为0
        if (inputEnabled == false)
        {
            targetDup = 0;
            targetDright = 0;
        }
        //将信号输出变的柔滑
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);
        //整形方法，方形变圆形
        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;
        //距原点的距离用来调forward的值
        //Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
        //得到运动的方向向量
        // Dvec= Dright2 * transform.right + Dup2 * transform.forward;
        UpdateDmagDvec(Dup2, Dright2);

        if (mouseEnable == true)
        {
            JUp = Input.GetAxis("Mouse Y") * mouseSensitivityY;
            JRight = Input.GetAxis("Mouse X") * mouseSensitivityX;
        }
        else
        {
            JUp = (Input.GetKey(keyJUp) ? 1.0f : 0) - (Input.GetKey(keyJDown) ? 1.0f : 0);
            JRight = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft) ? 1.0f : 0);

        }



        ///<summary>    
        ///按键处理
        ///</summary>
        run = (buttonA.IsPressing&&!buttonA.IsDelaying)||buttonA.IsExtending;                         
        jump = buttonA.OnPressed&&buttonA.IsExtending;
        roll = buttonA.OnReleased && buttonA.IsDelaying;
       // attack = buttonC.OnPressed;       
        lockon = buttonE.OnPressed;
        defense = buttonB.IsPressing;
        rb = buttonRB.OnPressed;       
        lb = buttonLB.OnPressed;
        rt = buttonRT.OnPressed;
        lt = buttonLT.OnPressed;

    }
    //private Vector2 SquareToCircle(Vector2 input)
    //{
    //    Vector2 output = Vector2.zero;
    //    output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
    //    output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
    //    return output;
    //}
    // run = Input.GetKey(keyA);
    //defense = Input.GetKey(keyD);
    //跳跃和翻滚
    //bool newJump = Input.GetKey(keyB);
    //if (newJump != lastJump && newJump==true)
    //{
    //    jump = true;
    //    //print("jump trigger!!!!!");
    //}
    //else
    //{
    //    jump = false;
    //}
    //lastJump = newJump;
    //攻击
    //bool newAttack = Input.GetKey(keyC);
    //if (newAttack != lastAttack && newAttack == true)
    //{
    //    attack = true;
    //    //print("jump trigger!!!!!");
    //}
    //else
    //{
    //    attack = false;
    //}
    //lastAttack = newAttack;
}
