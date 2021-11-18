using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage state and step of the game
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject warmUpPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject coinScore;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text coinValue;

    [System.NonSerialized] public int actualCoin = 0;
    
    #region Singleton

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    #endregion

    
    /// this enum is usually use by a lot of script and it's not a good way to go cause even if it can put all script at the same pace,
    /// it can also become a hindrance to future modifications or debugging
    public enum GAMEPHASE
    {
        WARM_UP,
        RUN,
        PUNCHED,
        GAMEOVER,
        SCORE
    }
    public GAMEPHASE actualPhase = GAMEPHASE.WARM_UP;

    // Update is called once per frame
    void Update()
    {
        coinText.text = actualCoin.ToString();
    }

    //on the first touch of the game, the camera go to it's position to start the run
    public void ReadyCamera()
    {
        CameraFollow.Instance.GoToRacePosition();
    }

    public void StartGame()
    {
        actualPhase = GAMEPHASE.RUN;
        warmUpPanel.SetActive(false);
        coinScore.SetActive(true);
        PlayerScript.Instance.BeginRaceAnimation();
    }

    public void GameOver()
    {
        actualPhase = GAMEPHASE.GAMEOVER;
        coinScore.SetActive(false);
        gameOverPanel.SetActive(true);
        coinValue.text = actualCoin.ToString();
    }

    public void Punched()
    {
        actualPhase = GAMEPHASE.PUNCHED;
        StartCoroutine(PunchedCoroutine());
    }

    //When punched, we wait that the player has end his fall to finish the run
    public IEnumerator PunchedCoroutine()
    {
        yield return new WaitForSeconds(5f);
        actualPhase = GAMEPHASE.GAMEOVER;
        GameOver();
    }

    public void AddCoin()
    {
        actualCoin++;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
