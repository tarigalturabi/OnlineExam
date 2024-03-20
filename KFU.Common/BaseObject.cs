//'/////////////////////////////////////////////////////////////////////////
//'<summary>
//'----------------------------------------------
//' Copyright 2013 Inegrated Business Solution
//' www.isb-me.com All Rights Reserved.
//'----------------------------------------------
//' Comment: MustInherit Class BaseObject
//' Authors: Malik Hourani
//' Reviewers: Malik Hourani
//'</summary>
//'----------------------------------------------  
//'/////////////////////////////////////////////////////////////////////////
#region "References"
using System;
using System.Data;
#endregion
namespace KFU.Common;
public abstract class BaseObject
{ 
    #region "Methods"
    public virtual bool MapData(DataSet ds)
    {
        if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
        {
            return MapData(ds.Tables[0].Rows[0]);
        }
        else
        {
            return false;
        }
    }
    public virtual bool MapData(DataTable dt)
    {
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            return MapData(dt.Rows[0]);
        }
        else
        {
            return false;
        }
    }
    public virtual bool MapData(DataRow row)
    {
        //You can put common data mapping items here (e.g. create date, modified date, etc)
        return true;
    }
    #region Get Functions

    //////////////////////////////////////////////////////////////////////////////
    protected static int GetInt(DataRow row, string columnName)
    {
        if (row.Table.Columns.Contains(columnName))
        {
            if (row[columnName].Equals(System.DBNull.Value) == false)
            {
                return Convert.ToInt32(row[columnName]);
            }
        }
        return Constants.NullInt;
    }

    //////////////////////////////////////////////////////////////////////////////
    protected static DateTime GetDateTime(DataRow row, string columnName)
    {
        if (row.Table.Columns.Contains(columnName))
        {
            if (row[columnName].Equals(System.DBNull.Value) == false)
            {
                return Convert.ToDateTime(row[columnName]);
            }
        }
        return Constants.NullDateTime;
    }


    //////////////////////////////////////////////////////////////////////////////
    protected static Decimal GetDecimal(DataRow row, string columnName)
    {

        if (row.Table.Columns.Contains(columnName))
        {
            if (row[columnName].Equals(System.DBNull.Value) == false)
            {
                return Convert.ToDecimal(row[columnName]);
            }
        }
        return Constants.NullDecimal;
    }

   

    //////////////////////////////////////////////////////////////////////////////
    protected static bool GetBool(DataRow row, string columnName)
    {
        return (row[columnName] != DBNull.Value) && Convert.ToBoolean(row[columnName]);
    }

    //////////////////////////////////////////////////////////////////////////////
    protected static string GetString(DataRow row, string columnName)
    {
        if (row.Table.Columns.Contains(columnName))
        {
            if (row[columnName].Equals(System.DBNull.Value) == false)
            {
                return row[columnName].ToString();
            }
        }
        return "";
    }

    //////////////////////////////////////////////////////////////////////////////
    protected static double GetDouble(DataRow row, string columnName)
    {
        if (row.Table.Columns.Contains(columnName))
        {
            if (row[columnName].Equals(System.DBNull.Value) == false)
            {
                return Convert.ToDouble(row[columnName]);
            }
        }
        return -1;
    }

    //////////////////////////////////////////////////////////////////////////////
    /// 
    protected static float GetFloat(DataRow row, string columnName)
    {
        if (row.Table.Columns.Contains(columnName))
        {
            if (row[columnName].Equals(System.DBNull.Value) == false)
            {
                return Convert.ToSingle(row[columnName]);
            }
        }
        return -1;
    }

    //////////////////////////////////////////////////////////////////////////////
    protected static Guid GetGuid(DataRow row, string columnName)
    {
        if (row.Table.Columns.Contains(columnName))
        {
            if (row[columnName].Equals(System.DBNull.Value) == false)
            {
                return (Guid)(row[columnName]);
            }
        }
        return Constants.NullGuid;
    }

    //////////////////////////////////////////////////////////////////////////////
    protected static long GetLong(DataRow row, string columnName)
    {
        if (row.Table.Columns.Contains(columnName))
        {
            if (row[columnName].Equals(System.DBNull.Value) == false)
            {
                return (long)(row[columnName]);
            }
        }
        return Constants.NullLong;
    }
    #endregion
    #endregion
}
