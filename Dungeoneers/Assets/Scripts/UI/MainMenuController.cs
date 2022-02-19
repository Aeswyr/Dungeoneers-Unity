using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
        
    public void Local() {
        UIHandler.instance.NextMenu(MenuType.LOCAL);
    }

    public void Online() {
        UIHandler.instance.NextMenu(MenuType.ONLINE);
    }

    public void Options() {
       UIHandler.instance.NextMenu(MenuType.OPTIONS);
    }

    public void Quit() {
        Application.Quit();
    }
}
