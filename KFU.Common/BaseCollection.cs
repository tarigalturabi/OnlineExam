//'/////////////////////////////////////////////////////////////////////////
//'<summary>
//'----------------------------------------------
//' Copyright 2013 Inegrated Business Solution 
//' www.isb-me.com All Rights Reserved.
//'----------------------------------------------
//' Comment: MustInherit Class BaseCollection
//' Authors: Malik Hourani
//' Reviewers: Malik Hourani
//'</summary>
//'----------------------------------------------  
//'/////////////////////////////////////////////////////////////////////////
#region "References"
using System;
using System.Collections.Generic;
using System.Data;
#endregion
namespace KFU.Common;
public abstract class BaseCollection<T> : List<T> where T : BaseObject, new()
{ 
    #region "Methods"
    ////////////////////////////////////////////////////////////
    /// <summary>
    ///
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    ////////////////////////////////////////////////////////////
    public bool MapObjects(DataSet ds)
    {
        if (((((ds) != null)) && (ds.Tables.Count > 0)))
        {
            return MapObjects(ds.Tables[0]);
        }
        else
        {
            return false;
        }
    }
    ////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    ////////////////////////////////////////////////////////////
    public bool MapObjects(DataTable dt)
    {
        Clear();
        int i = 0;
        while ((i < dt.Rows.Count))
        {
            T obj = new();
            obj.MapData(dt.Rows[i]);
            Add(obj);
            i++;
        }
        return true;
    }
    #endregion
}