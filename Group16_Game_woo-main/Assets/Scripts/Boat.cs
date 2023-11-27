using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class AbilitySlot
{
    public int SlotID;
    public GameObject AbilityInstance;
    public bool IsEmpty;
    public Vector3Int SlotPosition;


    public AbilitySlot(int InSlotID, GameObject InAbilityInstance, Vector3Int InSlotPosition )
    {
        this.SlotID = InSlotID;
        this.AbilityInstance = InAbilityInstance;
        this.SlotPosition = InSlotPosition;
        this.IsEmpty = this.AbilityInstance == null;
    }
}

public class Boat : MonoBehaviour
{
    //ATM this is the same data just represented twice
    //Its so little data so it seems fine plus it makes all the other functions clean
    public List<AbilitySlot> AbilitySlots = new List<AbilitySlot>();
    public Dictionary<int, List<AbilitySlot>> AbilityGroups = new Dictionary<int, List<AbilitySlot>>();

    //Ability to use when making a new ability slot
    public GameObject DefaultAbilityPrefab;

    //represented as (width, length)
    //width should be mimimum 3 to allow for -1 = left, 0 = middle 1 = right
    //length counts up from 0 so a size of 3 means 4 guns at indices 0-3
    public Vector2Int BoatSize = new Vector2Int(3,8);
    public Vector2 HullLengthOriginOffset = new Vector2(.5f, 0.0f);


    public GameObject FrontMesh;
    public GameObject BackMesh;
    public GameObject BodyMesh;
    public float WorldSizePerGrid = 0.5f;

    int LastSlotID = 0;
    public Vector3 Velocity_DEPRECIATED = new Vector3();
    Vector3 LastFramePosition = new Vector3();
    private Rigidbody ParentRB;

    void Awake()
    {
        ParentRB = GetComponentInParent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        LastFramePosition = transform.position;
        int Length = BoatSize.y;
        BoatSize.y = -1;
        HullLengthOriginOffset.y = WorldSizePerGrid * Length / 2.0f;
        ChangeBoatSize(Length);
    }

    // Update is called once per frame
    void Update()
    {
        //Ideally we use a rb or something so we don't need this?
        Velocity_DEPRECIATED = (transform.parent.position - LastFramePosition) / Time.deltaTime;
        LastFramePosition = transform.parent.position;
    }

    public List<AbilitySlot> GetAbilitySlots()
    {
        List<AbilitySlot> slots = new List<List<AbilitySlot>>(AbilityGroups.Values).SelectMany(list => list).ToList();
        return slots;
    }

    public void ActivateAbilitiesByGroup(int AbilitySlotIndex)
    {
        List<AbilitySlot> slots;
        if(AbilityGroups.TryGetValue(AbilitySlotIndex, out slots))
        {
            foreach(AbilitySlot slot in slots)
            {
                if (slot.AbilityInstance == null)
                {
                    continue;
                }
                GameplayAbility Ability = slot.AbilityInstance.GetComponent<GameplayAbility>();
                if(Ability != null)
                {
                    Ability.ActivateAbility();
                }
            }
        }
    }

    public bool ChangeBoatSize(int DeltaSize, bool AddNewAbilitySlots=true, bool FillNewSlot = true)
    {
        HullLengthOriginOffset.y = WorldSizePerGrid * (BoatSize.y + DeltaSize) / 2.0f;
        for (int i = 0; i < DeltaSize; i++)
        {
            BoatSize += new Vector2Int(0, 1);
            if (!AddNewAbilitySlots)
                continue;
            AddNewAbilitySlot(new Vector3Int(1, 1, BoatSize.y), 1, FillNewSlot);
            AddNewAbilitySlot(new Vector3Int(-1, 1, BoatSize.y), 2, FillNewSlot);
        }
        UpdateHullMesh();
        return true;
    }

    public Vector3 GetVelocity()
    {
        if(ParentRB != null)
        {
            return ParentRB.velocity;
        }
        return Velocity_DEPRECIATED;
    }

    public bool AddAbilityToSlot(Vector3Int SlotCoords, GameObject Ability, bool bReplaceExistingAbility = true)
    {
        AbilitySlot slot = AbilitySlots.First<AbilitySlot>(slot => slot.SlotPosition == SlotCoords);
        if(slot != null)
        {
            if(!slot.IsEmpty)
            {
                Destroy(slot.AbilityInstance);
                slot.AbilityInstance = null;
            }
            slot.AbilityInstance = Ability;
            int f = SlotCoords.x < 0 ? -1 : 1;
            Vector3 Position = new Vector3(
                SlotCoords.x * BoatSize.x * WorldSizePerGrid - (HullLengthOriginOffset.x * f),
                0.0f,
                -HullLengthOriginOffset.y + SlotCoords.z * WorldSizePerGrid
            );
            Ability.transform.SetLocalPositionAndRotation(Position, new Quaternion());
            Ability.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            if (SlotCoords.x < 0)
            {
                Ability.transform.Rotate(0.0f, 180.0f, 0.0f);
            }
            return true;
        }
        return false;
    }

