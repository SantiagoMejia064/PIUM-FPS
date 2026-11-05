using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public static class IntegrationValidation
{
    private static readonly List<string> errors = new List<string>();
    private static int checkedObjects;

    [MenuItem("Tools/PIUM/Validate Integration")]
    public static void Run()
    {
        errors.Clear();
        checkedObjects = 0;
        var setup = EditorSceneManager.GetSceneManagerSetup();
        try
        {
            string[] scenes = { "MainMenu", "Arena1", "Arena2", "Loading", "MainMenu_Respaldo", "Imported/MainMenu_Profesor" };
            foreach (string name in scenes)
            {
                var scene = EditorSceneManager.OpenScene("Assets/Scenes/" + name + ".unity", OpenSceneMode.Single);
                foreach (GameObject root in scene.GetRootGameObjects()) CheckHierarchy(root);
                if (name == "MainMenu")
                {
                    Require(UnityEngine.Object.FindObjectsByType<EventSystem>(FindObjectsSortMode.None).Length == 1, "MainMenu: EventSystem duplicado o ausente");
                    foreach (var button in UnityEngine.Object.FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                    {
                        if (!button.interactable) continue;
                        if (button.onClick.GetPersistentEventCount() == 0)
                            Debug.LogWarning("Boton conservado con eventos originales pendientes: " + button.name);
                        for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
                            Require(button.onClick.GetPersistentTarget(i) != null && !string.IsNullOrEmpty(button.onClick.GetPersistentMethodName(i)), "Evento invalido: " + button.name);
                    }
                    var selection = UnityEngine.Object.FindFirstObjectByType<PlayerSelectionManager>(FindObjectsInactive.Include);
                    Require(selection != null, "Falta PlayerSelectionManager");
                    if (selection != null)
                        for (int i = 0; i < 4; i++)
                            Require(selection.playerPanels[i] != null && selection.playerImages[i] != null && selection.characterSprites[i] != null && selection.characterPrefabs[i] != null, "Seleccion incompleta: " + i);
                }
                if (name.StartsWith("Arena"))
                {
                    var spawner = UnityEngine.Object.FindFirstObjectByType<PlayerSpawner>();
                    Require(spawner != null && spawner.GetComponent<SplitScreenConfiguration>() != null, name + ": falta spawner o split-screen");
                    if (spawner != null) Require(spawner.spawnPoints.Length >= 4 && Array.TrueForAll(spawner.spawnPoints, p => p != null), name + ": spawnPoints incompletos");
                }
            }
            foreach (string assetGuid in AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs" }))
            {
                string p = AssetDatabase.GUIDToAssetPath(assetGuid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(p);
                CheckHierarchy(prefab);
                if (p.Contains("/Prefab" ) && p.Contains("/Players/") && !p.EndsWith("Cube.prefab"))
                {
                    PlayerInput input = prefab.GetComponent<PlayerInput>();
                    Require(input != null && input.actions != null, p + ": falta PlayerInput/actions");
                    Require(prefab.GetComponent<PlayerMovement>() != null, p + ": falta movimiento");
                    Require(prefab.GetComponent<ArmasController>().startingWeaponPrefab != null, p + ": falta arma");
                }
            }
            foreach (string p in Directory.GetFiles("Assets/Scripts", "*.cs", SearchOption.AllDirectories))
            {
                MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(p.Replace('\\', '/'));
                if (File.ReadAllText(p).Contains(": MonoBehaviour"))
                    Require(script != null && script.GetClass() != null, "Clase no importable: " + p);
            }
            Require(EditorBuildSettings.scenes[0].path == "Assets/Scenes/MainMenu.unity" && EditorBuildSettings.scenes[0].enabled, "Escena inicial incorrecta");
            Require(LayerMask.LayerToName(6) == "Weapon", "Falta capa Weapon");
        }
        catch (Exception e) { errors.Add(e.ToString()); }
        finally { EditorSceneManager.RestoreSceneManagerSetup(setup); }
        string report = "Unity " + Application.unityVersion + "\nObjetos revisados: " + checkedObjects + "\nErrores: " + errors.Count + "\n" + string.Join("\n", errors);
        Directory.CreateDirectory("IntegrationReports");
        File.WriteAllText("IntegrationReports/unity-validation.txt", report);
        Debug.Log(report);
        if (Application.isBatchMode) EditorApplication.Exit(errors.Count == 0 ? 0 : 1);
    }

    private static void Require(bool condition, string error) { if (!condition) errors.Add(error); }
    private static void CheckHierarchy(GameObject root)
    {
        foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
        {
            checkedObjects++;
            Require(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(child.gameObject) == 0, "Missing Script: " + child.name);
            foreach (Component component in child.GetComponents<Component>())
            {
                if (component == null) continue;
                SerializedObject serialized = new SerializedObject(component);
                var property = serialized.GetIterator();
                while (property.NextVisible(true))
                    if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue == null && property.objectReferenceInstanceIDValue != 0)
                        errors.Add("Referencia perdida: " + child.name + "/" + component.GetType().Name + "." + property.propertyPath);
            }
        }
    }
}
