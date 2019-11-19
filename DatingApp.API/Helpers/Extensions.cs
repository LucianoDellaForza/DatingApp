using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            //dodajemo nas header sa nasom porukom
            response.Headers.Add("Application-Error", message);
            //ova dva headera su ubacena samo da bi ovaj gornji mogao da se prikaze (zbog angulara (drugaciji origin), u postmanu radi normalno)
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error"); //da prikaze nas header
            response.Headers.Add("Access-Control-Allow-Origin", "*");   //da ne bude cors policy problema zbog angulara
        }

        public static int CalculateAge(this DateTime theDateTime)
        {
            var age = DateTime.Today.Year - theDateTime.Year;
            if(theDateTime.AddYears(age) > DateTime.Today)  //if it wasnt his birthday
                age--;

            return age;
        }
    }
}