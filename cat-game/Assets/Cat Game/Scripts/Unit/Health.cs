﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CatGame.Combat;
using CatGame.Data;
using CatGame.Tiles;

namespace CatGame.Units
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private int maxHealth;
        public int currentHealth;

        [SerializeField]
        private GameObject healthBarPrefab;
        private GameObject healthBar;
        [SerializeField]
        private Vector3 healthBarOffset = new Vector3(0, 1.5f, 0);
        private Image healthBarImage;
        private bool healthBarIsActive;

        public bool isABase = false;

        [SerializeField]
        private Animator animator;

        void Start()
        {
            currentHealth = maxHealth;
            healthBar = Instantiate(healthBarPrefab, this.transform);
            healthBar.transform.position += healthBarOffset;
            healthBar.SetActive(false);
            healthBarImage = healthBar.GetComponentInChildren<Image>();
        }

        void Update()
        {
            if (healthBarIsActive)
            {
                healthBarImage.fillAmount = (float)currentHealth / (float)maxHealth;
            }
        }
        
        void OnMouseEnter()
        {
            healthBar.SetActive(true);
            healthBarIsActive = true;
        }

        void OnMouseExit()
        {
            healthBarIsActive = false;
            healthBar.SetActive(false);
        }

        public int GetHealth()
        {
            return currentHealth;
        }

        public void Heal(int heal)
        {
            currentHealth = Mathf.Clamp(currentHealth + heal, 0, maxHealth);
        }

        public void Damage(int damage)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
            if (currentHealth <= 0)
            {
                //if (animator != null)
                //{
                //animator.Play();
                //}

                Attacker attacker = this.GetComponent<Attacker>();
                if (attacker != null)
                {
                    attacker.AttackerDeath();
                }

                Destroy(this.gameObject);
            }
        }

        private void OnDestroy()
        {
            //This is not ideal and I would have preferred inheritance, but due to time constraints it has to be done this way.
            if (isABase) GameController.Instance.CheckIfWon();
            //RESETS THE BOARD TILES
            BoardManager.Instance.GetBoardTiles();
        }
    }
}