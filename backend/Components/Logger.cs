using System;
using Pastel;
using System.Text;
using System.Drawing;

public static class Logger
{
    public static void WriteLine(string path, string statuscode, string ip, string text)
    {
        var time = DateTime.Now;
        string watermark = "[GreenDrive]".PastelBg(Color.Brown);
        statuscode = $"[{statuscode}]";
        switch (statuscode)
        {
            case "403":
                statuscode = statuscode.PastelBg(Color.Yellow).Pastel(Color.Black);
                break;
            case "500":
                statuscode = statuscode.PastelBg(Color.Red);
                break;
            case "200":
                statuscode = statuscode.PastelBg(Color.Green).Pastel(Color.Black);
                break;
            default:
                statuscode = statuscode.PastelBg(Color.Black).Pastel(Color.White);
                break;
        }

        ip = $"[{ip}]".PastelBg(Color.DarkBlue).Pastel(Color.White);
        path = $"[{path}]".PastelBg(Color.DarkOrange).Pastel(Color.Black);
        Console.WriteLine(time + " " + watermark + " " + path + $" {ip}" + $" {statuscode} " + text);
    }
}