using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Sprite[] _livesSprites;

    [SerializeField]
    private Image _livesImg;

    [SerializeField]
    private GameObject _gameOverText;

    [SerializeField]
    private GameObject _restartText;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameOverText.SetActive(false);
        _scoreText.text = "Score: " + 0;

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is Null");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {

        _livesImg.sprite = _livesSprites[currentLives];
        if (currentLives < 1)
        {
            _gameManager.GameOver();
            _gameOverText.SetActive(true);
            _restartText.SetActive(true);
            StartCoroutine(GameOverTextFlicker());
        }
    }

    IEnumerator GameOverTextFlicker()
    {
        while (true)
        {
            _gameOverText.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            _gameOverText.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}