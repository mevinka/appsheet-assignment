using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Types;
using System.Text.RegularExpressions;


namespace ConsoleApp1
{
    public class Program
    {
        /*
        Param: A possible phone number string
        Returns true if the number is a seven digit number
        separated by dashes or spaces, with optional parens
        around the area code
        e.g. 555-555-5555 OR (555) 555-5555 OR 555 555 5555...etc
        */
        private static bool IsValidPhoneNumber(String number)
        {
            String parenPhone = @"^\(\d{3}\)\s*\d{3}(\s|-)\d{4}$";
            String phone = @"^\d{3}(\s*|-)\d{3}(\s|-)\d{4}$";
            Regex parenR = new Regex(parenPhone);
            Regex phoneR = new Regex(phone);
            return (Regex.IsMatch(number, parenPhone) || Regex.IsMatch(number, phone));
        }

        public static void Main(String[] args)
        {
            // Init: make connection to API, get all Ids
            ApiConnector apiConnection = new ApiConnector();
            int[] ids = apiConnection.GetAllIds();
            Types.Person[] minPeopleArr = new Types.Person[5];

            // To store the oldest person in our list of 5 youngest
            Types.Person oldest = null;

            int counter = 0; // To count off the first 5 IDs with valid phone numbers

            for (int i = 0; i < ids.Length; i++)
            {
                Person cur = apiConnection.MakeDetailsRequest(ids[i]);
                if (IsValidPhoneNumber(cur.number))
                {
                    if (counter < 5)        // Case: processed under 5 valid users
                    {
                        minPeopleArr[counter] = cur;
                        if(oldest == null || cur.age > oldest.age)
                        {
                            oldest = cur;
                        }
                        counter++;
                    }
                    else if (cur.age < oldest.age)  // Case: current person is younger than oldest one stored
                    {
                        int maxIndex = Array.IndexOf(minPeopleArr, oldest);

                        minPeopleArr[maxIndex] = cur;

                        oldest = minPeopleArr.Max();


                    }
                }
            }

            // Finished, output the list of people
            foreach (Person person in minPeopleArr)
            {
                Console.WriteLine(person.ToString());
            }

            Console.WriteLine("Hit Enter to quit");
            Console.Read();
        }
    }
}
