using UnityEngine;
using UnityEngine.UI;

public class MainMenuIntegration : MonoBehaviour
{
    public PlayerSelectionManager players;
    public MapSelectionManager maps;
    public StartGameController startGame;
    public GameObject characterPanel;
    public GameObject mapPanel;
    public GameObject summaryPanel;
    public Button characterContinue;
    public Text summaryPlayers;
    public Text summaryMap;

    private void Awake()
    {
        MatchConfiguration.Clear();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ContinueFromCharacters()
    {
        if (!players.PrepareMatchConfiguration()) return;
        characterPanel.SetActive(false);
        mapPanel.SetActive(true);
    }

    public void ChooseArena1() { maps.SelectMap1(); ShowSummary(); }
    public void ChooseArena2() { maps.SelectMap2(); ShowSummary(); }

    private void ShowSummary()
    {
        if (!MatchConfiguration.IsConfigured) return;
        maps.ContinueToStart();
        summaryPlayers.text = "Jugadores: " + MatchConfiguration.PlayerCount + "\nIniciar partida";
        summaryMap.text = "Mapa: " + MatchConfiguration.SelectedMapScene;
        mapPanel.SetActive(false);
        summaryPanel.SetActive(true);
    }

    public void BackToCharacters()
    {
        mapPanel.SetActive(false);
        characterPanel.SetActive(true);
    }

    public void BackToMaps()
    {
        summaryPanel.SetActive(false);
        mapPanel.SetActive(true);
    }

    public void StartMatch() { startGame.StartGame(); }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
