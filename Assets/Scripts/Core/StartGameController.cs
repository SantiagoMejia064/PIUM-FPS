
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameController : MonoBehaviour
{
    public void StartGame()
    {
        if (!MatchConfiguration.IsConfigured)
        {
            Debug.LogWarning("La partida no está configurada correctamente.");

            return;
        }

        string sceneName = MatchConfiguration.SelectedMapScene;

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError("No se puede cargar la escena: " + sceneName +". Comprueba que esté incluida en Build Profiles.");

            return;
        }

        Debug.Log("Cargando partida en: " + sceneName);

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
