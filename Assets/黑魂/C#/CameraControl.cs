using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 当角色开始挥刀时，角色会自动锁定离自己
/// </summary>
public class CameraControl : MonoBehaviour {
    private GameObject player;
    private GameObject cameraControl;
    public  UserInput pi;
    public Image lockDot;
    public bool isAI = false;
    private float tempEulerx;
    private GameObject model;
    private GameObject camera;
    private LockTarget lockTarget;
    //判断是否锁到敌人
    public bool lockState;

	// Use this for initialization
	void Awake () {
        //让游标消失
        Cursor.lockState = CursorLockMode.Locked;
        cameraControl = transform.parent.gameObject;
        player =cameraControl.transform.parent.gameObject;
        tempEulerx = 20.0f;
        if (!isAI)
        {
            camera = Camera.main.gameObject;
            lockDot.enabled = false;
        }
        model = player.GetComponent<PlayerController>().model;       
        lockState = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (lockTarget == null)
        {
            //取出人物的欧拉角
            Vector3 tempModelEuler = model.transform.eulerAngles;
            player.transform.Rotate(Vector3.up, pi.JRight * 100.0f * Time.fixedDeltaTime);
            //tempEulerx = cameraControl.transform.eulerAngles.x;
            //用加减的方法移动，不用Rotate方法
            tempEulerx -= pi.JUp * 80.0f * Time.fixedDeltaTime;
            //限制摄像头上下的角度
            tempEulerx = Mathf.Clamp(tempEulerx, -40, 40);
            cameraControl.transform.localEulerAngles = new Vector3(tempEulerx, 0, 0);
            //上述程序改动了角色胶囊的欧拉角，所以把原先的欧拉角重新赋给角色
            model.transform.eulerAngles = tempModelEuler;
        }
        else
        {
            Vector3 tempForward = lockTarget.obj.transform.position - model.transform.position;
            tempForward.y = 0;
            player.transform.forward = tempForward;
            //看向怪物脚底
            //cameraControl.transform.LookAt(lockTarget.obj.transform);
        }
        if (!isAI)
        {
            //把cameraPos的位置给到相机，使相机追上该位置
            camera.transform.position = Vector3.Lerp(camera.transform.position, transform.position, 0.2f);
            //把CameraPos欧拉角赋给相机(显得生硬）
            //camera.transform.eulerAngles = transform.eulerAngles;
            camera.transform.LookAt(cameraControl.transform);
        }
       
    }
    private void Update()
    {
        if (lockTarget != null)
        {
            if (!isAI)
            {
                lockDot.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position + new Vector3(0, lockTarget.halfHeight, 0));
            }
            
            if (Vector3.Distance(model.transform.position, lockTarget.obj.transform.position) > 10f)
            {
                LockProcess(null, false, false, isAI);
            }
            //ActorManager targetAm = lockTarget.obj.GetComponent<ActorManager>();
            if (lockTarget.am!=null&&lockTarget.am.sm.isDie)
            {
                LockProcess(null, false, false, isAI);
            }
        }
        
    }
    private void LockProcess(LockTarget _lockTarget,bool _lockDotEnable,bool _lockState,bool _isAI)
    {
        lockTarget =_lockTarget;
        if (!_isAI)
        {
            lockDot.enabled = _lockDotEnable;
        }      
        lockState = _lockState;
    }
    /// <summary>
    /// 锁定解锁方法
    /// </summary>
    public void LockUnlock()
    {
        //if (lockTarget == null)
        //{
            //try to lock，构造一个长方体检测前方是否有敌人可以锁定
            Vector3 modelOrigin1 = model.transform.position;//角色的脚底
            Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
            Vector3 boxCenter = modelOrigin2 + model.transform.forward * 5f;
            Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5f), model.transform.rotation,LayerMask.GetMask(isAI?"Player": "Enemy"));
        if (cols.Length == 0)
        {
            LockProcess(null, false, false, isAI);
        }
        else
        {
            foreach (var col in cols)
            {
                //如果锁定的还是同样的敌人就解锁
                if (lockTarget!=null&&lockTarget.obj == col.gameObject)
                {
                    LockProcess(null, false, false, isAI);
                    break;
                }
                // print(col.name);               
                LockProcess(new LockTarget(col.gameObject, col.bounds.extents.y), true, true, isAI);
                //lockDot.transform.position = Camera.main.WorldToScreenPoint(lockTarget.transform.position);
                break;
            }
        }                  
    }
    private class LockTarget
    {
        public GameObject obj;
        public float halfHeight;
        public ActorManager am;
        public LockTarget(GameObject _obj,float _halfHeight)
        {
            obj = _obj;
            halfHeight = _halfHeight;
            am = _obj.GetComponent<ActorManager>();
        }
    }




}
