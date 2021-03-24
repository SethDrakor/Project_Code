using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] Color highlightedColor;

    Pokemon _pokemon;

    
    //Cette fonction définit les données du pokémon
    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        levelText.text = "Lvl " + pokemon.Level;
        //Divise les HP du pokemon par son maximum de HP
        hpBar.SetHP((float) pokemon.HP / pokemon.MaxHp);
    }

    //Rend visible avec de la couleur la selection
    public void SetSelected(bool selected)
    {
        if (selected)
        {
            nameText.color = highlightedColor;
        }

        else
        {
            nameText.color = Color.black;
        }
    }
}
