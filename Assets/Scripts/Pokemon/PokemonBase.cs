using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Il est automatiquement répertorié dans le sous-menu Assets/Create afin de créer facilement des pokemons
[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new pokemon")]

public class PokemonBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    //statistiques de base des pokemons
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;

    //Liste des moves que peuvent apprendre les pokemons
    [SerializeField] List<LearnableMove> learnableMoves;


    //Permet à la classe pokemon d'utiliser les données de la classe PokemonBase
    public string Name
    {
        get { return name; }
    }

    //On spécifie quelle variable sera retourner et on récupère la propriété
    public string Description{
        get { return description; }
    }

    //Créé une propriété pour chacune de ses variables
    public Sprite FrontSprite
    {
        get { return frontSprite; }
    }

    public Sprite BackSprite
    {
        get { return backSprite; }
    }

    public PokemonType Type1
    {
        get { return type1; }
    }

    public PokemonType Type2
    {
        get { return type2; }
    }

    public int MaxHp
    {
        get { return maxHp; }
    }

    public int Attack
    {
        get {return attack; }
    }

    public int SpAttack
    {
        get { return spAttack; }
    }

    public int Defense
    {
        get {return defense; }
    }

    public int SpDefense
    {
        get { return spDefense; }
    }

    public int Speed
    {
        get { return speed; }
    }

    public List<LearnableMove> LearnableMoves{
        get { return learnableMoves; }
    }
}

//Sert à faire apparaitre dans l'inspector les moves que peuvent apprendre les pokemons
[System.Serializable]

//Les pokemons apprennent de nouvelles attaques au fur et à mesure qu'ils montent de niveau
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    //Le niveau auquel ils apprendront l'attaque
    [SerializeField] int level;

    public MoveBase Base{
        get { return moveBase; }
    }

    public int Level
    {
        get { return level; }
    }
}

//Cette fonction spécifie tout les types de pokémon qu'on a dans le jeu
public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon,
}
