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
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour
    {

        Animator animator;

        Vector2 Speed => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        void Start()
        {
            animator = GetComponent<Animator>();
        }


        void Update()
        {
            animator.SetFloat("Speed", Speed.magnitude);
        }
    }
}
