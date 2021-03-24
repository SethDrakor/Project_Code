using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    //vitesse du player
    public float moveSpeed;
    public LayerMask grassLayer;

    
    public event Action onEncountered;

    //si le player est en train de bouger
    private bool isMoving;
    private Vector2 input;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    //Permet au joueur de déplacer son personnage
    public void HandleUpdate()
    {
        if(!isMoving)
        {
            //Permet de se déplacer sur l'axe X et Y avec les flèches
            //Si on appuie sur la flèche droite l'inputsera à 1 et si on appuie sur la flèche gauche l'input sera à -1
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //Si on appuie sur une touche
            if(input != Vector2.zero)
            {
                //Le personnage est animé sur l'axe X ou Y
                animator.SetFloat("X", input.x);
                animator.SetFloat("Y", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                //On appelle la coroutine Move
                StartCoroutine(Move(targetPos));
            }
        }
        //On lance l'animation à la fin de la méthode HandleUpdate
        animator.SetBool("isMoving", isMoving);
    }

    //Cette coroutine va check si la différence entre la position qu'on veut atteindre et la position actuel du joueur est plus grand que la valeur
    //Elle fait bouger le joueur un petit peu et le fait avancer uniquement de case en case
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            
            yield return null;
        }
        //Si le joueur ne bouge plus il atterit sur la position qu'il a ciblée
        transform.position = targetPos;
        isMoving = false;

        //On appelle la fonction qui va gérer le déclenchement de la battle 
        CheckForEncounters();
    }

    private void CheckForEncounters()
    {
        //On vérifie si le joueur est en contact avec une tile Grass ou pas
        if(Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            //Permet au player de pouvoir marcher dans l'herbe sans déclencher un combat instantanément en ayant 10% de chance de rentrer dans une battle
            //Si le nombre est égale ou en dessous de 10 il rencontre un pokemon sauvage
            //1 fois sur 10 le joueur rentre dansune bataille
            if(UnityEngine.Random.Range(1, 101) <= 10)
            {
                animator.SetBool("isMoving", false);
                onEncountered();
            }
        }
    }
}
