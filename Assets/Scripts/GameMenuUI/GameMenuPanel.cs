using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace HorseMoon.GameMenuUI
{
    public abstract class GameMenuPanel : MonoBehaviour
    {
        public GameMenu Menu { get; private set; }

        public Vector2 Size => GetComponent<RectTransform>().sizeDelta;
        public GameObject FirstSelected;
        protected Action CancelMethod;

        protected void Awake()
        {
            Menu = GetComponentInParent<GameMenu>();
        }

        protected void Start()
        {
            EventSystem.current.SetSelectedGameObject(FirstSelected);
        }

        protected void Update()
        {
            if (Input.GetButtonDown("Cancel"))
                CancelMethod();
        }
    }
}

