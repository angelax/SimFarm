/* Daniel Kim
 * May 28, 2014
 * Sim Farm Form to display graphics of the program. 
 * Game about a farming simulation
 */
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
    public partial class DanielForm : Form
    {
        ////stores the x and y co-ords of each product placed on the grid
        //int[] placedX = new int[NUM_ROWS];
        //int[] placedY = new int[NUM_COLS];

        //grid size - num of rows/columns
        public const int NUM_ROWS = 10;
        public const int NUM_COLS = 10;

        //store whether or not crop is harvested
        //bool cropHarvested;

        ////store row/col of products that can currently be harvested
        //int rowCanHarvest;
        //int colCanHarvest;

        //store the row/col of the mouse click on grid to harvest
        //int harvestingRow;
        //int harvestingCol;

        ////store the row/col of the product that is dieing
        //int dyingRow;
        //int dyingCol;

        SimFarmModel model = new SimFarmModel(NUM_ROWS, NUM_COLS);

        //stores whether or not the product is being placed beside another animal or a plant
        bool isPlacedBeside = false;

        ////stores whether or not crop is available to harvest
        //bool[,] availableToHarvest = new bool[NUM_ROWS, NUM_COLS];

        ////stores whether or not crop is dead / reached its lifespan
        //bool[,] productIsDead = new bool[NUM_ROWS, NUM_COLS];

        ////stores the max products that can be on the grid (amount of grid squares available)
        //const int MAX_PRODUCTS = NUM_ROWS * NUM_COLS;

        ////the grid's row where the user wants to place a new product
        //int productPlacingRow;
        ////the grid's column where the user wants to place a product
        //int productPlacingCol;

        ////stores the XY coordinate of the nth product the user is placing
        //Point[,] XY = new Point[NUM_ROWS, NUM_COLS];
        ////stores the imagebounding rectangle of the nth coordinate user is placing
        //Rectangle[,] imageBounding = new Rectangle[NUM_ROWS, NUM_COLS];

        ////piece is placed on grid
        //bool placeProduct = false;

        //image index of product image that user clicks on
        int[,] imageClickedIndex = new int[NUM_ROWS, NUM_COLS];

        //number of products
        const int NUMBER_OF_PRODUCTS = 11;

        //Store imgs in array for simpler drawing and selection
        Image[] productImage = new Image[NUMBER_OF_PRODUCTS];

        ////store an array of all the available products
        //Product[] defaultProduct = new Product[NUMBER_OF_PRODUCTS];

        //store whether the product has been clicked on to plant/grow
        bool[] imageClickState = new bool[NUMBER_OF_PRODUCTS];

        //store the (x,y) location of the mouse when it is being dragged
        Point imageDraggedCoordinate;

        //store the (x,y) location of the mouse when it is being dropped
        Point imageDroppedCoordinate;

        //store image size and location
        Rectangle[] imageBoundingBox = new Rectangle[NUMBER_OF_PRODUCTS];
        Point[] imageXY = new Point[NUMBER_OF_PRODUCTS];

        //form dimensions
        const int FORM_WIDTH = 600;
        const int FORM_HEIGHT = 600;

        //dimensions of each grid block
        int columnWidth;
        int rowHeight;

        //mouse location offset
        const int X_OFFSET = -30;
        int Y_OFFSET = -30;

        //store grid that's drawn on screen
        Rectangle[,] _grid;

        //image indexes of all products
        const int IMAGE_INDEX_CORN = 0;
        const int IMAGE_INDEX_TOMATO = 1;
        const int IMAGE_INDEX_ASPARAGUS = 2;
        const int IMAGE_INDEX_BROCCOLI = 3;
        const int IMAGE_INDEX_POTATO = 4;
        const int IMAGE_INDEX_SWEET_POTATO = 5;
        const int IMAGE_INDEX_BEEF = 6;
        const int IMAGE_INDEX_CHICKEN = 7;
        const int IMAGE_INDEX_PORK = 8;
        const int IMAGE_INDEX_HEN = 9;
        const int IMAGE_INDEX_COW = 10;

        //ensure all img drawn to same size
        const int STANDARD_IMAGE_WIDTH = 60;
        const int STANDARD_IMAGE_HEIGHT = 60;
        Size imageSize;

        public DanielForm()
        {
            InitializeComponent();
            InitializeGameBoard();
            InitializeImages();

            lblMoney.Text = "Money: $" + model.GetMoney;
        }

        //assign images
        private void InitializeImages()
        {
            //standard image size
            imageSize = new Size(STANDARD_IMAGE_WIDTH, STANDARD_IMAGE_HEIGHT);

            // assign images to each productImage array
            productImage[IMAGE_INDEX_CORN] = Properties.Resources.corn;
            productImage[IMAGE_INDEX_TOMATO] = Properties.Resources.tomato;
            productImage[IMAGE_INDEX_ASPARAGUS] = Properties.Resources.asparagus;
            productImage[IMAGE_INDEX_BROCCOLI] = Properties.Resources.broccoli;
            productImage[IMAGE_INDEX_POTATO] = Properties.Resources.potato;
            productImage[IMAGE_INDEX_SWEET_POTATO] = Properties.Resources.sweet_potato;
            productImage[IMAGE_INDEX_BEEF] = Properties.Resources.beef;
            productImage[IMAGE_INDEX_CHICKEN] = Properties.Resources.chicken;
            productImage[IMAGE_INDEX_PORK] = Properties.Resources.pork;
            productImage[IMAGE_INDEX_HEN] = Properties.Resources.hen;
            productImage[IMAGE_INDEX_COW] = Properties.Resources.cow;

            ////assign the default products to each array index
            //defaultProduct[IMAGE_INDEX_CORN] = new Corn();
            //defaultProduct[IMAGE_INDEX_TOMATO] = new Tomato();
            //defaultProduct[IMAGE_INDEX_ASPARAGUS] = new Asparagus();
            //defaultProduct[IMAGE_INDEX_BROCCOLI] = new Broccoli();
            //defaultProduct[IMAGE_INDEX_POTATO] = new Potato();
            //defaultProduct[IMAGE_INDEX_SWEET_POTATO] = new SweetPotato();
            //defaultProduct[IMAGE_INDEX_BEEF] = new Beef();
            //defaultProduct[IMAGE_INDEX_CHICKEN] = new Chicken();
            //defaultProduct[IMAGE_INDEX_PORK] = new Pork();
            //defaultProduct[IMAGE_INDEX_HEN] = new Hen();
            //defaultProduct[IMAGE_INDEX_COW] = new Cow();

            //loop through every image and assign a location
            for (int i = 0; i < productImage.Length; i++)
            {
                //create location for current image
                imageXY[i] = new Point(this.Width + 100, i * STANDARD_IMAGE_WIDTH);
                //create bounding box for current image
                imageBoundingBox[i] = new Rectangle(imageXY[i], imageSize);
                //image has not been clicked yet
                imageClickState[i] = false;
            }
        }

        //initalize the game grid
        void InitializeGameBoard()
        {
            //form dimensions
            Height = FORM_HEIGHT;
            Width = FORM_WIDTH;

            //width and height of each grid square
            columnWidth = FORM_WIDTH / NUM_ROWS;
            rowHeight = FORM_HEIGHT / NUM_COLS;

            //Create graphical grid
            _grid = new Rectangle[NUM_ROWS, NUM_COLS];

            //create each rectangle of the grid
            //loop through each row and column
            for (int rows = 0; rows < NUM_ROWS; rows++)
            {
                for (int cols = 0; cols < NUM_COLS; cols++)
                {
                    //create new rectangle
                    _grid[rows, cols] = new Rectangle(cols * columnWidth, rows * rowHeight, columnWidth, rowHeight);
                }
            }
        }

        //paint the board onto the form
        private void SimFarmForm_Paint(object sender, PaintEventArgs e)
        {
            //draw rectangle at each row and column through the loop
            for (int rows = 0; rows < NUM_ROWS; rows++)
            {
                for (int cols = 0; cols < NUM_COLS; cols++)
                {
                    //fill in a green rectangle
                    e.Graphics.FillRectangle(Brushes.Green, _grid[rows, cols]);
                    //outline rectangle with white
                    e.Graphics.DrawRectangle(Pens.White, _grid[rows, cols]);
                }
            }

            //loops through the productImage array to draw every product
            for (int i = 0; i < NUMBER_OF_PRODUCTS; i++)
            {
                e.Graphics.DrawImage(productImage[i], imageBoundingBox[i]);

                //image clicked to be planted
                if (imageClickState[i] == true)
                {
                    //form a  new image bounding for the product
                    Rectangle imageBounding = new Rectangle(imageDraggedCoordinate, imageSize);
                    //draw the product image that is being dragged currently so it follows the mouse movements
                    e.Graphics.DrawImage(productImage[i], imageBounding);
                }
            }

            //re-draw all the currently placed products on the grid at every refresh
            for (int row = 0; row < NUM_ROWS; row++)
            {
                for (int col = 0; col < NUM_COLS; col++)
                {
                    if (model.Grid[row, col] != null)
                    {
                        //draw the image onto grid
                        e.Graphics.DrawImage(model.Grid[row, col].ProductImage, _grid[row, col]);

                        //the product is harvestable
                        if (model.Grid[row, col].CanHarvest == true)
                        {
                            //indicate user with an ellipse on grid of product
                            e.Graphics.DrawEllipse(Pens.Yellow, _grid[row, col]);
                        }
                    }
                }
            }

            ////loop through all the grid squares to see if product can be harvested yet
            //for (int row = 0; row < NUM_ROWS; row++)
            //{
            //    for (int col = 0; col < NUM_COLS; col++)
            //    {
            //        //the product at the location is available for harvest
            //        if (availableToHarvest[row, col] == true)
            //        {
            //            //draw an ellipse to notify user
            //            e.Graphics.DrawEllipse(Pens.Yellow, _grid[rowCanHarvest, colCanHarvest]);
            //        }
            //    }
            //}

            ////user has harvested a crop
            //if (cropHarvested == true)
            //{
            //    //draw over the product image
            //    e.Graphics.FillRectangle(Brushes.Green, _grid[harvestingRow, harvestingCol]);
            //    e.Graphics.DrawRectangle(Pens.White, _grid[harvestingRow, harvestingCol]);

            //    //reset the x and y co-ords of the product's placement
            //    placedX[harvestingRow] = 0;
            //    placedY[harvestingCol] = 0;

            //    //reset the product's placement on the grid
            //    XY[harvestingRow, harvestingCol] = new Point(0, 0);
            //    //reset the image bounding of the product
            //    imageBounding[harvestingRow, harvestingCol] = new Rectangle(0, 0, 0, 0);

            //    //product is not being harvested anymore
            //    cropHarvested = false;

            //    //product at the location is not available for harvest anymore
            //    availableToHarvest[harvestingRow, harvestingCol] = false;
            //}

            ////loop through the grid to see if a product has died/reach maxlifespan
            //for (int row = 0; row < NUM_ROWS; row++)
            //{
            //    for (int col = 0; col < NUM_COLS; col++)
            //    {
            //        //product is dead
            //        if (productIsDead[row, col] == true)
            //        {
            //            //draw over the image
            //            e.Graphics.DrawRectangle(Pens.White, _grid[dyingRow, dyingCol]);
            //            e.Graphics.FillRectangle(Brushes.Green, _grid[dyingRow, dyingCol]);

            //            //reset x and y coords of product dying
            //            placedX[dyingRow] = 0;
            //            placedY[dyingCol] = 0;

            //            //reset the point location of product's death
            //            XY[dyingRow, dyingCol] = new Point(0, 0);

            //            //reset the image bounding of product
            //            imageBounding[dyingRow, dyingCol] = new Rectangle(0, 0, 0, 0);

            //            //product is not dead at this location anymore
            //            productIsDead[dyingRow, dyingCol] = false;
            //        }
            //    }
            //}
        }

        //every tick = 1 month has passed by on the farm
        //update the farm age
        private void tmrFarmAge_Tick(object sender, EventArgs e)
        {
            //update all product info
            model.UpdateProducts();

            //update money label
            lblMoney.Text = "Money: $" + model.GetMoney;

            for (int row = 0; row < NUM_ROWS; row++)
            {
                for (int col = 0; col < NUM_COLS; col++)
                {
                    if (model.Grid[row, col] != null)
                    {
                        model.Grid[row, col].CanHarvest = model.CheckForHarvest(row, col);
                    }
                }
            }

            ////loop through all the grid squares to see if product can be harvested yet or if its dead/reached lifespan
            //for (int row = 0; row < NUM_ROWS; row++)
            //{
            //    for (int col = 0; col < NUM_COLS; col++)
            //    {
            //        //check for harvest availability
            //        availableToHarvest[row, col] = model.CheckForHarvest(row, col);
            //        //check for death
            //        productIsDead[row, col] = model.ProductDies(row, col);

            //        //product is dead
            //        if (productIsDead[row, col] == true)
            //        {
            //            //productIsDying = true;
            //            dyingRow = row;
            //            dyingCol = col;
            //        }

            //        //product can be harvested
            //        else if (availableToHarvest[row, col] == true)
            //        {
            //            //store harvested coordinates of product
            //            rowCanHarvest = row;
            //            colCanHarvest = col;
            //        }
            //    }
            //}
        }

        //update the simfarm form graphics
        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        //user clicks down on the mouse
        private void SimFarmForm_MouseDown(object sender, MouseEventArgs e)
        {
            //loop through images to get the image clicked state of every image
            for (int i = 0; i < productImage.Length; i++)
            {
                imageClickState[i] = imageBoundingBox[i].Contains(e.Location);
            }
        }

        //user clicks up on the mouse
        private void SimFarmForm_MouseUp(object sender, MouseEventArgs e)
        {
            //loop through all the images
            for (int i = 0; i < productImage.Length; i++)
            {
                //image has been clicked
                if (imageClickState[i] == true)
                {
                    //place the image on the grid
                    imageDroppedCoordinate = e.Location;

                    //loop through all the grid spaces to see where the user clicks
                    for (int row = 0; row < NUM_ROWS; row++)
                    {
                        for (int col = 0; col < NUM_COLS; col++)
                        {
                            //checks if current rectangle is the one clicked
                            if (_grid[row, col].Contains(imageDroppedCoordinate))
                            {
                                //the current square is empty, no product is already using the spot
                                if (model.EmptySquare(row, col) == true)
                                {
                                    //index of the image that has been clicked on to plant
                                    imageClickedIndex[row, col] = i;

                                    ////once user has placed a product, reset the bool to false
                                    //placeProduct = false;

                                    //build a corn
                                    if (imageClickedIndex[row, col] == IMAGE_INDEX_CORN)
                                    {
                                        isPlacedBeside = model.BuildCorn(row, col);
                                    }

                                    //build a tomato
                                    else if (imageClickedIndex[row, col] == IMAGE_INDEX_TOMATO)
                                    {
                                        isPlacedBeside = model.BuildTomato(row, col);
                                    }
                                    //build a asparagus
                                    else if (imageClickedIndex[row, col] == IMAGE_INDEX_ASPARAGUS)
                                    {
                                        isPlacedBeside = model.BuildAsparagus(row, col);
                                    }
                                    //build a broccoli
                                    else if (imageClickedIndex[row, col] == IMAGE_INDEX_BROCCOLI)
                                    {
                                        isPlacedBeside = model.BuildBroccoli(row, col);
                                    }
                                    //build a potato
                                    else if (imageClickedIndex[row, col] == IMAGE_INDEX_POTATO)
                                    {
                                        isPlacedBeside = model.BuildPotato(row, col);
                                    }
                                    //build a sweet potato
                                    else if (imageClickedIndex[row, col] == IMAGE_INDEX_SWEET_POTATO)
                                    {
                                        isPlacedBeside = model.BuildSweetPotato(row, col);
                                    }
                                    //build a pork
                                    else if (imageClickedIndex[row, col] == IMAGE_INDEX_PORK)
                                    {
                                        isPlacedBeside = model.BuildPork(row, col);
                                    }
                                    //build a chicken
                                    else if (imageClickedIndex[row, col] == IMAGE_INDEX_CHICKEN)
                                    {
                                        isPlacedBeside = model.BuildChicken(row, col);
                                    }
                                    //build a beef
                                    else if (imageClickedIndex[row, col] == IMAGE_INDEX_BEEF)
                                    {
                                        isPlacedBeside = model.BuildBeef(row, col);
                                    }
                                    //build a hen
                                    else if (imageClickedIndex[row, col] == IMAGE_INDEX_HEN)
                                    {
                                        isPlacedBeside = model.BuildHen(row, col);
                                    }
                                    //build a cow
                                    else
                                    {
                                        isPlacedBeside = model.BuildCow(row, col);
                                    }

                                    //product is allowed to be placed at the specific co-ordinate
                                    if (isPlacedBeside == true)
                                    {
                                        lblMoney.Text = "Money: $" + model.GetMoney;

                                        isPlacedBeside = false;
                                    }

                                    //show error cannot place product due to plants and animals not being able to be beside eachother
                                    else
                                    {
                                        isPlacedBeside = false;
                                        MessageBox.Show("Plants and Animals cannot be placed adjacent to eachother.");
                                    }
                                }

                                //show error that you cannot have 2 products on the same location
                                else
                                {
                                    //product is no longer being placed
                                    //placeProduct = false;
                                    MessageBox.Show("Cannot place two products on the same grid! Please choose a different area.");
                                }

                                //image is no longer clicked once mouse is released
                                imageClickState[i] = false;
                            }
                        }
                    }
                }
            }
        }

        //user moves the mouse around
        private void SimFarmForm_MouseMove(object sender, MouseEventArgs e)
        {
            //location of the mouse
            imageDraggedCoordinate = e.Location;
            //offset the dragged co-ordinate to make the picture be located at the center of the mouse pointer
            imageDraggedCoordinate.X += X_OFFSET;
            imageDraggedCoordinate.Y += Y_OFFSET;
        }

        // user clicks to harvest a plant
        private void DanielForm_MouseClick(object sender, MouseEventArgs e)
        {
            //loop through all the grid spaces to see where the user clicks
            for (int row = 0; row < NUM_ROWS; row++)
            {
                for (int col = 0; col < NUM_COLS; col++)
                {
                    //checks if current rectangle is the one clicked
                    if (_grid[row, col].Contains(e.Location))
                    {
                        //checks if there is a product at that location on grid
                        if (model.Grid[row, col] != null)
                        {
                            //the product at the coordinate is available to harvest
                            if (model.Grid[row, col].CanHarvest == true)
                            {
                                //harvest the crop
                                model.HarvestCrop(row, col);

                                //update the money label
                                lblMoney.Text = "Money: " + model.GetMoney;
                            }
                        }
                    }
                }
            }
        }
    }
}
