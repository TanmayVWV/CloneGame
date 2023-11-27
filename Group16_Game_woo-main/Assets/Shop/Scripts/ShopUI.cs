using Player_Core;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ShopUI : MonoBehaviour
{
    [SerializeField] List<ShopWeapon> AvailableWeapons;
    [SerializeField] Transform WeaponRoot;
    [SerializeField] GameObject WeaponButtonPrefab;
    
    [SerializeField] int MaxWeapons;
    [SerializeField] int MinWeapons;

    [SerializeField] List<ShopUpgrade> AvailableUpgrades;
    [SerializeField] Transform UpgradeRoot;
    [SerializeField] GameObject UpgradeButtonPrefab;

    [SerializeField] Transform StatsRoot;
    [SerializeField] GameObject StatPrefab;
    [SerializeField] StatManager statManager;

    [SerializeField] LayerMask layerMask;

    [SerializeField] Material defaultSlotMat;
    [SerializeField] Material selectedSlotMat;

    [SerializeField] Transform Left;
    [SerializeField] Transform Water;
    [SerializeField] float WaterHeight;

    [SerializeField] GameObject gridManagerPrefab;

    [SerializeField] Camera playerCam;
    [SerializeField] float CannonDistFromCamera;

    [SerializeField] TextMeshProUGUI playerMoneyText;

    private GameObject cannonPlayer;
    private GameObject cannonUI;
    private Renderer selectedSlot;

    private GameObject player;
    private GameObject boatObject;
    private Boat boat;
    private Vector3 defaultScale;

    private List<Shop_StatText> statsList = new List<Shop_StatText>();

    private GameObject gridManager;
    private Shop_SlotManager gridManagerScript;

    private Rigidbody rb;

    private void Awake()
    { }

    void Start()
    {

        player = GameManager.instance.player;
        if (player == null)
        {
            Debug.LogError("Player Not Found");
        }
        //player.GetComponent<PlayerMovement>().currentSpeed = 0;
        rb = player.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        boatObject = player.transform.Find("Boat").gameObject;
        boat = player.GetComponentInChildren<Boat>();

        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        player.transform.position = new Vector3(0, 0, 0);

        defaultScale = player.transform.localScale;
        player.transform.localScale = new Vector3(1, 1, 1);

        playerCam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 10, player.transform.position.z);
        gridManager = Instantiate(gridManagerPrefab);

        statManager = player.GetComponent<StatManager>();
        SetFunds();
        RefreshWeapons();
        RefreshUpgrades();
        createWeaponSlots();
        SetUpStats();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void RefreshWeapons()
    {
        int numWeapons = Random.Range(MinWeapons, MaxWeapons);

        for (int i = 0;  i < numWeapons; i++)
        {
            int randomIndex = Random.Range(0, AvailableWeapons.Count);
            ShopWeapon weapon = AvailableWeapons[randomIndex];

            var weaponObject = Instantiate(WeaponButtonPrefab, WeaponRoot);
            var weaponUI = weaponObject.GetComponent<Shop_WeaponButton>();

            weaponUI.Bind(weapon, OnBeginDrag, OnDrag, OnEndDrag);
        }
    }

    void OnBeginDrag(ShopWeapon weapon)
    {
        cannonPlayer = Instantiate(weapon.WeaponObject, transform);
        cannonUI = Instantiate(weapon.WeaponObject, transform);

        Vector3 mousePosPlayer = Input.mousePosition;
        Vector3 mousePosUI = Input.mousePosition;

        float distanceFromCamera = CannonDistFromCamera;

        mousePosPlayer.z = distanceFromCamera;
        mousePosUI.z = distanceFromCamera;

        Vector3 worldPositionPlayer = Camera.main.ScreenToWorldPoint(mousePosPlayer);
        Vector3 worldPositionUI = playerCam.ScreenToWorldPoint(mousePosUI);

        cannonPlayer.transform.position = worldPositionPlayer;
        cannonUI.transform.position = worldPositionUI;

        Quaternion startingRotation = Quaternion.Euler(0, 90, 0);
        cannonPlayer.transform.rotation = startingRotation;
        cannonUI.transform.rotation = startingRotation;

        Vector3 startingScale = new Vector3(5, 5, 5);
        cannonPlayer.transform.localScale = startingScale;
        cannonUI.transform.localScale = startingScale;
    }

    void OnDrag(ShopWeapon weapon)
    {
        if (cannonPlayer != null && cannonUI != null)
        {

            Vector3 mousePosPlayer = Input.mousePosition;
            Vector3 mousePosUI = Input.mousePosition;

            float distanceFromCamera = CannonDistFromCamera;

            mousePosPlayer.z = distanceFromCamera;
            mousePosUI.z = distanceFromCamera;

            Vector3 worldPositionPlayer = Camera.main.ScreenToWorldPoint(mousePosPlayer);
            Vector3 worldPositionUI = playerCam.ScreenToWorldPoint(mousePosUI);

            cannonPlayer.transform.position = worldPositionPlayer;
            cannonUI.transform.position = worldPositionUI;


            Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 200, layerMask))
            {
                resetItemSlot();
                selectedSlot = hit.collider.GetComponent<Renderer>();
                if (selectedSlot != null)
                {
                    selectedSlot.material = selectedSlotMat;
                    string[] strSlot = hit.collider.name.Split(",");
                    Vector3Int coords = new Vector3Int(int.Parse(strSlot[0]), 1, int.Parse(strSlot[1]));

                    if (coords[0] < 0)
                    {
                        cannonPlayer.transform.rotation = Quaternion.Euler(0, -90, 0);
                        cannonUI.transform.rotation = Quaternion.Euler(0, -90, 0);
                    }
                    else
                    {
                        cannonPlayer.transform.rotation = Quaternion.Euler(0, 90, 0);
                        cannonUI.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                }
            }
            else
            {
                resetItemSlot();
            }
        }
    }

    void OnEndDrag(GameObject weaponButton, ShopWeapon shopWeapon)
    {
        Destroy(cannonPlayer);
        //Destroy(cannonUI);
        // raycast from cam to mouse
        // if hit slot
        // get name of slot and destroy button
        resetItemSlot();

        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200, layerMask))
        {
            if (GameManager.instance.PlayerMoney - shopWeapon.Cost >= 0)
            {
                GameManager.instance.PlayerMoney -= shopWeapon.Cost;
                SetFunds();

                string[] strSlot = hit.collider.name.Split(",");
                Vector3Int coords = new Vector3Int(int.Parse(strSlot[0]), 1, int.Parse(strSlot[1]));

                cannonUI.transform.SetParent(boatObject.transform);
                cannonUI.transform.localScale = Vector3.one;

                boat.AddAbilityToSlot(coords, cannonUI);

                Destroy(weaponButton);
                return;
            }
        }
        Destroy(cannonUI);
    }

    private void resetItemSlot()
    {
        if (selectedSlot != null)
        {
            selectedSlot.material = defaultSlotMat;
            selectedSlot = null;
        }
    }

    void RefreshUpgrades()
    {
        AvailableUpgrades.Sort((upgrade1, upgrade2) => upgrade1.index.CompareTo(upgrade2.index));
        foreach (var upgrade in AvailableUpgrades)
        {
            var upgradeObject = Instantiate(UpgradeButtonPrefab, UpgradeRoot);
            var upgradeUI = upgradeObject.GetComponent<Shop_UpgradeButton>();

            upgradeUI.Bind(upgrade, OnUpgradeClicked);
        }
    }

    void OnUpgradeClicked(ShopUpgrade newUpgrade)
    {
        bool worked = false;
        if (GameManager.instance.PlayerMoney - newUpgrade.Cost >= 0)
        {
            switch (newUpgrade.Name)
            {
                case "Size":
                    if (statManager.IncrementSize(newUpgrade.Increment))
                    {
                        boat.ChangeBoatSize(newUpgrade.Increment, true, false);
                        gridManagerScript.ReSize();
                        statsList[newUpgrade.index].Increment(newUpgrade.Increment);
                        worked = true;
                    };
                    break;
                case "Health":
                    if (statManager.IncrementMaxHealth(newUpgrade.Increment))
                    {
                        statsList[newUpgrade.index].Increment(newUpgrade.Increment);
                        worked = true;
                    };
                    break;
                case "Speed":
                    if (statManager.IncrementSpeed(newUpgrade.Increment))
                    {
                        statsList[newUpgrade.index].Increment(newUpgrade.Increment);
                        worked = true;
                    }
                    break;
                case "Handling":
                    if (statManager.IncrementHandling(newUpgrade.Increment))
                    {
                        statsList[newUpgrade.index].Increment(newUpgrade.Increment);
                        worked = true;
                    }
                    break;
            }

            if (worked)
            {
                GameManager.instance.PlayerMoney -= newUpgrade.Cost;
                SetFunds();
            }
        }

    }

    private void SetUpStats()
    {
        foreach (var upgrade in AvailableUpgrades)
        {
            GameObject newStatsText = Instantiate(StatPrefab);
            Shop_StatText statsTextScript = newStatsText.GetComponent<Shop_StatText>();
            statsList.Add(statsTextScript);

            newStatsText.transform.SetParent(StatsRoot, false);
            int stat = 0;
            switch(upgrade.Name)
            {
                case "Size":
                    stat = statManager.GetSize();
                    break;
                case "Health":
                    stat = (int)statManager.GetMaxHealth();
                    break;
                case "Speed":
                    stat = (int)statManager.GetSpeed();
                    break;
                case "Handling":
                    stat = (int)statManager.GetHandling();
                    break;
            }
            statsTextScript.Bind(upgrade.Name, stat);
        }
    }

    public void OnContinueClick()
    {
        Destroy(gridManager);
        // load next scene
        player.transform.localScale = defaultScale;
        statManager.IncrementHealth(statManager.GetMaxHealth());
        GameManager.instance.ChangeScene(GameManager.Scenes.Prototype_level);
    }

    private void createWeaponSlots()
    {
        gridManager.transform.SetParent(boat.transform, false);
        gridManager.transform.position = new Vector3(0, 0, 0);
        gridManager.transform.localPosition = new Vector3(0, 1, 0);
        gridManagerScript = gridManager.GetComponent<Shop_SlotManager>();
        gridManagerScript.ReSize();
    }

    private void SetFunds()
    {
        int amount = GameManager.instance.PlayerMoney;
        string amountText = amount.ToString("C0", CultureInfo.GetCultureInfo("en-US"));

        playerMoneyText.text = amountText;
    }
}
