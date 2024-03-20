//'/////////////////////////////////////////////////////////////////////////
//'<summary>
//'----------------------------------------------
//' Copyright 2013 Inegrated Business Solution 
//' www.isb-me.com All Rights Reserved.
//'----------------------------------------------
//' Comment: Contains a listing of constants used throughout the application
//' Authors:  Malik Hourani
//' Reviewers: Malik.Hourani,
//'</summary>
//'----------------------------------------------  
//'/////////////////////////////////////////////////////////////////////////
namespace KFU.Common
{
    public sealed class Constants
    {
        /// 

        /// The value used to represent a null DateTime value
        /// 
        public static readonly DateTime NullDateTime = DateTime.MinValue;

        /// 

        /// The value used to represent a null decimal value
        /// 
        public static readonly decimal NullDecimal = decimal.MinValue;

        /// 

        /// The value used to represent a null double value
        /// 
        public static readonly double NullDouble = double.MinValue;

        /// 

        /// The value used to represent a null Guid value
        /// 
        public static readonly Guid NullGuid = Guid.Empty;

        /// 

        /// The value used to represent a null int value
        /// 
        public static readonly int NullInt = int.MinValue;

        /// 

        /// The value used to represent a null long value
        /// 
        public static readonly long NullLong = long.MinValue;

        /// 

        /// The value used to represent a null float value
        /// 
        public static readonly float NullFloat = float.MinValue;

        /// 

        /// The value used to represent a null string value
        /// 
        public static readonly string NullString = string.Empty;

        /// 

        /// Maximum DateTime value allowed by SQL Server
        /// 
        public static readonly DateTime SqlMaxDate = new(9999, 1, 3, 23, 59, 59);

        /// 

        /// Minimum DateTime value allowed by SQL Server
        /// 
        public static readonly DateTime SqlMinDate = new(1753, 1, 1, 00, 00, 00);
    }
}