using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ComplexNumbers
{
    public class Complex
    {
        //public class DivideByNull : Exception { };

        public double x
        {
            get;
            private set;
        }
        public double y
        { 
            get;
            private set; 
        }

        public Complex(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"{x} + {y}e";
        }

        public static Complex operator +(Complex left, Complex right)
        {
                return new Complex(left.x + right.x, left.y + right.y);
        }

        public static Complex operator -(Complex left, Complex right)
        {
            return new Complex(left.x - right.x, left.y - right.y);
        }

        public static Complex operator *(Complex left, Complex right)
        {
            return new Complex((left.x * right.x) - (left.y * right.y),(left.x * right.y) + (right.x * left.y));
        }

        public static Complex operator /(Complex left, Complex right)
        {
            double div = Math.Pow(right.x, 2) + Math.Pow(right.y, 2);
            //Console.WriteLine(div);
            if (div == 0)
            {
                throw new DivideByZeroException();
            }
            double re = (left.x * right.x) + -(left.y * (-right.y));
            double im = (left.x * -right.y) + (right.x * left.y);
            return new Complex(re/div, im/div);
        }
    }
}
