using System;
using System.Collections.Generic;
using NUnit.Framework;
using static RubiksCubeChallenge.RubiksCube;
using static RubiksCubeChallenge.RubiksCube.Colour;
using static RubiksCubeChallenge.RubiksCube.Face;

namespace RubiksCubeChallenge.UnitTests
{
    [TestFixture]
    public class RubiksCubeTests
    {
        private RubiksCube _cube;

        [SetUp]
        public void Setup()
        {
            _cube = new RubiksCube();
        }

        [Test]
        public void AssignArrayValues_MethodCalledOnFrontArray_ValuesAssignedCorrectly()
        {
            // Arrange
            Colour[,] oldValues = new Colour[,] { { Green, Green, Green }, { Green, Green, Green }, { Green, Green, Green } };
            Colour[,] newValues = new Colour[,] { { White, White, White }, { White, White, White }, { White, White, White } };

            // Act
            _cube.AssignArrayValues(oldValues, newValues);

            // Assert
            for (int i = 0; i < oldValues.GetLength(0); i++)
            {
                for (int j = 0; j < oldValues.GetLength(1); j++)
                {
                    Assert.That(oldValues[i, j], Is.EqualTo(White));
                }
            }
        }

        [Test]
        public void CreateLocalFacesDictionary_MethodCalled_LocalCopyOfFacesProperty()
        {
            // Arrange

            // Act
            Dictionary<Face, Colour[,]> localCopy = _cube.CreateLocalFacesDictionary();

            // Assert
            foreach (KeyValuePair<Face, Colour[,]> pair in localCopy)
            {
                AssertThatArrayValuesAreEqual(pair.Value, _cube.Faces[pair.Key]);
            }
        }

        [Test]
        public void PopulateDefaultCubeFaces_MethodCalled_FacesAssignedCorrespondingColours()
        {
            // Arrange
            // Setup()

            // Act
            // PopulateDefaultCubeFaces called in constructor (Setup())

            // Assert
            foreach (Face face in Enum.GetValues(typeof(Face)))
            {
                Colour[,] faceArray = _cube.Faces[face];
                for (int i = 0; i < faceArray.GetLength(0); i++)
                {
                    for (int j = 0; j < faceArray.GetLength(1); j++)
                    {
                        Assert.That((int)faceArray[i, j], Is.EqualTo((int)face));
                    }
                }
            }
        }

        [Test]
        public void RotateFace_RotateFrontClockwise_FrontValuesUpdatedCorrectly()
        {
            // Arrange
            _cube.Faces[Front] = new Colour[,] { { Green, Red, White }, { Blue, Orange, Yellow }, { Green, Red, White } };
            Dictionary<Face, Colour[,]> localDictionary = new Dictionary<Face, Colour[,]>();
            localDictionary.Add(Front, new Colour[,] { { Green, Red, White }, { Blue, Orange, Yellow }, { Green, Red, White } });
            Colour[,] expectedResult = new Colour[,] { { Green, Blue, Green}, { Red, Orange, Red}, { White, Yellow, White} };

            // Act
            _cube.RotateFace(localDictionary, Front, RotationDirection.Clockwise);

            // Assert
            AssertThatArrayValuesAreEqual(localDictionary[Front], expectedResult);
        }

        [Test]
        public void RotateFace_RotateFrontAnticlockwise_FrontValuesUpdatedCorrectly()
        {
            // Arrange
            _cube.Faces[Front] = new Colour[,] { { Green, Red, White }, { Blue, Orange, Yellow }, { Green, Red, White } };
            Dictionary<Face, Colour[,]> localDictionary = new Dictionary<Face, Colour[,]>();
            localDictionary.Add(Front, new Colour[,] { { Green, Red, White }, { Blue, Orange, Yellow }, { Green, Red, White } });
            Colour[,] expectedResult = new Colour[,] { { White, Yellow, White }, { Red, Orange, Red }, { Green, Blue, Green } };

            // Act
            _cube.RotateFace(localDictionary, Front, RotationDirection.Anticlockwise);

            // Assert
            AssertThatArrayValuesAreEqual(localDictionary[Front], expectedResult);
        }

        public void AssertThatArrayValuesAreEqual(Colour[,] array1, Colour[,] array2)
        {
            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    Assert.That(array1[i, j], Is.EqualTo(array2[i, j]));
                }
            }
        }
    }
}