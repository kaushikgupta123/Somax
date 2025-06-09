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
* Date         JIRA Item Person           Description
* ===========  ========= ================ ========================================================
* 2014-Sep-17  SOM-333   Roger Lawton     Change conversion method to ConvertTimeFromUTC
**************************************************************************************************
*/

using System;

namespace Common.Extensions 
{
  public static class DateTimeExtensions
  {
    public static DateTime ToUserTimeZone(this DateTime dateTime, string userTimeZone)
    {
      // Date/Time received from SQL Select is Unspecified 
      // We are supposed to store all dates and times as UTC
      // If unspecified - set the kind to UTC
      TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(userTimeZone);
      if (dateTime.Kind == DateTimeKind.Unspecified)
      {
        DateTime newdatetime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        return TimeZoneInfo.ConvertTime(newdatetime, timeZoneInfo);
      }
      else
      {
        return TimeZoneInfo.ConvertTime(dateTime, timeZoneInfo);
      }
    }
    /// <summary>
    /// Static method to convert from user timezone to UTC 
    /// </summary>
    /// <param name="dateTime"> DateTime object being changed</param>
    /// <param name="userTimeZone">string that identifies the user's time zone</param>
    public static DateTime ConvertFromUserToUTC(this DateTime dateTime, string userTimeZone)
    {
      DateTime newDateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
      TimeZoneInfo userTZ = TimeZoneInfo.FindSystemTimeZoneById(userTimeZone);
      return TimeZoneInfo.ConvertTime(newDateTime, userTZ, TimeZoneInfo.Utc);

    }

    /// <summary>
    /// Static method to convert from UTC to user timezone
    /// </summary>
    /// <param name="dateTime"> DateTime object being changed</param>
    /// <param name="userTimeZone">string that identifies the user's time zone</param>
    public static DateTime ConvertFromUTCToUser(this DateTime dateTime, string userTimeZone)
    {
      DateTime newDateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
      TimeZoneInfo userTZ = TimeZoneInfo.FindSystemTimeZoneById(userTimeZone);
      return TimeZoneInfo.ConvertTime(newDateTime, TimeZoneInfo.Utc, userTZ);

    }


    public static DateTime? ToUserTimeZone(this DateTime? dateTime, string userTimeZone)
    {
      if (dateTime.HasValue)
      {
        // Date/Time received from SQL Select is Unspecified 
        // We are supposed to store all dates and times as UTC
        // If unspecified - set the kind to UTC
        DateTime newdateTime = dateTime.GetValueOrDefault();
        newdateTime = DateTime.SpecifyKind(newdateTime, DateTimeKind.Utc);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(userTimeZone);
        return TimeZoneInfo.ConvertTime(newdateTime, timeZoneInfo);
      }
      else
      {
        return null;
      }
    }

    public static DateTime ToDefaultTimeZone(this DateTime dateTime)
    {
      return dateTime.ToUniversalTime();
    }

    public static string ToShortDate(this DateTime? dateTime)
    {
      return dateTime.HasValue ? dateTime.Value.ToShortDateString() : "n/a";
    }
  }
}
