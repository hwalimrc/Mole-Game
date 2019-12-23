using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public enum GameState
{
    Ready,
    Play,
    End
}

public class GameManager : MonoBehaviour 
{
    public GameState GS;
    public Mole[] mole;

    public GUIText scoreText;
    public GUIText limitText;
    public GUIText readyText;
    public GUIText finishText;

    public GameObject ReadyGUI;
    public GameObject PlayGUI;
    public GameObject finishGUI;

    public int score;
    int readyInt;
    float ready;
    float limit;

    void Start()
    {
        GS = GameState.Ready;
        ReadyGUI.SetActive(true);
        PlayGUI.SetActive(false);
        finishGUI.SetActive(false);

        score = 0;
        limit = 10.0f;
        ready = 5.0f;
        readyInt = 6;
    }

    void Update()
    {
        switch (GS)
        {
            case GameState.Ready:
                ready -= Time.deltaTime;
                if (ready <= readyInt)
                {
                    readyInt--;
                }
                readyText.text = ((int)ready).ToString();
                if (ready <= 1)
                {
                    Play();
                }
                break;

            case GameState.Play:
                limit -= Time.deltaTime;
                scoreText.text = score.ToString();
                limitText.text = string.Format("{0:F2}", limit);
                if (limit <= 0)
                {
                    End();
                }
                break;
        }
    }

    public void scoreModify(int scoreAmount)
    {
        score += scoreAmount;
        if (score < 0)
        {
            score = 0;
        }
    }

    void Play()
    {
        GS = GameState.Play;

        ReadyGUI.SetActive(false);
        PlayGUI.SetActive(true);

        for (int i = 0; i < 9; i++)
        {
            mole[i].Play();
        }
    }

    void End()
    {
        GS = GameState.End;

        PlayGUI.SetActive(false);
        finishGUI.SetActive(true);

        limit = 0;
        finishText.text = (score * 100).ToString();

        insert(score * 100);
        getscore();

        for (int i = 0; i < 9; i++)
        {
            mole[i].Stop();
        }
    }

    public void insert(int score)
    {
        string address = "http://127.0.0.1/insert.php";
        WWWForm Form = new WWWForm();
        // Form.AddField("Score", score.ToString());
        Form.AddField("Score", score);

        WWW wwwURL = new WWW(address, Form);
    }

    public void getscore()
    {
        WebRequest totalscore = WebRequest.Create("http://127.0.0.1/select.php");
        WebResponse response = totalscore.GetResponse();
        StreamReader stream = new StreamReader(response.GetResponseStream());
        string firstStr = stream.ReadToEnd();
        Debug.Log(firstStr);
        string[] split = firstStr.Split(new char[] {'/'});
        for (int i = 0; i < 3; i++)
        {
            Debug.Log(i+1 + "위: " + split[i] + "\n");
        }
    }
}
