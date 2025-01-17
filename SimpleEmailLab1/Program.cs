﻿using System;
using System.Net.Mail;
using System.ComponentModel;

namespace SimpleEmailLab1
{
    class Program
    {
        static void Main()
        {
           
            Console.WriteLine("Welcome to the command line email client!");
            var loop = true;
            while (loop)
            {
                Console.WriteLine("\nNew Message");
                Console.Write("To: ");
                string to = Console.ReadLine();

                Console.Write("Subject: ");
                string subject = Console.ReadLine();

                Console.Write("Body: ");
                string body = Console.ReadLine();
                
                if (!string.IsNullOrEmpty(body))
                {
                    Console.WriteLine($"Body Text: \n{body}");
                    Console.Write("Are you sure you want to send? (Y/N): ");
                    var key = Console.ReadKey().Key;
                    if (key != ConsoleKey.Y)
                    {
                        continue;
                    }

                  
                    MailMessage mail = new MailMessage();
                    SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("introemailapp1729@gmail.com"); // throwaway email
                    try
                    {
                        mail.To.Add(to);
                    }
                    catch (ArgumentException e)
                    {
                        // if to address is empty or has a problem, it prints then breaks out of the loop
                        Console.Error.WriteLine(e.Message);
                        break;
                    }
                    mail.Subject = subject;
                    mail.Body = body;
                  
                    smtpServer.Port = 25;
                    // credentials for throwaway email; doesn't matter if they're in plaintext
                    smtpServer.Credentials = new System.Net.NetworkCredential("introemailapp1729", "aeaaavdrqnlncldc");
                    smtpServer.EnableSsl = true;

                    smtpServer.Send(mail);
                    Console.WriteLine("done!");
                    // Link event handler
                    smtpServer.SendCompleted += SmtpServer_SendCompleted;

                    Console.Write("\nWould you like to send another email? (Y/N): ");
                    key = Console.ReadKey().Key;
                    if (key != ConsoleKey.Y)
                    {
                        loop = false; // breaks out of loop if user is done
                    }
                }

            }
            
        }

        static void SmtpServer_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // Encourage quick failing
            if (e.Cancelled)
            {
                Console.Error.WriteLine("Message was cancelled");
                return;
            }

            if (e.Error != null)
            {
                Console.Error.WriteLine($"An error occured: {e.Error.Message}");
                return;
            }
            Console.WriteLine("Email sent!");

        }
    }
}