using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native, IsByteOrdered = true, ValidationMethodName = "ValidateInput")]
public struct BankAccount : INullable, IBinarySerialize
{
    private string control { get; set; }
    private string partOne { get; set; }
    private string partTwo { get; set; }
    private string partThree { get; set; }
    private string partFour { get; set; }
    private string partFive { get; set; }
    private string partSix { get; set; }


    private bool validateInput()
    {
        return BankAccount.validate(this);
    }

    private static bool validate(BankAccount b)
    {
        int[] weight = { 1, 10, 3, 30, 9, 90, 27, 76, 81, 34, 49, 5, 50, 15, 53, 45, 62, 38, 89, 17, 73, 51, 25, 56, 75, 71, 31, 19, 93, 57 };
        String temp = b.partOne + b.partTwo + b.partThree + b.partFour + b.partFive + b.partSix + "2521" + b.control;
        int[] accountTable = new int[26];
        for (int i = 0; i < 26; i++)
        {
            accountTable[i] = (int)Char.GetNumericValue(temp[0]);
        }

        int sum = 0;
        for (int i = 25; i >= 0; i++)
        {
            sum += accountTable[i] * weight[25 - i];
        }
        if (sum % 97 == 1)
            return true;
        else
            return false;
    }

    public override string ToString()  {
        return this.control+ " " + this.partOne + " " + this.partTwo + " " + this.partThree+" " + this.partFour+ " " + this.partFive+ " " + this.partSix;
    }

    public static BankAccount Null
    {
        get
        {
           BankAccount b = new BankAccount();
           return b;

        }
    }
    public static BankAccount parse(SqlString account)    {
        BankAccount bankAccount = new BankAccount();
        if (account.Value.Length != 26)
            throw new ArgumentException("Numer konta bankowego ma nieprawidlowa dlugosc!");
        else
        {    
            bankAccount.control = account.Value.Substring(0, 2);
            bankAccount.partOne = account.Value.Substring(2, 4);
            bankAccount.partTwo = account.Value.Substring(6, 4);
            bankAccount.partThree = account.Value.Substring(10, 4);
            bankAccount.partFour = account.Value.Substring(14, 4);
            bankAccount.partFive = account.Value.Substring(18, 4);
            bankAccount.partSix = account.Value.Substring(22, 4);
        }
            if(validate(bankAccount))
                return bankAccount;
            else
                throw new ArgumentException("Numer konta bankowego jest nieprawidlowy!");
        }

     void IBinarySerialize.Write(System.IO.BinaryWriter w)
    {
        if (control.Length != 0)
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
        control = r.ReadString();
        partOne = r.ReadString();
        partTwo = r.ReadString();
        partThree = r.ReadString();
        partFour = r.ReadString();
        partFive = r.ReadString();
        partSix = r.ReadString();
    }


}