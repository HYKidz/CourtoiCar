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

    private Dictionary<string, GameObject> _carActive = new Dictionary<string, GameObject>();

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
                    _carActive.Add(documentSnapshot.Id,GOCar);
                    GOCar.transform.SetParent(gameObject.transform);
                    CarPanel panel = GOCar.GetComponent<CarPanel>();
                    // Debug.Log(car["Anne"]);
                    panel.Marque = car["Marque"].ToString();
                    panel.Serie = car["Serie"].ToString();
                    panel.Plaque = car["Plaque"].ToString();
                    panel.Anne = car["Anne"].ToString();
                    panel.ID = documentSnapshot.Id;
                    panel._click.AddListener(Click);
                    // Debug.Log("");
                }
            }
        });
    }

    void Click(string id)
    {
        Debug.Log(id);
    }


}
