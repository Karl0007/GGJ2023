using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GGJ;
public class WarningTtime : MonoBehaviour
{
    public Text 预警时间;
    public Image 左箭头;
    public Image 右箭头;
    public float 剩余时间值;

    // Start is called before the first frame update
    void Start()
    {
        预警时间.text = 剩余时间值.ToString();
    }
    public void 变化(Wave 波数类型)
    {
        预警时间.color = 波数类型.属性.GetColor();
        左箭头.color= 波数类型.属性.GetColor();
        右箭头.color = 波数类型.属性.GetColor();
        var 变化百分比 = 1+波数类型.当前创建数量 / 30;
        this.transform.localScale =new Vector3(this.transform.localScale.x* 变化百分比, this.transform.localScale.y * 变化百分比, this.transform.localScale.z * 变化百分比);

    }
    // Update is called once per frame
    void Update()
    {
        剩余时间值-= Time.deltaTime;
        预警时间.text = 剩余时间值.ToString("f2");
        if (剩余时间值<=0)
        {
            Destroy(this.transform.parent.gameObject);
        }
    }

    public void 左边()
    {
        左箭头.gameObject.SetActive(true);
        右箭头.gameObject.SetActive(false);
    }
    public void 右边()
    {
        右箭头.gameObject.SetActive(true);
        左箭头.gameObject.SetActive(false);
    }
}
