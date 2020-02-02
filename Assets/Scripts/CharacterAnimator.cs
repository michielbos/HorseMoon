using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * by bit assembly
 * 
 * It looks stupid, but what the speed property does is reading a relevant property and converting to something that represents data.
 * Since right now, the only propertly that represents movement is just raw inputs, so that is what is being used.
 * 
 */
namespace HorseMoon
{
    [RequireComponent(typeof(Animator), typeof(CharacterControl))]
    public class CharacterAnimator : MonoBehaviour
    {
        Animator animator;
        CharacterControl controller;

        Vector2 Speed => Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1f);

        void Start()
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<CharacterControl>();
        }


        void Update()
        {
            animator.SetFloat("Speed", controller.CurrentSpeed.magnitude);
        }
    }
}
