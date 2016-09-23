﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace MathNet.Numerics.UnitTests.OptimizationTests.TestFunctions
{
    public class BrownAndDennisFunction : BaseTestFunction
    {
        public static IEnumerable<TestCase> TestCases
        {
            get
            {
                yield return new TestCase()
                {
                    Function = new BrownAndDennisFunction(20),
                    InitialGuess = new double[] { 25, 5, -5, -1 },
                    MinimalValue = 85822.2,
                    MinimizingPoint = null, 
                    CaseName = "unbounded"
                };
                yield return new TestCase()
                {
                    Function = new BrownAndDennisFunction(20),
                    InitialGuess = new double[] { 25, 5, -5, -1 },
                    MinimalValue = 85822.2,
                    MinimizingPoint = null,
                    LowerBound = new double[] { -1000, -1000, -1000, -1000 },
                    UpperBound = new double[] {1000, 1000, 1000, 1000 },
                    CaseName = "loose bounds"                    
                };
                yield return new TestCase()
                {
                    Function = new BrownAndDennisFunction(20),
                    InitialGuess = new double[] { 25, 5, -5, -1 },
                    MinimalValue = 0.88860479e5,
                    MinimizingPoint = null,
                    LowerBound = new double[] { -10, 0, -100, -20 },
                    UpperBound = new double[] { 100, 15, 0, 0.2 },
                    CaseName = "tight bounds"
                };
            }
        }
        
        private readonly int _items;

        public BrownAndDennisFunction(int items)
        {
            if (items < 4)
                throw new ArgumentException("items must be >= 4");
            _items = items;
        }

        public override string Description
        {
            get
            {
                return "Brown & Dennis fun (MGH #16)";
            }
        }

        public override int ItemDimension
        {
            get
            {
                return _items;
            }
        }

        public override int ParameterDimension
        {
            get
            {
                return 4;
            }
        }

        public override void ItemGradientByRef(Vector<double> x, int itemIndex, Vector<double> output)
        {
            var ii = itemIndex + 1;
            var t = ii / 5.0;
            output[0] = 2 * (x[0] + t * x[1] - Math.Exp(t));
            output[1] = (2*ii/25.0) * (5 * x[0] + ii * x[1] - 5 * Math.Exp(t));
            output[2] = 2 * (x[2] + x[3] * Math.Sin(t) - Math.Cos(t));
            output[3] = 2 * Math.Sin(t) * (x[2] + Math.Sin(t) * x[3] - Math.Cos(t));
        }

        public override void ItemHessianByRef(Vector<double> x, int itemIndex, Matrix<double> output)
        {
            for (int ii = 0; ii < 4; ++ii)
                for (int jj = 0; jj < 4; ++jj)
                    output[ii, jj] = 0;
            var i = itemIndex + 1;
            var t = i / 5.0;
            output[0, 0] = 2;
            output[0, 1] = 2 * t;
            output[1, 0] = 2 * t;
            output[1, 1] = 2 * t * t;
            output[2, 2] = 2;
            output[2, 3] = 2 * Math.Sin(t);
            output[3, 2] = 2 * Math.Sin(t);
            output[3, 3] = 2 * Math.Pow(Math.Sin(t), 2);
        }

        public override double ItemValue(Vector<double> x, int itemIndex)
        {
            var ii = itemIndex + 1;
            var t = ii / 5.0;
            return Math.Pow(x[0] + t * x[1] - Math.Exp(t), 2.0) + Math.Pow(x[2] + x[3] * Math.Sin(t) - Math.Cos(t), 2);
        }
    }
}
