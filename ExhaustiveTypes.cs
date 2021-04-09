using System;  
using System.Data;  
using System.Data.SqlTypes;  
using Microsoft.SqlServer.Server;
//using System.Runtime.InteropServices; // for StructLayout(LayoutKind.Sequential)

public enum ExhaustiveType : ushort { 
    NULL = 0, 
    // Kept in order to make alternatives exhaustive. Everything that does not fall within the alternatives below ends up here.
    Unknown, 
    // (Any value) It is not known whether there is a value or not, and if there is a value it could be anything.
    Unassigned, 
    // (No value) It it known that there is no current value, but there could have been and could become a value.
    Undefined, 
    // (Not Applicable) It is known that there cannot be a value.
    Uninstantiated,
    // (Never assigned) A value is expected to come and is being awaited.
    Hidden,
    // (Blanked out, Anonymised) There is a value, but it is not allowed to be shown or recorded.
    Corrupt, 
    // (Unreadable, Unintelligible) There is a value, but it could not be understood well enough to be put in the database.
    Mismatch,
    // (No correspondence) The result of a process yielded no matching values.
    Error, 
    // (Does not compute) The result of a process yielded an error along the way.
    Infinitesimal,
    // The value is closer to zero than the database can represent, or we would like to represent.
    Beginning, 
    // (Negative Infinity, The beginning of something, Left open interval) The value is negative and larger than the database can represent, or we would like to represent.
    End,
    // (Positive Infinity, The end of something, Right open interval) The value is positive and larger than the database can represent, or we would like to represent.
    Known,
    // Used for values that are known, which in other words corresponds to being not null.
};

public static class ExhaustiveTypeFactory {
    public static string Description(this ExhaustiveType an_exhaustive_type) {
        switch (an_exhaustive_type) {
            case ExhaustiveType.NULL :              { return "NULL"; } 
            case ExhaustiveType.Unknown :           { return "Unknown"; } 
            case ExhaustiveType.Unassigned :        { return "Unassigned"; } 
            case ExhaustiveType.Undefined :         { return "Undefined"; } 
            case ExhaustiveType.Uninstantiated :    { return "Uninstantiated"; } 
            case ExhaustiveType.Hidden :            { return "Hidden"; } 
            case ExhaustiveType.Corrupt :           { return "Corrupt"; } 
            case ExhaustiveType.Mismatch :          { return "Mismatch"; } 
            case ExhaustiveType.Error :             { return "Error"; } 
            case ExhaustiveType.Infinitesimal :     { return "Infinitesimal"; } 
            case ExhaustiveType.Beginning :         { return "Beginning"; } 
            case ExhaustiveType.End :               { return "End"; } 
            case ExhaustiveType.Known :             { return "Known"; } 
        };
        return "NULL";
    }
    public static ExhaustiveType Create(string a_type_description) { 
        switch (a_type_description) {
            case "NULL" :               { return ExhaustiveType.NULL; } 
            case "Unknown" :            { return ExhaustiveType.Unknown; } 
            case "Unassigned" :         { return ExhaustiveType.Unassigned; } 
            case "Undefined" :          { return ExhaustiveType.Undefined; } 
            case "Uninstantiated" :     { return ExhaustiveType.Uninstantiated; } 
            case "Hidden" :             { return ExhaustiveType.Hidden; } 
            case "Corrupt" :            { return ExhaustiveType.Corrupt; } 
            case "Mismatch" :           { return ExhaustiveType.Mismatch; } 
            case "Error" :              { return ExhaustiveType.Error; } 
            case "Infinitesimal" :      { return ExhaustiveType.Infinitesimal; } 
            case "Beginning" :          { return ExhaustiveType.Beginning; } 
            case "End" :                { return ExhaustiveType.End; } 
            case "Known" :              { return ExhaustiveType.Known; } 
        };
        return ExhaustiveType.NULL;
    }
}

[Serializable]  
//[StructLayout(LayoutKind.Sequential)] 
[Microsoft.SqlServer.Server.SqlUserDefinedType(
    Format.Native,  
    IsByteOrdered = true, 
    IsFixedLength = true,
    Name = "ExhaustiveInt",
    ValidationMethodName = "Validate"
)]  
public struct ExhaustiveInt : INullable {  
    private ushort the_type;
    private SqlInt32 the_value; 
    // This is the spirit of #nevernull
    public bool IsNull { 
        get {
            return false; 
        }
    }  
    public ExhaustiveInt(SqlInt32 a_value, string a_type) {
        this.the_value = a_value;
        this.the_type = (ushort) ExhaustiveTypeFactory.Create(a_type);
    }
    public static ExhaustiveInt Null {  
        get {  
            return new ExhaustiveInt(SqlInt32.Null, "NULL");
        }  
    }  
    public static ExhaustiveInt Parse(SqlString string_value) {
        ExhaustiveInt an_exhaustive;
        if(string_value.IsNull) 
            an_exhaustive = new ExhaustiveInt(SqlInt32.Null, "NULL");
        else 
            an_exhaustive = new ExhaustiveInt(SqlInt32.Parse(string_value.Value), "Known"); 
        return an_exhaustive;
    }
    public static ExhaustiveInt Type(SqlString a_type) {
        ExhaustiveInt an_exhaustive;
        if(a_type.IsNull) 
            an_exhaustive = new ExhaustiveInt(SqlInt32.Null, "NULL");
        else 
            an_exhaustive = new ExhaustiveInt(SqlInt32.Null, a_type.Value); 
        return an_exhaustive;
    }
    public override string ToString()  {  
        return this.the_value.IsNull ? ExhaustiveTypeFactory.Description((ExhaustiveType) this.the_type) : this.the_value.ToString();         
    }      
    public bool IsType(SqlString a_type)  {  
        return this.the_type == (ushort) ExhaustiveTypeFactory.Create(a_type.Value);  
    }      
    public bool Validate() {  
        return true;  
    }        
    public SqlInt32 Value {
        get {
            return the_value;
        }
    }
}  

