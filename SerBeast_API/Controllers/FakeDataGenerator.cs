﻿using Bogus;
using Microsoft.AspNetCore.Mvc;
using SerBeast_API.Model.Dto;

namespace SerBeast_API.Controllers
{
    public class FakeDataGenerator : Controller
    {
        private static readonly List<string> BarangayList = new()
        {
            "Bagumbayan", "Bambang", "Calzada", "Cembo", "Central Bicutan", "Central Signal Village",
            "Comembo", "East Rembo", "Fort Bonifacio", "Hagonoy", "Ibayo Tipas", "Katuparan",
            "Ligid Tipas", "Lower Bicutan", "Maharlika Village", "Napindan", "New Lower Bicutan",
            "North Daang Hari", "North Signal Village", "Northside", "Palingon", "Pembo", "Pinagsama",
            "Pitogo", "Rizal", "Santa Ana", "South Cembo", "South Daang Hari", "South Signal Village",
            "Southside", "Tanyag", "Tuktukan", "Upper Bicutan", "Ususan", "Wawa", "Western Bicutan",
            "West Rembo"
        };

        public static RegisterRequestDTO GenerateFakeCustomer()
        {
            var faker = new Faker("en");

            return new RegisterRequestDTO
            {
                FirstName = faker.Name.FirstName(),
                MiddleInitial = faker.Random.String2(1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"),
                LastName = faker.Name.LastName(),
                Email = faker.Internet.Email(),
                Password = "customer",
                PhoneNumber = "09" + faker.Random.Number(100000000, 999999999),
                HouseLotBlockNumber = faker.Address.BuildingNumber(),
                Street = faker.Address.StreetName(),
                Barangay = faker.PickRandom(BarangayList) 
            };
        }
    }
}
