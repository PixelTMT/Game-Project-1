using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class game_manager : MonoBehaviour
{
    [SerializeField] UIChangeManage _UIC;
    [SerializeField] Player_Control _player;
    [SerializeField] public PlayerUI _pUI;
    [SerializeField] public GameOverUI _oUI;
    [SerializeField] GameObject backLightPauseUI;

    [HideInInspector] public bool isPause = false;
    [HideInInspector] public bool isGameOver = false;
    private void Start()
    {
        if (backLightPauseUI != null) backLightPauseUI.SetActive(false);
    }
    private void Update()
    {
        if (isGameOver) return;
        _pUI.UpdateHP(_player._live);
        _pUI.UpdateScore(_player._score);
        PauseManage();
    }
    void PauseManage()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausing();
        }
    }
    public void pausing()
    {
        if (!isPause)
        {
            isPause = true;
            Time.timeScale = 0;
            _UIC.OpenUIName("Pause");
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            if (backLightPauseUI != null) backLightPauseUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            isPause = false;
            _UIC.OpenUIName("");
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            if (backLightPauseUI != null) backLightPauseUI.SetActive(false);
        }
    }

    public void GameOver()
    {
        _UIC.OpenUIName("GameOver");
        _oUI.UpdateGameOver("Game Over");
        _oUI.UpdateScore(_player._score);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        _player.enabled = false;
        isGameOver = true;
        _pUI.UpdateHP(0);
    }
    public void GameFinish()
    {
        _UIC.OpenUIName("GameOver");
        _oUI.UpdateGameOver("Congrats");
        _oUI.UpdateScore(_player._score, _player.dateTime);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        _player.enabled = false;
        isGameOver = true;
    }

    [Serializable]
    public class PlayerUI
    {
        public TextMeshProUGUI HP_Text;
        public TextMeshProUGUI Score_Text;
        public void UpdateHP(int n)
        {
            HP_Text.text = $"{n}x";
        }
        public void UpdateScore(int n)
        {
            Score_Text.text = $"Score: {n}";
        }
    }
    [Serializable]
    public class GameOverUI
    {
        public TextMeshProUGUI GameOver_Text;
        public TextMeshProUGUI Score_Text;
        public void UpdateGameOver(string msg)
        {
            GameOver_Text.text = $"{msg}";
        }
        public void UpdateScore(int score)
        {
            Score_Text.text = $"Score: {score}";
        }
        public void UpdateScore(int score, DateTime time)
        {
            Score_Text.text = $"Score: {score}\nTime: {DateTime.Now - time}";
        }
    }
}