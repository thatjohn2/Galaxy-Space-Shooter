using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _ammoText;

    [SerializeField]
    private Sprite[] _livesSprites;

    [SerializeField]
    private Image _livesImg;

    [SerializeField]
    private GameObject _gameOverText;

    [SerializeField]
    private GameObject _restartText;

    [SerializeField]
    private GameObject _chargeFillBar;

    [SerializeField]
    private GameObject _barStartObject;

    private Vector3 _barStart;
    private Vector3 _barScale;

    private RectTransform _barRect;

    private float _maxCharge = 100f;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameOverText.SetActive(false);
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Ammo: " + 15;
        _barRect = _chargeFillBar.GetComponent<RectTransform>();
        _barScale = _barRect.localScale;


        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is Null");
        }

        if (_barStartObject == null)
        {
            Debug.LogError("bar start object is NULL");
        }
        else
        {
            _barStart = _barStartObject.transform.position;
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateAmmo(int ammo)
    {
        _ammoText.text = "Ammo: " + ammo;
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

    public void UpdateCharge(float currentCharge)
    {
        _barRect.localScale = Vector3.Scale(new Vector3(currentCharge / _maxCharge, 1, 1), _barScale);
        Vector3[] corners = new Vector3[4];
        _barRect.GetWorldCorners(corners);
        float width = Mathf.Abs(corners[0].x - corners[3].x);
        _barRect.position = _barStart - new Vector3(width / 2, 0, 0);
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