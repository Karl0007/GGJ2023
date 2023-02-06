using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 摄像机震动 : MonoBehaviour
{
    public float x轴;
    public float y轴;
    public bool 震动 = false;
    public bool 开始震动 = false;
    public float 震动持续时间;
    public float 震动幅度;


    private Vector3 原始位置;
    private float 时间;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator WaitForSecond(float a)
    {
        yield return new WaitForSeconds(a);//等待
        震动 = false;
        transform.localPosition = 原始位置;
    }
    public void 震动效果()
    {
        震动 = true;
        开始震动 = true;
    }

    // Update is called once per frame
    void Update()
    {
            if (震动)
            {
                
                transform.localPosition = 原始位置 + Random.insideUnitSphere * 震动幅度;
            }

            if (开始震动)
            {
                StartCoroutine(WaitForSecond(震动持续时间));
                开始震动 = false;
            }

        if (!震动)//当为flase时记录当前摄像头位置
        {
            原始位置 = transform.position; //时间 = 震动持续时间;
            
        }
    }

}
