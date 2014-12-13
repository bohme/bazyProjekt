//------------------------------------------------------------------------------
// <copyright file="CSSqlUserDefinedType.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.UserDefined, IsByteOrdered = true, ValidationMethodName = "ValidateInput", MaxByteSize = 170)]
public struct Pesel : INullable, IBinarySerialize
{
    private int Year { get; set; }
    private int Month { get; set; }
    private int Day { get; set; }
    private int Code { get; set; }
    private string PeselNumber { get; set; }
    private bool _null;


    private bool ValidateInput()
    {
        return Pesel.Validate(this);
    }

    private static bool Validate(Pesel p)
    {
        bool validate = true;

        if (p.Year < 0 && p.Year > 99)
            validate = false;
        if (p.Month <= 0 && p.Month > 99)
            validate = false;
        if (p.Day <= 0 && p.Day > 31)
            validate = false;

        //przeprowadzenie walidacji numeru PESEL
        string pesel = p.PeselNumber;
        int var = (int)Char.GetNumericValue(pesel[0]) +
                    3 * (int)Char.GetNumericValue(pesel[1]) +
                    7 * (int)Char.GetNumericValue(pesel[2]) +
                    9 * (int)Char.GetNumericValue(pesel[3]) +
                        (int)Char.GetNumericValue(pesel[4]) +
                    3 * (int)Char.GetNumericValue(pesel[5]) +
                    7 * (int)Char.GetNumericValue(pesel[6]) +
                    9 * (int)Char.GetNumericValue(pesel[7]) +
                        (int)Char.GetNumericValue(pesel[8]) +
                    3 * (int)Char.GetNumericValue(pesel[9]) +
                        (int)Char.GetNumericValue(pesel[10]);

        //reszta z dzielenie musi byc rowna 0
        if (var % 10 != 0)
            validate = false;

        return validate;
    }

    public override string ToString()
    {
        return PeselNumber;
    }

    public bool IsNull
    {
        get
        {
            return _null;
        }
    }

    public static Pesel Null
    {
        get
        {
            Pesel h = new Pesel();
            h._null = true;
            return h;
        }
    }

    public static Pesel Parse(SqlString s)
    {
        if (s.IsNull)
            return Null;

        //sprawdz czy dlugosc jest odpowiednia
        if (s.Value.Length != 11)
            throw new ArgumentException("Numer PESEL ma nieprawidlowa dlugosc!");

        //zparsuj zadany numer
        Pesel u = new Pesel();
        u.Year = int.Parse(s.Value.Substring(0, 2));
        u.Month = int.Parse(s.Value.Substring(2, 2));
        u.Day = int.Parse(s.Value.Substring(4, 2));
        u.Code = int.Parse(s.Value.Substring(6));
        u.PeselNumber = s.Value;

        //przeprowadz walidacje numeru
        if (!Validate(u))
            throw new ArgumentException("Numer PESEL jest nieprawidlowy!");
        return u;
    }

    void IBinarySerialize.Write(System.IO.BinaryWriter w)
    {
        w.Write(IsNull);

        if (!IsNull)
        {
            w.Write(Year);
            w.Write(Month);
            w.Write(Day);
            w.Write(Code);
            w.Write(PeselNumber);
        }
    }

    void IBinarySerialize.Read(System.IO.BinaryReader r)
    {
        _null = r.ReadBoolean();

        if (!IsNull)
        {
            Year = r.ReadInt32();
            Month = r.ReadInt32();
            Day = r.ReadInt32();
            Code = r.ReadInt32();
            PeselNumber = r.ReadString();
        }
    }

}