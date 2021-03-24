using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

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

    //La barre de vie du pokemon du joueur diminue lentement
    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float) _pokemon.HP / _pokemon.MaxHp);
    }
}
