
using UnityEngine;
using UnityEngine.UI;

public class MapSelectionManager : MonoBehaviour
{
    [Header("Boton Continuar")]
    public Button continueButton;
    
    public void SelectMap1()
    {
        SelectMap("Arena1");
    }

    public void SelectMap2()
    {
        SelectMap("Arena2");
    }

    private void SelectMap(string sceneName)
    {
        if (!MatchConfiguration.HasValidPlayers)
        {
            Debug.LogWarning("Primero debes confirmar los personajes.");

            return;
        }

        bool saved = MatchConfiguration.TrySetMap(sceneName);

        if (!saved)
        {
            Debug.LogError("No se pudo guardar el mapa.");
            return;
        }

        if (continueButton != null)
        {
            continueButton.interactable = true;
        }

        Debug.Log("Mapa seleccionado: " + sceneName);
    }

    public void ContinueToStart()
    {
        if (!MatchConfiguration.HasValidPlayers)
        {
            Debug.LogWarning("No hay una seleccion valida de personajes.");

            return;
        }

        if (string.IsNullOrEmpty(MatchConfiguration.SelectedMapScene))
        {
            Debug.LogWarning("Debes seleccionar un mapa antes de continuar.");

            return;
        }

        Debug.Log("Resumen listo para mostrar.");
    }
}
