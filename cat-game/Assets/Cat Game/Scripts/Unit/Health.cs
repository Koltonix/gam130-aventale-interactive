using System.Collections;
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
        private int MaxHealth;

        private int currentHealth;
        public int CurrentHealth
        { 
            get { return currentHealth; }
            set
            {
                currentHealth = value;
                if (healthBar != null) healthBarCoroutine = StartCoroutine(ShowHealthBarTemporarily(healthBar, timeToShowHealth));
            }
        }

        [SerializeField]
        private float timeToShowHealth = .5f;

        [SerializeField]
        private GameObject healthBarPrefab;
        private GameObject healthBar;
        [SerializeField]
        private Vector3 healthBarOffset = new Vector3(0, 1.5f, 0);
        private Image healthBarImage;
        private bool healthBarIsActive;
        private Coroutine healthBarCoroutine;

        public bool isABase = false;

        [SerializeField]
        private Animator animator;

        void Start()
        {
            CurrentHealth = MaxHealth;
            healthBar = Instantiate(healthBarPrefab, this.transform);
            healthBar.transform.position += healthBarOffset;
            healthBar.SetActive(false);
            healthBarImage = healthBar.GetComponentInChildren<Image>();
        }

        void Update()
        {
            if (healthBarIsActive)
            {
                healthBarImage.fillAmount = (float)CurrentHealth / (float)MaxHealth;
            }
        }
        
        void OnMouseEnter()
        {
            healthBarIsActive = true;
            healthBar.SetActive(true);
        }

        void OnMouseExit()
        {
            if (healthBarCoroutine == null)
            {
                healthBarIsActive = false;
                healthBar.SetActive(false);
            }
        }

        public int GetHealth()
        {
            return CurrentHealth;
        }

        public void Heal(int heal)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + heal, 0, MaxHealth);
        }

        public void Damage(int damage)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);
            if (CurrentHealth <= 0)
            {
                //if (animator != null)
                //{
                //animator.Play();
                //}

                Attacker attacker = this.GetComponent<Attacker>();

                Destroyed();
                Destroy(this.gameObject);
            }
        }

        private IEnumerator ShowHealthBarTemporarily(GameObject healthBar, float timeToShow)
        {
            healthBar.SetActive(true);
            healthBarIsActive = true;

            yield return new WaitForSeconds(timeToShow);
            healthBar.SetActive(false);
            healthBarIsActive = false;

            healthBarCoroutine = null;
        }

        private void Destroyed()
        {
            //This is not ideal and I would have preferred inheritance, but due to time constraints it has to be done this way.
            if (isABase) GameController.Instance.CheckIfWon();
            //RESETS THE BOARD TILES
            BoardManager.Instance.GetBoardTiles();
        }
    }
}