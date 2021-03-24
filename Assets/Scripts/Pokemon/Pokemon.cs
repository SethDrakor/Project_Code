//On va calculer toutes les valeurs en fonction du niveau

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Les classes sont visibles dans l'inspector
[System.Serializable]

//classe pokemon
public class Pokemon
{   
    //Les variables de PokemonBase sont visibles dans l'inspector et accessible en dehors de la classe
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;

    //Appelle la fonction public string Name qui permet d'utiliser les données de la classe PokemonBase
    public PokemonBase Base
    {
        get
        {
            return _base;
        }
    }

    public int Level
    {
        get 
        {
            return level;
        }
    }

    public int HP { get; set; }
    //Liste des moves
    public List<Move> Moves { get; set; }

    public void Init()
    {
        HP = MaxHp;

        //Génère des attaques basés sur leurs niveaux
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
            {
                Moves.Add(new Move(move.Base));
            }

            if(Moves.Count >= 4)
            {
                break;
            }
        }
    }


    //Créé les propriétes pour les stats des pokémon    
    public int Attack
    {
        //Formule tirée du jeu pokemon qui permet de calculer l'attaque en foncton du niveau actuel du pokemon
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }

    public int Defense
    {
        //Formule tirée du jeu pokemon qui permet de calculer la défense en foncton du niveau actuel du pokemon
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; }
    }

    public int SpAttack
    {
        //Formule tirée du jeu pokemon qui permet de calculer l'attaque spéciale en foncton du niveau actuel du pokemon
        get { return Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5; }
    }

    public int SpDefense
    {
        //Formule tirée du jeu pokemon qui permet de calculer la défense spéciale en foncton du niveau actuel du pokemon
        get { return Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5; }
    }

    public int Speed
    {
        //Formule tirée du jeu pokemon qui permet de calculer la vitesse en foncton du niveau actuel du pokemon
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; }
    }

    public int MaxHp
    {
        //Formule tirée du jeu pokemon qui permet de calculer le maximum de HP en foncton du niveau actuel du pokemon
        get { return Mathf.FloorToInt((Base.MaxHp * Level) / 100f) + 10; }
    }

    //Méthode pour calculer les dommages en fonction du level, des stats, de la puissance de son attaque et de la défense du pokemon adverse avec une formule tirée du jeu Pokemon
    public bool TakeDamage(Move move, Pokemon attacker)
    {
        //Un chiffre aléatoire entre 0.85 et 1 pour définir le nombre de dégats de l'attaque
        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;
        if(HP <= 0)
        {
            HP = 0;
            return true;
        }

        return false;
    }

    //Méthode pour que le pokémon ennemie lance une attaque aléatoire
    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}
