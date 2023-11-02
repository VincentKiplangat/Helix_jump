using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestText;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.singleton;
    }

    private void Update()
    {
        bestText.text = "Best: " + gameManager.best;
        scoreText.text = "Score: " + gameManager.score;
    }
}