    public bool AddNewAbilitySlot(Vector3Int SlotPosition, int AbilitySlotGroup, bool FillAbilitySlot=true)
    {
        LastSlotID++;

        int f = SlotPosition.x < 0 ? -1 : 1;
        AbilitySlot AS;
        if(FillAbilitySlot && DefaultAbilityPrefab != null)
        {
            GameObject Ability = Instantiate(DefaultAbilityPrefab, transform, false);

            if(CompareTag("Player"))
            {
                Ability.tag = "Player";
            }
            else
            {
                Ability.tag = "Enemy";
            }

            Vector3 Position = new Vector3(
                SlotPosition.x * BoatSize.x * WorldSizePerGrid - (HullLengthOriginOffset.x * f),
                0.0f,
                -HullLengthOriginOffset.y + SlotPosition.z * WorldSizePerGrid);
            Ability.transform.SetLocalPositionAndRotation(Position, new Quaternion());
            Ability.transform.Rotate(0f, 90f, 0f);
            if (SlotPosition.x < 0)
            {
                Ability.transform.Rotate(0.0f, 180.0f, 0.0f);
            }
            AS = new AbilitySlot(LastSlotID++, Ability, SlotPosition);
        }
        else
        {
            AS = new AbilitySlot(LastSlotID++, null, SlotPosition);
        }
        

        AbilitySlots.Add(AS);
        if(AbilityGroups.ContainsKey(AbilitySlotGroup)) 
        {
            AbilityGroups[AbilitySlotGroup].Add(AS);
        }
        else
        {
            List<AbilitySlot> ASGroup = new List<AbilitySlot>();
            ASGroup.Add(AS);
            AbilityGroups.Add(AbilitySlotGroup, ASGroup);
        }
        return true;
    }

    public void UpdateHullMesh()
    {
        //Get Boat Size
        float Width = BoatSize.x * WorldSizePerGrid;
        float Length = BoatSize.y * WorldSizePerGrid;
        float HalfLength = Length / 2.0f;


        //Mesh FMesh = FrontMesh.GetComponent<MeshFilter>().mesh;
        //Mesh MiddleMesh = BodyMesh.GetComponent<MeshFilter>().mesh;
        //Mesh BMesh = BackMesh.GetComponent<MeshFilter>().mesh;

        //float ScaleFactor = Width  / FMesh.bounds.size.x;
        //float FrontOffset = FMesh.bounds.size.z / 2;
        //FrontMesh.transform.localScale = new Vector3(ScaleFactor, ScaleFactor, ScaleFactor);
        //FrontMesh.transform.localPosition = new Vector3(0, 0, HalfLength + FrontOffset);

        //ScaleFactor = Width / BMesh.bounds.size.x;
        //float BackOffset = BMesh.bounds.size.z / 2;
        //BackMesh.transform.localScale = new Vector3(ScaleFactor, ScaleFactor, ScaleFactor);
        //BackMesh.transform.localPosition = new Vector3(0,0, -HalfLength - BackOffset);

        //float ScaleFactorX = Width / MiddleMesh.bounds.size.x;
        //float ScaleFactorZ = Length / MiddleMesh.bounds.size.z;
        
        //BodyMesh.transform.localScale = new Vector3(ScaleFactorX, ScaleFactorX, ScaleFactorZ);

        foreach (AbilitySlot AS in AbilitySlots)
        {
            UpdateSlotPosition(AS);
        }
    }

    public void UpdateSlotPosition(AbilitySlot Slot)
    {
        GameObject Ability = Slot.AbilityInstance;
        if(Ability == null)
        {
            return;
        }
        int f = Slot.SlotPosition.x < 0 ? -1 : 1;
        Vector3 Position = new Vector3(
            Slot.SlotPosition.x * BoatSize.x * WorldSizePerGrid - (HullLengthOriginOffset.x * f),
            0.0f,
            -HullLengthOriginOffset.y + Slot.SlotPosition.z * WorldSizePerGrid);
        Ability.transform.SetLocalPositionAndRotation(Position, new Quaternion());
        Ability.transform.Rotate(0f, 90f, 0f);
        if (Slot.SlotPosition.x < 0)
        {
            Ability.transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

}
