using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CargoType {
    Unknown,
    Wood,
    Metal,
    Mixed
}

public class Cargo : MonoBehaviour {

    public CargoType type = CargoType.Wood;
    
}
