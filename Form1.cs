﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOLStartUpTemplate2
{
    public partial class Form1 : Form
    {
        //INITIALIZATIONS
        #region INITIALIZATIONS
        // The universe array
        bool[,] universe = new bool[19, 19];
        //scratchpad
        bool[,] scratchPad = new bool[19, 19];


        // Drawing colors
        Color gridColor = Color.DarkCyan;
        Color cellColor = Color.DeepPink;
        // The Timer class
        Timer timer = new Timer();
        // Generation count
        int generations = 0;
        //int count;
        int alive = 0;
        int gameSpeed = 50;
        int count;
        bool isHUDvisible = false;
        bool isFinite = true;
        #endregion
        //NEXTGEN AND GAME SIZE
        #region NEXTGEN AND GAME SIZE
        public Form1()
        {
            InitializeComponent();
            // Setup the timer
            timer.Interval = gameSpeed; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
            //panel back color default
            //reding porperty
            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;
            gridColor = Properties.Settings.Default.GridColor;
            cellColor = Properties.Settings.Default.CellColor;
            //graphicsPanel1.
        }
        // Calculate the next generation of cells
        private void NextGeneration()
        {

            for (float y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (float x = 0; x < universe.GetLength(0); x++)
                {
                    scratchPad[(int)x, (int)y] = false;
                    if (isFinite == true)
                    {
                         count = CountNeighborsFinite((int)x, (int)y);
                    }
                    else
                    {
                         count = CountNeighborsToroidal((int)x, (int)y);
                    }
                    
                    // Apply the rules
                    if (universe[(int)x, (int)y] == true)
                    {
                        //Any living cell in the current universe with less than 2 living neighbors dies in the next generation as if by under-population.
                        //If a cell meets this criteria in the universe array then make the same cell dead in the scratch pad array.
                        if (count < 2)
                        {
                            scratchPad[(int)x, (int)y] = false;
                        }
                        //Any living cell with more than 3 living neighbors will die in the next generation as if by over-population.
                        //If so in the universe then kill it in the scratch pad.  ....
                        if (count > 3)
                        {
                            scratchPad[(int)x, (int)y] = false;
                        }
                        //Any living cell with 2 or 3 living neighbors will live on into the next generation.
                        //If this is the case in the universe then the same cell lives in the scratch pad.
                        if (count == 2 || count == 3)
                        {
                            scratchPad[(int)x, (int)y] = true;
                        }
                    }
                    //Any dead cell with exactly 3 living neighbors will be born into the next generation as if by reproduction.
                    //If so in the universe then make that cell alive in the scratch pad.
                    if (universe[(int)x, (int)y] == false)
                    {
                        if (count == 3)
                        {
                            scratchPad[(int)x, (int)y] = true;
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
            //THIS WOULD BE MY ALIVE COUNT... IF I HAD ONE!!!!!
            toolStripStatusLabelAlive.Text = "Alive = " + alive.ToString();

            //Invalidate
            graphicsPanel1.Invalidate();
        }
        #endregion

        //NEIGHBORS
        #region NEIGHBORS
        // count neighbors toroidal... WONKY
        private int CountNeighborsToroidal(int x, int y)
        {
            int count = 0;
            //float xLen = universe.GetLength(0);
            //float yLen = universe.GetLength(1);
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
                    if (universe[xCheck, yCheck] == true) count++;

                }
            }
            return count;
        }
        // count neighbors finite
        private int CountNeighborsFinite(int x, int y)
        {
            int count = 0;
            float xLen = universe.GetLength(0);
            float yLen = universe.GetLength(1);
            for (float yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (float xOffset = -1; xOffset <= 1; xOffset++)
                {
                    float xCheck = x + xOffset;
                    float yCheck = y + yOffset;
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
                    if (universe[(int)xCheck, (int)yCheck] == true)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        #region BUTTONS
        private void finiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isFinite = true;
        }
        private void toroidalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isFinite= false;
        }
        #endregion

        #endregion

        //GRAPHICS PANEL
        #region GRAPHICS PANEL
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
            float cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);
            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);
            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);
            // Iterate through the universe in the y, top to bottom
            for (float y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (float x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    // RectangleF
                    RectangleF cellRect = RectangleF.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;
                    // Fill the cell with a brush if alive
                    if (universe[(int)x, (int)y] == true)
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
                float cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                float cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);
                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                float x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                float y = e.Y / cellHeight;
                // Toggle the cell's state
                universe[(int)x, (int)y] = !universe[(int)x, (int)y];
                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }
        #endregion

        //NEW GAME
        #region NEW GAME
        private void NewGame()
        {
            timer.Dispose();
            generations = 0;
            for (float y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (float x = 0; x < universe.GetLength(0); x++)
                {
                    universe[(int)x, (int)y] = false;
                }
            graphicsPanel1.Invalidate();
            }
        }
        #region BUTTONS
        // File Menu New
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }
        #endregion
        #endregion

        //CONTROLS
        #region CONTROLS
        private void Play()
        {
            timer.Enabled = true; // start timer running
            graphicsPanel1.Invalidate();
        }
        private void Pause()
        {
            
            timer.Enabled = false; // stop timer running
            graphicsPanel1.Invalidate();
        }
        private void Next()
        {
            NextGeneration();
            graphicsPanel1.Invalidate();
        }
        #region BUTTONS
        //play button
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Play();
        }
        // pause button
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Pause();
        }
        //Next Button
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Next();
        }

        //Play Menu
        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Play();
        }
        //Pause Menu
        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pause();
        }
        //Next Menu
        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Next();
        }
        #endregion
        #endregion

        //SAVING
        #region SAVING
        //save on close
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //update property
            Properties.Settings.Default.PanelColor = graphicsPanel1.BackColor;
            Properties.Settings.Default.GridColor = gridColor;
            Properties.Settings.Default.CellColor = cellColor;
            Properties.Settings.Default.Save();
        }
        #region BUTTONS
        // File menu close
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //save menu
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        //save as
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            
        }
        #endregion
        #endregion

        //Color
        #region COLOR METHODS
        //color background
        private void ColorBackground()
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = Color.Black;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                graphicsPanel1.BackColor = dlg.Color;
            }
        }
        private void ColorGrid()
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = Color.DarkCyan;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                gridColor = dlg.Color;
                graphicsPanel1.Invalidate();
            }
        }
        private void ColorCell()
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = Color.DarkCyan;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                cellColor = dlg.Color;
                graphicsPanel1.Invalidate();
            }
        }
        #region BUTTONS
        private void backgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ColorBackground();
        }
        //Color gridColor 
        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorGrid();
        }
        //Color Cell

        private void cellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorCell();
        }
        //right clicky colors
        private void backgroundToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorBackground();
        }
        //right clicky colors
        private void gridToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorGrid();
        }
        //right clicky colors
        private void cellToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorCell();
        }
        #endregion
        #endregion

        //Randomize
        #region RANDOMIZE
        private void Randomize()
        {
            generations = 0;
            Random rand = new Random();
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {

                    if (rand.Next(0, 4) == 0)
                    {
                        universe[x, y] = true;

                    }
                }
            }
            graphicsPanel1.Invalidate();
        }
        private void RandomizeSeed(int seed)
        {
            generations = 0;
            Random rand = new Random(seed);
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {

                    if (rand.Next(0, 4) == 0)
                    {
                        universe[x, y] = true;

                    }
                }
            }
            graphicsPanel1.Invalidate();
        }
        private void SetRandomSeed()
        {
            generations = 0;

            int seed = 0;
            ModalDialog dlg = new ModalDialog();
            dlg.Seed = seed;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                seed = dlg.Seed;
                RandomizeSeed(seed);
                graphicsPanel1.Invalidate();
            }
        }
        #region BUTTONS
        // random form tool strip
        private void randomFromTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Randomize();
        }
        //right clicky random seed
        private void randomFromSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetRandomSeed();
        }
        #endregion
        #endregion

        //Game Speed
        #region GAME SPEED
        //set gamespeed
        private void SetSpeed()
        {
            int gameSpeed = 50;
            ModalDialog dlg = new ModalDialog();
            dlg.GameSpeed = gameSpeed;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                gameSpeed = dlg.GameSpeed;
                timer.Interval = gameSpeed;
                graphicsPanel1.Invalidate();
            }
        }
        #region BUTTONS
        private void speedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSpeed();
        }
        private void speedToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SetSpeed();
        }
        #endregion
        // end?
        #endregion


    }
}

