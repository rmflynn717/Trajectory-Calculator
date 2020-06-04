using System;
using static System.Math;
using static TrajectoryCalculator.GravitationalAcceleration;

namespace TrajectoryCalculator
{
    class Program
    {
        public static double ReadPositiveDouble()
        {
            double userParsedInput;
            while (true)
            {
                Console.WriteLine();
                string userInput = Console.ReadLine();
                bool parsed = double.TryParse(userInput, out userParsedInput);
                if (parsed && userParsedInput >= 0)
                {
                    break;
                }
                Console.WriteLine("Invalid entry, please enter a non-negative value.");
            }
            return userParsedInput;
        }

        public static double ComputeLaunchAngle(
            double horizontalDistance,
            double initialSpeed)
        {
            //Computes launch angle in degrees
            double launchAngle;
            launchAngle = Asin(horizontalDistance * AccelerationDueToGravity /
                            Pow(initialSpeed, 2)) / 2;
            return launchAngle * 180 / PI;
        }

        public static double ComputeTrajectoryHeight(
            double initialSpeed,
            double launchAngle)
        {
            //Computes Maximum Height of Trajectory
            double height = Pow(initialSpeed * Sin(launchAngle * PI / 180), 2) /
                                    (2 * AccelerationDueToGravity);
            return height;
        }

        public static void VelocityTuner(
            double initialSpeed,
            double horizontalDistance,
            double ceilingHeight)
        {
            double height;
            do
            {
                initialSpeed += 0.1;
                double launchAngle = ComputeLaunchAngle(
                    horizontalDistance,
                    initialSpeed);
                height = ComputeTrajectoryHeight(
                    initialSpeed,
                    launchAngle);
                Console.WriteLine($"Trying v0 {initialSpeed:0.0} m/s, " +
                                    $"theta {launchAngle:0.000}. Max height   " +
                                    $"{height:0.000} meters");
            }
            while (height >= ceilingHeight);
        }

        private static void Main()
        {
            //Data gathering from user
            Console.WriteLine("Please enter a value for horizontal distance (in meters):");
            double horizontalDistance = ReadPositiveDouble();
            Console.WriteLine("Please enter a value for basket diameter (in meters):");
            double basketDiameter = ReadPositiveDouble();
            Console.WriteLine("Please enter a value for initial ball speed (in m/s)");
            double initialSpeed = ReadPositiveDouble();
            Console.WriteLine("Please enter a value for ceiling height (in meters)");
            double ceilingHeight = ReadPositiveDouble();

            //Optimal angle computation
            double optimalLaunchAngle = ComputeLaunchAngle(
                horizontalDistance,
                initialSpeed);
            Console.WriteLine();
            Console.WriteLine($"Optimal Angle    {optimalLaunchAngle:0.000}");

            //Minimal and Maximum Angle Computations
            double maximalLaunchAngle = ComputeLaunchAngle(
                horizontalDistance + basketDiameter / 2,
                 initialSpeed);
            double minimalLaunchAngle = ComputeLaunchAngle(
                horizontalDistance - basketDiameter / 2,
                initialSpeed);
            Console.WriteLine();
            Console.WriteLine($"Range of acceptable angles is from " +
                                $"{minimalLaunchAngle:0.000} " +
                                $"to {maximalLaunchAngle:0.000}");

            //Determine maximum altitude
            double maximumHeight = ComputeTrajectoryHeight(
                initialSpeed,
                optimalLaunchAngle);
            Console.WriteLine();
            Console.WriteLine($"The maximum height of the ball is {maximumHeight:0.000}" +
                $" meters");

            //Check if ball bumps ceiling
            Console.WriteLine($"Will the ball hit the ceiling?  " +
                $"{maximumHeight > ceilingHeight}");

            //Tunes velocity to accommodate height constraint
            if (maximumHeight > ceilingHeight)
            {
                VelocityTuner(initialSpeed, horizontalDistance, ceilingHeight);
            }
        }
    }
}
