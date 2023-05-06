using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Space : MonoBehaviour
{
    public Button button;
    public Text buttonText;
    private GameController gameController;
    private int linePosition;
    private int columnPosition;

    public void SetControllerReference(GameController control)
    {
        gameController = control;
    }

    public void SetSpace()
    {
        buttonText.text = gameController.GetSide();
        button.interactable = false;
        gameController.EndTurn();
    }

    public void SetPanelPosition(int line, int column)
    {
        linePosition = line;
        columnPosition = column;
    }

    // Start is called before the first frame update
    /*void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
