using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CatGame.Units
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private int maxHealth;
        private int currentHealth;

        [SerializeField]
        private GameObject healthBarPrefab;
        private GameObject healthBar;
        private Image healthBarImage;

        [SerializeField]
        private Animator animator;


        void Start()
        {
            currentHealth = maxHealth;
            healthBar = Instantiate(healthBarPrefab, this.transform);
            healthBar.transform.position += new Vector3(0,1.5f,0);
            healthBar.SetActive(false);
            healthBarImage = healthBar.GetComponentInChildren<Image>();
        }
        
        void OnMouseEnter()
        {
            healthBarImage.fillAmount = currentHealth / maxHealth;
            healthBar.SetActive(true);
        }

        void OnMouseExit()
        {
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
            currentHealth = Mathf.Clamp(currentHealth + damage, 0, maxHealth);
            if (currentHealth == 0)
            {
                //if (animator != null)
                //{
                    //animator.Play();
                //}
                Delete();
            }
        }
        public void Delete()
        {
            Destroy(gameObject);
        }
    }
}