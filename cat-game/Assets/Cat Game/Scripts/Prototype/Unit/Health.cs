using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatGame.Units
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private int maxHealth;
        private int currentHealth;
        
        public int GetHealth()
        {
            return currentHealth;
        }

        public void Heal(int heal)
        {
            currentHealth = Mathf.Clamp(currentHealth + heal, 0, maxHealth);
        }
    }
}