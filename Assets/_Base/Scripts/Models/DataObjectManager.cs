using _Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataObjectManager
{
    public DataObjectManager()
    {
        Debug.Log("ListData1: " + _myData);
        _myData = new List<DataObject>();
        Debug.Log("ListData2: " + _myData);
    }

    private List<DataObject> _myData;

    public List<DataObject> MyData
    {
        get
        {
            if (_myData == null) _myData = new List<DataObject>();
            return _myData;
        }
    }

    public void SetData(DataObject dataObject)
    {
        _myData.Add(dataObject);
    }
}
