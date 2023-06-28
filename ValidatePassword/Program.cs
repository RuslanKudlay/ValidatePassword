using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ValidatePassword
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ValidPassword validPassword = new ValidPassword();

            Console.Write("Enter file path: ");
            string fileName = Console.ReadLine();
            string[] lines = await validPassword.ReadFileAsync(fileName);
            if(lines.Length == 0)
            {
                Console.ReadLine();
            }
            else
            {
                List<ValidPassword> validLines = validPassword.ValidateLines(lines);
                int countCorrectPasswords = validPassword.CheckPasswordRequirements(validLines);
                Console.Clear();
                Console.WriteLine($"Count valid passwords: {countCorrectPasswords}");
                Console.ReadLine();
            }
        }
    }
}
