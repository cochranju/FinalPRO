using FinalPRO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace FinalPRO
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a dictionary to store the pending packages
            // The key is the resident's name and the value is a list of packages for that resident
            var pendingPackages = new Dictionary<string, List<Package>>();

            // Create a list to store unknown packages
            var unknownPackages = new List<Package>();

            // Create a dictionary to store the login credentials for the office staff
            var loginCredentials = new Dictionary<string, string>();
            loginCredentials["admin"] = "password123";

            // Continuously prompt the office staff to log in
            bool isLoggedIn = false;
            while (!isLoggedIn)
            {
                Console.WriteLine("Please log in with your username and password:");
                Console.Write("Username: ");
                string username = Console.ReadLine();
                Console.Write("Password: ");
                string password = Console.ReadLine();

                if (loginCredentials.ContainsKey(username) && loginCredentials[username] == password)
                {
                    isLoggedIn = true;
                    Console.WriteLine("Log in successful. Welcome to the Package Tracker.");
                }
                else
                {
                    Console.WriteLine("Invalid username or password. Please try again.");
                }
            }

            // Continuously prompt the office staff for actions to take
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1. Add a new package for a resident");
                Console.WriteLine("2. Remove a picked-up package from the pending area");
                Console.WriteLine("3. Retrieve package history for a resident");
                Console.WriteLine("4. Quit");
                Console.Write("Enter the number of your choice: ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    // Add a new package for a resident
                    Console.Write("Enter the resident's name: ");
                    string residentName = Console.ReadLine();
                    Console.Write("Enter the package agency (FedEx, USPS, UPS, etc.): ");
                    string agency = Console.ReadLine();
                    Console.Write("Enter the delivery date (yyyy-MM-dd): ");
                    DateTime deliveryDate = DateTime.Parse(Console.ReadLine());

                    // Create a new package and add it to the pending area
                    var package = new Package(agency, deliveryDate);
                    if (pendingPackages.ContainsKey(residentName))
                    {
                        pendingPackages[residentName].Add(package);
                    }
                    else
                    {
                        pendingPackages[residentName] = new List<Package> { package };
                    }

                    // Send an email notification to the resident
                    try
                    {
                        SendEmail(residentName, package);
                        Console.WriteLine($"An email notification has been sent to {residentName}.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error sending email to {residentName}: {e.Message}");
                    }
                }
                else if (choice == "2")
                {
                    // Remove a picked-up package from the pending area
                    Console.Write("Enter the resident's name: ");
                    string residentName = Console.ReadLine();
                    Console.Write("Enter the package agency (FedEx, USPS, UPS, etc.): ");
                    string agency = Console.ReadLine();
                    Console.Write("Enter the delivery date (yyyy-MM-dd): ");
                    DateTime deliveryDate = DateTime.Parse(Console.ReadLine());

                    // Search for the package in the pending area
                    if (pendingPackages.ContainsKey(residentName))
                    {
                        var packages = pendingPackages[residentName];
                        var package = packages.FirstOrDefault(p => p.Agency == agency && p.DeliveryDate == deliveryDate);
                        if (package != null)
                        {
                            // Remove the package from the pending area
                            packages.Remove(package);
                            Console.WriteLine("Package removed from pending area.");
                        }
                        else
                        {
                            Console.WriteLine("Package not found in pending area.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Resident not found in pending area.");
                    }
                }
                else if (choice == "3")
                {
                    // Retrieve package history for a resident
                    Console.Write("Enter the resident's name: ");
                    string residentName = Console.ReadLine();
                    Console.Write("Enter the resident's unit number: ");
                    int unitNumber = int.Parse(Console.ReadLine());

                    // Search for the resident in the pending area
                    if (pendingPackages.ContainsKey(residentName))
                    {
                        var packages = pendingPackages[residentName];
                        Console.WriteLine($"Package history for resident {residentName} in unit {unitNumber}:");
                        foreach (var package in packages)
                        {
                            Console.WriteLine($"Agency: {package.Agency}, Delivery Date: {package.DeliveryDate}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Resident not found in pending area.");
                    }
                }
                else if (choice == "4")
                {
                    // Quit the program
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
        }

        static void SendEmail(string recipient, Package package)
        {
            var mail = new MailMessage();
            mail.To.Add(recipient);
            mail.Subject = "Package Delivery Notification";
            mail.Body = $"A package from {package.Agency} is waiting for pickup in the leasing office. " +
                        $"It was delivered on {package.DeliveryDate.ToShortDateString()}.";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new System.Net.NetworkCredential("youremail@gmail.com", "yourpassword"),
                EnableSsl = true
            };
            client.Send(mail);
        }
    }

    // Class to represent a package
    class Package
    {
        public string Agency { get; set; }
        public DateTime DeliveryDate { get; set; }

        public Package(string agency, DateTime deliveryDate)
        {
            Agency = agency;
            DeliveryDate = deliveryDate;
        }
    }
}