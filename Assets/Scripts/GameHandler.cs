using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    private SoundManager soundManager;
    public ThrowBall controls;
    private int player;
    public int currentRound;
    private int currentBall;
    private int currentScore;
    private int prevScore;
    private List<Player> players;
    private bool roundEnd;
    public Text pinsText;
    public Text[] semiFrameBoard;
    public Text[] frameBoard;
    public Text[] semiFrameBoard2;
    public Text[] frameBoard2;
    private int[] semiFrame;
    private int semiFrameIndex;
    private int[] frame;
    private int[] semiFrame2;
    private int semiFrameIndex2;
    private int[] frame2;
    public Button restart;
    public GameObject panelGameOver;
    public TMP_Text scoreFinal;
    private int record;
    public TMP_Text recordText;
    public TMP_Text _record;


    public Player CurrentPlayer
    {
        get { return players == null ? null : players[player]; }
    }
    public int totalPlayers;


    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1f);

        foreach (Player p in players) { p.Reset(); }
        player = 0;

        while (currentRound <= 10)
        {
            foreach (Player p in players)
            {
                pinsText.text = "";
                roundEnd = false;
                currentScore = 0;
                controls.Play();
                yield return new WaitUntil(() => roundEnd); // Wait until we know the result
                if (currentScore != 10)
                {
                    prevScore = currentScore;
                    p.AddScore(currentScore);

                    //Register Score
                    if (p.ID == 1)
                    {
                        semiFrame[semiFrameIndex] = currentScore;
                        semiFrameBoard[semiFrameIndex].text = currentScore.ToString();
                        semiFrameIndex++;
                    }


                    currentBall++;
                    roundEnd = false;
                    currentScore = 0;
                    controls.PlayAgain();
                    yield return new WaitUntil(() => roundEnd); // Wait until we know the result

                    if (currentScore + prevScore == 10)
                    {
                        pinsText.text = "¡SEMIPLENO!";
                        if (p.ID == 1)
                        {
                            semiFrame[currentRound * 2 - 1] = currentScore + prevScore;
                            semiFrameBoard[currentRound * 2 - 1].text = "/";
                            frame[currentRound - 1] = 10;


                            if (currentRound >= 2 && semiFrame[semiFrameIndex - 3] == 10)
                            {
                                frame[currentRound - 2] += 10;
                                frameBoard[currentRound - 2].text = frame[currentRound - 2].ToString();
                            }
                            if (currentRound >= 3 && semiFrame[semiFrameIndex - 5] == 10)
                            {
                                frame[currentRound - 3] += semiFrame[currentRound * 2 - 2];
                                frameBoard[currentRound - 3].text = frame[currentRound - 3].ToString();
                            }

                            frameBoard[currentRound - 1].text = frame.Sum().ToString(); //frame[currentRound - 1].ToString();
                            semiFrameIndex++;
                        }
                        
                        prevScore = 0;
                    }
                    else
                    {
                        if (p.ID == 1)
                        {
                            semiFrame[currentRound * 2 - 1] = currentScore;
                            semiFrameBoard[currentRound * 2 - 1].text = currentScore.ToString();
                            frame[currentRound - 1] = semiFrame[currentRound * 2 - 1] + semiFrame[currentRound * 2 - 2];


                            if (currentRound >= 2 && semiFrame[semiFrameIndex - 3] == 10)
                            {
                                frame[currentRound - 2] += semiFrame[currentRound * 2 - 1] + semiFrame[currentRound * 2 - 2];
                                frameBoard[currentRound - 2].text = frame[currentRound - 2].ToString();
                            }
                            if (currentRound >= 3 && semiFrame[semiFrameIndex - 5] == 10)
                            {
                                frame[currentRound - 3] += semiFrame[currentRound * 2 - 2];
                                frameBoard[currentRound - 3].text = frame[currentRound - 3].ToString();
                            }

                            frameBoard[currentRound - 1].text = frame.Sum().ToString(); //frame[currentRound - 1].ToString();
                            semiFrameIndex++;
                        }
                        
                    }

                    currentBall = 1;
                    prevScore = 0;
                }
                else
                {
                    if (p.ID == 1)
                    {
                        semiFrame[semiFrameIndex] = 10;
                        semiFrameBoard[semiFrameIndex].text = "X";
                        frame[currentRound - 1] = 10;


                        if (currentRound >= 2 && semiFrame[semiFrameIndex - 2] == 10)
                        {
                            frame[currentRound - 2] += 10; //frame[currentRound - 1];
                            frameBoard[currentRound - 2].text = frame[currentRound - 2].ToString();
                        }
                        if (currentRound >= 3 && semiFrame[semiFrameIndex - 2] == 10 && semiFrame[semiFrameIndex - 4] == 10)
                        {
                            frame[currentRound - 3] += 10; //frame[currentRound - 1];
                            frameBoard[currentRound - 3].text = frame[currentRound - 3].ToString();
                        }

                        frameBoard[currentRound - 1].text = frame.Sum().ToString(); //frame[currentRound - 1].ToString();
                        semiFrameIndex += 2;
                    }


                    prevScore = 0;


                    pinsText.text = "¡PLENO!";
                }
                yield return new WaitForSeconds(5f);
            }
            currentRound++;
        }


    }

    // Use this for initialization
    public void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();

        players = new List<Player>();
        restart.gameObject.SetActive(false);
        prevScore = 0;
        currentRound = 1;
        currentBall = 1;
        semiFrame = new int[21];
        semiFrameIndex = 0;
        frame = new int[11];
        panelGameOver.SetActive(false);
        StartCoroutine(StartGame());

        for (int i = 0; i < totalPlayers; i++)
        {
            players.Add(new Player());
        }
        
        _record.text = PlayerPrefs.GetInt("record").ToString();

    }

    // Update is called once per frame
    void Update()
    {
        soundManager = FindObjectOfType<SoundManager>();
        if (currentRound > 10)
        {
            frameBoard[10].text = frame.Sum().ToString();
            panelGameOver.SetActive(true);           
            scoreFinal.text = frame.Sum().ToString();
            record = frame.Sum();
            recordText.text = PlayerPrefs.GetInt("record", record).ToString();
            if (record > PlayerPrefs.GetInt("record", 0))
            {
                PlayerPrefs.SetInt("record", record);
                recordText.text = PlayerPrefs.GetInt("record", 0).ToString();
            }
            StopAllCoroutines();
            restart.gameObject.SetActive(true);
            restart.onClick.AddListener(() => Restart());
            //PlaySound(soundManager.finalSound, 0.2f);


        }
        
        else
        {
            restart.gameObject.SetActive(false);
            
        }

    }
    private void PlaySound(AudioClip clip, float volumeMultiplier = 1f)
    {
        if (clip && soundManager.fxEnabled)
        {
            AudioSource.PlayClipAtPoint(
                clip, Camera.main.transform.position,
                Mathf.Clamp(soundManager.fxVolume * volumeMultiplier, 0.05f, 0.8f));
        }
    }

    public void PinFall()
    {
        currentScore++;
    }

    public void RoundEnd()
    {
        print("La ronda terminó con " + currentScore + " points.");
        pinsText.text = "Has tirado " + currentScore + " bolos.";

        roundEnd = true;

    }


    //Reinicia el juego
    public void Restart()
    {
        prevScore = 0;
        currentRound = 1;
        currentBall = 1;
        semiFrameIndex = 0;

        for (int i = 0; i < 21; i++)
        {
            semiFrame[i] = 0;

            semiFrameBoard[i].text = "";

        }
        for (int j = 0; j < 11; j++)
        {
            frame[j] = 0;

            frameBoard[j].text = "";

        }
        StartCoroutine(StartGame());
        panelGameOver.SetActive(false);
    }
    public void IrA(string nombre)
    {

        SceneManager.LoadScene(nombre);
    }

    public void Salir()
    {
        Application.Quit();
    }
   
}