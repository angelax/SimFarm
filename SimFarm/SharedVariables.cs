/*Angela Xu and Daniel Kim
 * May 15 2014
 * SimFarm allows the user to build products, harvest them and earn money*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimFarm
{
    class SharedVariables
    {
        public SharedVariables(int rows, int cols)
        {
            NumRows = rows;
            NumCols = cols;

            _grid = new Product[NumRows, NumCols];

        }

        //grid size - number of rows and columns
        private int _numRows;
        //gets and sets grid
        public int NumRows
        {
            get
            {
                return _numRows;
            }
            
            set
            {
                _numRows = value;
            }
        }
        public int _numCols;
        //gets and sets num of cols
        public int NumCols
        {
            get
            {
                return _numCols;
            }
            set
            {
                _numCols = value;
            }
        }
        //stores the amount of money the user has
        private double _money = 100000;
        //gets and sets money
        public double Money
        {
            get
            {
                return _money;
            }
            set
            {
                _money = value;
            }
        }
        //stores the amount of revenue the user makes
        private double _revenue = 0;
        //gets and sets revenue
        public double Revenue
        {
            get
            {
                return _revenue;
            }
            set
            {
                _revenue = value;
            }
        }
        //stores the grid
        private Product[,] _grid;
        //gets and sets grid
        public Product[,] Grid
        {
            get
            {
                return _grid;
            }
            set
            {
                _grid = value;
            }
        }
        //stores the age of the farm
        private int _ageOfFarm = 0;
        //gets and sets age of farm
        public int AgeOfFarm
        {
            get
            {
                return _ageOfFarm;
            }
            set
            {
                _ageOfFarm = value;
            }
        }
        //creates a random number
        Random numberGenerator = new Random();


    }
}
