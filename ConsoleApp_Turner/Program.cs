using System;
using System.Linq;
using ConsoleApp_Turner.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ConsoleApp_Turner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null && args.Count() > 2)
            {
                String filmTitle = args[0].Trim('"');
                String filmProperty = args[1];
                String filmContextField = args[2];

                var client = new RestClient("http://swapi.co/api/films");
                var request = new RestRequest(Method.GET);

                var apiResponse = client.Execute(request);

                if(apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var response = JsonConvert.DeserializeObject<FilmsResponse>(apiResponse.Content);

                    if(response.results != null && response.results.Count() > 0)
                    {
                        var requestedFilm = response.results.FirstOrDefault(f => f.title.Equals(filmTitle));


                        if(requestedFilm != null)
                        {
                            var filmPropertyResults = PropertyHelper.GetPropertyField<Result, string[]>(requestedFilm, filmProperty);

                            filmPropertyResults.AsEnumerable().AsParallel().ForAll(s =>
                            {
                                DisplayFilmInfo(s, filmContextField);
                            });

                            Console.ReadLine();
                        }
                    }
                }
                else
                {
                    Console.WriteLine(String.Format("Error while calling api. ResponseCode: {0}", apiResponse.StatusCode));
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
                Console.WriteLine("Please enter the required 3 input fields following the notes below.");
                Console.WriteLine("1. A film title, enclosed in double-quotes");
                Console.WriteLine("2. A property name on the[film entity](http://swapi.co/documentation#films) which represents a collection of other entities.");
                Console.WriteLine("3. A property name, which will exist on the entity identified in number 2 above, and that propety will be a string property(not an array).");
                Console.WriteLine("You will need to close this console and restart the application to enter your input fields.");
                Console.Read();
            }
        }

        static void DisplayFilmInfo(string apiUrl, string fieldName)
        {
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.GET);

            var apiResponse = client.Execute(request);

            if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var response = JsonConvert.DeserializeObject<FilmFieldResponse>(apiResponse.Content);

                var fieldValue = PropertyHelper.GetPropertyField<FilmFieldResponse, string>(response, fieldName);

                Console.WriteLine(fieldValue);
            }
        }
    }
}
