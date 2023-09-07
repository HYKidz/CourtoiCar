using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetCarData : MonoBehaviour
{
    [SerializeField] private string _carPath ="car_list/test";
    [SerializeField] private TMP_InputField _marqueField;
    [SerializeField] private TMP_InputField _plaqueField;
    [SerializeField] private TMP_InputField _serieField;

    [SerializeField] private TMP_InputField _anneField;

    [SerializeField] private Button _submitButton;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _submitButton.onClick.AddListener(()=>
        {
           var carData = new CarData
           {
            Marque = _marqueField.text,
            Plaque = _plaqueField.text,
            Serie = _serieField.text,
            Anne = int.Parse(_anneField.text)
           };
           var firestore = FirebaseFirestore.DefaultInstance;
           firestore.Document(_carPath).SetAsync(carData);
        });
    }
}
