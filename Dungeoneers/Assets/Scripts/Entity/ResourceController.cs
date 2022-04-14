using UnityEngine.UI;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    private int hpMax;
    private int hp;
    [SerializeField] private RectTransform bar;
    [SerializeField] private RectTransform background;
    [SerializeField] private Image barDisp;



    public void SetMaxHP(int val, bool maxed = true) {
        hpMax = val;
        int width = 12;
        if (hpMax > 100)
            width = 48;
        else if (hpMax > 50)
            width = 24;
            
        bar.sizeDelta = new Vector2(width - 4, bar.rect.height);
        background.sizeDelta = new Vector2(width, background.rect.height);
        if (maxed) {
            SetHP(val);
        }
    }

    public void SetHP(int val) {
        hp = val;
        barDisp.fillAmount = (float)hp / hpMax;
    }
}
