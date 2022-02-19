using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour
{
    [SerializeField] private Image left, right;
    // Start is called before the first frame update
    void Start()
    {
        left.enabled = false;
        right.enabled = false;
    }

    void ToggleHighlight(int toggle) {
        left.enabled = toggle != 0;
        right.enabled = toggle != 0;
    }



}
