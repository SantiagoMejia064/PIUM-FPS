using UnityEngine;
using UnityEngine.UI;

public class SummaryUI : MonoBehaviour
{
    [Header("Textos del resumen")]
    public Text playerCountText;
    public Text mapNameText;

    private void OnEnable()
    {
        UpdateSummary();
    }

    public void UpdateSummary()
    {
        if (playerCountText != null)
        {
            playerCountText.text = "Cantidad de jugadores: " + MatchConfiguration.PlayerCount + " de 4";
        }

        if (mapNameText != null)
        {
            string selectedMap = MatchConfiguration.SelectedMapScene;

            if (selectedMap == "Arena1")
            {
                mapNameText.text = "Mapa seleccionado: Arena 1";
            }
            else if (selectedMap == "Arena2")
            {
                mapNameText.text = "Mapa seleccionado: Arena 2";
            }
            else
            {
                mapNameText.text = "Mapa seleccionado: Sin seleccionar";
            }
        }
    }
}
