using UnityEngine;

namespace CatGame.Combat
{
    public class Attacker : MonoBehaviour
    {
        [Header("Attack Stats")]
        public int Damage;
        public int AttackRange = 12;
        public int AttackAP = 2;

        public bool canAttackBuildings = true;
        public bool canAttackUnits = true;

        [Header("(DEBUG) Current Targeted Transform")]
        public Transform enemy;
    }
}