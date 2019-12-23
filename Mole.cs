using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoleState
{
    Ready,
    None,
    Open,
    Idle,
    Close,
    Catch
}

public class Mole : MonoBehaviour {
    public MoleState MS;
    public Texture[] OpenImage;
    public Texture[] IdleImage;
    public Texture[] CloseImage;
    public Texture[] CatchImage;
    public Texture NoneImage;

    public AudioClip OpenSound;
    public AudioClip CatchSound;

    public GameManager GM;
    public KeyCode key;

    int AniCount;
    float timeForCount;
    float timeToWait;
    bool active;

    void Start()
    {
        active = false;
        this.GetComponent<Renderer>().material.mainTexture = NoneImage;
    }

    public void Play()
    {
        active = true;
        None_On();
        AniCount = 0;
    }

    public void Stop()
    {
        active = false;
        if (MS == MoleState.Open || MS == MoleState.Idle)
        {
            MS = MoleState.Close;
        }
    }

    void Update()
    {
        switch (MS)
        {
            case MoleState.None:
                None_Ing();
                break;

            case MoleState.Open:
                Open_Ing();
                break;

            case MoleState.Idle:
                Idle_Ing();
                break;

            case MoleState.Catch:
                Catch_Ing();
                break;

            case MoleState.Close:
                Close_Ing();
                break;
        }

        if (Input.GetKey(key) == true && (MS == MoleState.Idle || MS == MoleState.Open))
        {
            Catch_On();
        }
    }

    public void None_On()
    {
        AniCount = 0;
        MS = MoleState.None;
        timeForCount = 0;
        timeToWait = Random.Range(3.0f, 8.0f);
        if (!active) { MS = MoleState.Ready; }
    }

    public void None_Ing()
    {
        this.GetComponent<Renderer>().material.mainTexture = NoneImage;
        timeForCount += Time.deltaTime;
        if (timeForCount > timeToWait)
            Open_On();
        if (!active)
        { MS = MoleState.Ready; }
    }

    public void Open_On()
    {
        AniCount = 0;
        MS = MoleState.Open;
        GetComponent<AudioSource>().clip = OpenSound;
        GetComponent<AudioSource>().Play();
    }

    public void Open_Ing()
    {
        this.GetComponent<Renderer>().material.mainTexture = OpenImage[AniCount];
        if (++AniCount >= OpenImage.Length)
        {
            Idle_On();
        }
    }

    public void Idle_On()
    {
        AniCount = 0;
        MS = MoleState.Idle;
        timeForCount = 0;
        timeToWait = Random.Range(1.0f, 3.0f);
    }

    public void Idle_Ing()
    {
        this.GetComponent<Renderer>().material.mainTexture = IdleImage[AniCount];
        if (++AniCount >= IdleImage.Length)
        {
            AniCount = 0;
        }

        timeForCount += Time.deltaTime;
        if (timeForCount > timeToWait)
        {
            Close_On();
        }
    }

    public void Catch_On()
    {
        AniCount = 0;
        MS = MoleState.Catch;
        GM.scoreModify(1);
    }

    public void Catch_Ing()
    {
        this.GetComponent<Renderer>().material.mainTexture = CatchImage[AniCount];
        if (++AniCount >= CatchImage.Length)
        {
            None_On(); 
        }
    }

    public void Close_On()
    {
        AniCount = 0;
        MS = MoleState.Close;

        GM.scoreModify(-1);
    }

    public void Close_Ing()
    {
        this.GetComponent<Renderer>().material.mainTexture = CloseImage[AniCount];
        if (++AniCount >= CloseImage.Length)
        {
            None_On();
        }
    }
}
