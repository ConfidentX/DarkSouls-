using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer  {

    //定义状态枚举
    public enum State {

        IDLE,
        RUN,
        FINISHED
    }

    public State state;

    //预设计时器的时长
    public float duration = 1.0f;
    //流逝的时间
    private float elapsedTime = 0;

    public void Tick( )
    {
        if (state == State.IDLE)
        {

        }
        else if (state == State.RUN)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= duration)
            {
                state = State.FINISHED;
            }
        }
        else if(state==State.FINISHED)
        {

        }
        else
        {

        }
    }
    //开启计时器
    public void Go()
    {
        elapsedTime = 0;
        state = State.RUN;
    }

}
