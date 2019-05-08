using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratePile : MonoBehaviour {

    [SerializeField]
    CargoType cargo = CargoType.Unknown;
    [SerializeField]
    int amount = 0;

    Animator anim;

    public int cargoType { get { return (int)cargo; } }

    void Start () {
        anim = GetComponent<Animator> ();
    }

    void Update () {
        if (anim) {
            anim.SetInteger ("Amount", amount);
        }
    }

    void IncreasePile (CargoType type) {
        if (cargo == CargoType.Unknown || amount == 0) {
            cargo = type;
        } else if (cargo != type) {
            cargo = CargoType.Mixed;
        }

        amount++;
    }

    public int LoadToTruck (int capacity) {
        int amt = Mathf.Min (amount, capacity);
        amount -= amt;
        return amt;
    }

    void OnTriggerEnter2D (Collider2D col) {
        if (col.CompareTag("Product")) {
            CargoType cargoT = col.GetComponent<Cargo> () != null ?
                               col.GetComponent<Cargo> ().type : CargoType.Unknown;
            IncreasePile (cargoT);
            Destroy (col.gameObject);
        }
    }
}
