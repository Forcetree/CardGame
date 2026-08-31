using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public abstract class PlaySpace<MatType> : MonoBehaviour where MatType : FieldMat
{
    [Header("PlaySpace Mat Typing")]
    [SerializeField] protected MatType matPrefab;

    public List<MatType> activeMats = new();

    // Properties
    public virtual int RawValue => activeMats.Sum(mat => mat.value);

    // Public Methods
    public virtual void GenerateField(Transform[] spawnLocations)
    {
        for (int i = 0; i < spawnLocations.Length; i++)
        {
            SpawnMatAtLocation(spawnLocations[i]);
        }
    }

    public virtual void DestroyField() // Destroys all the active mats in the field
    {
        foreach (var mat in activeMats) Destroy(mat.gameObject);
        activeMats.Clear();
    }

    public virtual void ClearField() // Resets the active mats in the field (empties the mat)
    {
        foreach (var mat in activeMats) mat.ClearMat();
    }

    // Protected Methods
    protected void SpawnMatAtLocation(Transform spawnLocation)
    {
        MatType newMat = Instantiate(matPrefab, spawnLocation.position, Quaternion.identity);
        newMat.gameObject.transform.parent = this.transform;
        activeMats.Add(newMat);
    }
}