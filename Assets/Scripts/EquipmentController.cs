using System.Collections.Generic;
using UnityEngine;

public enum Hand {
    Undetermined,
    Right,
    Left
}

public class EquipmentController : MonoBehaviour {

    [SerializeField]
    Transform handR;
    [SerializeField]
    Transform handL;

    GunController gunR, gunL;
    List<GunController> availableGuns = new List<GunController> ();

    void Start () {

    }

    void Update () {
        gunR = handR.GetComponentInChildren<GunController> ();
        gunL = handL.GetComponentInChildren<GunController> ();

        for (int i = availableGuns.Count - 1; i >= 0; i--) {
            if (availableGuns[i] == null || availableGuns[i].equipped) {
                availableGuns.RemoveAt (i);
            }
        }

        if (Input.GetKey (KeyCode.LeftShift)) {
            Hand hand = Hand.Undetermined;
            if (Input.GetButtonDown ("Fire1")) {
                hand = Hand.Left;
            } else if (Input.GetButtonDown ("Fire2")) {
                hand = Hand.Right;
            }

            if (hand != Hand.Undetermined) {
                if (IsEquipped (hand)) {
                    Unequip (hand);
                } else if (availableGuns.Count > 0) {
                    Equip (availableGuns[0], hand);
                }
            }
        }
    }

    public bool IsEquipped (Hand hand) {
        switch (hand) {
            case Hand.Right:
                return gunR != null;
            case Hand.Left:
                return gunL != null;
        }
        return false;
    }

    void Equip (GunController gun, Hand hand) {
        switch (hand) {
            case Hand.Right:
                gun.BeEquipped (handR);
                break;
            case Hand.Left:
                gun.BeEquipped (handL);
                break;
        }
    }

    void Unequip (Hand hand) {
        switch (hand) {
            case Hand.Right:
                if (gunR) {
                    gunR.BeDropped ();
                }
                break;
            case Hand.Left:
                if (gunL) {
                    gunL.BeDropped ();
                }
                break;
        }
    }

    public void Fire (Hand hand) {
        switch (hand) {
            case Hand.Right:
                if (gunR) {
                    gunR.Fire ();
                }
                break;
            case Hand.Left:
                if (gunL) {
                    gunL.Fire ();
                }
                break;
        }
    }

    void OnTriggerEnter2D (Collider2D col) {
        if (col.CompareTag ("Equippable")) {
            GunController gun = col.GetComponent<GunController> ();
            if (gun && !availableGuns.Contains (gun)) {
                availableGuns.Add (gun);
            }
        }
    }

    void OnTriggerExit2D (Collider2D col) {
        if (col.CompareTag ("Equippable")) {
            GunController gun = col.GetComponent<GunController> ();
            if (gun && availableGuns.Contains (gun)) {
                availableGuns.Remove (gun);
            }
        }
    }
}
