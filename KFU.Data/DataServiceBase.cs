//'/////////////////////////////////////////////////////////////////////////
//'<summary>
//'----------------------------------------------
//' Copyright 2023 King Faisal University 
//' www.isb-me.com All Rights Reserved.
//'----------------------------------------------
//' Comment: Defines common DataService routines for transaction management,stored procedure execution, parameter creation, and null value checking
//' Authors: Malik Hourani
//' Reviewers:
//'</summary>
//'----------------------------------------------  
//'/////////////////////////////////////////////////////////////////////////
#region "References"
using KFU.Common;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
#endregion
namespace KFU.Data
{
    public class DataServiceBase
    {
        ////////////////////////////////////////////////////////////////////////
        // Fields
        ////////////////////////////////////////////////////////////////////////
        private readonly bool _isOwner;   //True if service owns the transaction        
        private OracleTransaction _txn;     //Reference to the current transaction

        ////////////////////////////////////////////////////////////////////////
        // Properties 
        ////////////////////////////////////////////////////////////////////////
        public IDbTransaction Txn
        {
            get => _txn;
            set { _txn = (OracleTransaction)value; }
        }

        ////////////////////////////////////////////////////////////////////////
        // Constructors
        ////////////////////////////////////////////////////////////////////////
        public DataServiceBase() : this(null) { }


        public DataServiceBase(IDbTransaction txn)
        {
            if (txn == null)
            {
                _isOwner = true;
            }
            else
            {
                _txn = (OracleTransaction)txn;
                _isOwner = false;
            }
        }

        ////////////////////////////////////////////////////////////////////////
        // Connection and Transaction Methods
        ////////////////////////////////////////////////////////////////////////
        protected static string GetConnectionString()
        {
            //return "user id=C##Tarig;password=123456;data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.147.28.72)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));";
            return "user id=OnlineExam;password=123456;data source=10.146.24.83:1521;";
        }
        public static IDbTransaction BeginTransaction
        {
            get
            {
                OracleConnection oracleConnection = new(GetConnectionString());
                using OracleConnection txnConnection =
                    oracleConnection;
                txnConnection.Open();
                return txnConnection.BeginTransaction();
            }
        }
        ////////////////////////////////////////////////////////////////////////
        // ExecuteDataSet Methods
        ////////////////////////////////////////////////////////////////////////
        protected DataSet ExecuteDataSet(string procName,
            params IDataParameter[] procParams)
        {
            return ExecuteDataSet(out _, procName, procParams);
        }


        protected DataSet ExecuteDataSet(out OracleCommand cmd, string procName,
            params IDataParameter[] procParams)
        {
            OracleConnection cnx = null;
            DataSet ds = new();
            OracleDataAdapter da = new();
            cmd = null;

            try
            {
                //Setup command object
                cmd = new OracleCommand(procName)
                {
                    CommandType = CommandType.StoredProcedure
                };
                if (procParams != null)
                {
                    for (int index = 0; index < procParams.Length; index++)
                    {
                        cmd.Parameters.Add(procParams[index]);
                    }
                }
                da.SelectCommand = cmd;

                //Determine the transaction owner and process accordingly
                if (_isOwner)
                {
                    cnx = new OracleConnection(GetConnectionString());
                    cmd.Connection = cnx;
                    cnx.Open();
                }
                else
                {
                    cmd.Connection = _txn.Connection;
                    cmd.Transaction = _txn;
                }

                //Fill the dataset
                da.Fill(ds);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                da?.Dispose();
                cmd?.Dispose();
                if (_isOwner)
                {
                    cnx.Dispose(); //Implicitly calls cnx.Close()
                }
            }
            return ds;
        }


