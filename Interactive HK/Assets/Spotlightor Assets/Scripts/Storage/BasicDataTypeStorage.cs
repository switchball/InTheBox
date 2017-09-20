using UnityEngine;
using System.Collections;
using System;

public static class BasicDataTypeStorage
{
	private static DateTime DateTimeReference = new DateTime (2013, 1, 1);

	public static void SetDateTimeWithFloatPrecision (Enum identifier, DateTime dateTime)
	{
		SetDateTimeWithFloatPrecision (ConvertEnumToStringIdentifier (identifier), dateTime);
	}

	public static void SetDateTimeWithFloatPrecision (string identifier, DateTime dateTime)
	{
		float hoursToRefDate = (float)dateTime.Subtract (DateTimeReference).TotalHours;
		SetFloat (identifier, hoursToRefDate);
	}

	public static DateTime GetDateTimeWithFloatPrecision (Enum identifier)
	{
		return GetDateTimeWithFloatPrecision (ConvertEnumToStringIdentifier (identifier));
	}

	public static DateTime GetDateTimeWithFloatPrecision (string identifier)
	{
		float hoursToRefDate = GetFloat (identifier);
		if (hoursToRefDate == 0)
			return DateTime.Now;
		else
			return DateTimeReference.AddHours (hoursToRefDate);
	}

	public static void DeleteDateTimeWithFlotPrecision (Enum identifier)
	{
		BasicDataTypeStorage.DeleteFloat (identifier);
	}

	public static void DeleteDateTimeWithFlotPrecision (string identifier)
	{
		BasicDataTypeStorage.DeleteFloat (identifier);
	}

	public static void SetDateTimeDay (Enum identifier, DateTime dateTime)
	{
		SetDateTimeDay (ConvertEnumToStringIdentifier (identifier), dateTime);
	}

	public static void SetDateTimeDay (string identifier, DateTime dateTime)
	{
		int daysToRef = Mathf.FloorToInt ((float)dateTime.Subtract (DateTimeReference).TotalDays);
		SetInt (identifier, daysToRef);
	}

	public static int GetDateTimeDaysToRef (Enum identifier)
	{
		return GetDateTimeDaysToRef (ConvertEnumToStringIdentifier (identifier));
	}

	public static int GetDateTimeDaysToRef (string identifier)
	{
		int daysToRef = GetInt (identifier);
		if (daysToRef == 0)
			return  Mathf.FloorToInt ((float)DateTime.Now.Subtract (DateTimeReference).TotalDays);
		else
			return daysToRef;
	}

	public static DateTime GetDateTimeDay (Enum identifier)
	{
		return GetDateTimeDay (ConvertEnumToStringIdentifier (identifier));
	}

	public static DateTime GetDateTimeDay (string identifier)
	{
		int daysToRef = GetInt (identifier);
		if (daysToRef == 0)
			return new DateTime (DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
		else
			return DateTimeReference.AddDays ((double)daysToRef);
	}

	public static bool UpdateIntMax (Enum identifier, int newValue)
	{
		return UpdateIntMax (ConvertEnumToStringIdentifier (identifier), newValue);
	}

	public static bool UpdateIntMax (string identifier, int newValue)
	{
		if (!HasKey (identifier) || GetInt (identifier) < newValue) {
			SetInt (identifier, newValue);
			return true;
		} else
			return false;
	}

	public static void SetInt (Enum identifier, int newValue)
	{
		SetInt (ConvertEnumToStringIdentifier (identifier), newValue);
	}

	public static void SetInt (string identifier, int newValue)
	{
		SaveSlotsStorage.ActiveSaveSlot.Data.Ints [identifier] = newValue;
	}

	public static int GetInt (Enum identifier)
	{
		return GetInt (ConvertEnumToStringIdentifier (identifier));
	}

	public static int GetInt (string identifier)
	{
		if (SaveSlotsStorage.ActiveSaveSlot.Data.Ints.ContainsKey (identifier))
			return SaveSlotsStorage.ActiveSaveSlot.Data.Ints [identifier];
		else
			return 0;
	}

	public static void DeleteInt (Enum identifier)
	{
		DeleteInt (ConvertEnumToStringIdentifier (identifier));
	}

	public static void DeleteInt (string identifier)
	{
		SaveSlotsStorage.ActiveSaveSlot.Data.Ints.Remove (identifier);
	}

	public static void SetFloat (Enum identifier, float newValue)
	{
		SetFloat (ConvertEnumToStringIdentifier (identifier), newValue);
	}

	public static void SetFloat (string identifier, float newValue)
	{
		SaveSlotsStorage.ActiveSaveSlot.Data.Floats [identifier] = newValue;
	}

	public static float GetFloat (Enum identifier)
	{
		return GetFloat (ConvertEnumToStringIdentifier (identifier));
	}

	public static float GetFloat (string identifier)
	{
		if (SaveSlotsStorage.ActiveSaveSlot.Data.Floats.ContainsKey (identifier))
			return SaveSlotsStorage.ActiveSaveSlot.Data.Floats [identifier];
		else
			return 0f;
	}

	public static void DeleteFloat (Enum identifier)
	{
		DeleteInt (ConvertEnumToStringIdentifier (identifier));
	}

	public static void DeleteFloat (string identifier)
	{
		SaveSlotsStorage.ActiveSaveSlot.Data.Floats.Remove (identifier);
	}

	public static void SetString (Enum identifier, string newValue)
	{
		SetString (ConvertEnumToStringIdentifier (identifier), newValue);
	}

	public static void SetString (string identifier, string newValue)
	{
		SaveSlotsStorage.ActiveSaveSlot.Data.Strings [identifier] = newValue;
	}

	public static string GetString (Enum identifier)
	{
		return GetString (ConvertEnumToStringIdentifier (identifier));
	}

	public static string GetString (string identifier)
	{
		if (SaveSlotsStorage.ActiveSaveSlot.Data.Strings.ContainsKey (identifier))
			return SaveSlotsStorage.ActiveSaveSlot.Data.Strings [identifier];
		else
			return "";
	}

	public static void DeleteString (Enum identifier)
	{
		DeleteString (ConvertEnumToStringIdentifier (identifier));
	}

	public static void DeleteString (string identifier)
	{
		SaveSlotsStorage.ActiveSaveSlot.Data.Strings.Remove (identifier);
	}

	public static bool HasKey (Enum identifier)
	{
		return HasKey (ConvertEnumToStringIdentifier (identifier));
	}

	public static bool HasKey (string identifier)
	{
		bool hasKey = false;
		hasKey |= SaveSlotsStorage.ActiveSaveSlot.Data.Ints.ContainsKey (identifier);
		hasKey |= SaveSlotsStorage.ActiveSaveSlot.Data.Floats.ContainsKey (identifier);
		hasKey |= SaveSlotsStorage.ActiveSaveSlot.Data.Strings.ContainsKey (identifier);
		return hasKey;
	}

	public static string ConvertEnumToStringIdentifier (Enum identifer)
	{
		return identifer.GetType ().ToString () + "_" + EnumHelper.EnumToString (identifer);
	}
}
