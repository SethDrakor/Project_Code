using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{

   [SerializeField] bool isPlayerUnit;

   public Pokemon Pokemon { get; set; }

   //Créé un pokemon de base de la classe Pokemon
   public void Setup(Pokemon pokemon)
   {
       Pokemon = pokemon;
       //Si c'est le pokemon du joueur
       if (isPlayerUnit)
       {
           //Le dos du pokemon
           GetComponent<Image>().sprite = Pokemon.Base.BackSprite;
       }

       else
       {
           //Le pokemon de face
           GetComponent<Image>().sprite = Pokemon.Base.FrontSprite;
       }
   }
}
