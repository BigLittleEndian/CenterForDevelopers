using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xunit;

namespace CenterForDevelopers
{
    // C#7 - Local Functions Are Bringing Cleaner Code And Better Performance Comparing To Lambda Expressions
    // http://www.biglittleendian.com/article/102

    public class LocalFunctionsTest
    {
        /************************************   

            Enumeration and async example

         ************************************/

        public class Hero
        {
            public string Name { get; set; }
            public int Version { get; set; }
            public Hero(string name, int version)
            {
                Name = name;
                Version = version;
            }

            public async Task<int> IncreaseVersion(int versionIncrement)
            {
                if(versionIncrement < 1) throw new InvalidOperationException("Can't do that!");

                await Task.Delay(200);

                Version += versionIncrement;
                return Version;
            } 

            public Task<int> IncreaseVersionWithLocal(int versionIncrement) // Not async - just returning Task
            {
                if(versionIncrement < 1) throw new InvalidOperationException("Can't do that!");

                return IncrementVersion();

                async Task<int> IncrementVersion()
                {
                    await Task.Delay(200);

                    Version += versionIncrement;
                    return Version;
                }
            } 
        }

        public class City
        {
            private List<Hero> Heroes { get; }

            public City()
            {
                Heroes = new List<Hero>();
            }

            public void AddHero(string name, int version)
            {
                Heroes.Add(new Hero(name, version));
            }

            // Code could be written more optimal (using LINQ) but for the purpose of showing yield example it is dumbed-down. 
            public IEnumerable<Hero> GetHeroes(int minVersion)
            {
                int maxVersion = minVersion + 10;

                int count = Heroes.Count();

                if (count == 0) throw new InvalidOperationException("Not initialized.");

                for (int i = 0; i < count; i++)
                {
                    var hero = Heroes[i];

                    if(hero.Version >= minVersion && hero.Version <= maxVersion)
                    {
                        yield return hero;
                    }
                }
            }
         
            public IEnumerable<Hero> GetHeroesWithLocal(int minVersion)
            {
                int maxVersion = minVersion + 10;

                int count = Heroes.Count();

                if (count == 0) throw new InvalidOperationException("Not initialized.");

                return LoopThruHeroes();

                // Local Function (declared after usage):
                IEnumerable<Hero> LoopThruHeroes()
                {
                    for (int i = 0; i < count; i++)
                    {
                        var hero = Heroes[i];

                        if(hero.Version >= minVersion && hero.Version <= maxVersion)
                        {
                            yield return hero;
                        }
                    }
                }
            }
        }

        // Unit tests

        [Fact]
        public void GetHeroesTest()
        {
            bool loopExecuted = false;

            var sanFransokyo = new City();

            // Comment this out to play with exception:
            sanFransokyo.AddHero("Big Hero", 6);
            sanFransokyo.AddHero("Small Hero", 5);  

            // * A *
            // With Local Function you will get exception here:
            var cityHeroes = sanFransokyo.GetHeroes(5);
            // var cityHeroes = sanFransokyo.GetHeroesWithLocal(5);

            // * B *
            // Without Local Function you will get exception here:
            foreach(var hero in cityHeroes)
            {
                Assert.True(hero.Version >= 5);
                loopExecuted = true;
            }

            Assert.True(loopExecuted);
        }

        [Fact]
        public async void IncreaseVersionTest()
        {
            var hero = new Hero("Big Hero", 6);

            // To play with exception pass "-1" below:
            var increase = 1;

            // * A *
            // With Local Function you will get exception here:
            var taskToWait = hero.IncreaseVersion(increase);
            // var taskToWait = hero.IncreaseVersionWithLocal(increase);

            // * B *
            // Without Local Function you will get stuck here with faulted task:
            var newVersion = await taskToWait;

            Assert.Equal(7, newVersion);
        }

        /************************************   

            Local vs Lambda example

         ************************************/
        public class HowItIsMade
        {
            private int a = 1;

            #region Lambda
            public int SumWithLambda(int b)
            {
                int c = 3;

                Func<int, int> LocalLambda = (x) =>  { return x + b + c; };

                return LocalLambda(a);
            }

            public int SumWithLambdaAndThis(int b)
            {
                int c = 3;

                Func<int> LocalLambdaWithThis = () =>  { return this.a + b + c; };

                return LocalLambdaWithThis();
            }
            #endregion

            #region LocalFunction
            public int SumWithLocalFunction(int b)
            {
                int c = 3;

                return LocalFunction(a);

                int LocalFunction(int x)
                { 
                    return x + b + c; 
                };
            }

            public int SumWithLocalFunctionAndThis(int b)
            {
                int c = 3;

               return LocalFunctionWithThis();

                int LocalFunctionWithThis()
                { 
                    return this.a + b + c; 
                };
            }
            #endregion
        }

        // Unit Tests

        [Fact]
        public void HowItIsMadeTest()
        {
            var howItIsMade = new HowItIsMade();

            Assert.Equal(6, howItIsMade.SumWithLambda(2));
            Assert.Equal(6, howItIsMade.SumWithLambdaAndThis(2));
            Assert.Equal(6, howItIsMade.SumWithLocalFunction(2));
            Assert.Equal(6, howItIsMade.SumWithLocalFunctionAndThis(2));
        }
    }
}

