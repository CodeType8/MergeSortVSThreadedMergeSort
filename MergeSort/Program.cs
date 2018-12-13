/*
Yongmin Joh 011535529
Assignment 10 Threaded Merge Sort
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MergeSort
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> num = new List<int>();
            List<int> temp = new List<int>();
            List<int> size = new List<int>(new int[] { 8, 64, 256, 1024 });
            Random ran = new Random();

            //double start = 0, end = 0;
            long start = 0;
            long end = 0;
            
            //display introduction
            Console.WriteLine("Starting tests of merge sort vs. threaded merge sort");
            Console.WriteLine("  Array sizes under test: [8, 64, 256, 1024]");
            Console.WriteLine("");

            foreach (int n in size)
            {
                //double start = 0, end = 0;

                Console.WriteLine(" Starting test for size " + n.ToString() + " - Test completed:");

                for (int i = 0; i < n; i++)
                {
                    num.Add(ran.Next(0, Int32.MaxValue));
                }

                //normal sort time check start
                temp = new List<int>(num);
                start = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                normalSort(temp);

                //check the time difference for normal sort
                end = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                //display
                Console.WriteLine("  Normal Sort Time (ms):        " + (end - start).ToString());


                //thread sort time check start
                temp = new List<int>(num);
                start = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                threadedSort(temp);

                //check the time difference for threaded sort
                end = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                //display
                Console.WriteLine("  Threaded Sort Time (ms):      " + (end - start).ToString());

                Console.WriteLine("");
            }
        }

        public static void normalSort(List<int> list)
        {
            if (list.Count > 1)
            {
                //cut the list half
                List<int> left = list.GetRange(0, list.Count / 2);
                List<int> right = list.GetRange(list.Count / 2, list.Count - left.Count);

                //declare
                int i = 0;
                int j = 0;

                //run
                normalSort(left);
                normalSort(right);
                
                //sorting
                while (i < left.Count && j < right.Count)
                {
                    if (left[i] <= right[j])
                    {
                        list.Add(left[i]);
                        i++;
                    }
                    else
                    {
                        list.Add(right[j]);
                        j++;
                    }
                }

                while (i < left.Count)
                {
                    list.Add(left[i]);
                    i++;
                }

                while (j < right.Count)
                {
                    list.Add(right[j]);
                    j++;
                }
            }
        }

        public static void threadedSort(List<int> list)
        {
            if (list.Count > 1)
            {
                //cut the list half
                List<int> leftList = list.GetRange(0, list.Count / 2);
                List<int> rightList = list.GetRange(list.Count / 2, list.Count - leftList.Count);

                //get thread
                Thread leftThread = new Thread(() => threadedSort(leftList));
                Thread rightThread = new Thread(() => threadedSort(rightList));

                //declare
                int i = 0;
                int j = 0;

                //run
                leftThread.Start();
                rightThread.Start();

                //getready to thread
                leftThread.Join();
                rightThread.Join();

                /*
                leftThread.Start();
                rightThread.Start();
                */

                //sorting
                while (i < leftList.Count && j < rightList.Count)
                {
                    if (leftList[i] <= rightList[j])
                    {
                        list.Add(leftList[i]);
                        i++;
                    }
                    else
                    {
                        list.Add(rightList[j]);
                        j++;
                    }
                }

                while (i < leftList.Count) //add in the rest of the elements of the non-exhausted list if there are any
                {
                    list.Add(leftList[i]);
                    i++;
                }

                while (j < rightList.Count)
                {
                    list.Add(rightList[j]);
                    j++;
                }
            }
        }
        
    }
}
