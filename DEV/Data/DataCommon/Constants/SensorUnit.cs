/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person                 Description
* ===========  ========= ====================== =================================================
* 
**************************************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Constants
{
    public class SensorUnit
    {
        //public const string Fahrenheit = "F";//--Loc:Fahrenheit--//
        //public const string Celsius = "C";//Loc:--Celsius--//
        //public const string Amps = "Amps";//--Loc:Amps--//
        //public const string PSI = "PSI";//Loc:--PSI--// 
        //public const string Volts = "Volts";//Loc:--Volts--// 

        public string Fahrenheit { get; } = "F";//--Loc:Fahrenheit--//
        public string Celsius { get; } = "C";//Loc:--Celsius--//
        public string Amps { get; } = "Amps";//--Loc:Amps--//
        public string PSI { get; } = "PSI";//Loc:--PSI--// 
        public string Volts { get; } = "Volts";//Loc:--Volts--// 
    }
}