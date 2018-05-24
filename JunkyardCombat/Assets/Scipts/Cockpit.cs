using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cockpit : Part {
    [SerializeField] private Player player;
    Player Owner {
        get {
            return player;
        }
    }

}
