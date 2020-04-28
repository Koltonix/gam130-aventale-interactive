using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CatGame.Combat;
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
                if (healthBar != null)
                {
                    if (healthBarCoroutine != null) StopCoroutine(healthBarCoroutine);
                    healthBarCoroutine = StartCoroutine(ShowHealthBarTemporarily(healthBar, healthLerpSpeed));
                }
            }
        }

        [SerializeField]
        private float healthLerpSpeed = 0.75f;
        [SerializeField]
        private float disableDelay = 0.1f;

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

        private IEnumerator ShowHealthBarTemporarily(GameObject healthBar, float lerpSpeed)
        {
            healthBar.SetActive(true);
            healthBarIsActive = true;

            yield return StartCoroutine(LerpHealthBar(healthBarImage, lerpSpeed));
            yield return new WaitForSeconds(disableDelay);
            healthBar.SetActive(false);
            healthBarIsActive = false;

            healthBarCoroutine = null;
        }
        
        private IEnumerator LerpHealthBar(Image healthBarImage, float lerpSpeed)
        {
            float t = 0.0f;
            while (t <= 1.0f)
            {
                t += lerpSpeed * Time.deltaTime;

                float targetHealth = (float)CurrentHealth / (float)MaxHealth;
                healthBarImage.fillAmount = Mathf.Lerp(healthBarImage.fillAmount, targetHealth, t);
                yield return null;
            }

            yield return null;
            
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