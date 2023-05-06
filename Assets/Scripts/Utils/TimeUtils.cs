using System;

public static class TimeUtils
{
	public static int MinToSec(this int value) => value * 60;

	public static int SecToMin(this float value) => (int)(value / 60);

	public static long CurrentTimeStamp()
    {
		return DateTimeOffset.Now.ToUnixTimeSeconds();
    }
}