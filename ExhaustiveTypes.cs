using System;  
using System.Data;  
using System.Data.SqlTypes;  
using Microsoft.SqlServer.Server;

public enum ExhaustiveType : byte { 
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
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native, 
    IsByteOrdered = true, IsFixedLength = true, Name = "ExhaustiveInt", ValidationMethodName = "Validate"
)]  
public struct ExhaustiveInt : INullable {  
    private byte the_type;
    private SqlInt32 the_value; 
    public bool IsNull { 
        get { return false; }  // This is the spirit of #nevernull
    }  
    public ExhaustiveInt(SqlInt32 a_value, string a_type) {
        this.the_value = a_value;
        this.the_type = (byte) ExhaustiveTypeFactory.Create(a_type);
    }
    public static ExhaustiveInt Null {  
        get { return new ExhaustiveInt(SqlInt32.Null, "NULL"); }  
    }  
    public static ExhaustiveInt Parse(SqlString string_value) {
        if(string_value.IsNull) 
            return new ExhaustiveInt(SqlInt32.Null, "NULL");
        return new ExhaustiveInt(SqlInt32.Parse(string_value.Value), "Known"); 
    }
    public static ExhaustiveInt Type(SqlString a_type) {
        if(a_type.IsNull) 
            return new ExhaustiveInt(SqlInt32.Null, "NULL");
        return new ExhaustiveInt(SqlInt32.Null, a_type.Value); 
    }
    public override string ToString()  {  
        return this.the_value.IsNull ? ExhaustiveTypeFactory.Description((ExhaustiveType) this.the_type) : this.the_value.ToString();         
    }      
    public bool IsType(SqlString a_type)  {  
        return this.the_type == (byte) ExhaustiveTypeFactory.Create(a_type.Value);  
    }      
    public bool Validate() {  
        return true;  
    }        
    public SqlInt32 Value {
        get { return the_value; }
    }
}  

[Serializable]  
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native, 
    IsByteOrdered = true, IsFixedLength = true, Name = "ExhaustiveBigInt", ValidationMethodName = "Validate"
)]  
public struct ExhaustiveBigInt : INullable {  
    private byte the_type;
    private SqlInt64 the_value; 
    public bool IsNull { 
        get { return false; }  // This is the spirit of #nevernull
    }  
    public ExhaustiveBigInt(SqlInt64 a_value, string a_type) {
        this.the_value = a_value;
        this.the_type = (byte) ExhaustiveTypeFactory.Create(a_type);
    }
    public static ExhaustiveBigInt Null {  
        get { return new ExhaustiveBigInt(SqlInt64.Null, "NULL"); }  
    }  
    public static ExhaustiveBigInt Parse(SqlString string_value) {
        if(string_value.IsNull) 
            return new ExhaustiveBigInt(SqlInt64.Null, "NULL");
        return new ExhaustiveBigInt(SqlInt64.Parse(string_value.Value), "Known"); 
    }
    public static ExhaustiveBigInt Type(SqlString a_type) {
        if(a_type.IsNull) 
            return new ExhaustiveBigInt(SqlInt64.Null, "NULL");
        return new ExhaustiveBigInt(SqlInt64.Null, a_type.Value); 
    }
    public override string ToString()  {  
        return this.the_value.IsNull ? ExhaustiveTypeFactory.Description((ExhaustiveType) this.the_type) : this.the_value.ToString();         
    }      
    public bool IsType(SqlString a_type)  {  
        return this.the_type == (byte) ExhaustiveTypeFactory.Create(a_type.Value);  
    }      
    public bool Validate() {  
        return true;  
    }        
    public SqlInt64 Value {
        get { return the_value; }
    }
}  

[Serializable]  
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native, 
    IsByteOrdered = true, IsFixedLength = true, Name = "ExhaustiveSmallInt", ValidationMethodName = "Validate"
)]  
public struct ExhaustiveSmallInt : INullable {  
    private byte the_type;
    private SqlInt16 the_value; 
    public bool IsNull { 
        get { return false; }  // This is the spirit of #nevernull
    }  
    public ExhaustiveSmallInt(SqlInt16 a_value, string a_type) {
        this.the_value = a_value;
        this.the_type = (byte) ExhaustiveTypeFactory.Create(a_type);
    }
    public static ExhaustiveSmallInt Null {  
        get { return new ExhaustiveSmallInt(SqlInt16.Null, "NULL"); }  
    }  
    public static ExhaustiveSmallInt Parse(SqlString string_value) {
        if(string_value.IsNull) 
            return new ExhaustiveSmallInt(SqlInt16.Null, "NULL");
        return new ExhaustiveSmallInt(SqlInt16.Parse(string_value.Value), "Known"); 
    }
    public static ExhaustiveSmallInt Type(SqlString a_type) {
        if(a_type.IsNull) 
            return new ExhaustiveSmallInt(SqlInt16.Null, "NULL");
        return new ExhaustiveSmallInt(SqlInt16.Null, a_type.Value); 
    }
    public override string ToString()  {  
        return this.the_value.IsNull ? ExhaustiveTypeFactory.Description((ExhaustiveType) this.the_type) : this.the_value.ToString();         
    }      
    public bool IsType(SqlString a_type)  {  
        return this.the_type == (byte) ExhaustiveTypeFactory.Create(a_type.Value);  
    }      
    public bool Validate() {  
        return true;  
    }        
    public SqlInt16 Value {
        get { return the_value; }
    }
}  

