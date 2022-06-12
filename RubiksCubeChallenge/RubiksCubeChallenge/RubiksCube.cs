using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeChallenge
{
    public class RubiksCube
    {
        public enum Colour
        {
            Green, // 0
            Red, // 1
            White, // 2
            Blue, // 3
            Orange, // 4
            Yellow // 5
        }

        public enum Face
        {
            Front, // 0
            Right, // 1
            Up, // 2
            Back, // 3
            Left, // 4
            Down // 5
        }

        public enum RotationDirection
        {
            Clockwise,
            Anticlockwise
        }

        // FIELDS

        private Dictionary<Face, Colour[,]> _faces = new Dictionary<Face, Colour[,]>();

        // PROPERTIES

        internal Dictionary<Face, Colour[,]> Faces
        {
            get
            {
                return _faces;
            }
            private set
            {

            }
        }

        // CONSTRUCTORS

        public RubiksCube()
        {
            foreach (Face face in Enum.GetValues(typeof(Face)))
            {
                Faces.Add(face, new Colour[3, 3]);
            }

            PopulateDefaultCubeFaces();
        }

        // PUBLIC METHODS

        /// <summary>
        /// Displays each face of the cube as a 3x3 grid on the console.
        /// </summary>
        public void DisplayCube()
        {
            // Cycle through all faces of the cube, calling DisplayFace() on each one.
            foreach (Face face in Enum.GetValues(typeof(Face)))
            {
                Console.WriteLine("\n" + face + " face:");
                DisplayFace(face);
            }
        }

        /// <summary>
        /// Display the specified face as a 3x3 grid on the console
        /// </summary>
        /// <param name="face">The face to be displayed</param>
        public void DisplayFace(Face face)
        {
            string faceString = "";
            Colour[,] faceArray = Faces[face];

            for (int i = 0; i < faceArray.GetLength(0); i++)
            {
                for (int j = 0; j < faceArray.GetLength(1); j++)
                {
                    if (i == 2 && j == 2)
                    {
                        faceString += faceArray[i,j];
                    }
                    else if (j == 2)
                    {
                        faceString += faceArray[i, j] + "\n";
                    }
                    else
                    {
                        faceString += faceArray[i,j] + " | ";
                    }
                }
            }

            Console.WriteLine("\n" + faceString);
        }

        /// <summary>
        /// Rotate specified face by 90 degrees, updating both the face itself and those adjacent that are affected.
        /// </summary>
        /// <param name="face">Face to be rotated</param>
        /// <param name="clockwise">Whether or not to rotate clockwise. If false, rotate anticlockwise</param>
        public void RotateFace90Degrees(Face face, RotationDirection direction)
        {
            Dictionary<Face, Colour[,]> localFaces = CreateLocalFacesDictionary();

            UpdateAdjacentFaces(localFaces, face, direction);
            RotateFace(localFaces, face, direction);

            // Assign manipulated local arrays to their corresponding properties.
            foreach (Face cubeFace in Enum.GetValues(typeof(Face)))
            {
                AssignArrayValues(Faces[cubeFace], localFaces[cubeFace]);
            }
        }

        // PRIVATE METHODS

        /// <summary>
        /// Assign values from one array to another. Used to assign FaceArray values to a local duplicate, and vice versa.
        /// </summary>
        /// <param name="valuesFrom">Values to be assigned</param>
        /// <param name="valuesTo">Array to be assigned new values</param>
        internal void AssignArrayValues(Colour[,] oldValues, Colour[,] newValues)
        {
            for (int i = 0; i < oldValues.GetLength(0); i++)
            {
                for (int j = 0; j < oldValues.GetLength(1); j++)
                {
                    oldValues[i, j] = newValues[i, j];
                }
            }
        }

        /// <summary>
        /// Create a local copy of Faces that can be transformed in relation to the property.
        /// </summary>
        /// <returns>new Colour[6][,] localFaces populated with Faces values</returns>
        internal Dictionary<Face, Colour[,]> CreateLocalFacesDictionary()
        {
            Dictionary<Face, Colour[,]> localFaces = new Dictionary<Face, Colour[,]>();

            foreach (Face face in Enum.GetValues(typeof(Face)))
            {
                localFaces.Add(face, new Colour[3, 3]);
                AssignArrayValues(localFaces[face], Faces[face]);
            }

            return localFaces;
        }

        /// <summary>
        /// Populate RubiksCube object with default colours for each Face. Front:Green, Right:Red, Up:White, Back:Blue, Left:Orange, Down:Yellow
        /// </summary>
        internal void PopulateDefaultCubeFaces()
        {
            // Counter to auto-populate colour for associated Face
            int counter = 0;

            foreach (Face face in Enum.GetValues(typeof(Face)))
            {
                Faces.TryGetValue(face, out Colour[,] faceArray);

                for (int i = 0; i < faceArray.GetLength(0); i++)
                {
                    for (int j = 0; j < faceArray.GetLength(1); j++)
                    {
                        faceArray[i, j] = (Colour)counter;
                    }
                }
                counter++;
            }
        }

        /// <summary>
        /// Rotates the specified face 90 degrees in the specified direction. Updates that face only.
        /// </summary>
        /// <param name="localFaces">Local array to be modified</param>
        /// <param name="face">Face to be rotated</param>
        /// <param name="clockwise">Whether or not to rotate clockwise. If false, rotate anticlockwise</param>
        internal void RotateFace(Dictionary<Face, Colour[,]> localFaces, Face face, RotationDirection direction)
        {
            switch (direction)
            {
                case RotationDirection.Clockwise:
                    localFaces[face][0, 0] = Faces[face][2, 0];
                    localFaces[face][0, 1] = Faces[face][1, 0];
                    localFaces[face][0, 2] = Faces[face][0, 0];
                    localFaces[face][1, 0] = Faces[face][2, 1];
                    localFaces[face][2, 0] = Faces[face][2, 2];
                    localFaces[face][2, 1] = Faces[face][1, 2];
                    localFaces[face][2, 2] = Faces[face][0, 2];
                    localFaces[face][1, 2] = Faces[face][0, 1];
                    break;
                case RotationDirection.Anticlockwise:
                    localFaces[face][0, 0] = Faces[face][0, 2];
                    localFaces[face][0, 1] = Faces[face][1, 2];
                    localFaces[face][0, 2] = Faces[face][2, 2];
                    localFaces[face][1, 0] = Faces[face][0, 1];
                    localFaces[face][2, 0] = Faces[face][0, 0];
                    localFaces[face][2, 1] = Faces[face][1, 0];
                    localFaces[face][2, 2] = Faces[face][2, 0];
                    localFaces[face][1, 2] = Faces[face][2, 1];
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Rotates the specified face 90 degrees in the specified direction. Updates the adjacent faces only.
        /// </summary>
        /// <param name="localFaces">Local array to be modified</param>
        /// <param name="face">Face to be rotated</param>
        /// <param name="clockwise">Whether or not to rotate clockwise. If false, rotate anticlockwise</param>
        internal void UpdateAdjacentFaces(Dictionary<Face, Colour[,]> localFaces, Face face, RotationDirection direction)
        {
            switch (face)
            {
                case Face.Front:
                    switch (direction)
                    {
                        case RotationDirection.Clockwise:
                            for (int i = 0; i < 3; i++)
                            {
                                localFaces[Face.Right][i, 0] = Faces[Face.Up][2, i];
                                localFaces[Face.Up][2, i] = Faces[Face.Left][2, 2 - i];
                                localFaces[Face.Left][i, 2] = Faces[Face.Down][0, i];
                                localFaces[Face.Down][0, i] = Faces[Face.Right][0, 2 - i];
                            }
                            break;
                        case RotationDirection.Anticlockwise:
                            for (int i = 0; i < 3; i++)
                            {
                                localFaces[Face.Right][i, 0] = Faces[Face.Down][0, 2 - i];
                                localFaces[Face.Up][2, i] = Faces[Face.Right][i, 0];
                                localFaces[Face.Left][i, 2] = Faces[Face.Up][2, i];
                                localFaces[Face.Down][0, i] = Faces[Face.Left][i, 2];
                            }
                            break;
                        default:
                            throw new ArgumentException("Rotation Direction must be Clockwise or Anticlockwise");
                    }
                    break;
                case Face.Right:
                    switch (direction)
                    {
                        case RotationDirection.Clockwise:
                            for (int i = 0; i < 3; i++)
                            {
                                localFaces[Face.Front][i, 2] = Faces[Face.Down][i, 2];
                                localFaces[Face.Up][i, 2] = Faces[Face.Front][i, 2];
                                localFaces[Face.Back][i, 0] = Faces[Face.Up][2 - i, 2];
                                localFaces[Face.Down][i, 2] = Faces[Face.Back][2 - i, 0];
                            }
                            break;
                        case RotationDirection.Anticlockwise:
                            for (int i = 0; i < 3; i++)
                            {
                                localFaces[Face.Front][i, 2] = Faces[Face.Up][i, 2];
                                localFaces[Face.Up][i, 2] = Faces[Face.Back][2 - i, 0];
                                localFaces[Face.Back][i, 0] = Faces[Face.Down][2 - i, 2];
                                localFaces[Face.Down][i, 2] = Faces[Face.Front][i, 2];
                            }
                            break;
                        default:
                            throw new ArgumentException("Rotation Direction must be Clockwise or Anticlockwise");
                    }
                    break;
                case Face.Up:
                    switch (direction)
                    {
                        case RotationDirection.Clockwise:
                            for (int i = 0; i < 3; i++)
                            {
                                localFaces[Face.Front][0, i] = Faces[Face.Right][0, i];
                                localFaces[Face.Right][0, i] = Faces[Face.Back][0, i];
                                localFaces[Face.Back][0, i] = Faces[Face.Left][0, i];
                                localFaces[Face.Left][0, i] = Faces[Face.Front][0, i];
                            }
                            break;
                        case RotationDirection.Anticlockwise:
                            for (int i = 0; i < 3; i++)
                            {
                                localFaces[Face.Front][0, i] = Faces[Face.Left][0, i];
                                localFaces[Face.Right][0, i] = Faces[Face.Front][0, i];
                                localFaces[Face.Back][0, i] = Faces[Face.Right][0, i];
                                localFaces[Face.Left][0, i] = Faces[Face.Back][0, i];
                            }
                            break;
                        default:
                            throw new ArgumentException("Rotation Direction must be Clockwise or Anticlockwise");
                    }
                    break;
                case Face.Back:
                    switch (direction)
                    {
                        case RotationDirection.Clockwise:
                            for (int i = 0; i < 3; i++)
                            {
                                localFaces[Face.Up][0, i] = Faces[Face.Right][i, 2];
                                localFaces[Face.Right][i, 2] = Faces[Face.Down][2, 2 - i];
                                localFaces[Face.Left][i, 0] = Faces[Face.Up][0, 2 - i];
                                localFaces[Face.Down][2, i] = Faces[Face.Left][i, 0];
                            }
                            break;
                        case RotationDirection.Anticlockwise:
                            for (int i = 0; i < 3; i++)
                            {
                                localFaces[Face.Up][0, i] = Faces[Face.Left][2 - i, 0];
                                localFaces[Face.Right][i, 2] = Faces[Face.Up][0, i];
                                localFaces[Face.Left][i, 0] = Faces[Face.Down][2, i];
                                localFaces[Face.Down][2, i] = Faces[Face.Right][2 - i, 2];
                            }
                            break;
                        default:
                            throw new ArgumentException("Rotation Direction must be Clockwise or Anticlockwise");
                    }
                    break;
                case Face.Left:
                    switch (direction)
                    {
                        case RotationDirection.Clockwise:
                            for (int i = 0; i < 3; i++)
                            {
                                localFaces[Face.Front][i, 0] = Faces[Face.Up][i, 0];
                                localFaces[Face.Up][i, 0] = Faces[Face.Back][2 - i, 2];
                                localFaces[Face.Back][i, 2] = Faces[Face.Down][2 - i, 0];
                                localFaces[Face.Down][i, 0] = Faces[Face.Front][i, 0];
                            }
                            break;
                        case RotationDirection.Anticlockwise:
                            for (int i = 0; i < 3; i++)
                            {
                                localFaces[Face.Front][i, 0] = Faces[Face.Down][i, 0];
                                localFaces[Face.Up][i, 0] = Faces[Face.Front][i, 0];
                                localFaces[Face.Back][i, 2] = Faces[Face.Up][2 - i, 0];
                                localFaces[Face.Down][i, 0] = Faces[Face.Back][2 - i, 2];
                            }
                            break;
                        default:
                            throw new ArgumentException("Rotation Direction must be Clockwise or Anticlockwise");
                    }
                    break;
                case Face.Down:
                    switch (direction)
                    {
                        case RotationDirection.Clockwise:
                            for (int i = 0; i < 3; i++)
                            {
                                localFaces[Face.Front][2, i] = Faces[Face.Left][2, i];
                                localFaces[Face.Right][2, i] = Faces[Face.Front][2, i];
                                localFaces[Face.Back][2, i] = Faces[Face.Right][2, i];
                                localFaces[Face.Left][2, i] = Faces[Face.Back][2, i];
                            }
                            break;
                        case RotationDirection.Anticlockwise:
                            for (int i = 0; i < 3; i++)
                            {
                                localFaces[Face.Front][2, i] = Faces[Face.Right][2, i];
                                localFaces[Face.Right][2, i] = Faces[Face.Back][2, i];
                                localFaces[Face.Back][2, i] = Faces[Face.Left][2, i];
                                localFaces[Face.Left][2, i] = Faces[Face.Front][2, i];
                            }
                            break;
                        default:
                            throw new ArgumentException("Rotation Direction must be Clockwise or Anticlockwise");
                    }
                    break;
                default:
                    throw new ArgumentException("Unexpected Face argument");
            }
        }

    }
}
