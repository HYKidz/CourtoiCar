using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CarPanel : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TMP_Text _marqueText;
    public class MyStringObjectEvent : UnityEvent<string>
    {

    }
    public MyStringObjectEvent _click = new MyStringObjectEvent();
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
        private string _anne;
        public string Anne {get=> _anne; set
        {
            if(value== _anne) return;
            _anne = value;
            _anneText.text = $"Anne:{value}";
        }}

        private string _ID;
        public string ID {get=>_ID; set => _ID = value;}
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    // void Awake()
    // {
    //     OnPointerDown
    // }
    public void OnPointerDown(PointerEventData data)
    {
        _click.Invoke(_ID);
    }
}
