using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CenterForDevelopers.Controllers
{
    public class ExampleCSharpController : Controller
    {
        /*  Examples:
            Project: CenterForDevelopersTest
            File: ExampleCSharp/ValueTupleTest.cs
         */
        public IActionResult ValueTuple()
        {
            return View(("Too lazy to create a model", 555));
        }

        /*  Examples:
            Project: CenterForDevelopersTest
            File: ExampleCSharp/LocalFunctionsTest.cs      
        */
        public IActionResult LocalFunctions()
        {
            int startNumber = 100;   // variable captured in localfunction

            return View(GetFinalNumber(7)); 

            int GetFinalNumber(int increment) // local function (declared after usage)
            {
                return startNumber + increment;
            }
        }

        /*  Examples:
            Project: CenterForDevelopersTest
            File: ExampleCSharp/OutAndRefReturnTest.cs      
        */
        public IActionResult OutAndRefReturn()
        {
            var local = 555;

            ref var referenceToLocal = ref local;

            referenceToLocal = 444;

            return View(local);
        }

		/*  Examples:
			Project: CenterForDevelopersTest
			File: ExampleCSharp/PatternMatchingTest.cs      
		*/
		public IActionResult PatternMatching()
		{
			var valuesToSum = new List<object> { 1, 2, new List<object> { 3, 4 } };

			int LocalSumFunction(IEnumerable<object> numbers)
			{
				var sum = 0;

				foreach (var item in numbers)
				{
					if (item is int value)
						sum += value;
					else if (item is IEnumerable<object> list)
						sum += LocalSumFunction(list);
				}

				return sum;
			}

			var totalSum = LocalSumFunction(valuesToSum);

			return View(null /*viewName*/, totalSum);
		}

    }
}
