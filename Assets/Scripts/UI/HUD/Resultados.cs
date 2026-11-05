using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Resultados : MonoBehaviour
{
    [Header("Referencias")]
    public MatchManager matchManager;
    public GameObject resultsPanel;
    public Text winnerText;
    public Text[] positionTexts;
    public Text[] playerTexts;
    public Text[] killsTexts;
    public Text[] deathsTexts;

    private void Awake()
    {
        if (resultsPanel != null)
        {
            resultsPanel.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (matchManager != null)
        {
            matchManager.MatchEnded += ShowResults;
        }
    }

    private void OnDisable()
    {
        if (matchManager != null)
        {
            matchManager.MatchEnded -= ShowResults;
        }
    }

    private void ShowResults(PlayerStats winner)
    {
        if (resultsPanel == null)
        {
            Debug.LogError("No se ha asignado Results Panel.");
            return;
        }

        resultsPanel.SetActive(true);

        if (winnerText != null)
        {
            if (winner != null)
            {
                winnerText.text = "GANADOR: " + winner.name;
            }
            else
            {
                winnerText.text = "EMPATE";
            }
        }

        UpdateResults();
    }

    private void UpdateResults()
    {
        if (matchManager == null)
        {
            return;
        }

        PlayerStats[] players = matchManager.GetPlayers();

        // Ordenar jugadores por eliminaciones.
        for (int i = 0; i < players.Length - 1; i++)
        {
            for (int j = 0; j < players.Length - 1 - i; j++)
            {
                if (players[j].Kills < players[j + 1].Kills)
                {
                    PlayerStats temp = players[j];
                    players[j] = players[j + 1];
                    players[j + 1] = temp;
                }
            }
        }

        for (int i = 0; i < positionTexts.Length; i++)
        {
            bool playerExists = i < players.Length;

            positionTexts[i].gameObject.SetActive(playerExists);
            playerTexts[i].gameObject.SetActive(playerExists);
            killsTexts[i].gameObject.SetActive(playerExists);
            deathsTexts[i].gameObject.SetActive(playerExists);

            if (playerExists)
            {
                positionTexts[i].text = (i + 1).ToString();
                playerTexts[i].text = players[i].name;
                killsTexts[i].text = players[i].Kills.ToString();
                deathsTexts[i].text = players[i].Deaths.ToString();
            }
        }
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;

        int currentScene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentScene, LoadSceneMode.Single);
    }
}
