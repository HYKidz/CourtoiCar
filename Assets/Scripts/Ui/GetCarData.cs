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
    //a changer plus tard pour que sa sois selon le user
    [SerializeField] private string _carPath = "car_list";

    [SerializeField] private GameObject _carPanel;

    [SerializeField] private GridLayoutGroup _grid;
    private Dictionary<string, GameObject> _carActive = new Dictionary<string, GameObject>();
    [SerializeField]private float _minYCellSize;
    [SerializeField]private float _maxYCellSize;
    [SerializeField]private float _vitesseAnim;
    private float _actualYCellSize;

    // private ListenerRegistration _listenerRegistration;
    void Start()
    {
        _actualYCellSize = _grid.cellSize.y;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        Query carQuery = db.Collection(_carPath);
        carQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot allCarSnapshot = task.Result;
            Debug.Log(allCarSnapshot.Documents);
            foreach (DocumentSnapshot documentSnapshot in allCarSnapshot.Documents)
            {

                Debug.Log(String.Format("Document Data for {0} document:", documentSnapshot.Id));
                Dictionary<string, object> car = documentSnapshot.ToDictionary();
                if (_carPanel != null)
                {

                    GameObject GOCar = Instantiate(_carPanel, gameObject.transform.position, quaternion.identity);
                    _carActive.Add(documentSnapshot.Id,GOCar);
                    GOCar.transform.SetParent(gameObject.transform);
                    GOCar.transform.localScale = Vector3.one;
                    CarPanel panel = GOCar.GetComponent<CarPanel>();
                    panel.Marque = car["Marque"].ToString();
                    panel.Serie = car["Serie"].ToString();
                    panel.Plaque = car["Plaque"].ToString();
                    panel.Anne = car["Anne"].ToString();
                    if(car["Info"]!= null)panel.Info = car["Info"].ToString();
                    else panel.Info = "Aucun information";
                    
                    panel.ID = documentSnapshot.Id;
                    panel._click.AddListener(Click);
                    // Debug.Log("");
                }
            }
        });
    }

    void Click(string id)
    {
        foreach (KeyValuePair<string,GameObject> car  in _carActive)
        {
            if(car.Key == id)
            {
                //Play anim and set info
                continue;
            } 
            car.Value.SetActive(!car.Value.activeInHierarchy);
            // Anim();
        }
    }

    //pas ideal, a revoir
    private void Anim()
    {
        float target =0f;
        float lerp = 0f;
        if(_actualYCellSize>100) target = _minYCellSize;
        else target = _maxYCellSize;
        while (lerp<=1)
        {
            
            lerp += Time.deltaTime/_vitesseAnim;
            _grid.cellSize = new Vector2(_grid.cellSize.x,(int)Mathf.Lerp(_grid.cellSize.y, target, lerp));
            _actualYCellSize = _grid.cellSize.y;
        }



    }

    // IEnumerator AnimSelect()
    // {
    //     float lerp = Time.deltaTime * _vitesseAnim;
        // _grid.cellSize.y = in
        
    // }


}
