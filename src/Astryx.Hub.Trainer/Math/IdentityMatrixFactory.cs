using System;
using Astryx.Abstractions.Exceptions;
using Astryx.Abstractions.Math;
using Astryx.Substrate.AstryxMath;

namespace Astryx.Hub.Trainer.Math
{
    public static class IdentityMatrixFactory
    {
        /// <summary>
        /// Creates a deterministic identity matrix with optional seed variance.
        /// Matches TestHarness behavior exactly.
        /// </summary>
        public static IMatrix CreateIdentity(float seed = 1f)
        {
            try
            {
                // Base identity matrix
                var matrix = new Matrix(new float[,]
                {
                    { 1f, 0f },
                    { 0f, 1f }
                });

                if (matrix == null || matrix.Values == null)
                {
                    throw SubstrateException.Create(
                        "Failed to construct base identity matrix.",
                        SubstrateErrorLevel.Fatal,
                        SubstrateErrorCode.InvalidVector,
                        context: "IdentityMatrixFactory.CreateIdentity");
                }

                // No seed variance
                if (seed == 1f)
                    return matrix;

                // Deterministic variance
                float delta = (seed - 1f) * 0.01f;

                matrix.Values[0, 0] += delta;
                matrix.Values[1, 1] -= delta;

                return matrix;
            }
            catch (SubstrateException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw SubstrateException.Create(
                    "Failed to create identity matrix.",
                    SubstrateErrorLevel.Fatal,
                    SubstrateErrorCode.InvalidVector,
                    context: "IdentityMatrixFactory.CreateIdentity",
                    inner: ex);
            }
        }
    }
}