using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TestYield
{
    class Program
    {
        public class PowersOf2
        {
            public static System.Collections.Generic.IEnumerable<int> Power(int number, int exponent)
            {
                int result = 1;

                for (int i = 0; i < exponent; i++)
                {
                    Console.WriteLine( "Iterator is {0}",i);
                    result = result * number;
                    yield return result;
                }
            }

            // Output: 2 4 8 16 32 64 128 256
        }

        public static class GalaxyClass
        {
            public static void ShowGalaxies()
            {
                var theGalaxies = new Galaxies();
                foreach (Galaxy theGalaxy in theGalaxies.NextGalaxy)
                {
                    Console.WriteLine(theGalaxy.Name + " distance " + theGalaxy.MegaLightYears.ToString());
                }
            }

            public class Galaxies
            {

                public System.Collections.Generic.IEnumerable<Galaxy> NextGalaxy
                {
                    get
                    {
                        Console.WriteLine("First");
                        yield return new Galaxy { Name = "Tadpole", MegaLightYears = 400 };
                        Console.WriteLine("Second");
                        yield return new Galaxy { Name = "Pinwheel", MegaLightYears = 25 };
                        Console.WriteLine("Third");
                        yield return new Galaxy { Name = "Milky Way", MegaLightYears = 0 };
                        Console.WriteLine("Fourth");
                        yield return new Galaxy { Name = "Andromeda", MegaLightYears = 3 };
                    }
                }

            }

            public class Galaxy
            {
                public String Name { get; set; }
                public int MegaLightYears { get; set; }
            }
        }

        static void Main(string[] args)
        {
            int power = 0;
            // Display powers of 2 up to the exponent of 8: 
            foreach (int i in PowersOf2.Power(2, 8))
            {
                power += 1;
                Console.WriteLine("2^{0} = {1} ", power, i);
            }
            // Display powers of 2 up to the exponent of 8: 
            power = 0;
            foreach (int i in PowersOf2.Power(2, 8))
            {
                power += 1;
                Console.WriteLine("2^{0} = {1} ", power , i);
            }

            GalaxyClass.ShowGalaxies();
        }
    }
}
