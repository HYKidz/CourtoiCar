using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _marqueText;
    private string _marque;
    public string Marque
    {
        get => _marque; set
        {
            if (value == _marque) return;
            _marque = value;
            _marqueText.text = $"Marque:{value}";
        }
    }
    [SerializeField] private TMP_Text _plaqueText;
    private string _plaque;
    public string Plaque
    {
        get => _plaque; set
        {
            if (value == _plaque) return;
            _plaque = value;
            _plaqueText.text = $"Plaque:{value}";
        }
    }
    [SerializeField] private TMP_Text _serieText;
    private string _serie;
    public string Serie
    {
        get => _serie; set
        {
            if(value == _serie) return;
            _serie = value;
            _serieText.text = $"Serie:{value}";
        }
    }
        [SerializeField]private TMP_Text _anneText;
        private int _anne;
        public int Anne {get=> _anne; set
        {
            if(value== _anne) return;
            _anne = value;
            _anneText.text = $"Anne:{value}";
        }}
}
