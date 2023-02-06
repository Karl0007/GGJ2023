using GGJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    public int 最小数量;
    public int 最大数量;

    public int 预警时间;
    public int 出现时间;
    public int 间隔时间;
    public bool 是否可以发射;
    public float 累计间隔时间;
    public BallType 属性;
    public int 当前创建数量;
    public Vector3 波次出现位置;
    public bool 处于预警;
}
