using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetCarData : MonoBehaviour
{
    [SerializeField] private string _carPath ="car_list";
    [SerializeField] private TMP_InputField _marqueField;
    [SerializeField] private TMP_InputField _plaqueField;
    [SerializeField] private TMP_InputField _serieField;

    [SerializeField] private TMP_InputField _anneField;
    [SerializeField] private TMP_InputField _infoField;

    [SerializeField] private Button _submitButton;
    [SerializeField] private GameObject _cam;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        _submitButton.onClick.AddListener(async ()=>
        {
            Dictionary<string, object> car = new Dictionary<string, object>
            {
            {"Marque", _marqueField.text},
            {"Plaque", _plaqueField.text},
            {"Serie", _serieField.text},
            {"Anne", int.Parse(_anneField.text)},
            {"Info", _infoField.text},

            };
            DocumentReference AddedDocRef = await db.Collection(_carPath).AddAsync(car);
        //    var carData = new CarData
        //    {
        //    };
        //    var firestore = FirebaseFirestore.DefaultInstance;
        //    firestore.Document(_carPath).SetAsync(carData);

           //pas la meilleur fason de l'appeler, mais work for now
           SceneManager.LoadScene("List");
        });
    }
    public void HandlePicture()
    {
        _cam.SetActive(!_cam.activeInHierarchy);
    }
}