[Serializable]  
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native, 
    IsByteOrdered = true, IsFixedLength = true, Name = "ExhaustiveTinyInt", ValidationMethodName = "Validate"
)]  
public struct ExhaustiveTinyInt : INullable {  
    private byte the_type;
    private SqlByte the_value; 
    public bool IsNull { 
        get { return false; }  // This is the spirit of #nevernull
    }  
    public ExhaustiveTinyInt(SqlByte a_value, string a_type) {
        this.the_value = a_value;
        this.the_type = (byte) ExhaustiveTypeFactory.Create(a_type);
    }
    public static ExhaustiveTinyInt Null {  
        get { return new ExhaustiveTinyInt(SqlByte.Null, "NULL"); }  
    }  
    public static ExhaustiveTinyInt Parse(SqlString string_value) {
        if(string_value.IsNull) 
            return new ExhaustiveTinyInt(SqlByte.Null, "NULL");
        return new ExhaustiveTinyInt(SqlByte.Parse(string_value.Value), "Known"); 
    }
    public static ExhaustiveTinyInt Type(SqlString a_type) {
        if(a_type.IsNull) 
            return new ExhaustiveTinyInt(SqlByte.Null, "NULL");
        return new ExhaustiveTinyInt(SqlByte.Null, a_type.Value); 
    }
    public override string ToString()  {  
        return this.the_value.IsNull ? ExhaustiveTypeFactory.Description((ExhaustiveType) this.the_type) : this.the_value.ToString();         
    }      
    public bool IsType(SqlString a_type)  {  
        return this.the_type == (byte) ExhaustiveTypeFactory.Create(a_type.Value);  
    }      
    public bool Validate() {  
        return true;  
    }        
    public SqlByte Value {
        get { return the_value; }
    }
}  

[Serializable]  
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native, 
    IsByteOrdered = true, IsFixedLength = true, Name = "ExhaustiveBit", ValidationMethodName = "Validate"
)]  
public struct ExhaustiveBit : INullable {  
    private byte the_type;
    private SqlBoolean the_value; 
    public bool IsNull { 
        get { return false; }  // This is the spirit of #nevernull
    }  
    public ExhaustiveBit(SqlBoolean a_value, string a_type) {
        this.the_value = a_value;
        this.the_type = (byte) ExhaustiveTypeFactory.Create(a_type);
    }
    public static ExhaustiveBit Null {  
        get { return new ExhaustiveBit(SqlBoolean.Null, "NULL"); }  
    }  
    public static ExhaustiveBit Parse(SqlString string_value) {
        if(string_value.IsNull) 
            return new ExhaustiveBit(SqlBoolean.Null, "NULL");
        return new ExhaustiveBit(SqlBoolean.Parse(string_value.Value), "Known"); 
    }
    public static ExhaustiveBit Type(SqlString a_type) {
        if(a_type.IsNull) 
            return new ExhaustiveBit(SqlBoolean.Null, "NULL");
        return new ExhaustiveBit(SqlBoolean.Null, a_type.Value); 
    }
    public override string ToString()  {  
        return this.the_value.IsNull ? ExhaustiveTypeFactory.Description((ExhaustiveType) this.the_type) : this.the_value.ToString();         
    }      
    public bool IsType(SqlString a_type)  {  
        return this.the_type == (byte) ExhaustiveTypeFactory.Create(a_type.Value);  
    }      
    public bool Validate() {  
        return true;  
    }        
    public SqlBoolean Value {
        get { return the_value; }
    }
}  

