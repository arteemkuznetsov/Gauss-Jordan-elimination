using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GaussJordan
{
    public class GaussJordanSolver // класс, находящий решения СЛАУ, предварительно созданную через класс LinearSystemView
    {
        public static double[] resultVector;
        public GaussJordanSolver(int n, double[,] array) // Order of Matrix(n) 
        {

            double[,] a = new double[n, n + 1];

            Debug.WriteLine("\nIn function:");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n + 1; j++)
                {
                    a[i, j] = array[i, j];
                   // Debug.Write(a[i, j] + " ");
                }
                //Debug.WriteLine("");
            }

            int flag = 0;

            // Performing Matrix transformation 
            flag = GaussJordanSolver.PerformOperation(a, n);

            if (flag == 1)
                flag = GaussJordanSolver.CheckConsistency(a, n, flag);

            // Printing Final Matrix 
            Debug.WriteLine("Final Augumented Matrix is : ");
            GaussJordanSolver.PrintMatrix(a, n);
            Debug.WriteLine("\n");

            // Printing Solutions(if exist) 
            GaussJordanSolver.PrintResult(a, n, flag); // убрать статические вызовы и сделать вместо матрицы float свою матрицу

        }



        // Function to print the matrix 
        public static void PrintMatrix(double[,] a, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j <= n; j++)
                    Debug.Write(a[i, j] + " ");
                Debug.Write("\n");
            }
        }

        // function to reduce matrix to reduced 
        // row echelon form. 
        public static int PerformOperation(double[,] a, int n)
        {
            int i, j, k = 0, c, flag = 0;

            // Performing elementary operations 
            for (i = 0; i < n; i++)
            {
                if (a[i, i] == 0)
                {
                    c = 1;
                    while (a[i + c, i] == 0 && (i + c) < n)
                        c++;
                    if ((i + c) == n)
                    {
                        flag = 1;
                        break;
                    }
                    for (j = i, k = 0; k <= n; k++)
                    {
                        double temp = a[j, k];
                        a[j, k] = a[j + c, k];
                        a[j + c, k] = temp;
                    }
                }

                for (j = 0; j < n; j++)
                {

                    // Excluding all i == j 
                    if (i != j)
                    {

                        // Converting Matrix to reduced row 
                        // echelon form(diagonal matrix) 
                        double p = a[j, i] / a[i, i];

                        for (k = 0; k <= n; k++)
                            a[j, k] = a[j, k] - (a[i, k]) * p;
                    }
                }
            }
            return flag;
        }

        // Function to print the desired result  
        // if unique solutions exists, otherwise  
        // prints no solution or infinite solutions  
        // depending upon the input given. 
        public static void PrintResult(double[,] a,
                                int n, int flag)
        {
            Debug.Write("Result is : ");

            if (flag == 2)
                Debug.WriteLine("Infinite Solutions Exists");
            else if (flag == 3)
                Debug.WriteLine("No Solution Exists");

            // Printing the solution by dividing  
            // constants by their respective 
            // diagonal elements 
            else
            {
                resultVector = new double[n];
                for (int i = 0; i < n; i++)
                {
                    double root = a[i, n] / a[i, i];
                    resultVector[i] = root;
                    Debug.Write(resultVector[i] + " ");
                }
            }
            Debug.WriteLine("\n\n");
        }

        // To check whether infinite solutions  
        // exists or no solution exists 
        public static int CheckConsistency(double[,] a,
                                    int n, int flag)
        {
            int i, j;
            double sum;

            // flag == 2 for infinite solution 
            // flag == 3 for No solution 
            flag = 3;
            for (i = 0; i < n; i++)
            {
                sum = 0;
                for (j = 0; j < n; j++)
                    sum = sum + a[i, j];
                if (sum == a[i, j])
                    flag = 2;
            }
            return flag;
        }
    }
}
