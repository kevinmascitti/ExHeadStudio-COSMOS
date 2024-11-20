using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AccessoryType
{
    Bracelet,
    Medallion,
    Ring
}

public class Accessory : MonoBehaviour
{
    public AccessoryType type;
    public Element element;
    public Stats bonus;
}