/* --------- NOTE THAT SqlDecimal IS NOT Blittable AND REQUIRES Format.UserDefined --------- */
[Serializable]  
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.UserDefined, MaxByteSize = 19,
    IsByteOrdered = true, IsFixedLength = true, Name = "ExhaustiveDecimal", ValidationMethodName = "Validate"
)]  
public struct ExhaustiveDecimal : INullable, IBinarySerialize {  
    private byte the_type;
    private SqlDecimal the_value; 
    public bool IsNull { 
        get { return false; }  // This is the spirit of #nevernull
    }  
    public ExhaustiveDecimal(SqlDecimal a_value, string a_type) {
        this.the_value = a_value;
        this.the_type = (byte) ExhaustiveTypeFactory.Create(a_type);
    }
    public static ExhaustiveDecimal Null {  
        get { return new ExhaustiveDecimal(SqlDecimal.Null, "NULL"); }  
    }  
    public static ExhaustiveDecimal Parse(SqlString string_value) {
        if(string_value.IsNull) 
            return new ExhaustiveDecimal(SqlDecimal.Null, "NULL");
        return new ExhaustiveDecimal(SqlDecimal.Parse(string_value.Value), "Known"); 
    }
    public static ExhaustiveDecimal Type(SqlString a_type) {
        if(a_type.IsNull) 
            return new ExhaustiveDecimal(SqlDecimal.Null, "NULL");
        return new ExhaustiveDecimal(SqlDecimal.Null, a_type.Value); 
    }
    public override string ToString()  {  
        return this.the_value.IsNull ? ExhaustiveTypeFactory.Description((ExhaustiveType) this.the_type) : this.the_value.ToString();         
    }      
    public bool IsType(SqlString a_type)  {  
        return this.the_type == (byte) ExhaustiveTypeFactory.Create(a_type.Value);  
    }      
    public bool Validate() {  
        return true;  
    }        
    public SqlDecimal Value {
        get { return the_value; }
    }
    public void Write(System.IO.BinaryWriter w) {
        w.Write(this.the_type);
        w.Write(this.the_value.Precision);
        w.Write(this.the_value.Scale);
        w.Write((byte) (this.the_value.IsPositive ? 1 : 0));
        for (int i = 0; i < this.the_value.Data.Length; i++)
            w.Write(this.the_value.Data[i]);
    }
    public void Read(System.IO.BinaryReader r) {
        this.the_type = r.ReadByte();
        byte bPrecision = r.ReadByte();
        byte bScale = r.ReadByte();
        bool fPositive = r.ReadByte() == 1 ? true : false;
        int data1 = r.ReadInt32();
        int data2 = r.ReadInt32();
        int data3 = r.ReadInt32();
        int data4 = r.ReadInt32();
        this.the_value = new SqlDecimal(bPrecision, bScale, fPositive, data1, data2, data3, data4);
    }  
}  

[Serializable]  
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native, 
    IsByteOrdered = true, IsFixedLength = true, Name = "ExhaustiveDouble", ValidationMethodName = "Validate"
)]  
public struct ExhaustiveDouble : INullable {  
    private byte the_type;
    private SqlDouble the_value; 
    public bool IsNull { 
        get { return false; }  // This is the spirit of #nevernull
    }  
    public ExhaustiveDouble(SqlDouble a_value, string a_type) {
        this.the_value = a_value;
        this.the_type = (byte) ExhaustiveTypeFactory.Create(a_type);
    }
    public static ExhaustiveDouble Null {  
        get { return new ExhaustiveDouble(SqlDouble.Null, "NULL"); }  
    }  
    public static ExhaustiveDouble Parse(SqlString string_value) {
        if(string_value.IsNull) 
            return new ExhaustiveDouble(SqlDouble.Null, "NULL");
        return new ExhaustiveDouble(SqlDouble.Parse(string_value.Value), "Known"); 
    }
    public static ExhaustiveDouble Type(SqlString a_type) {
        if(a_type.IsNull) 
            return new ExhaustiveDouble(SqlDouble.Null, "NULL");
        return new ExhaustiveDouble(SqlDouble.Null, a_type.Value); 
    }
    public override string ToString()  {  
        return this.the_value.IsNull ? ExhaustiveTypeFactory.Description((ExhaustiveType) this.the_type) : this.the_value.ToString();         
    }      
    public bool IsType(SqlString a_type)  {  
        return this.the_type == (byte) ExhaustiveTypeFactory.Create(a_type.Value);  
    }      
    public bool Validate() {  
        return true;  
    }        
    public SqlDouble Value {
        get { return the_value; }
    }
}  

