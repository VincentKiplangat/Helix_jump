using System.Collections;
using System.Collections.Generic;
using UnityEditor.Advertisements;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int best;
    public int score;

    public int currentStage = 0;
    public static GameManager singleton;
    public AudioSource winLevel;
    public AudioSource deathSound;
    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }
        
        best = PlayerPrefs.GetInt("Highscore");
    }

    // Update is called once per frame
    public void NextLevel ()
    {
        currentStage++;
        winLevel.Play();

        FindObjectOfType<BallController>().ResetBall();
        FindObjectOfType<HelixController>().LoadStage(currentStage);
        Debug.Log("Next level called");
    }

    public void RestartLevel ()
    {
        Debug.Log("Game Over!");
        //Show ads
        singleton.score=0;
        FindObjectOfType<BallController>().ResetBall(); 
        FindObjectOfType<HelixController>().LoadStage(currentStage);
    }

    public void AddScore (int scoreToAdd)
    {
        score += scoreToAdd;

        if (score > best)
        {
            best = score;
            PlayerPrefs.SetInt("Highscore", score);
        }
    }
}
