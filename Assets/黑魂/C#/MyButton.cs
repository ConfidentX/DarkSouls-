using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 提供IsPressing信号，让后续模块得知目前用户输入状态
/// 提供OnPressed信号，让后续模块得知目前是否刚刚按下此键
/// 提供OnReleased信号，让后续模块得知目前是否刚刚释放此键
/// </summary>
public class MyButton
{
    public bool IsPressing = false;
    public bool OnPressed = false;
    public bool OnReleased = false;
    public bool IsExtending = false;
    public bool IsDelaying = false;

    private bool curState = false;
    private bool lastState = false;


    public float extendingDuration = 0.2f;
    public float delayingDuration = 0.2f;
    //计时器
    private MyTimer extTimer = new MyTimer();
    private MyTimer delayTimer = new MyTimer();
    public void Tick(bool input)
    {
        extTimer.Tick();
        delayTimer.Tick();

        curState = input;
        IsPressing = curState;
        OnPressed = false;
        OnReleased = false;
        IsExtending = false;
        IsDelaying = false;
        if (curState != lastState)
        {
            if (curState == true)
            {
                OnPressed = true;
                StartTimer(delayTimer, delayingDuration);
            }
            else
            {
                //刚刚释放按钮
                OnReleased = true;
                StartTimer(extTimer, extendingDuration);
            }
        }
        lastState = curState;
        //如果计时器正在运行， IsExtending设为真
        if (extTimer.state == MyTimer.State.RUN)
        {
            IsExtending = true;
        }
        if (delayTimer.state == MyTimer.State.RUN)
        {
            IsDelaying = true;
        }
        
    }
    private void StartTimer(MyTimer timer, float duration)
    {
        timer.duration = duration;
        timer.Go();
    }

}
