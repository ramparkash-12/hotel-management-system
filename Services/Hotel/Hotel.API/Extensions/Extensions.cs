using System;
using Microsoft.AspNetCore.Http;

namespace Hotel.API.Extensions
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message) 
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");    
            response.Headers.Add("Access-Control-Allow-Origin", "*");    
        }

        public static DateTime ConvertStringToDateTime(string dateTime)
        {
            try
            {
                return Convert.ToDateTime(dateTime);
            }
            catch(Exception)
            {
                return DateTime.MinValue;
            }
        }
    }
}