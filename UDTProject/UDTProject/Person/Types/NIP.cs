using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.UserDefined, IsByteOrdered = true, ValidationMethodName = "ValidateInput", MaxByteSize = 170)]
public struct NIP : INullable, IBinarySerialize
{
    private string prefix { get; set; }
    private string rest { get; set; }
    private string nipNumber { get; set; }

    private bool _null;


    private bool ValidateInput()
    {
        return NIP.Validate(this);
    }

    private static bool Validate(NIP b)
    {
        string temp = b.nipNumber;
        int[] weigth = {6, 5, 7, 2, 3, 4, 5, 6, 7 };
        int sum = 6 * (int)Char.GetNumericValue(temp[0]) +
                    5 * (int)Char.GetNumericValue(temp[1]) +
                    7 * (int)Char.GetNumericValue(temp[2]) +
                    2 * (int)Char.GetNumericValue(temp[3]) +
                    3 * (int)Char.GetNumericValue(temp[4]) +
                    4 * (int)Char.GetNumericValue(temp[5]) +
                    5 * (int)Char.GetNumericValue(temp[6]) +
                    6 * (int)Char.GetNumericValue(temp[7]) +
                    7 * (int)Char.GetNumericValue(temp[8]);

        if (sum % 11 == (int)Char.GetNumericValue(temp[9]))
            return true;
        else
            return false;
    }

    public override string ToString()
    {
        return nipNumber;
    }

    public bool IsNull
    {
        get
        {
            return _null;
        }
    }

    public static NIP Null
    {
        get
        {
            NIP b = new NIP();
            b._null = true;
            return b;
        }
    }

    public static NIP Parse(SqlString s)
    {
        if (s.IsNull)
            return Null;
        if(s.Value.Length!=10)
            throw new ArgumentException("Numer NIP ma nieprawidlowa dlugosc!");
        NIP nip = new NIP();
        nip.prefix = s.Value.Substring(0, 3);
        nip.rest = s.Value.Substring(3);
        nip.nipNumber = s.Value;

        if (Validate(nip))
            return nip;
        else
            throw new ArgumentException("Numer NIP jest nieprawid³owy!");

    }

    void IBinarySerialize.Write(System.IO.BinaryWriter w)
    {
        w.Write(IsNull);

        if (!IsNull)
        {
            w.Write(prefix);
            w.Write(rest);
            w.Write(nipNumber);
        }
    }

    void IBinarySerialize.Read(System.IO.BinaryReader r)
    {
        _null = r.ReadBoolean();

        if (!IsNull)
        {
            prefix = r.ReadString();
            rest = r.ReadString();
            nipNumber = r.ReadString();
        }
    }
}