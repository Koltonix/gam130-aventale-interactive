using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatGame.Units;
using CatGame.Controls;
using CatGame.ControlScheme;
using CatGame.Data;

namespace CatGame.Combat
{
    public class Attacker : MonoBehaviour
    {
        private RaycastHit mouseOver;

        public int Damage;
        public int AttackRange = 12;
        public int AttackAP = 2;
        [SerializeField]
        private GameObject arrow;
        private bool active;
        public Transform enemy;
        [SerializeField]
        private UserInput input;

        void Start()
        {
            input = GameObject.FindObjectOfType<UserInput>();
            active = false;
            arrow.SetActive(active);
            TurnManager.Instance.onPlayerCycle += NextTurn;
        }

        void Update()
        {
            mouseOver = input.GetRaycastHit();
            if (active)
            {
                if (mouseOver.collider != null)
                {
                    enemy = mouseOver.collider.gameObject.transform;
                    if (mouseOver.collider.GetComponent<Health>() != null && mouseOver.collider.GetComponent<Unit>().owner != this.GetComponent<Unit>().owner && Vector3.Distance(arrow.transform.position, enemy.transform.position) <= AttackRange)
                    {
                        arrow.SetActive(true);
                        arrow.transform.LookAt(enemy);
                        arrow.transform.localScale = new Vector3(arrow.transform.localScale.x, arrow.transform.localScale.y, Vector3.Distance(arrow.transform.position, enemy.transform.position));

                        if (Input.GetKeyDown(Keybinds.KeybindsManager.movementSelect))
                        {
                            mouseOver.collider.GetComponent<Health>().Damage(Damage);
                            this.GetComponent<Unit>().owner.GetPlayerReference().ActionPoints -= AttackAP;
                        }
                    }
                    else
                    {
                        arrow.SetActive(false);
                    }
                }
                else
                {
                    arrow.SetActive(false);
                }
            }
            if (Input.GetKeyDown(Keybinds.KeybindsManager.attackSelect) && mouseOver.collider.gameObject == this.gameObject && this.GetComponent<Unit>().owner.GetPlayerReference().isActive)
            {
                active = !active;
            }
        }

        void NextTurn(Player player)
        {
            active = false;
            arrow.SetActive(false);
        }
    }
}