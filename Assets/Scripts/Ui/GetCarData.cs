using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class GetCarData : MonoBehaviour
{
    [SerializeField] private string _carPath ="car_list/test";

    [SerializeField] private TMP_Text _marqueText;
    [SerializeField] private TMP_Text _plaqueText;
    [SerializeField] private TMP_Text _serieText;
    [SerializeField] private TMP_Text _anneText;

    private ListenerRegistration _listenerRegistration;

    void Start()
    {
        var firestore = FirebaseFirestore.DefaultInstance;

        _listenerRegistration = firestore.Document(_carPath).Listen(snapshot =>{
            var carData = snapshot.ConvertTo<CarData>();

            _marqueText.text = $"Marque: {carData.Marque}";
            _plaqueText.text = $"Plaque: {carData.Plaque}";
            _serieText.text = $"Serie: {carData.Serie}";
            _anneText.text = $"Anne: {carData.Anne}";
            
        });
    }
    void OnDestroy()
    {
        _listenerRegistration.Stop();
    }


}