[Serializable]  
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native, 
    IsByteOrdered = true, IsFixedLength = true, Name = "ExhaustiveMoney", ValidationMethodName = "Validate"
)]  
public struct ExhaustiveMoney : INullable {  
    private byte the_type;
    private SqlMoney the_value; 
    public bool IsNull { 
        get { return false; }  // This is the spirit of #nevernull
    }  
    public ExhaustiveMoney(SqlMoney a_value, string a_type) {
        this.the_value = a_value;
        this.the_type = (byte) ExhaustiveTypeFactory.Create(a_type);
    }
    public static ExhaustiveMoney Null {  
        get { return new ExhaustiveMoney(SqlMoney.Null, "NULL"); }  
    }  
    public static ExhaustiveMoney Parse(SqlString string_value) {
        if(string_value.IsNull) 
            return new ExhaustiveMoney(SqlMoney.Null, "NULL");
        return new ExhaustiveMoney(SqlMoney.Parse(string_value.Value), "Known"); 
    }
    public static ExhaustiveMoney Type(SqlString a_type) {
        if(a_type.IsNull) 
            return new ExhaustiveMoney(SqlMoney.Null, "NULL");
        return new ExhaustiveMoney(SqlMoney.Null, a_type.Value); 
    }
    public override string ToString()  {  
        return this.the_value.IsNull ? ExhaustiveTypeFactory.Description((ExhaustiveType) this.the_type) : this.the_value.ToString();         
    }      
    public bool IsType(SqlString a_type)  {  
        return this.the_type == (byte) ExhaustiveTypeFactory.Create(a_type.Value);  
    }      
    public bool Validate() {  
        return true;  
    }        
    public SqlMoney Value {
        get { return the_value; }
    }
}  

[Serializable]  
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native, 
    IsByteOrdered = true, IsFixedLength = true, Name = "ExhaustiveSingle", ValidationMethodName = "Validate"
)]  
public struct ExhaustiveSingle : INullable {  
    private byte the_type;
    private SqlSingle the_value; 
    public bool IsNull { 
        get { return false; }  // This is the spirit of #nevernull
    }  
    public ExhaustiveSingle(SqlSingle a_value, string a_type) {
        this.the_value = a_value;
        this.the_type = (byte) ExhaustiveTypeFactory.Create(a_type);
    }
    public static ExhaustiveSingle Null {  
        get { return new ExhaustiveSingle(SqlSingle.Null, "NULL"); }  
    }  
    public static ExhaustiveSingle Parse(SqlString string_value) {
        if(string_value.IsNull) 
            return new ExhaustiveSingle(SqlSingle.Null, "NULL");
        return new ExhaustiveSingle(SqlSingle.Parse(string_value.Value), "Known"); 
    }
    public static ExhaustiveSingle Type(SqlString a_type) {
        if(a_type.IsNull) 
            return new ExhaustiveSingle(SqlSingle.Null, "NULL");
        return new ExhaustiveSingle(SqlSingle.Null, a_type.Value); 
    }
    public override string ToString()  {  
        return this.the_value.IsNull ? ExhaustiveTypeFactory.Description((ExhaustiveType) this.the_type) : this.the_value.ToString();         
    }      
    public bool IsType(SqlString a_type)  {  
        return this.the_type == (byte) ExhaustiveTypeFactory.Create(a_type.Value);  
    }      
    public bool Validate() {  
        return true;  
    }        
    public SqlSingle Value {
        get { return the_value; }
    }
}  

[Serializable]  
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native, 
    IsByteOrdered = true, IsFixedLength = true, Name = "ExhaustiveDateTime", ValidationMethodName = "Validate"
)]  
public struct ExhaustiveDateTime : INullable {  
    private byte the_type;
    private SqlDateTime the_value; 
    public bool IsNull { 
        get { return false; }  // This is the spirit of #nevernull
    }  
    public ExhaustiveDateTime(SqlDateTime a_value, string a_type) {
        this.the_value = a_value;
        this.the_type = (byte) ExhaustiveTypeFactory.Create(a_type);
    }
    public static ExhaustiveDateTime Null {  
        get { return new ExhaustiveDateTime(SqlDateTime.Null, "NULL"); }  
    }  
    public static ExhaustiveDateTime Parse(SqlString string_value) {
        if(string_value.IsNull) 
            return new ExhaustiveDateTime(SqlDateTime.Null, "NULL");
        return new ExhaustiveDateTime(SqlDateTime.Parse(string_value.Value), "Known"); 
    }
    public static ExhaustiveDateTime Type(SqlString a_type) {
        if(a_type.IsNull) 
            return new ExhaustiveDateTime(SqlDateTime.Null, "NULL");
        return new ExhaustiveDateTime(SqlDateTime.Null, a_type.Value); 
    }
    public override string ToString()  {  
        return this.the_value.IsNull ? ExhaustiveTypeFactory.Description((ExhaustiveType) this.the_type) : this.the_value.ToString();         
    }      
    public bool IsType(SqlString a_type)  {  
        return this.the_type == (byte) ExhaustiveTypeFactory.Create(a_type.Value);  
    }      
    public bool Validate() {  
        return true;  
    }        
    public SqlDateTime Value {
        get { return the_value; }
    }
}  

