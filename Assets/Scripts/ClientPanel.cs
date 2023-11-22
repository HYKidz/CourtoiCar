using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClientPanel : MonoBehaviour
{
    [SerializeField] private string _clientPath = "clien_list";
    [SerializeField] private TMP_Text _nomText;
    private string _nom;
    public string Nom
    {
        get => _nom; set
        {
            if(value == _nom) return;
            _nom = value;
            _nomText.text = $"Nom:{value}";
        }
    }
    private bool _dispo;
    public bool Dispo
    {
        get => _dispo; set
        {
            _dispo = value;
            // if(_dispo)
        }
    }

}
