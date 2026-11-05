using System.Collections.Generic;
using UnityEngine;

public class SplitScreenConfiguration : MonoBehaviour
{
    private List<Camera> playerCameras = new List<Camera>();

    public void RegisterCamera(Camera playerCamera)
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("Se intento registrar una camara vacia.");
            return;
        }

        if (playerCameras.Contains(playerCamera))
        {
            Debug.LogWarning("Esta camara ya estaba registrada.");
            return;
        }

        playerCamera.enabled = true;
        playerCameras.Add(playerCamera);

        Debug.Log(
            "Camara registrada para el jugador " + playerCameras.Count
        );

        ConfigureSplitScreen();
    }

    public void ConfigureSplitScreen()
    {
        int playerCount = playerCameras.Count;

        if (playerCount == 0)
        {
            return;
        }

        if (playerCount == 1)
        {
            playerCameras[0].rect = new Rect(0f, 0f, 1f, 1f);
        }
        else if (playerCount == 2)
        {
            playerCameras[0].rect = new Rect(0f, 0f, 0.5f, 1f);
            playerCameras[1].rect = new Rect(0.5f, 0f, 0.5f, 1f);
        }
        else if (playerCount == 3)
        {
            playerCameras[0].rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
            playerCameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            playerCameras[2].rect = new Rect(0f, 0f, 1f, 0.5f);
        }
        else if (playerCount == 4)
        {
            playerCameras[0].rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
            playerCameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            playerCameras[2].rect = new Rect(0f, 0f, 0.5f, 0.5f);
            playerCameras[3].rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
        }

        foreach (Camera camera in playerCameras)
        {
            foreach (Camera child in camera.GetComponentsInChildren<Camera>(true))
                if (child != camera) child.rect = camera.rect;
        }

        Debug.Log("Split-screen actualizado: " + playerCount + " jugadores.");
    }
}
