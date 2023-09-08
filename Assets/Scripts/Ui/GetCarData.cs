using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class GetCarData : MonoBehaviour
{
    [SerializeField] private string _carPath = "car_list";

    [SerializeField] private GameObject _carPanel;

    // private ListenerRegistration _listenerRegistration;
    void Start()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        Query carQuery = db.Collection(_carPath);
        carQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot allCarSnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in allCarSnapshot.Documents)
            {

                Debug.Log(String.Format("Document Data for {0} document:", documentSnapshot.Id));
                Dictionary<string, object> car = documentSnapshot.ToDictionary();
                if (_carPanel != null)
                {

                    GameObject GOCar = Instantiate(_carPanel, gameObject.transform.position, quaternion.identity);
                    GOCar.transform.SetParent(gameObject.transform);
                    CarPanel panel = GOCar.GetComponent<CarPanel>();
                    Debug.Log(car["Anne"]);
                    panel.Marque = car["Marque"].ToString();
                    panel.Serie = car["Serie"].ToString();
                    panel.Plaque = car["Plaque"].ToString();
                    panel.Anne = car["Anne"].ToString();
                    Debug.Log("");
                }
            }
        });
        // var firestore = FirebaseFirestore.DefaultInstance;

        // _listenerRegistration = firestore.Document(_carPath).Listen(snapshot =>
        // {
        //     var carData = snapshot.ConvertTo<CarData>();
        //     if (_carPanel != null)
        //     {

        //         GameObject car = Instantiate(_carPanel, gameObject.transform.position, quaternion.identity);
        //         car.transform.SetParent(gameObject.transform);
        //         CarPanel panel = car.GetComponent<CarPanel>();
        //         panel.Marque = carData.Marque;
        //         panel.Serie = carData.Serie;
        //         panel.Plaque = carData.Plaque;
        //         panel.Anne = carData.Anne;

        //     }
        // });
    }
    // void OnDestroy()
    // {
    //     _listenerRegistration.Stop();
    // }


}
