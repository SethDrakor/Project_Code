using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    
   [SerializeField] Text messageText;

    PartyMemberUI[] memberSlots;
    List<Pokemon> pokemons;

    //Retounr tout les components de PartyMemberUI qui sont attachés aux childs objects de PartyScreen
    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    //Permet d'avoir 6 emplacement de pokemon
    public void SetPartyData(List<Pokemon> pokemons)
    {
        this.pokemons = pokemons;

        for(int i = 0; i < memberSlots.Length; i++)
        {
            if(i < pokemons.Count)
            {
                memberSlots[i].SetData(pokemons[i]);
            }

            else
            {
                //Ne montre que les pokemon qu'a le joueurs sans afficher les emplacements
                memberSlots[i].gameObject.SetActive(false);
            }

            messageText.text = "Choose a Pokemon";
        }
    }

    //Met en valeur avec de la couleur la selection des emplacements des pokemons
    public void UpdateMemberSelection(int selectedMember)
    {
        for (int i = 0; i < pokemons.Count; i++)
        {
            if (i == selectedMember)
            {
                memberSlots[i].SetSelected(true);
            }

            else
            {
                memberSlots[i].SetSelected(false);
            }
        }
    }

    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}
