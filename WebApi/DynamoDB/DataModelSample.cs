/*******************************************************************************
* Copyright 2009-2013 Amazon.com, Inc. or its affiliates. All Rights Reserved.
* 
* Licensed under the Apache License, Version 2.0 (the "License"). You may
* not use this file except in compliance with the License. A copy of the
* License is located at
* 
* http://aws.amazon.com/apache2.0/
* 
* or in the "license" file accompanying this file. This file is
* distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
* KIND, either express or implied. See the License for the specific
* language governing permissions and limitations under the License.
*******************************************************************************/

using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace WebApi.DynamoDB
{
    public partial class Program
    {
        public static void RunDataModelSample(DynamoDBContext context)
        {
            Console.WriteLine("Creating actors");
            WebApi.DynamoDB.Actor christianBale = new WebApi.DynamoDB.Actor
            {
                Name = "Christian Bale",
                Bio = "Christian Charles Philip Bale is an excellent horseman and an avid reader.",
                BirthDate = new DateTime(1974, 1, 30),
                Address = new WebApi.DynamoDB.Address
                {
                    City = "Los Angeles",
                    Country = "USA"
                },
                HeightInMeters = 1.83f
            };
            WebApi.DynamoDB.Actor michaelCaine = new WebApi.DynamoDB.Actor
            {
                Name = "Michael Caine",
                Bio = "Maurice Joseph Micklewhite is an English actor, better known as Michael Caine",
                BirthDate = new DateTime(1933, 3, 14),
                Address = new WebApi.DynamoDB.Address
                {
                    City = "London",
                    Country = "England"
                },
                HeightInMeters = 1.88f
            };




            WebApi.DynamoDB.Actor aotuo = new WebApi.DynamoDB.Actor
            {
                Name = "tzkwizard",
                Bio = "java and .net developer",
                BirthDate = new DateTime(1988, 9, 9),
                Address = new WebApi.DynamoDB.Address
                { 
                    Street = "beijing road",
                    City="Fort Worth",
                    Country = "US"
                    
                },
                HeightInMeters = 1.78f

            };




            Console.WriteLine("Creating movie");
            WebApi.DynamoDB.Movie darkKnight = new WebApi.DynamoDB.Movie
            {
                Title = "The Dark Knight",
                ReleaseDate = new DateTime(2008, 7, 18),
                Genres = new List<string> { "Action", "Crime", "Drama" },
                ActorNames = new List<string>
                {
                    christianBale.Name,
                    michaelCaine.Name
                }
            };




            Console.WriteLine("Saving actors and movie");
            context.Save<WebApi.DynamoDB.Actor>(michaelCaine);
            context.Save<WebApi.DynamoDB.Actor>(christianBale);
            context.Save<WebApi.DynamoDB.Movie>(darkKnight);
            context.Save<WebApi.DynamoDB.Actor>(aotuo);


            Console.WriteLine("Creating and saving new actor");
            WebApi.DynamoDB.Actor maggieGyllenhaal = new WebApi.DynamoDB.Actor
            {
                Name = "Maggie Gyllenhaal",
                BirthDate = new DateTime(1977, 11, 16),
                Bio = "Maggie Gyllenhaal studied briefly at the Royal Academy of Dramatic Arts in London.",
                Address = new WebApi.DynamoDB.Address
                {
                    City = "New York City",
                    Country = "USA"
                },
                HeightInMeters = 1.75f
            };
            context.Save<WebApi.DynamoDB.Actor>(maggieGyllenhaal);

            Console.WriteLine();
            Console.WriteLine("Loading existing movie");
            WebApi.DynamoDB.Movie existingMovie = context.Load<WebApi.DynamoDB.Movie>("The Dark Knight", new DateTime(2008, 7, 18));
            Console.WriteLine(existingMovie.ToString());

            Console.WriteLine();
            Console.WriteLine("Loading nonexistent movie");
            WebApi.DynamoDB.Movie nonexistentMovie = context.Load<WebApi.DynamoDB.Movie>("The Dark Knight", new DateTime(2008, 7, 19));
            Console.WriteLine("Movie is null : " + (nonexistentMovie == null));

            Console.WriteLine("Updating movie and saving");
            existingMovie.ActorNames.Add(maggieGyllenhaal.Name);
            existingMovie.Genres.Add("Thriller");
            context.Save<WebApi.DynamoDB.Movie>(existingMovie);

            Console.WriteLine("Adding movie with same hash key but different range key");
            WebApi.DynamoDB.Movie darkKnight89 = new WebApi.DynamoDB.Movie
            {
                Title = "The Dark Knight",
                Genres = new List<string> { "Drama" },
                ReleaseDate = new DateTime(1989, 2, 23),
                ActorNames = new List<string>
                {
                    "Juan Diego",
                    "Fernando Guillén",
                    "Manuel de Blas"
                }
            };
            context.Save<WebApi.DynamoDB.Movie>(darkKnight89);

            IEnumerable<WebApi.DynamoDB.Movie> movieQueryResults;

            Console.WriteLine();
            Console.WriteLine("Running query 1, expecting 1 result");
            movieQueryResults = context.Query<WebApi.DynamoDB.Movie>("The Dark Knight", QueryOperator.GreaterThan, new DateTime(1995, 1, 1));
            foreach (var result in movieQueryResults)
                Console.WriteLine(result.ToString());

            Console.WriteLine();
            Console.WriteLine("Running query 2, expecting 2 results");
            movieQueryResults = context.Query<WebApi.DynamoDB.Movie>("The Dark Knight", QueryOperator.Between, new DateTime(1970, 1, 1), new DateTime(2011, 1, 1));
            foreach (var result in movieQueryResults)
                Console.WriteLine(result.ToString());


            IEnumerable<WebApi.DynamoDB.Actor> actorScanResults;

            Console.WriteLine();
            Console.WriteLine("Running scan 1, expecting 2 results");
            actorScanResults = context.Scan<WebApi.DynamoDB.Actor>(
                new ScanCondition("HeightInMeters", ScanOperator.LessThan, 1.85f));
            foreach (var result in actorScanResults)
                Console.WriteLine(result.ToString());

            Console.WriteLine();
            Console.WriteLine("Running scan 2, expecting 1 result");
            WebApi.DynamoDB.Address scanAddress = new WebApi.DynamoDB.Address { City = "New York City", Country = "USA" };
            actorScanResults = context.Scan<WebApi.DynamoDB.Actor>(
                new ScanCondition("Address", ScanOperator.Equal, scanAddress));
            foreach (var result in actorScanResults)
                Console.WriteLine(result.ToString());
        }
    }
}
