/*Angela Xu and Daniel Kim
 * May 15 2014
 * SimFarm allows the user to build products, harvest them and earn money*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimFarm
{
    public partial class AngelaForm : Form
    {
        //stores whether game is over
        bool gameOver;
        //stores whether product is placed
        bool isPlaced = true;
        //stores whether the square is empty to place product
        bool placeProduct = false;
        //creates new farm model
        SimFarmModel model = new SimFarmModel(NUM_ROWS, NUM_COLS);

        //stores width and height of form
        const int FORM_WIDTH = 1015;
        const int FORM_HEIGHT = 350;

        //stores the offset for the x and y coordinates
        const int XOFFSET = 36;
        const int YOFFSET = 100;

        //stores width and height of columns
        int columnWidth;
        int rowHieght;

        //stores the grid
        Rectangle[,] _grid;

        //stores the number of rows and columns
        const int NUM_COLS = 20;
        const int NUM_ROWS = 5;

        //store the index of the image that is clicked
        int[] imageClickedIndex = new int[NUM_ROWS * NUM_COLS];

        //stores the number of images
        const int NUMBER_OF_IMAGES = 11;
        //creates a new image array
        Image[] image = new Image[NUMBER_OF_IMAGES];
        //stores all the prodcts
        Product[] product = new Product[NUMBER_OF_IMAGES];
        //stores whether the image is clicked
        bool[] imageClickState = new bool[NUMBER_OF_IMAGES];
        //stores location and size of image
        Rectangle[] imageBoundingBox = new Rectangle[NUMBER_OF_IMAGES];
        //stores location of image
        Point[] imageXY = new Point[NUMBER_OF_IMAGES];
        //stores location and size of image in grid
        Rectangle[,] imageBounding = new Rectangle[NUM_ROWS, NUM_COLS];
        //stores location of image in grid
        Point[,] XY = new Point[NUM_ROWS, NUM_COLS];
        //stores size of image
        Size imageSize;
        //store the (x,y) location of the mouse when it is being dragged
        Point imageDraggedCoordinate;
        //store the (x,y) location of the mouse when it is being dropped
        Point imageDroppedCoordinate;
        //stores the index of the product being placed
        int index;
        //the grid's row where the user wants to place a new product
        int rowProductPlaced;
        //the grid's column where the user wants to place a product
        int colProductPlaced;
        //stores the index of each image
        const int IMAGE_INDEX_CORN = 0;
        const int IMAGE_INDEX_TOMATO = 1;
        const int IMAGE_INDEX_ASPARAGUS = 2;
        const int IMAGE_INDEX_BROCCOLI = 3;
        const int IMAGE_INDEX_POTATO = 4;
        const int IMAGE_INDEX_SWEET_POTATO = 5;
        const int IMAGE_INDEX_CHICKEN = 6;
        const int IMAGE_INDEX_BEEF = 7;
        const int IMAGE_INDEX_PORK = 8;
        const int IMAGE_INDEX_HEN = 9;
        const int IMAGE_INDEX_COW = 10;

        //stores size of each image
        const int STANDARD_IMAGE_WIDTH = 48;
        const int STANDARD_IMAGE_HEIGHT = 48;

        /// <summary>
        /// AngelaFormConstructor
        /// </summary>
        public AngelaForm()
        {
            //calls subprograms to set up the game
            InitializeComponent();
            InitializeGameBoard();
            InitializeImage();
        }

        //puts all the images in an array and gives it a size and location
        public void InitializeImage()
        {
            //images are stored in the image array
            image[IMAGE_INDEX_ASPARAGUS] = Properties.Resources.asparagus;
            image[IMAGE_INDEX_CORN] = Properties.Resources.corn;
            image[IMAGE_INDEX_TOMATO] = Properties.Resources.tomato;
            image[IMAGE_INDEX_BROCCOLI] = Properties.Resources.broccoli;
            image[IMAGE_INDEX_POTATO] = Properties.Resources.potato;
            image[IMAGE_INDEX_SWEET_POTATO] = Properties.Resources.sweet_potato;
            image[IMAGE_INDEX_CHICKEN] = Properties.Resources.chicken;
            image[IMAGE_INDEX_BEEF] = Properties.Resources.beef;
            image[IMAGE_INDEX_PORK] = Properties.Resources.pork;
            image[IMAGE_INDEX_HEN] = Properties.Resources.hen;
            image[IMAGE_INDEX_COW] = Properties.Resources.cow;
            //imageSize stores the size of each image
            imageSize = new Size(STANDARD_IMAGE_WIDTH, STANDARD_IMAGE_HEIGHT);

            //loops through the images array to give a size and location and whether image is clicked
            for (int i = 0; i < image.Length; i++)
            {
                imageXY[i] = new Point(FORM_WIDTH - 50, i * STANDARD_IMAGE_WIDTH);
                imageBoundingBox[i] = new Rectangle(imageXY[i], imageSize);
                imageClickState[i] = false;
            }
        }

        //draws the grid to create the game board
        public void InitializeGameBoard()
        {
            //gets the column width by taking the form size minius the offset and then dividing that by the number of columns and rows
            columnWidth = (FORM_WIDTH - XOFFSET) / NUM_COLS;
            rowHieght = (FORM_HEIGHT - YOFFSET) / NUM_ROWS;

            //stores the grid
            _grid = new Rectangle[NUM_ROWS, NUM_COLS];

            //loops through the grid to create each square
            for (int rows = 0; rows < NUM_ROWS; rows++)
            {
                for (int cols = 0; cols < NUM_COLS; cols++)
                {
                    _grid[rows, cols] = new Rectangle(cols * columnWidth, rows * rowHieght, columnWidth, rowHieght);
                }
            }
        }

        //Draws the objects on the form
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //loops through the grid to draw them on the form
            for (int rows = 0; rows < NUM_ROWS; rows++)
            {
                for (int cols = 0; cols < NUM_COLS; cols++)
                {
                    //draws the rectangles
                    e.Graphics.DrawRectangle(Pens.DarkSlateBlue, _grid[rows, cols]);
                }
            }

            //loops through the images to draw them on the form
            for (int i = 0; i < NUMBER_OF_IMAGES; i++)
            {
                e.Graphics.DrawImage(image[i], imageBoundingBox[i]);

                //if image is clicked, the image is drawn
                if (imageClickState[i] == true)
                {
                    Rectangle imageBounding = new Rectangle(imageDraggedCoordinate, imageSize);
                    e.Graphics.DrawImage(image[i], imageBounding);

                }
            }

            if (isPlaced == true)
            {
                //saves the image's X and Y coordinates
                XY[rowProductPlaced, colProductPlaced] = new Point(colProductPlaced * columnWidth, rowProductPlaced * rowHieght);
                //saves the image's coordinates and size
                imageBounding[rowProductPlaced, colProductPlaced] = new Rectangle(XY[rowProductPlaced, colProductPlaced], imageSize);
            }

            //loops through the grid to draw them on the form
            for (int rows = 0; rows < NUM_ROWS; rows++)
            {
                for (int cols = 0; cols < NUM_COLS; cols++)
                {
                    //if the grid is empty it will not draw it
                    if (model.Grid[rows, cols] != null)
                    {
                        //draw image at the coordinate on the grid
                        e.Graphics.DrawImage(model.Grid[rows, cols].ProductImage, imageBounding[rows, cols]);
                    }
                }
            }

            //loops to find the location of the products on the grid that can be harvested
            for (int row = 0; row < NUM_ROWS; row++)
            {
                for (int col = 0; col < NUM_COLS; col++)
                {
                    //checks if the product can be harvested if there is a product in the grid location
                    if (model.Grid[row, col] != null)
                    {
                        //if it can be harvested a circle will be drawn around the product
                        if (model.Grid[row, col].CanHarvest == true)
                        {
                            e.Graphics.DrawEllipse(Pens.DarkOrange, _grid[row, col]);
                        }
                    }
                }
            }
        }

        //timer updates the graphics on the screen and performs any calculations/ operations
        //that need to be done regularly in the program
        private void tmrTime_Tick(object sender, EventArgs e)
        {
            //update all product info
            model.UpdateGame();
            //checks if game is over
            gameOver = model.GameIsOver();
            //checks for disasters
            model.Disaster();
            //subtract upkeep cost of all products
            model.SubtractUpkeepCost();

            //if game is over a message box will tell the user and the timer will be disabled
            if (gameOver == true)
            {
                MessageBox.Show("Game is Over");
                tmrTime.Enabled = false;
                tmrScreen.Enabled = false;
            }

            //loops through the array
            for (int row = 0; row < NUM_ROWS; row++)
            {
                for (int col = 0; col < NUM_COLS; col++)
                {
                    //checks if there is a product at the location
                    if (model.Grid[row, col] != null)
                    {
                        //checks if it can be harvasted
                        model.Grid[row, col].CanHarvest = model.CheckForHarvest(row, col);
                        //checks if product is died
                        model.ProductDies(row, col);
                    }
                }
            }
        }

        //when mouse is pressed down it will get the location of image that is click
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //loops get if each image is clicked
            for (int i = 0; i < image.Length; i++)
            {
                imageClickState[i] = imageBoundingBox[i].Contains(e.Location);
            }
        }

        //when mouse is lifted up it will set the image
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            //loops through to find which image is clicked
            for (int i = 0; i < product.Length; i++)
            {
                //if image is clicked it will save its location
                if (imageClickState[i] == true)
                {
                    //saves the coordinate of the location fo the mouse
                    imageDroppedCoordinate = e.Location;

                    //loops to find the row and col of the location
                    for (int row = 0; row < NUM_ROWS; row++)
                    {
                        for (int col = 0; col < NUM_COLS; col++)
                        {
                            //if location on gird equals location of mouse, the row and col will be saved
                            if (_grid[row, col].Contains(e.Location))
                            {
                                //saves the row and cols image is placed at and the index of the image
                                rowProductPlaced = row;
                                colProductPlaced = col;
                                index = i;
                            }
                        }
                    }

                    //changes image click state and place product to false
                    imageClickState[i] = false;
                    placeProduct = true;
                }
            }

            //if place product is true, it will check if the square is empty and then the product it is placing
            if (placeProduct == true)
            {
                //once user has placed a product, reset the bool to false
                placeProduct = false;

                //checks if the square is empty
                if (model.EmptySquare(rowProductPlaced, colProductPlaced) == true)
                {
                    //checks if corn is being placed
                    if (index == IMAGE_INDEX_CORN)
                    {
                        //calls BuildCorn to check whether the corn was placed and stores it
                        isPlaced = model.BuildCorn(rowProductPlaced, colProductPlaced);
                    }

                    //checks if tomato is being placed
                    else if (index == IMAGE_INDEX_TOMATO)
                    {
                        //calls BuildTomato to check whether the tomato was placed and stores it
                        isPlaced = model.BuildTomato(rowProductPlaced, colProductPlaced);
                    }

                    //checks if asparagus is being placed
                    else if (index == IMAGE_INDEX_ASPARAGUS)
                    {
                        //calls BuildAsparagus to check whether the asparagus was placed and stores it
                        isPlaced = model.BuildAsparagus(rowProductPlaced, colProductPlaced);
                    }

                    //checks if broccoli is being placed
                    else if (index == IMAGE_INDEX_BROCCOLI)
                    {
                        //calls BuildBroccoli to check whether the broccoli was placed and stores it
                        isPlaced = model.BuildBroccoli(rowProductPlaced, colProductPlaced);
                    }

                    //checks if potato is being placed
                    else if (index == IMAGE_INDEX_POTATO)
                    {
                        //calls BuildPotato to check whether the potato was placed and stores it
                        isPlaced = model.BuildPotato(rowProductPlaced, colProductPlaced);
                    }

                    //checks if potato is being placed
                    else if (index == IMAGE_INDEX_SWEET_POTATO)
                    {
                        //calls BuildSweetPotato to check whether the sweet potato was placed and stores it
                        isPlaced = model.BuildSweetPotato(rowProductPlaced, colProductPlaced);
                    }

                    //checks if pork is being placed
                    else if (index == IMAGE_INDEX_PORK)
                    {
                        //calls BuildPork to check whether the pork was placed and stores it
                        isPlaced = model.BuildPork(rowProductPlaced, colProductPlaced);
                    }

                    //checks if chicken is being placed
                    else if (index == IMAGE_INDEX_CHICKEN)
                    {
                        //calls BuildChicken to check whether the chicken was placed and stores it
                        isPlaced = model.BuildChicken(rowProductPlaced, colProductPlaced);
                    }

                    //checks if beef is being placed
                    else if (index == IMAGE_INDEX_BEEF)
                    {
                        //calls BuildBeef to check whether the beef was placed and stores it
                        isPlaced = model.BuildBeef(rowProductPlaced, colProductPlaced);
                    }

                    //checks if hen is being placed
                    else if (index == IMAGE_INDEX_HEN)
                    {
                        //calls BuildHen to check whether the hen was placed and stores it
                        isPlaced = model.BuildHen(rowProductPlaced, colProductPlaced);
                    }

                    //else cow is being placed
                    else
                    {
                        //calls BuildCow to check whether the cow was placed and stores it
                        isPlaced = model.BuildCow(rowProductPlaced, colProductPlaced);
                    }
                }
                //if the grid there was not empty, a message box will tell the user
                else
                {
                    //message box tells the user they cannot place 2 products on the same grid
                    MessageBox.Show("Cannot place two products on the same grid! Please choose a different area.");
                }
            }
        }

        //when mouse is moved it finds the location of the mouse
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //saves the location when the user is dragging the image
            imageDraggedCoordinate = e.Location;
        }

        //when timer ticks it will refresh
        private void tmrScreen_Tick(object sender, EventArgs e)
        {
            //refreshes the form
            Refresh();

            //update money label
            lblMoney.Text = "Money: " + model.GetMoney;
        }

        private void AngelaForm_MouseClick(object sender, MouseEventArgs e)
        {
            //loop through all the grid spaces to see where the user clicks
            for (int row = 0; row < NUM_ROWS; row++)
            {
                for (int col = 0; col < NUM_COLS; col++)
                {
                    //checks if current rectangle is the one clicked
                    if (_grid[row, col].Contains(e.Location))
                    {
                        //product exists at location on grid
                        if (model.Grid[row, col] != null)
                        {
                            //the product at the co-ordinate is available to harvest
                            if (model.Grid[row, col].CanHarvest == true)
                            {
                                //harvest the crop
                                model.HarvestCrop(row, col);
                            }
                        }
                    }
                }
            }
        }
    }
}
