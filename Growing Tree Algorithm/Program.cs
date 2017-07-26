using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace Growing_Tree_Algorithm
{
    class Program
    {
        private class Tile
        {
            public int x, y;
            public bool visited = false;
            public int type = 0;

        }


        static void Main(string[] args)
        {


            Console.Write("Enter width:");
            int width = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter height:");
            int height = Convert.ToInt32(Console.ReadLine());

            //Checks if dimensions are odd
            if (width % 2 == 0)
            {
                Console.WriteLine("Width must not be an even number.Reduced width by 1.");
                width -= 1;
            }

            if (height % 2 == 0)
            {
                Console.WriteLine("Height must not be an even number.Reduced height by 1.");
                height -= 1;
            }

            //Checks if dimensions are too small
            if (width < 3)
            {
                Console.WriteLine("Minimum width is 3.Set width to 3.");
                width = 3;
            }

            if (height < 3)
            {
                Console.WriteLine("Minimum height is 3.Set height to 3.");
                height = 3;
            }

            //Checks if dimensions are too big
            if (width > 5999)
            {
                Console.WriteLine("Maximum width is 5999.Set width to 5999.");
                width = 5999;
            }

            if (height > 5999)
            {
                Console.WriteLine("Maximum height is 5999.Set height to 5999.");
                height = 5999;
            }

            //Used to count seconds elapsed in the end.
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //Holds the map
            Tile[,] map = new Tile[width, height];
            map.Initialize();

            //Holds the available cells we can expand from
            List<Tile> availableCells = new List<Tile>();

            //Initialize map
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = new Tile();
                    map[x, y].x = x;
                    map[x, y].y = y;
                }
            }

            //Places starting cell tiles.In the beginning the map looks like this(W=wall,C=cell)
            //W W W W W W W
            //W C W C W C W
            //W W W W W W W
            //W C W C W C W
            //W W W W W W W
            //W C W C W C W
            //W W W W W W W
            for (int x = 1; x < width - 1; x += 2)
            {
                for (int y = 1; y < height - 1; y += 2)
                {
                    map[x, y].type = 1;
                }
            }

            //Pick a random tile
            Random rng = new Random();
            int xrand, yrand;
            //Keep picking until you find one that is actually a cell and not a wall
            //We could always start from a certain point, e.g. map[0,0] but I chose not to do so 
            do
            {
                xrand = rng.Next(1, width - 1);
                yrand = rng.Next(1, height - 1);
            } while (xrand % 2 != 1 || yrand % 2 != 1);

            //We visited this cell
            map[xrand, yrand].visited = true;

            //Add randomly picked cell to list
            availableCells.Add(map[xrand, yrand]);

            //Run the loop until you cannot expand anymore
            while (availableCells.Count > 0)
            {
                //Size of the list
                int lsize = availableCells.Count - 1;

                int curr_x = availableCells[lsize].x;
                int curr_y = availableCells[lsize].y;

                
                //Holds the current cells' valid neighbors
                List<Tile> neighborCells = new List<Tile>();
                //neighborCells.Clear();

                //Check potential neighbors,if they are not visited,add them to the list
                if (curr_x + 2 < width - 1 && map[curr_x + 2, curr_y].visited == false)
                {
                    neighborCells.Add(map[curr_x + 2, curr_y]);
                }
                if (curr_x - 2 >= 1 && map[curr_x - 2, curr_y].visited == false)
                {
                    neighborCells.Add(map[curr_x - 2, curr_y]);
                }
                if (curr_y + 2 < height - 1 && map[curr_x, curr_y + 2].visited == false)
                {
                    neighborCells.Add(map[curr_x, curr_y + 2]);
                }
                if (curr_y - 2 >= 1 && map[curr_x, curr_y - 2].visited == false)
                {
                    neighborCells.Add(map[curr_x, curr_y - 2]);
                }

                //If there are no available neighboring cells,remove the current cell from the list
                if (neighborCells.Count == 0)
                {
                    availableCells.RemoveAt(lsize);
                }
                else
                {
                    //Pick a neighbor,break wall between current cell and neighbor
                    Tile selected = neighborCells[rng.Next(0, neighborCells.Count)];
                    if (selected.x == curr_x)
                    {
                        if (curr_y > selected.y)
                        {
                            map[curr_x, curr_y - 1].type = 1;
                        }
                        else
                        {
                            map[curr_x, curr_y + 1].type = 1;
                        }

                    }
                    else
                    {
                        if (curr_x > selected.x)
                        {
                            map[curr_x - 1, curr_y].type = 1;
                        }
                        else
                        {
                            map[curr_x + 1, curr_y].type = 1;
                        }
                    }
                    //Make neighbor visited
                    selected.visited = true;
                    //Add neighbor to the list
                    availableCells.Add(selected);
                }
            }
            
            //Create a bitmap
            Bitmap bmp = new Bitmap(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int val = map[x, y].type * 255;
                    bmp.SetPixel(x, y, Color.FromArgb(255, val, val, val));
                }
            }

            //Save image and open the .png file
            string dir = Directory.GetCurrentDirectory() + "\\maze.png";
            bmp.Save(dir);
            Process.Start(dir);

            //Stop the clock,print total seconds,wait for [Enter] key to close
            sw.Stop();
            Console.WriteLine("Total seconds:" + sw.ElapsedMilliseconds / 1000.0);
            Console.WriteLine("Press [Enter] to close...");
            Console.ReadLine();
        }
    }
}

