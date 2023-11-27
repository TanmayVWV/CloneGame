/** Contributors: Jordan Planchot 
 *                Zackary Mowbray**/

using System;
using Cinemachine;
using Player_Core;
using QuestPointer;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField] GameObject objectiveShipPrefab;
    public GameObject objectiveShip;
    [SerializeField] GameObject objectiveIslandPrefab;
    public GameObject objectiveIsland;
    public GameObject objectiveCurrent;
    public int PlayerMoney;

    private CinemachineFreeLook BoatCamera;
    [SerializeField] CinemachineFreeLook BoatCameraPrefab;

    public GameObject player;
    [SerializeField] GameObject playerPrefab;

    // Boolean to track if player has killed the boss yet
    public Boolean bossKilled;
    public enum Scenes
    {
        Prototype_level,
        ShopScene
    }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += (currentScene, nextScene) => HandleSceneChange(nextScene);
        player = Instantiate(playerPrefab);
        SetupCam();
        CreateObjectives();
    }

    private void CreateObjectives()
    {
        if ( objectiveShip == null && objectiveIsland == null)
        {
            Vector3 shipPos = new Vector3(72.5f, 0, 24.4f);

            objectiveShip = Instantiate(objectiveShipPrefab, shipPos, Quaternion.identity);

            Vector3 islandPos = new Vector3(-73.7f, 0, 195.3f);

            objectiveIsland = Instantiate(objectiveIslandPrefab, islandPos, Quaternion.identity);

            objectiveCurrent = objectiveShip;
        }


    }

    private void SetupCam()
    {
        if (BoatCamera == null && player != null)
        {
            CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
            BoatCamera = Instantiate(BoatCameraPrefab);
            BoatCamera.Follow = player.transform;
            BoatCamera.LookAt = player.transform;
            cameraFollow.cam = BoatCamera;
        }
    }

    public void HandleDeath()
    {
        if (player != null)
        {
            Destroy(player);
        }
        PlayerMoney = 0;
        player = Instantiate(playerPrefab);
        ChangeScene(Scenes.Prototype_level);

    }

    private void HandleSceneChange(Scene scene)
    {
        string sceneName = scene.name;
        GameObject questUI = player.transform.Find("UI_QuestPointer").gameObject;
        //PlayerMovement playerMove = player.GetComponent<PlayerMovement>();
        movement2 playerMove = player.GetComponent<movement2>();
        switch (sceneName)
        {

            case "ShopScene":
                // turn quest ui off
                questUI.SetActive(false);

                // disable movement
                playerMove.DisableControls();
                break;

            case "Prototype_level":
                // turn quest ui on
                questUI.SetActive(true);

                // enable movement
                playerMove.EnableControls();

                //create objectives
                CreateObjectives();

                // reset quest pointer
                GameObject questPointer = player.transform.Find("UI_QuestPointer").gameObject;
                RotateQuestPointer pointerScript = questPointer.GetComponent<RotateQuestPointer>();
                pointerScript.objective = objectiveShip;
                bossKilled = false;

                // create camera
                SetupCam();
                break;
        }
    }
    public void ChangeScene(Scenes sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }
}
