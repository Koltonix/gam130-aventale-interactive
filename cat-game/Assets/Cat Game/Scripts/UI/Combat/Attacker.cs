﻿using System.Collections;
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

        [Header("Attack Stats")]
        public int Damage;
        public int AttackRange = 12;
        public int AttackAP = 2;

        [Header("Arrow Reference")]
        [SerializeField]
        private GameObject arrow;
        private bool active;

        [Header("(DEBUG) Current Targeted Transform")]
        public Transform enemy;

        [Header("(DEBUG) Input Singleton")]
        [SerializeField]
        private UserInput input;

        [Header("Arrow Colour")]
        [SerializeField]
        private Color32 canAttackColour;
        [SerializeField]
        private Color32 cantAttackColour;

        void Start()
        {
            input = GameObject.FindObjectOfType<UserInput>();
            active = false;
            arrow.SetActive(active);
            TurnManager.Instance.onPlayerCycle += NextTurn;
            AttackerNotifier.OnAttackerClick += Disable;
        }

        void Update()
        {
            mouseOver = input.GetRaycastHit();
            if (active)
            {
                if (mouseOver.collider != null)
                {
                    enemy = mouseOver.collider.gameObject.transform;
                    if (Vector3.Distance(arrow.transform.position, enemy.transform.position) <= AttackRange)
                    {
                        arrow.SetActive(true);
                        arrow.transform.LookAt(enemy);
                        arrow.transform.localScale = new Vector3(arrow.transform.localScale.x, arrow.transform.localScale.y, Vector3.Distance(arrow.transform.position, enemy.transform.position));
                        
                        if (mouseOver.collider.GetComponent<Health>() != null && mouseOver.collider.GetComponent<Unit>().owner != this.GetComponent<Unit>().owner)
                        {
                            arrow.GetComponentInChildren<Renderer>().material.color = canAttackColour;
                            if (Input.GetKeyDown(Keybinds.KeybindsManager.movementSelect))
                            {
                                Debug.Log("Attack for " + Damage.ToString() + " damage.");
                                mouseOver.collider.gameObject.GetComponent<Health>().Damage(Damage);
                                this.GetComponent<Unit>().owner.GetPlayerReference().ActionPoints -= AttackAP;
                            }
                        }
                        else
                        {
                            arrow.GetComponentInChildren<Renderer>().material.color = cantAttackColour;
                        }
                    }
                    else
                    {
                        arrow.SetActive(false);
                    }
                }
                if (mouseOver.collider == null)
                {
                    arrow.SetActive(false);
                }
            }
            if (
                Input.GetKeyDown(Keybinds.KeybindsManager.attackSelect) 
                && mouseOver.collider.gameObject == this.gameObject 
                && this.GetComponent<Unit>().owner.GetPlayerReference().isActive
                )
            {
                if (!active)
                {
                    AttackerNotifier.Clicked();
                    active = true;
                }
                else
                {
                    AttackerNotifier.Clicked();
                }
            }
        }

        public void AttackerDeath()
        {
            AttackerNotifier.OnAttackerClick -= Disable;
            TurnManager.Instance.onPlayerCycle -= NextTurn;
        }

        void NextTurn(Player player)
        {
            active = false;
            arrow.SetActive(false);
        }

        void Disable()
        {
            active = false;
            arrow.SetActive(false);
        }
    }
}