using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatDisplayController : MonoBehaviour
{
    private int stat = 0;
    [SerializeField] private TextMeshProUGUI num;
    [SerializeField] private TextMeshProUGUI type;
    [SerializeField] private CharacterCreationPanelController panel;
    public void StatUp() {
        if (stat >= 6 || panel.GetStatPoints() <= 0)
            return;
        stat++;
        panel.AdjStatPoints(-1);
        num.text = stat.ToString();
    }

    public void StatDown() {
        if (stat <= 1)
            return;
        stat--;
        panel.AdjStatPoints(1);
        num.text = stat.ToString();
    }

    public void SetStat(int stat, string name) {
        type.text = name;
        num.text = stat.ToString();
        this.stat = stat;
    }

    public int GetStat() {
        return stat;
    }
}
