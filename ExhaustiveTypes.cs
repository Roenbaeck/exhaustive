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
    private Int32   the_value;  
    private bool    is_unknown;
  
    public bool IsNull { 
        get {
            return false; 
        }
    }  

    public static ExhaustiveInt Null {  
        get {  
            ExhaustiveInt an_exhaustive_int = new ExhaustiveInt();  
            an_exhaustive_int.is_unknown = true;
            an_exhaustive_int.the_value = 0;
            return an_exhaustive_int;  
        }  
    }  
    
    public static ExhaustiveInt Parse(SqlString string_value) {
        ExhaustiveInt v = new ExhaustiveInt();  
        if(string_value.IsNull) {
            v.is_unknown = true;
            v.the_value = 0;
        }
        else {
            v.is_unknown = false;
            v.the_value = Int32.Parse(string_value.Value);
        }
        return v;
    }

    public override string ToString()  {  
        if (this.is_unknown)  
            return "Unknown";  
        else 
            return this.the_value.ToString(); 
    }  

    private bool Validate()  {  
        return true;  
    }  
  
    [SqlMethod(OnNullCall = false)]  
    public bool IsUnknown()  {  
        return this.is_unknown;  
    }  
}  


