using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ResourceController : MonoBehaviour
{
    private int hpMax;
    private int hp, armor;
    [SerializeField] private RectTransform bar;
    [SerializeField] private RectTransform background;
    [SerializeField] private Image barDisp;
    [SerializeField] private GameObject armorObj;
    [SerializeField] private TextMeshProUGUI armorText;



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

    public void SetArmor(int val) {
        armor = val;
        if (armor > 0) {
            armorText.text = armor.ToString();
            armorObj.SetActive(true);
        } else
            armorObj.SetActive(false);
    }
}
