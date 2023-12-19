using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Storage;
using System;

public class SetCarData : MonoBehaviour
{
    [SerializeField] private string _carPath ="car_list";
    [SerializeField] private TMP_InputField _marqueField;
    [SerializeField] private TMP_InputField _plaqueField;
    [SerializeField] private TMP_InputField _serieField;

    [SerializeField] private TMP_InputField _anneField;
    [SerializeField] private TMP_InputField _infoField;
    [SerializeField] private PhotoController _photo;
    [SerializeField] private GameObject _cam;
    private FirebaseFirestore db;
    private List<string> _picList = new List<string>();

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        _photo.SendPicPath.AddListener(RetrievePathPicture);
        _photo.SendInfo.AddListener(SendInfo);
        // _submitButton.onClick.AddListener(async ()=>
        // {
        //     Dictionary<string, object> car = new Dictionary<string, object>
        //     {
        //     {"Marque", _marqueField.text},
        //     {"Plaque", _plaqueField.text},
        //     {"Serie", _serieField.text},
        //     {"Anne", int.Parse(_anneField.text)},
        //     {"Info", _infoField.text},
        //     {"Picture",_picList }

        //     };
        //     //a voir cause fucked
        // //    Dictionary<int, byte[]> dicpic=_photo.GetPicture();
    
        // //    StartCoroutine(UploadCoroutine(dicpic));
        //     DocumentReference AddedDocRef = await db.Collection(_carPath).AddAsync(car);
        // //    var carData = new CarData
        // //    {
        // //    };
        // //    var firestore = FirebaseFirestore.DefaultInstance;
        // //    firestore.Document(_carPath).SetAsync(carData);
        // 1280    720
        //    //pas la meilleur fason de l'appeler, mais work for now
        //    SceneManager.LoadScene("List");
        // });
    }
    public void Click()
    {
            _photo.GetPicture(_marqueField.text);
    }
    private async void SendInfo()
    {
        Debug.Log("Im invoked");
         Dictionary<string, object> car = new Dictionary<string, object>
            {
            {"Marque", _marqueField.text},
            {"Plaque", _plaqueField.text},
            {"Serie", _serieField.text},
            {"Anne", int.Parse(_anneField.text)},
            {"Info", _infoField.text},
            {"Picture",_picList },
            {"Disponible", true}

            };
            DocumentReference AddedDocRef = await db.Collection(_carPath).AddAsync(car);
            
           //pas la meilleur fason de l'appeler, mais work for now
           SceneManager.LoadScene("List");

    }
   
    public void HandlePicture()
    {
        _cam.SetActive(!_cam.activeInHierarchy);
    }
    //J'aime pas sa 
    private void RetrievePathPicture(string path)
    {
        _picList.Add(path);
    }
}
