using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMSincSemafors
{
    internal class ProcessList
    {

        private const int MAX_LIST_NUMBERS = 100; // Max numbers initially in list
        private const int NUM_THREADS = 5; // Number of threats to create
        private const int SEM_CAPACITY = 1; // Semaphore capacity

        private List<int> nums; // List with diferent numbers 
        private SemaphoreSlim sem;

        // Class constructor
        public ProcessList()
        {
            // Initialize list
            this.nums = new List<int>();
            
            for (var i = 1; i <= MAX_LIST_NUMBERS; i++)
            {
                nums.Add(i);
            }

            // Initialize semaphore with capacity = 1
            this.sem = new SemaphoreSlim(SEM_CAPACITY);
        }

        // Start the process
        // Create all the threats
        public void Start()
        {
            for (var i = 1; i <= NUM_THREADS; i++)
            {
                Thread th = new Thread(ProcessItem)
                {
                    Name = "Th_" + i
                };
                th.Start();
            }
        }

        // Process each Item
        // Every threat must call this function to do the work
        private void ProcessItem()
        {
            int listCount;
            int currentPosition;
            int currentItem;
            int sleepTime;
            bool mustStop; // Control if the process must process next number

            mustStop = false;

            // Loop until there is any number in the list
            while (!mustStop)
            {
                Console.WriteLine("Thread: " + Thread.CurrentThread.Name + " wants to enter critical section");
                
                // Wait for sempahore before enter critical section
                sem.Wait();

                Console.WriteLine("Thread: " + Thread.CurrentThread.Name + " is inside the critial section");

                listCount = this.nums.Count; // Number of items in list. 
                if (listCount > 0)
                {
                    Console.WriteLine("Thread: " + Thread.CurrentThread.Name + " info: " + listCount + " numbers in the list");

                    currentPosition = new Random().Next(listCount); // Get a random position in the list
                    currentItem = nums[currentPosition]; // Search and store the item 
                    nums.RemoveAt(currentPosition); // Remove the item from the list

                    Console.WriteLine("Thread: " + Thread.CurrentThread.Name + " is waitting for " + currentItem + " seconds.");

                    // Every thread must sleep diferent time
                    sleepTime = currentItem * 1000; // Convert number to milliseconds
                    Thread.Sleep(sleepTime);

                    Console.WriteLine("Thread: " + Thread.CurrentThread.Name + " has finished to process number " + currentItem);

                } else
                {
                    // If there isn't any number in the list the process must finish
                    mustStop = true;
                }

                // Release the semaphore after the critical section            
                sem.Release();
                
            }

            Console.WriteLine("Thread: " + Thread.CurrentThread.Name + " has finished.");
        }
    }
}
