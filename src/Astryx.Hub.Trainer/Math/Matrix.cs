using System;
using Astryx.Abstractions.Exceptions;
using Astryx.Abstractions.Math;

namespace Astryx.Hub.Trainer.Math
{
    /// <summary>
    /// Public implementation of IMatrix for Trainer and Substrate use.
    /// Mirrors the behavior expected by Astryx runtime.
    /// </summary>
    public sealed class Matrix : IMatrix
    {
        public float[,] Values { get; }

        public int Rows => Values.GetLength(0);
        public int Cols => Values.GetLength(1);

        public Matrix(float[,] values)
        {
            if (values == null)
            {
                throw SubstrateException.Create(
                    "Matrix values cannot be null.",
                    SubstrateErrorLevel.Fatal,
                    SubstrateErrorCode.InvalidVector,
                    context: "Matrix.ctor");
            }

            if (values.GetLength(0) == 0 || values.GetLength(1) == 0)
            {
                throw SubstrateException.Create(
                    "Matrix must have non-zero dimensions.",
                    SubstrateErrorLevel.Fatal,
                    SubstrateErrorCode.InvalidVector,
                    context: "Matrix.ctor");
            }

            Values = values;
        }

        public IMatrix Add(IMatrix other)
        {
            if (other == null)
            {
                throw SubstrateException.Create(
                    "Cannot add null matrix.",
                    SubstrateErrorLevel.Critical,
                    SubstrateErrorCode.InvalidVector,
                    context: "Matrix.Add");
            }

            if (other.Rows != Rows || other.Cols != Cols)
            {
                throw SubstrateException.Create(
                    "Matrix dimensions do not match for addition.",
                    SubstrateErrorLevel.Critical,
                    SubstrateErrorCode.InvalidVector,
                    context: "Matrix.Add");
            }

            var result = new float[Rows, Cols];

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    result[r, c] = Values[r, c] + other.Values[r, c];
                }
            }

            return new Matrix(result);
        }
    }
}