/* ------------------ NOTE THAT SqlString DOES NOT HAVE A Parse METHOD ------------------ */
[Serializable]  
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native, 
    IsByteOrdered = true, IsFixedLength = true, Name = "ExhaustiveString", ValidationMethodName = "Validate"
)]  
public struct ExhaustiveString : INullable {  
    private byte the_type;
    private SqlString the_value; 
    public bool IsNull { 
        get { return false; }  // This is the spirit of #nevernull
    }  
    public ExhaustiveString(SqlString a_value, string a_type) {
        this.the_value = a_value;
        this.the_type = (byte) ExhaustiveTypeFactory.Create(a_type);
    }
    public static ExhaustiveString Null {  
        get { return new ExhaustiveString(SqlString.Null, "NULL"); }  
    }  
    public static ExhaustiveString Parse(SqlString string_value) {
        if(string_value.IsNull) 
            return new ExhaustiveString(SqlString.Null, "NULL");
        return new ExhaustiveString(new SqlString(string_value.Value), "Known"); 
    }
    public static ExhaustiveString Type(SqlString a_type) {
        if(a_type.IsNull) 
            return new ExhaustiveString(SqlString.Null, "NULL");
        return new ExhaustiveString(SqlString.Null, a_type.Value); 
    }
    public override string ToString()  {  
        return this.the_value.IsNull ? ExhaustiveTypeFactory.Description((ExhaustiveType) this.the_type) : this.the_value.ToString();         
    }      
    public bool IsType(SqlString a_type)  {  
        return this.the_type == (byte) ExhaustiveTypeFactory.Create(a_type.Value);  
    }      
    public bool Validate() {  
        return true;  
    }        
    public SqlString Value {
        get { return the_value; }
    }
}  

/* ------------------ NOTE THAT DateTime? NEEDS TO BE USED INSTEAD OF SqlDateTime ------------------ */
[Serializable]  
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.UserDefined, MaxByteSize = 10, 
    IsByteOrdered = true, IsFixedLength = true, Name = "ExhaustiveDateTime2", ValidationMethodName = "Validate"
)]  
public struct ExhaustiveDateTime2 : INullable, IBinarySerialize {  
    private byte the_type;
    private DateTime? the_value; 
    public bool IsNull { 
        get { return false; }  // This is the spirit of #nevernull
    }  
    public ExhaustiveDateTime2(DateTime? a_value, string a_type) {
        this.the_value = a_value;
        this.the_type = (byte) ExhaustiveTypeFactory.Create(a_type);
    }
    public static ExhaustiveDateTime2 Null {  
        get { return new ExhaustiveDateTime2(null, "NULL"); }  
    }  
    public static ExhaustiveDateTime2 Parse(SqlString string_value) {
        if(string_value.IsNull) 
            return new ExhaustiveDateTime2(null, "NULL");
        return new ExhaustiveDateTime2(DateTime.Parse(string_value.Value), "Known"); 
    }
    public static ExhaustiveDateTime2 Type(SqlString a_type) {
        if(a_type.IsNull) 
            return new ExhaustiveDateTime2(null, "NULL");
        return new ExhaustiveDateTime2(null, a_type.Value); 
    }
    public override string ToString()  {  
        return this.the_value.HasValue ? this.the_value.ToString() : ExhaustiveTypeFactory.Description((ExhaustiveType) this.the_type);         
    }      
    public bool IsType(SqlString a_type)  {  
        return this.the_type == (byte) ExhaustiveTypeFactory.Create(a_type.Value);  
    }      
    public bool Validate() {  
        return true;  
    }        
    public DateTime Value {
        get { return the_value.Value; }
    }
    public void Write(System.IO.BinaryWriter w) {
        w.Write(this.the_type);
        if(this.the_value.HasValue) {
            w.Write((byte) 1);
            w.Write(this.the_value.Value.Ticks);
        }
        else {
            w.Write((byte) 0);
        }
    }
    public void Read(System.IO.BinaryReader r) {
        this.the_type = r.ReadByte();
        if(r.ReadByte() == 1) 
            this.the_value = new DateTime(r.ReadInt64());
        else
            this.the_value = null;        
    }  
}  