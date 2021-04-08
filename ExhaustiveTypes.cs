using System;  
using System.Data;  
using System.Data.SqlTypes;  
using Microsoft.SqlServer.Server;

[Serializable]  
[Microsoft.SqlServer.Server.SqlUserDefinedType(
    Format.Native,  
    IsByteOrdered = true, 
    IsFixedLength = true,
    Name = "ExhaustiveInt",
    ValidationMethodName = "Validate"
)]  
public struct ExhaustiveInt : INullable {  
    private SqlInt32 the_value;    
    public bool IsNull { 
        get {
            return false; 
        }
    }  
    public static ExhaustiveInt Null {  
        get {  
            ExhaustiveInt an_exhaustive = new ExhaustiveInt();  
            an_exhaustive.the_value = SqlInt32.Null;
            return an_exhaustive;  
        }  
    }  
    public static ExhaustiveInt Parse(SqlString string_value) {
        ExhaustiveInt v = new ExhaustiveInt();  
        v.the_value = string_value.IsNull ? SqlInt32.Null : SqlInt32.Parse(string_value.Value);
        return v;
    }
    public override string ToString()  {  
        return this.the_value.IsNull ? "Unknown" : this.the_value.ToString(); 
    }  
    private bool Validate()  {  
        return true;  
    }  
    [SqlMethod(OnNullCall = false)]  
    public bool IsUnknown()  {  
        return this.the_value.IsNull;  
    }  
}  

[Serializable]  
[Microsoft.SqlServer.Server.SqlUserDefinedType(
    Format.Native,  
    IsByteOrdered = true, 
    IsFixedLength = true,
    Name = "ExhaustiveBigInt",
    ValidationMethodName = "Validate"
)]  
public struct ExhaustiveBigInt : INullable {  
    private SqlInt64 the_value;    
    public bool IsNull { 
        get {
            return false; 
        }
    }  
    public static ExhaustiveBigInt Null {  
        get {  
            ExhaustiveBigInt an_exhaustive = new ExhaustiveBigInt();  
            an_exhaustive.the_value = SqlInt64.Null;
            return an_exhaustive;  
        }  
    }  
    public static ExhaustiveBigInt Parse(SqlString string_value) {
        ExhaustiveBigInt v = new ExhaustiveBigInt();  
        v.the_value = string_value.IsNull ? SqlInt64.Null : SqlInt64.Parse(string_value.Value);
        return v;
    }
    public override string ToString()  {  
        return this.the_value.IsNull ? "Unknown" : this.the_value.ToString(); 
    }  
    private bool Validate()  {  
        return true;  
    }  
    [SqlMethod(OnNullCall = false)]  
    public bool IsUnknown()  {  
        return this.the_value.IsNull;  
    }  
}  