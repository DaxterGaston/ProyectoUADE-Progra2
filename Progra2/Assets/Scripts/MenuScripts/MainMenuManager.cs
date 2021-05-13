using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ButtonSwitch
{
    PlayButton,
    HelpButton,
    CreditsButton,
    QuitButton
}
public class MainMenuManager : MonoBehaviour
{
    public List<Button> buttons;
    public RectTransform selectorObject;
    public GameObject mainWindow;
    public GameObject helpWindow;
    public GameObject creditsWindow;
    public Button goBackButton;
    private Button currentButton;
    private int buttonIndex;

    private void Start()
    {
        buttonIndex = 0;
        currentButton = buttons[0];
        mainWindow.SetActive(true);
        helpWindow.SetActive(false);
        creditsWindow.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (mainWindow.activeSelf)
            {
                buttonIndex--;
                if (buttonIndex < 0)
                {
                    buttonIndex = buttons.Count - 1;
                }
                currentButton = buttons[buttonIndex];
                selectorObject.position = new Vector3(selectorObject.position.x, currentButton.transform.position.y);
                
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (mainWindow.activeSelf)
            {
                buttonIndex++;
                if (buttonIndex > buttons.Count - 1)
                {
                    buttonIndex = 0;
                }
                currentButton = buttons[buttonIndex];
                selectorObject.position = new Vector3(selectorObject.position.x, currentButton.transform.position.y);
                
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!mainWindow.activeSelf)
            {
                goBackButton.onClick.Invoke();
                return;
            }
            currentButton.onClick.Invoke(); 
        }
    }

    public void ButtonAssign(int num)
    {
        Enum aux = (ButtonSwitch) num;
        switch (aux)
        {
           case ButtonSwitch.PlayButton:
               SceneManager.LoadScene("Level");
               return;
           case ButtonSwitch.HelpButton:
               mainWindow.SetActive(false);
               helpWindow.SetActive(true);
               selectorObject.position = new Vector3(selectorObject.position.x, goBackButton.transform.position.y);
               return;
           case ButtonSwitch.CreditsButton:
               mainWindow.SetActive(false);
               creditsWindow.SetActive(true);
               selectorObject.position = new Vector3(selectorObject.position.x, goBackButton.transform.position.y);
               return;
           case ButtonSwitch.QuitButton:
               if (Application.isEditor)
               {
                   UnityEditor.EditorApplication.isPlaying = false;
               }
               Application.Quit();
               return;
           default: return;
        } 
    }

    public void GoBackButtonHandler()
    {
        selectorObject.position = new Vector3(selectorObject.position.x, currentButton.transform.position.y);
        mainWindow.SetActive(true);
        helpWindow.SetActive(false);
        creditsWindow.SetActive(false);
    }
}
