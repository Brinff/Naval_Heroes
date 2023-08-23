using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEcsData
{

}

public interface IEcsDataProvider
{
    public IEcsData[] ProvideData();
}