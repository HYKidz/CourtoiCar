using Firebase.Firestore;
using UnityEngine;

[FirestoreData]
public struct CarData 
{
    [FirestoreProperty]
    public string Marque {get; set;}

    [FirestoreProperty]

    public string Plaque{get; set;}

    [FirestoreProperty]

    public string Serie {get; set;}

    [FirestoreProperty]

    public int Anne {get; set;}

}
