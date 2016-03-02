﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LVMS.FactuurSturen.TestClient.Properties;

namespace LVMS.FactuurSturen.TestClient
{
    /// <summary>
    /// Test application for testing the C# FactuurSturen.nl library.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Task t = Connect();
            t.Wait();
        }

        static async Task Connect()
        {
            Console.WriteLine("FactuurSturen.nl API Test Client");

            var credentials = GetCredentials();
            Console.WriteLine("Connecting...");

            var client = new FactuurSturenClient();
            await client.LoginAsync(credentials.UserName, credentials.Password);
            Console.WriteLine("Connected.");

            //var clients = await client.GetClients();
            //var products = await client.GetProducts();

            //var product = await client.GetProduct(1);
            //var retval = await client.CreateProduct(product);
            //await client.DeleteProduct(product);

            var invoices = await client.GetInvoicesWithFilter(InvoiceFilters.Paid);

        }

        /// <summary>
        /// Get plain-text credentials that are needed for connecting with the API.
        /// Credentials are loaded from a text file or from a prompt.
        /// </summary>
        /// <returns></returns>
        private static NetworkCredential GetCredentials()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Settings.Default.CredentialsFile) && File.Exists(
                    Environment.ExpandEnvironmentVariables(Settings.Default.CredentialsFile)))
                {
                    var lines = File.ReadLines(Environment.ExpandEnvironmentVariables(Settings.Default.CredentialsFile)).ToArray();
                    return new NetworkCredential(lines[0], lines[1]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't read credentials from file. Error: " + ex.Message);
            }
            return GetCredentialsViaPrompt();
        }

        /// <summary>
        /// Prompt the user for a user name and password.
        /// </summary>
        /// <returns></returns>
        private static NetworkCredential GetCredentialsViaPrompt()
        {
            Console.WriteLine("Please enter your FactuurSturen.nl credentials.");
            Console.Write("User name: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = ReadPassword();
            Console.WriteLine();

            return new NetworkCredential(username, password);
        }

        // Taken from http://stackoverflow.com/a/7049688/393367

        /// <summary>
        /// Like System.Console.ReadLine(), only with a mask.
        /// </summary>
        /// <param name="mask">a <c>char</c> representing your choice of console mask</param>
        /// <returns>the string the user typed in </returns>
        public static string ReadPassword(char mask)
        {
            const int ENTER = 13, BACKSP = 8, CTRLBACKSP = 127;
            int[] FILTERED = { 0, 27, 9, 10 /*, 32 space, if you care */ }; // const

            var pass = new Stack<char>();
            char chr = (char)0;

            while ((chr = Console.ReadKey(true).KeyChar) != ENTER)
            {
                if (chr == BACKSP)
                {
                    if (pass.Count > 0)
                    {
                        Console.Write("\b \b");
                        pass.Pop();
                    }
                }
                else if (chr == CTRLBACKSP)
                {
                    while (pass.Count > 0)
                    {
                        Console.Write("\b \b");
                        pass.Pop();
                    }
                }
                else if (FILTERED.Count(x => chr == x) > 0) { }
                else
                {
                    pass.Push((char)chr);
                    Console.Write(mask);
                }
            }

            Console.WriteLine();

            return new string(pass.Reverse().ToArray());
        }

        /// <summary>
        /// Like System.Console.ReadLine(), only with a mask.
        /// </summary>
        /// <returns>the string the user typed in </returns>
        public static string ReadPassword()
        {
            return ReadPassword('*');
        }
    }
}
