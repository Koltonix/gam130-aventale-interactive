using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatGame.Combat
{
    public static class AttackerNotifier
    {
        public delegate void DisableAttackers();
        public static event DisableAttackers OnAttackerClick;

        public static void Clicked()
        {
            OnAttackerClick?.Invoke();
        }
    }
}