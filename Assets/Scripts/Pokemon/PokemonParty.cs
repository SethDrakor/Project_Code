using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PokemonParty : MonoBehaviour
{
   [SerializeField] List<Pokemon> pokemons;

    //Retourne la liste des pokemons
   public List<Pokemon> Pokemons
   {
       get
       {
           return pokemons;
       }
   }

   //Les pokemons sont initilisés un par un
   private void Start()
   {
       foreach (var pokemon in pokemons)
       {
           pokemon.Init();
       }
   }

    //Le pokemon du joueur qui apparait au début d'une battle ne peut pas etre mort
   public Pokemon GetHealthyPokemon()
    {
        return pokemons.Where(x => x.HP > 0).FirstOrDefault();
    }
}
