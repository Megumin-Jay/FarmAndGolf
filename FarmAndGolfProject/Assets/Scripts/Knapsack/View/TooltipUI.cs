using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    public Text OutlineText;
    public Text ContentText;

    //更新显示
    public void UpdateTooltip(string text)//更新的接口
    {
        OutlineText.text = text;
        ContentText.text = text;
    }

    //显示
    public void Show()
    {
        gameObject.SetActive(true);
    }

    //隐藏
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetLocalPosition(Vector2 position)
    {
        transform.localPosition = position;//信息栏的鼠标跟随（利用锚点轴心的设置可以改变和鼠标的相对位置！）
    }
}
