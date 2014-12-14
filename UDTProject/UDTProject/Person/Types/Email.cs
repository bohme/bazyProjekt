using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.UserDefined, IsByteOrdered = true, ValidationMethodName = "ValidateInput", MaxByteSize = 170)]
public struct Email : INullable, IBinarySerialize
{
    private string beforeM { get; set; }
    private string afterM { get; set; }
    private string FullEmail { get; set; }
    private bool _null;


    private bool ValidateInput()
    {
        return Email.Validate(this);
    }

    private static bool Validate(Email p)
    {
        if (String.IsNullOrEmpty(p.FullEmail))
            return false;
        else if (System.Text.RegularExpressions.Regex.IsMatch(p.FullEmail, "[a-zA-Z0-9]+@[a-zA-Z0-9]*\\.[a-zA-Z0-9]+"))
        {
            return true;
        }
        else return false;

    }

    public override string ToString()
    {
        return FullEmail;
    }

    public bool IsNull
    {
        get
        {
            return _null;
        }
    }

    public static Email Null
    {
        get
        {
            Email h = new Email();
            h._null = true;
            return h;
        }
    }

    public static Email Parse(SqlString s)
    {
        if (s.IsNull)
            return Null;

        Email email = new Email();
        email.FullEmail = s.Value;
        email.beforeM = s.Value.Substring(0, s.Value.IndexOf("@"));
        email.afterM = s.Value.Substring(s.Value.IndexOf("@"));

        if (Validate(email))
            return email;
        else
            throw new ArgumentException("Niew³aœciwy format adresu Email!");
        
    }

    void IBinarySerialize.Write(System.IO.BinaryWriter w)
    {
        w.Write(IsNull);

        if (!IsNull)
        {
            w.Write(beforeM);
            w.Write(afterM);
            w.Write(FullEmail);
        }
    }

    void IBinarySerialize.Read(System.IO.BinaryReader r)
    {
        _null = r.ReadBoolean();

        if (!IsNull)
        {
            beforeM = r.ReadString();
            afterM = r.ReadString();
            FullEmail = r.ReadString();
        }
    }

}