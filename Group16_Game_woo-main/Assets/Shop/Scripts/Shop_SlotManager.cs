using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class Shop_SlotManager : MonoBehaviour
{
    [SerializeField] GameObject slot;

    private GameObject boat;
    private Boat boatScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ReSize()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        boat = transform.parent.gameObject;
        boatScript = boat.GetComponent<Boat>();
        Vector2 lengthOffset = boatScript.HullLengthOriginOffset;
        float SlotSize = boatScript.WorldSizePerGrid;
        int size = boatScript.BoatSize.y + 1;

        //Left Side
        for (int i = size - 1; i >= 0; i--)
        {
            float x = -1;
            float y = (i * SlotSize) - lengthOffset.y;

            Vector3 pos = new Vector3(x, 0, y);
            Vector3 Offset = new Vector3(-SlotSize/2, -lengthOffset.x + 0.01f, 0);

            var slotObject = Instantiate(slot, transform);
            slotObject.transform.position = Vector3.zero;
            slotObject.transform.localPosition = pos + Offset;
            slotObject.name = $"{x},{i}";
            slotObject.transform.localScale = new Vector3(0.045f, 0.045f, 0.045f);
        }
        //Right Side
        for (int i = size - 1; i >= 0; i--)
        {
            float x = 1;
            float y = (i * SlotSize) - lengthOffset.y;

            Vector3 pos = new Vector3(x, 0, y);
            Vector3 Offset = new Vector3(SlotSize/2, -lengthOffset.x + 0.01f, 0);

            var slotObject = Instantiate(slot, transform);
            slotObject.transform.position = Vector3.zero;
            slotObject.transform.localPosition = pos + Offset;
            slotObject.name = $"{x},{i}";
            slotObject.transform.localScale = new Vector3(0.045f, 0.045f, 0.045f);

        }
    }

}
