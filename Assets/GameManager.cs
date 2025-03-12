using UnityEngine;
using FMODUnity; // Подключаем FMOD

public class GameManager : MonoBehaviour
{
    public static int PlayerScore1 = 0; // Счёт игрока 1
    public static int PlayerScore2 = 0; // Счёт игрока 2

    public GUISkin layout; // Стиль GUI
    [EventRef] public string winEvent = "event:/wins_pluq"; // Звук победы из FMOD
    [EventRef] public string counterEvent = "event:/counter"; // Звук начисления очков из FMOD

    private GameObject theBall; // Ссылка на мяч
    private bool playerOneWinSoundPlayed = false; // Флаг для звука победы игрока 1
    private bool playerTwoWinSoundPlayed = false; // Флаг для звука победы игрока 2

    void Start()
    {
        theBall = GameObject.FindGameObjectWithTag("Ball"); // Находим мяч по тегу
    }

    // Метод для начисления очков
    public static void Score(string wallID)
    {
        if (wallID == "RightWall")
        {
            PlayerScore1++; // Увеличиваем счёт игрока 1
            PlayCounterSound(); // Воспроизводим звук начисления очков
        }
        else
        {
            PlayerScore2++; // Увеличиваем счёт игрока 2
            PlayCounterSound(); // Воспроизводим звук начисления очков
        }
    }

    // Метод для воспроизведения звука начисления очков
    private static void PlayCounterSound()
    {
        if (!string.IsNullOrEmpty(GameManager.Instance.counterEvent))
        {
            RuntimeManager.PlayOneShot(GameManager.Instance.counterEvent); // Воспроизводим звук через FMOD
        }
        else
        {
            Debug.LogWarning("FMOD Counter Event не назначен!");
        }
    }

    void OnGUI()
    {
        GUI.skin = layout;

        // Отображаем счёт игроков
        GUI.Label(new Rect(Screen.width / 2 - 150 - 12, 20, 100, 100), "" + PlayerScore1);
        GUI.Label(new Rect(Screen.width / 2 + 150 + 12, 20, 100, 100), "" + PlayerScore2);

        // Кнопка рестарта
        if (GUI.Button(new Rect(Screen.width / 2 - 60, 35, 120, 53), "RESTART"))
        {
            ResetGame(); // Сбрасываем игру
        }

        // Проверяем победу игрока 1
        if (PlayerScore1 == 10)
        {
            DisplayWinMessage("PLAYER ONE WINS");
            PlayWinSound(ref playerOneWinSoundPlayed); // Воспроизводим звук победы
        }
        // Проверяем победу игрока 2
        else if (PlayerScore2 == 10)
        {
            DisplayWinMessage("PLAYER TWO WINS");
            PlayWinSound(ref playerTwoWinSoundPlayed); // Воспроизводим звук победы
        }
    }

    // Метод для сброса игры
    private void ResetGame()
    {
        PlayerScore1 = 0;
        PlayerScore2 = 0;
        playerOneWinSoundPlayed = false; // Сбрасываем флаг звука победы игрока 1
        playerTwoWinSoundPlayed = false; // Сбрасываем флаг звука победы игрока 2
        theBall.SendMessage("RestartGame", 0.5f, SendMessageOptions.RequireReceiver); // Перезапускаем мяч
    }

    // Метод для отображения сообщения о победе
    private void DisplayWinMessage(string message)
    {
        GUI.Label(new Rect(Screen.width / 2 - 150, 200, 2000, 1000), message);
        theBall.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver); // Останавливаем мяч
    }

    // Метод для воспроизведения звука победы
    private void PlayWinSound(ref bool soundPlayedFlag)
    {
        if (!soundPlayedFlag && !string.IsNullOrEmpty(winEvent))
        {
            RuntimeManager.PlayOneShot(winEvent); // Воспроизводим звук через FMOD
            soundPlayedFlag = true; // Устанавливаем флаг
        }
    }

    // Синглтон для доступа к GameManager из статических методов
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    Debug.LogError("GameManager не найден на сцене!");
                }
            }
            return _instance;
        }
    }
}