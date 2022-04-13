using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOLStartUpTemplate2
{
    public partial class Form1 : Form
    {
        // The universe array
        bool[,] universe = new bool[10, 10];
        //scratchpad
        bool[,] scratchPad = new bool[10, 10];
        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        public Form1()
        {
            InitializeComponent();

            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {

            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {

                    int count = CountNeighborsFinite(x, y);
                    // CountNeighborsFinite(x, y);   CountNeighborsToroidal(x, y);
                    // Apply the rules


                    //if (universe[x, y] == true)
                    //{
                    //    if (count >= 2 && count <= 3)
                    //    {
                    //        universe[x, y] = true;
                    //        scratchPad[x, y] = true;
                    //        ++count;
                    //    }
                    //}
                    //else if (count == 3)
                    //{
                    //    scratchPad[x, y] = true;
                    //    universe[x, y] = true;
                    //    ++count;
                    //}


                    //if (universe[x, y] == true && count <= 2 || universe[x, y] == true && count == 3)
                    //{
                    //    universe[x, y] = true;
                    //    scratchPad[x, y] = true;

                    //}
                    if (universe[x, y] == true)
                    {
                        //Any living cell in the current universe with less than 2 living neighbors dies in the next generation as if by under-population.
                        //If a cell meets this criteria in the universe array then make the same cell dead in the scratch pad array.
                        if (count < 2)
                        {
                            //universe[x, y] = false;
                            scratchPad[x, y] = false;
                        }
                        //Any living cell with more than 3 living neighbors will die in the next generation as if by over-population.
                        //If so in the universe then kill it in the scratch pad.  ....
                        if (count > 3)
                        {
                            //universe[x, y] = false;
                            scratchPad[x, y] = false;
                        }
                        //Any living cell with 2 or 3 living neighbors will live on into the next generation.
                        //If this is the case in the universe then the same cell lives in the scratch pad.
                        if (count == 2 || count == 3)
                        {
                            //universe[x, y] = true;
                            scratchPad[x, y] = true;
                        }

                    }
                    //Any dead cell with exactly 3 living neighbors will be born into the next generation as if by reproduction.
                    //If so in the universe then make that cell alive in the scratch pad.
                    if (universe[x, y] == false)
                    {

                        if (count == 3)
                        {
                            scratchPad[x, y] = true;
                            //universe[x, y] = true;
                        }
                    }




                    //turn in on/off the scratchPad - second 2d array
                }
            }

            //Copy from scratchPad to universe
            //make sure to clear scratchPad
            bool[,] temp = universe;
            universe = scratchPad;
            scratchPad = temp;

            // Increment generation count
            generations++;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();

            //Invalidate
            graphicsPanel1.Invalidate();
        }

        //check this again
        // count neighbors toroidal
        private int CountNeighborsToroidal(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);


            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then set to xLen - 1
                    if (xCheck < 0)
                    {
                        xLen = -1;

                    }
                    // if yCheck is less than 0 then set to yLen - 1
                    if (yCheck < 0)
                    {
                        yLen = -1;


                    }
                    // if xCheck is greater than or equal too xLen then set to 0
                    if (xCheck >= xLen)
                    {
                        xCheck = 0;


                    }
                    // if yCheck is greater than or equal too yLen then set to 0
                    if (yCheck >= yLen)
                    {
                        yCheck = 0;


                    }

                    if (universe[xCheck, yCheck] == true)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        //check this again 
        // count neighbors finite
        private int CountNeighborsFinite(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);


            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;

                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    if (xCheck < 0)
                    {
                        continue;
                    }
                    if (yCheck < 0)
                    {
                        continue;
                    }
                    if (xCheck >= xLen)
                    {
                        continue;
                    }
                    if (yCheck >= yLen)
                    {
                        continue;
                    }
                    if (universe[xCheck, yCheck] == true)
                    {
                        count++;
                    }
                }
            }
            return count;

        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
            graphicsPanel1.Invalidate();
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            //use floats to fix scaling 
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    // RectangleF
                    Rectangle cellRect = Rectangle.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                //floats

                // Calculate the width and height of each cell in pixels
                int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = e.Y / cellHeight;

                // Toggle the cell's state
                universe[x, y] = !universe[x, y];

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        // File Menu New
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            generations = 0;

            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
            }
            //try putting the reset generation count here
            graphicsPanel1.Invalidate();
        }
        //play button
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true; // start timer running
            graphicsPanel1.Invalidate();
        }
        // pause button
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            graphicsPanel1.Invalidate();
        }


        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            NextGeneration();
            graphicsPanel1.Invalidate();
        }

        private void finiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CountNeighborsFinite(10, 10);
            graphicsPanel1.Invalidate();
        }

        private void toroidalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CountNeighborsToroidal(10, 10);
            graphicsPanel1.Invalidate();
        }

        private void gameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {

        }
    }
}
