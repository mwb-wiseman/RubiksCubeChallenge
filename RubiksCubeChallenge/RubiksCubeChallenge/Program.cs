using System;
using static RubiksCubeChallenge.RubiksCube;

namespace RubiksCubeChallenge
{
    public class Program
    {
        static void Main(string[] args)
        {
            RubiksCube cube = new RubiksCube();

            // Test specified in challenge
            cube.RotateFace90Degrees(Face.Front, RotationDirection.Clockwise);
            cube.RotateFace90Degrees(Face.Right, RotationDirection.Anticlockwise);
            cube.RotateFace90Degrees(Face.Up, RotationDirection.Clockwise);
            cube.RotateFace90Degrees(Face.Back, RotationDirection.Anticlockwise);
            cube.RotateFace90Degrees(Face.Left, RotationDirection.Clockwise);
            cube.RotateFace90Degrees(Face.Down, RotationDirection.Anticlockwise);

            cube.DisplayCube();
            Console.Read();
        }
    }
}