        protected async Task<(DataSet, OracleCommand)> ExecuteDataSetAsync(string procName,
            params IDataParameter[] procParams)
        {
            OracleConnection cnx = null;
            DataSet ds = new();
            OracleDataAdapter da = new();
            OracleCommand cmd = null;

            try
            {
                //Setup command object
                cmd = new OracleCommand(procName)
                {
                    CommandType = CommandType.StoredProcedure
                };
                if (procParams != null)
                {
                    for (int index = 0; index < procParams.Length; index++)
                    {
                        cmd.Parameters.Add(procParams[index]);
                    }
                }
                da.SelectCommand = cmd;

                //Determine the transaction owner and process accordingly
                if (_isOwner)
                {
                    cnx = new OracleConnection(GetConnectionString());
                    cmd.Connection = cnx;
                    await cnx.OpenAsync();
                }
                else
                {
                    cmd.Connection = _txn.Connection;
                    cmd.Transaction = _txn;
                }

                //Fill the dataset
                da.Fill(ds);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {


                da?.Dispose();
                if (cmd != null)
                {
                    await cmd.DisposeAsync();
                }
                if (_isOwner)
                {
                    await cnx.DisposeAsync(); //Implicitly calls cnx.Close()
                    await cnx.CloseAsync();
                }
            }
            return (ds, cmd);
        }

        ////////////////////////////////////////////////////////////////////////
        // ExecuteNonQuery Methods
        ////////////////////////////////////////////////////////////////////////

        protected async Task<OracleCommand> ExecuteNonQueryAsync(string procName,
            params IDataParameter[] procParams)
        {
            //Method variables
            OracleConnection cnx = null;
            OracleCommand cmd = null;  //Avoids "Use of unassigned variable" compiler error
            try
            {
                //Setup command object
                cmd = new OracleCommand(procName)
                {
                    CommandType = CommandType.StoredProcedure
                };
                for (int index = 0; index < procParams.Length; index++)
                {
                    cmd.Parameters.Add(procParams[index]);
                }

                //Determine the transaction owner and process accordingly
                if (_isOwner)
                {
                    cnx = new OracleConnection(GetConnectionString());
                    cmd.Connection = cnx;
                    await cnx.OpenAsync();
                }
                else
                {
                    cmd.Connection = _txn.Connection;
                    cmd.Transaction = _txn;
                }

                //Execute the command
                await cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_isOwner)
                {
                    await cnx.DisposeAsync(); //Implicitly calls cnx.Close()
                    await cnx.CloseAsync();
                }
                if (cmd != null)
                {
                    await cmd.DisposeAsync();

                }
            }
            return cmd;
        }

        protected void ExecuteNonQuery(string procName,
            params IDataParameter[] procParams)
        {
            ExecuteNonQuery(out _, procName, procParams);
        }


        protected void ExecuteNonQuery(out OracleCommand cmd, string procName,
            params IDataParameter[] procParams)
        {
            //Method variables
            OracleConnection cnx = null;
            cmd = null;  //Avoids "Use of unassigned variable" compiler error

            try
            {
                //Setup command object
                cmd = new OracleCommand(procName)
                {
                    CommandType = CommandType.StoredProcedure
                };
                for (int index = 0; index < procParams.Length; index++)
                {
                    cmd.Parameters.Add(procParams[index]);
                }

                //Determine the transaction owner and process accordingly
                if (_isOwner)
                {
                    cnx = new OracleConnection(GetConnectionString());
                    cmd.Connection = cnx;
                    cnx.Open();
                }
                else
                {
                    cmd.Connection = _txn.Connection;
                    cmd.Transaction = _txn;
                }

                //Execute the command
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_isOwner)
                {
                    cnx.Dispose(); //Implicitly calls cnx.Close()
                }
                cmd?.Dispose();
            }
        }

