using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ValidatePassword
{
    public class ValidPassword
    {
        public char RequiredCharacter { get; set; }
        public RangeNumber RangeNumber { get; set; }
        public string Password { get; set; }

        public async Task<string[]> ReadFileAsync(string fileName)
        {
            string[] fileLines = new string[0];
            try
            {
                if (!File.Exists(fileName))
                {
                    throw new FileNotFoundException();
                }
                else
                {
                    fileLines = await File.ReadAllLinesAsync(fileName);
                    return fileLines;
                }
                
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File not found: {fileName}");
                return fileLines;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
                return fileLines;
            }
        }

        public List<ValidPassword> ValidateLines(string[] lines)
        {
            List<ValidPassword> validPasswords = new List<ValidPassword>();
            foreach (var line in lines)
            {
                try
                {
                    Regex regex = new Regex(@"([a-zA-Z])\s(\d+)-(\d+):\s(.{1,100})");
                    Match match = regex.Match(line);

                    if (match.Success)
                    {
                        int from = int.Parse(match.Groups[2].Value);
                        int to = int.Parse(match.Groups[3].Value);
                        var validPassword = new ValidPassword
                        {
                            RequiredCharacter = match.Groups[1].Value[0],
                            RangeNumber = new RangeNumber
                            {
                                From = from,
                                To = to
                            },
                            Password = match.Groups[4].Value
                        };

                        validPasswords.Add(validPassword);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occured: {ex.Message}");
                }
                
            }
            return validPasswords;
        }

        public int CheckPasswordRequirements(List<ValidPassword> validPasswords)
        {
            int countCorrectPasswords = 0;

            foreach (var item in validPasswords)
            {
                string pattern = $"^(.*[{item.RequiredCharacter}{char.ToUpper(item.RequiredCharacter)}{char.ToLower(item.RequiredCharacter)}].*){{{item.RangeNumber.From},{item.RangeNumber.To}}}$";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(item.Password);

                if (match.Success)
                {
                    countCorrectPasswords++;
                }
            }
            return countCorrectPasswords;
        }
    }
}
