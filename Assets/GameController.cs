using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject canvas;
    public GameObject buttonAndTextPefrab;
    public Text[] spaceList;
    public GameObject gameOverPanel;
    public Text gameOverText;
    public GameObject restartButton;
    public GameObject Board;
    public Text LineAndColumnText;
    private string side;
    private int moves;
    private int maxMoves;
    private Text[,] spacesPositions;
    private int maxLines;
    private int maxColumns;

    // Start is called before the first frame update
    void Start()
    {
        side = "X";
        gameOverPanel.SetActive(false);
        moves = 0;
        maxMoves = 9;
        restartButton.SetActive(false);
        maxLines = 3;
        maxColumns = 3;
        spacesPositions = FillPanel();
        SetGameControllerReferenceForButtons();
    }

    private Text[,] FillPanel()
    {
        Text[,] positions = new Text[maxLines, maxColumns];
        int index = 0;

        for (int x = 0; x < positions.GetLength(0); x++)
        {
            for (int y = 0; y < positions.GetLength(1); y++)
            {
                positions[x, y] = spaceList[index];
                spaceList[index].GetComponentInParent<Space>().SetPanelPosition(x,y);
                index++;
            }
        }
        return positions;
    }

    void SetGameControllerReferenceForButtons()
    {
        for (int x = 0; x < spacesPositions.GetLength(0); x++)
        {
            for (int y = 0; y < spacesPositions.GetLength(1); y++)
            {
                spacesPositions[x, y].GetComponentInParent<Space>().SetControllerReference(this);
            }
        }
    }

    public string GetSide()
    {
        return side;
    }

    void ChangeSide()
    {
        if (side == "X")
            side = "O";
        else
            side = "X";
    }

    public void EndTurn()
    {
        moves++;
        if (CheckLines())
        {
            GameOver();
        }
        else if (CheckColumns())
        {
            GameOver();
        }
        else if (CheckDiagonals())
        {
            GameOver();
        }
        if (moves >= maxMoves)
        {
            gameOverPanel = Instantiate(gameOverPanel);
            gameOverPanel.transform.SetParent(canvas.transform, true);
            gameOverPanel.SetActive(true);

            gameOverText = gameOverPanel.transform.GetComponentInChildren<Text>();
            gameOverText.transform.SetParent(canvas.transform, true);
            gameOverText.text = "Tie!";

            restartButton = Instantiate(restartButton);
            restartButton.transform.SetParent(canvas.transform, true);
            restartButton.SetActive(true);
        }
        ChangeSide();
        ChangeBoard();

    }

    private void ChangeBoard()
    {
        if (!gameOverPanel.activeSelf)
        {
            switch (moves % 3)
            {
                case 0:
                    int randomNumber = Random.Range(0, 2);
                    if (randomNumber == 1)
                    {
                        AddLine();
                        LineAndColumnText.text = "A new Line was added! 3 plays until another Line or Column appears!";
                    }
                    else
                    {
                        AddColumn();
                        LineAndColumnText.text = "A new Column was added! 3 plays until another Line or Column appears!";
                    }
                    break;
                case 1:
                    LineAndColumnText.text = "A new Line or Column will be added in 2 plays!!";
                    break;
                case 2:
                    LineAndColumnText.text = "A new Line or Column will be added in 1 play!!";
                    break;
                default:
                    break;
            }
        }
    }

    private bool CheckLines()
    {
        int sidePositionCounts = 0;
        for (int line = 0; line < spacesPositions.GetLength(0); line++)
        {
            for (int column = 0; column < spacesPositions.GetLength(1); column++)
            {
                if (spacesPositions[line, column].text == side)
                {
                    sidePositionCounts++;
                }
                if (spacesPositions[line, column].text == "" || spacesPositions[line, column].text != side)
                {
                    sidePositionCounts = 0;
                }
                if (sidePositionCounts == 3)
                {
                    return true;
                }
            }
            sidePositionCounts = 0;
        }
        return false;
    }

    private bool CheckColumns()
    {
        int sidePositionCounts = 0;
        for (int column = 0; column < spacesPositions.GetLength(1); column++)
        {
            for (int line = 0; line < spacesPositions.GetLength(0); line++)
            {
                if (spacesPositions[line, column].text == side)
                {
                    sidePositionCounts++;
                }
                if (spacesPositions[line, column].text == "" || spacesPositions[line, column].text != side)
                {
                    sidePositionCounts = 0;
                }
                if (sidePositionCounts == 3)
                {
                    return true;
                }
            }
            sidePositionCounts = 0;
        }
        return false;
    }

    private bool CheckDiagonals()
    {
        for (int line = 0; line < spacesPositions.GetLength(0); line++)
        {
            for (int column = 0; column < spacesPositions.GetLength(1); column++)
            {
                try { 
                    if (spacesPositions[line, column].text == side
                        && spacesPositions[line + 1, column + 1].text == side
                        && spacesPositions[line + 2, column + 2].text == side)
                    {
                        return true;
                    }
                } catch { } 
                try
                {
                    if (spacesPositions[line, column].text == side
                        && spacesPositions[line - 1, column + 1].text == side
                        && spacesPositions[line - 2, column + 2].text == side)
                    {
                        return true;
                    }
                } catch { }
            }
        }
        return false;
    }

    void GameOver()
    {
        gameOverPanel = Instantiate(gameOverPanel);
        gameOverPanel.transform.SetParent(canvas.transform, true);
        gameOverPanel.SetActive(true);

        gameOverText = gameOverPanel.transform.GetComponentInChildren<Text>();
        gameOverText.transform.SetParent(canvas.transform, true);
        gameOverText.text = side + " wins!";

        restartButton = Instantiate(restartButton);
        restartButton.transform.SetParent(canvas.transform, true);
        restartButton.SetActive(true);

        SetInteractable(false);
        LineAndColumnText.text = "";
    }

    void SetInteractable(bool setting)
    {
        for (int line = 0; line < spacesPositions.GetLength(0); line++)
        {
            for (int column = 0; column < spacesPositions.GetLength(1); column++)
            {
                spacesPositions[line, column].GetComponentInParent<Button>().interactable = setting;
            }
        }
    }

    public void Restart()
    {
        side = "X";
        moves = 0;
        gameOverPanel.SetActive(false);
        SetInteractable(true);
        restartButton.SetActive(false);
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    const int horizontal = 105;
    const int vertical = 100;
    const int width = 75;
    const int length = 70;
    const int verticalLineHorizontalIncrement = -50;
    const double horizontalLineVerticalIncrement = -47.5;
    const double verticalLineW = 18;
    const double verticalLineH = 274;
    const double horizontalLineW = 296;
    const double horizontalLineH = 18;

    public void AddColumn()
    {
        GameObject prefabCopy = new GameObject();
        int randomNumber = Random.Range(0, 2);
        if (randomNumber == 1)
        {
            spacesPositions = AddColumnToArray(true);

            for (int i = 0; i < maxLines; i++)
            {
                float rightMaxX = spacesPositions[i, maxColumns - 2].GetComponentInParent<Button>().transform.position.x;
                float rightMaxY = spacesPositions[i, maxColumns - 2].GetComponentInParent<Button>().transform.position.y;
                prefabCopy = Instantiate(buttonAndTextPefrab, new Vector3(rightMaxX + horizontal, rightMaxY, 0), Quaternion.identity);
                prefabCopy.transform.SetParent(Board.transform, true);
                spacesPositions[i, maxColumns - 1] = prefabCopy.transform.GetChild(0).GetComponent<Text>();
            }
        }
        else
        {
            spacesPositions = AddColumnToArray(false);

            for (int i = 0; i < maxLines; i++)
            {
                float leftMaxX = spacesPositions[i, 1].GetComponentInParent<Button>().transform.position.x;
                float leftMaxY = spacesPositions[i, 1].GetComponentInParent<Button>().transform.position.y;
                prefabCopy = Instantiate(buttonAndTextPefrab, new Vector3(leftMaxX - horizontal, leftMaxY, 0), Quaternion.identity);
                prefabCopy.transform.SetParent(Board.transform, true);
                spacesPositions[i, 0] = prefabCopy.transform.GetChild(0).GetComponent<Text>();
            }
        }
        SetGameControllerReferenceForButtons();
    }

    public Text[,] AddColumnToArray(bool rightSide)
    {
        Text[,] newMatrix;
        if (rightSide)
        {
            newMatrix = new Text[spacesPositions.GetLength(0), spacesPositions.GetLength(1) + 1];
            maxColumns++;
            for (int line = 0; line < spacesPositions.GetLength(0); line++)
            {
                for (int column = 0; column < spacesPositions.GetLength(1); column++)
                {
                    newMatrix[line, column] = spacesPositions[line, column];
                }
            }
        }
        else
        {
            newMatrix = new Text[spacesPositions.GetLength(0), spacesPositions.GetLength(1) + 1];
            maxColumns++;
            for (int line = 0; line < spacesPositions.GetLength(0); line++)
            {
                for (int column = 0; column < spacesPositions.GetLength(1); column++)
                {
                    newMatrix[line, column + 1] = spacesPositions[line, column];
                }
            }
        }
        maxMoves = spacesPositions.GetLength(0) * spacesPositions.GetLength(1);
        return newMatrix;
    }

    public void AddLine()
    {
        GameObject prefabCopy = new GameObject();
        int randomNumber = Random.Range(0, 2);
        if (randomNumber == 1)
        {
            spacesPositions = AddLineToArray(true);

            for (int i = 0; i < maxColumns; i++)
            {
                float upperMaxX = spacesPositions[1, i].GetComponentInParent<Button>().transform.position.x;
                float upperMaxY = spacesPositions[1, i].GetComponentInParent<Button>().transform.position.y;
                prefabCopy = Instantiate(buttonAndTextPefrab, new Vector3(upperMaxX, upperMaxY + vertical, 0), Quaternion.identity);
                prefabCopy.transform.SetParent(Board.transform, true);
                spacesPositions[0, i] = prefabCopy.transform.GetChild(0).GetComponent<Text>();
            }
            Board.transform.position = new Vector3(Board.transform.position.x, Board.transform.position.y - 100, 0);
        }
        else
        {
            spacesPositions = AddLineToArray(false);

            for (int i = 0; i < maxColumns; i++)
            {
                float lowerMaxX = spacesPositions[maxLines - 2, i].GetComponentInParent<Button>().transform.position.x;
                float lowerMaxY = spacesPositions[maxLines - 2, i].GetComponentInParent<Button>().transform.position.y;
                prefabCopy = Instantiate(buttonAndTextPefrab, new Vector3(lowerMaxX, lowerMaxY - vertical, 0), Quaternion.identity);
                prefabCopy.transform.SetParent(Board.transform, true);
                spacesPositions[maxLines - 1, i] = prefabCopy.transform.GetChild(0).GetComponent<Text>();
            }
        }
        SetGameControllerReferenceForButtons();
    }

    public Text[,] AddLineToArray(bool upperSide)
    {
        Text[,] newMatrix;
        if (upperSide)
        {
            newMatrix = new Text[spacesPositions.GetLength(0) + 1, spacesPositions.GetLength(1)];
            maxLines++;
            for (int line = 0; line < spacesPositions.GetLength(0); line++)
            {
                for (int column = 0; column < spacesPositions.GetLength(1); column++)
                {
                    newMatrix[line + 1, column] = spacesPositions[line, column];
                }
            }
        }
        else
        {
            newMatrix = new Text[spacesPositions.GetLength(0) + 1, spacesPositions.GetLength(1)];
            maxLines++;
            for (int line = 0; line < spacesPositions.GetLength(0); line++)
            {
                for (int column = 0; column < spacesPositions.GetLength(1); column++)
                {
                    newMatrix[line, column] = spacesPositions[line, column];
                }
            }
        }
        maxMoves = spacesPositions.GetLength(0) * spacesPositions.GetLength(1);
        return newMatrix;
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}