        ////////////////////////////////////////////////////////////////////////
        // CreateParameter Methods
        ////////////////////////////////////////////////////////////////////////
        protected static OracleParameter CreateParameter(string paramName,
            OracleDbType paramType, object paramValue)
        {
            OracleParameter param = new(paramName, paramType);

            if (paramValue != DBNull.Value)
            {
                switch (paramType)
                {
                    case OracleDbType.Varchar2:
                    case OracleDbType.NVarchar2:
                    case OracleDbType.Char:
                    case OracleDbType.NChar:
                    case OracleDbType.NClob:
                        paramValue = CheckParamValue((string)paramValue);
                        break;
                    case OracleDbType.Clob:
                        paramValue = CheckParamValue((string)paramValue);
                        break;
                    case OracleDbType.Date:
                        paramValue = CheckParamValue((DateTime)paramValue);
                        break;
                    case OracleDbType.Int32:
                        paramValue = CheckParamValue((int)paramValue);
                        break;
                    case OracleDbType.BinaryFloat:
                        paramValue = CheckParamValue((float)paramValue);
                        break;
                    case OracleDbType.Decimal:
                        paramValue = CheckParamValue((decimal)paramValue);
                        break;
                }
            }
            param.Value = paramValue;
            return param;
        }
        protected static OracleParameter CreateParameter(string paramName, OracleDbType paramType, ParameterDirection direction)
        {
            OracleParameter returnVal = CreateParameter(paramName, paramType, DBNull.Value);
            returnVal.Direction = direction;
            return returnVal;
        }
        protected static OracleParameter CreateParameter(string paramName, OracleDbType paramType, object paramValue, ParameterDirection direction)
        {
            OracleParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Direction = direction;
            return returnVal;
        }
        protected static OracleParameter CreateParameter(string paramName, OracleDbType paramType, object paramValue, int size)
        {
            OracleParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Size = size;
            return returnVal;
        }

        protected static OracleParameter CreateParameter(string paramName, OracleDbType paramType, object paramValue, int size, ParameterDirection direction)
        {
            OracleParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Direction = direction;
            returnVal.Size = size;
            return returnVal;
        }

        protected static OracleParameter CreateParameter(string paramName, OracleDbType paramType, object paramValue, int size, byte precision)
        {
            OracleParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Size = size;
            returnVal.Precision = precision;
            return returnVal;
        }

        protected static OracleParameter CreateParameter(string paramName, OracleDbType paramType, object paramValue, int size, byte precision, ParameterDirection direction)
        {
            OracleParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Direction = direction;
            returnVal.Size = size;
            returnVal.Precision = precision;
            return returnVal;
        }

        protected static OracleParameter CreateParameter(string paramName, OracleDbType paramType, OracleCollectionType collectionType, object paramValue, int size)
        {
            OracleParameter parameter = new(paramName, paramType);
            parameter.CollectionType = collectionType;
            parameter.Value = paramValue;
            parameter.Size = size;
            return parameter;
        }

        ////////////////////////////////////////////////////////////////////////
        // CheckParamValue Methods
        ////////////////////////////////////////////////////////////////////////
        protected static Guid GetGuid(object value)
        {
            Guid returnVal = Constants.NullGuid;
            if (value is string @string)
            {
                returnVal = new Guid(@string);
            }
            else if (value is Guid guid)
            {
                returnVal = guid;
            }
            return returnVal;
        }

        protected static object CheckParamValue(string paramValue)
        {
            if (string.IsNullOrEmpty(paramValue))
            {
                return DBNull.Value;
            }
            else
            {
                return paramValue;
            }
        }

        protected static object CheckParamValue(Guid paramValue)
        {
            if (paramValue.Equals(Constants.NullGuid))
            {
                return DBNull.Value;
            }
            else
            {
                return paramValue;
            }
        }

        protected static object CheckParamValue(DateTime paramValue)
        {
            if (paramValue.Equals(Constants.NullDateTime))
            {
                return DBNull.Value;
            }
            else
            {
                return paramValue;
            }
        }

        protected static object CheckParamValue(double paramValue)
        {
            if (paramValue.Equals(Constants.NullDouble))
            {
                return DBNull.Value;
            }
            else
            {
                return paramValue;
            }
        }

        protected static object CheckParamValue(float paramValue)
        {
            if (paramValue.Equals(Constants.NullFloat))
            {
                return DBNull.Value;
            }
            else
            {
                return paramValue;
            }
        }

        protected static object CheckParamValue(Decimal paramValue)
        {
            if (paramValue.Equals(Constants.NullDecimal))
            {
                return DBNull.Value;
            }
            else
            {
                return paramValue;
            }
        }

        protected static object CheckParamValue(int paramValue)
        {
            if (paramValue.Equals(Constants.NullInt))
            {
                return DBNull.Value;
            }
            else
            {
                return paramValue;
            }
        }

    }
}