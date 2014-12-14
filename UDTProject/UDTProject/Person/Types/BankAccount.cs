using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Text;


[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.UserDefined, IsByteOrdered = true, ValidationMethodName = "ValidateInput", MaxByteSize = 170)]
public struct BankAccount : INullable, IBinarySerialize
{
    private string control { get; set; }
    private string partOne { get; set; }
    private string partTwo { get; set; }
    private string partThree { get; set; }
    private string partFour { get; set; }
    private string partFive { get; set; }
    private string partSix { get; set; }
    private bool _null;


    private bool ValidateInput()
    {
        return BankAccount.Validate(this);
    }

    private static bool Validate(BankAccount b)
    {
        string bankAccount = b.control + b.partOne + b.partTwo + b.partThree + b.partFour + b.partFive + b.partSix;
        bankAccount = bankAccount.ToUpper(); //IN ORDER TO COPE WITH THE REGEX BELOW
        if (String.IsNullOrEmpty(bankAccount))
            return false;
        else if (System.Text.RegularExpressions.Regex.IsMatch(bankAccount, "^[A-Z0-9]"))
        {
            bankAccount = bankAccount.Replace(" ", String.Empty);
            string bank =
            bankAccount.Substring(4, bankAccount.Length - 4) + bankAccount.Substring(0, 4);
            int asciiShift = 55;
            StringBuilder sb = new StringBuilder();
            foreach (char c in bank)
            {
                int v;
                if (Char.IsLetter(c)) v = c - asciiShift;
                else v = int.Parse(c.ToString());
                sb.Append(v);
            }
            string checkSumString = sb.ToString();
            int checksum = int.Parse(checkSumString.Substring(0, 1));
            for (int i = 1; i < checkSumString.Length; i++)
            {
                int v = int.Parse(checkSumString.Substring(i, 1));
                checksum *= 10;
                checksum += v;
                checksum %= 97;
            }
            return checksum == 1;
        }
        else
            return false;
    }

    public override string ToString()  {
        return this.control+ " " + this.partOne + " " + this.partTwo + " " + this.partThree+" " + this.partFour+ " " + this.partFive+ " " + this.partSix;
    }

    public bool IsNull
    {
        get
        {
            return _null;
        }
    }

    public static BankAccount Null
    {
        get
        {
           BankAccount b = new BankAccount();
           b._null = true;
           return b;
        }
    }

    public static BankAccount Parse(SqlString account)    {
        if (account.IsNull)
            return Null;

        if (account.Value.Length != 26)
            throw new ArgumentException("Numer konta bankowego ma nieprawidlowa dlugosc!");
  
        BankAccount bankAccount = new BankAccount();
        bankAccount.control = account.Value.Substring(0, 2);
        bankAccount.partOne = account.Value.Substring(2, 4);
        bankAccount.partTwo = account.Value.Substring(6, 4);
        bankAccount.partThree = account.Value.Substring(10, 4);
        bankAccount.partFour = account.Value.Substring(14, 4);
        bankAccount.partFive = account.Value.Substring(18, 4);
        bankAccount.partSix = account.Value.Substring(22, 4);

        if(!Validate(bankAccount))
            throw new ArgumentException("Numer konta bankowego jest nieprawidlowy!");

        return bankAccount;
    }

    void IBinarySerialize.Write(System.IO.BinaryWriter w)
    {
        w.Write(IsNull);

        if (!IsNull)
        {
            w.Write(control);
            w.Write(partOne);
            w.Write(partTwo);
            w.Write(partThree);
            w.Write(partFour);
            w.Write(partFive);
            w.Write(partSix);
        }
    }

    void IBinarySerialize.Read(System.IO.BinaryReader r)
    {
        _null = r.ReadBoolean();

        if (!IsNull)
        {
            control = r.ReadString();
            partOne = r.ReadString();
            partTwo = r.ReadString();
            partThree = r.ReadString();
            partFour = r.ReadString();
            partFive = r.ReadString();
            partSix = r.ReadString();
        }
    }
}