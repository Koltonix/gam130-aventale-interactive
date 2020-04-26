using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatGame.Units;
using CatGame.Controls;
using CatGame.ControlScheme;
using CatGame.Data;
using CatGame.Tiles;
using CatGame.UI;

namespace CatGame.Combat
{
    public class Attacker : MonoBehaviour
    {
        [Header("Attack Stats")]
        public int Damage;
        public int AttackRange = 12;
        public int AttackAP = 2;
        public bool canAttackBuildings = true;

        [Header("(DEBUG) Current Targeted Transform")]
        public Transform enemy;
    }
}