using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

namespace Assignment1
{
    public partial class Form1 : Form
    {

        #region Intial Setup

        #region Global Variable Setup

        //global to the form class.
        PictureBox[,] aliens = new PictureBox[4, 4];
        PictureBox[] defenseBlocks = new PictureBox[4];
        string directionOfAlienMove = "R";
        PictureBox player = new System.Windows.Forms.PictureBox();
        int score = 0;
        int bulletsFired = 0;
        int bulletsSuccessful = 0;
        int bulletsShotOutOfSky = 0;
        int direction;

        Random r = new Random();
        List<PictureBox> alienBullets = new List<PictureBox>();

        BonusDropper bonusDropperPlane = new BonusDropper();
        PictureBox bonusPackage = new PictureBox();

        //Declare alien sprites at the top of the program so that they can be loaded once only.
        Image[,] alienSprites = new Image[4, 2];

        #endregion

        #region Config

        //-----------------
        //settings
        int level = 1; //default level
        int alienMoveAmt = 10;
        int alienBulletMoveAmt = 10;
        int damage = 20;
        int defenseBlockWidth = 200;
        int alienSize = 50;
        int alienSpacingX = 100;
        int alienSpacingY = 70;
        int bonusDropperPlaneMoveAmt = 20;
        int bonusDropperFlightPath = -400;
        int bonusPackageSize = 20;
        int bonusPackageMoveAmt = 10;
        int alienDistanceFromTop = 100;

        //-----------------

        #endregion

        #region Game Load - Flow
        public Form1()
        {
            InitializeComponent();
        }

        //Game load flow
        private void Game_Load(object sender, EventArgs e)
        {
            PopulateSpriteArray();
            DrawAliens();
            DrawDefenseBlocks();
            DrawPlayer();
        }

        #endregion

        #region Game Load - Helpers

        //Helper that populates the sprite array.
        private void PopulateSpriteArray()
        {
            //populate the image sprites array
            for (int i = 0; i < alienSprites.GetLength(0); i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    alienSprites[i, j] = Properties.Resources.alien0_0;
                }

            }
        }
        
        //Helper that draws sprites at load
        private void DrawAliens()
        {
            //Draw grid of aliens
            for (int i = 0; i < aliens.GetLength(0); i++)
            {

                for (int j = 0; j < aliens.GetLength(1); j++)
                {
                    aliens[i, j] = new System.Windows.Forms.PictureBox();
                    aliens[i, j].Location = new System.Drawing.Point((alienDistanceFromTop + i * alienSpacingX), (alienDistanceFromTop + j * alienSpacingY));
                    aliens[i, j].Name = "alien " + i + "-" + j;
                    aliens[i, j].Size = new System.Drawing.Size(alienSize, alienSize);
                    aliens[i, j].Image = alienSprites[i, 1];
                    aliens[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                    this.Controls.Add(this.aliens[i, j]);
                }
            }
        }

        //Helper that draws defence blocks on load.
        private void DrawDefenseBlocks()
        {
            
            for (int i = 0; i < defenseBlocks.Length; i++)
            {
                defenseBlocks[i] = new System.Windows.Forms.PictureBox();
                defenseBlocks[i].Location = new System.Drawing.Point((((this.Width / 4) * (i + 1) - (this.Width / 8))- defenseBlockWidth/2), this.Height - 200);
                defenseBlocks[i].Name = "block " + i;
                defenseBlocks[i].Size = new System.Drawing.Size(defenseBlockWidth, 20);
                defenseBlocks[i].BackColor = Color.Red;
                this.Controls.Add(this.defenseBlocks[i]);
            }
        }

        //Helper that draws player on load

        private void DrawPlayer()
        {
            player.Name = "player";
            player.Size = new System.Drawing.Size(100, 50);
            player.SizeMode = PictureBoxSizeMode.StretchImage;
            player.Image = Properties.Resources.space_invader_player;
            player.Location = new System.Drawing.Point((this.Width / 2) - player.Width, this.Height - 100);
            this.Controls.Add(this.player);
        }

        #endregion

        #endregion

        #region Timer Handlers

        private void alienMoveTimer_Tick(object sender, EventArgs e)
        {
            if (directionOfAlienMove == "R")
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        this.aliens[i, j].Left = this.aliens[i, j].Left + alienMoveAmt;
                    }
                }
            }
            if (directionOfAlienMove == "L")
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        this.aliens[i, j].Left = this.aliens[i, j].Left - alienMoveAmt;
                    }
                }
            }

            if (this.aliens[3, 0].Right > this.Width)
            {
                directionOfAlienMove = "L";
            }
            else if (this.aliens[0, 0].Left <= 0)
            {
                directionOfAlienMove = "R";
            }
        }

        public void bulletTimer_Tick(object sender, EventArgs e)
        {
            //--------------
            //PLAYER BULLET

            //if the bullet has passed the top of the page
            if (bullet.Top < 0)
            {
                resetPlayerBullet();
            }
            //otherwise, move the bullet...
            else
            {
                bullet.Top = bullet.Top - 10;
                //... and check whether the bullet is in contact with any of the aliens.
                bool contact = false;

                for (int x = 0; x < (aliens.GetLength(0)); x++)
                {
                    if (!contact)
                    {
                        for (int y = 0; y < (aliens.GetLength(1)); y++)
                        {
                            //for readability & efficiency
                            System.Windows.Forms.PictureBox alien = aliens[x, y];

                            if (alien.Visible)
                            {

                                //if the bullet and alien collide...
                                if (twoThingsImpacted(bullet, alien))
                                {
                                    //hide bullet
                                    contact = true;
                                    bulletTimer.Enabled = false;
                                    bullet.Visible = false;
                                    //stash away above the top edge of the screen
                                    bullet.Top = 0 - bullet.Height;
                                    bulletsSuccessful++;

                                    //get the current size of the impacted alien
                                    System.Drawing.Size currentSize = alien.Size;
                                    if (currentSize.Width > 30)
                                    {
                                        //----------------
                                        //injure the alien
                                        int currentHeight = currentSize.Height;
                                        int currentWidth = currentSize.Width;

                                        //shrink the alient
                                        alien.Size = new System.Drawing.Size(currentHeight - damage, currentWidth - damage);
                                        //recenter the alien in its position.
                                        alien.Left = alien.Left + (damage / 2);
                                        //----------------
                                    }
                                    else
                                    {
                                        alien.Visible = false;
                                        score++;
                                    }
                                }
                            }
                        }
                    }
                }

                //... or any of the defense blocks
                for (int i = 0; i < defenseBlocks.Count(); i++)
                {
                    if (twoThingsImpacted(bullet, defenseBlocks[i]))
                    {
                        if (defenseBlocks[i].Width > 10)
                        {
                            defenseBlocks[i].Width = defenseBlocks[i].Width - 10;
                            defenseBlocks[i].Left = defenseBlocks[i].Left + 5;
                            bulletTimer.Enabled = false;
                            bullet.Visible = false;
                            //stash away above the top edge of the screen
                            bullet.Top = 0 - bullet.Height;
                        }
                    }
                }
            }
        }

        private void alientBulletMoveTimer_Tick(object sender, EventArgs e)
        {
            //-----------------
            //ALIEN BULLETS

            //create a list to hold the bullets to remove from the GUI
            List<int> bulletsToRemove = new List<int>();

            //for all bullets currently in the GUI...
            for (int i = 0; i < alienBullets.Count(); i++)
            {

                // check if they are off the screen...
                if (alienBullets[i].Top >= this.Height)
                {
                    //if so, add their index to the list to be removed.
                    bulletsToRemove.Add(i);
                }

                //check if they are in contact with the player
                if (twoThingsImpacted(alienBullets[i], player))
                {
                    //End Game
                    this.BackColor = Color.Red;
                    alienMoveTimer.Enabled = false;
                    bulletTimer.Enabled = false;
                    alienFireTimer.Enabled = false;
                    alienBulletMoveTimer.Enabled = false;
                }

                //check if they are in contact with the players bullet
                if (twoThingsImpacted(alienBullets[i], bullet))
                {
                    resetPlayerBullet();
                    //if so, add their index to the list to be removed.
                    bulletsToRemove.Add(i);
                    bulletsShotOutOfSky++;
                }

                //check if they are in contact with a defensive barrier
                for (int j = 0; j < defenseBlocks.Length; j++)
                {
                    if (twoThingsImpacted(alienBullets[i], defenseBlocks[j]))
                    {
                        if (defenseBlocks[i].Width > 10)
                        {
                            defenseBlocks[j].Width = defenseBlocks[j].Width - 10;
                            defenseBlocks[j].Left = defenseBlocks[j].Left + 5;
                            bulletsToRemove.Add(i);
                        }
                    }
                }

                if (alienBullets[i].Top < this.Height)
                {
                    // add to their position.
                    alienBullets[i].Top = alienBullets[i].Top + alienBulletMoveAmt;
                }
            }



            //remove any spent bullets from the screen
            for (int i = 0; i < bulletsToRemove.Count; i++)
            {
                alienBullets[bulletsToRemove[i]].Visible = false;
                alienBullets.RemoveAt(bulletsToRemove[i]);
            }
        }

        private void alienFireTimer_Tick(object sender, EventArgs e)
        {
            //--------------
            //HOW THIS WORKS
            //--------------
            //each time the timer ticks, a random column is generated.  
            //the following code checks whether there is an alien in that column and then
            //reports the position of the lowest row occupied by an alien.
            //Assuming the below grid where x is a visible alien... 
            // | x | x | x | o |
            // | x | o | x | o |
            // | o | o | x | o |
            // | o | o | o | o |
            //checkAlientInColumn(arr, 0) = 1
            //checkAlientInColumn(arr, 1) = 0
            //checkAlientInColumn(arr, 2) = 2
            //checkAlientInColumn(arr, 3) = -1

            //if no aliens are found in the selected column,  random column is selected again and the check completed

            int alienLowestRow = -1;
            while (alienLowestRow == -1)
            {
                //randomly generate the firing alien
                int firingAlienColumn = r.Next(0, this.aliens.GetLength(0));

                //check that there are aliens in that column
                alienLowestRow = checkAlienInColumn(this.aliens, firingAlienColumn);


                if (alienLowestRow != -1)
                {
                    alienFire(firingAlienColumn, alienLowestRow);
                }
            }
        }

        //ups the intensity of the game every x seconds
        private void intensityTimer_Tick(object sender, EventArgs e)
        {
            increaseIntensity(level);
        }

        //triggers the presence of the bonus dropper plane.
        private void bonusTimer_Tick(object sender, EventArgs e)
        {
            //randomly generate the movement direction
            direction = r.Next(0,2);
            //0 = left -> right, 1 = right -> left
            int xPosition = direction == 0 ? 0 : this.Width;

            bonusDropperPlane.PictureBox = new PictureBox();
            bonusDropperPlane.PictureBox.Location = new System.Drawing.Point((xPosition), (this.Height + bonusDropperFlightPath));
            bonusDropperPlane.PictureBox.Name = "bonusDropperPlane";
            bonusDropperPlane.PictureBox.Size = new System.Drawing.Size(alienSize, alienSize);
            bonusDropperPlane.PictureBox.Image = direction == 0 ? Properties.Resources.aircraftspriterh : Properties.Resources.aircraftspritelh;
            bonusDropperPlane.PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            bonusDropperPlane.DropTriggerX = r.Next(0, this.Width);
            bonusDropperPlane.Dropped = false;
            this.Controls.Add(bonusDropperPlane.PictureBox);
        }

        private void bonusPlaneMoveTimer_Tick(object sender, EventArgs e)
        {
            //if the bonusDropper has been added to the screen.
            if (bonusDropperPlane.PictureBox != null)
            {
               
                //if the package hasn't already been dropped, assess whether the plane has passed the trigger point.  
                if (!bonusDropperPlane.Dropped)
                {
                    int bonusDropperPosition = bonusDropperPlane.PictureBox.Left;
                    int dropTrigger = bonusDropperPlane.DropTriggerX;
                    
                    if ((direction == 1 && bonusDropperPosition <= dropTrigger) || (direction == 0 && bonusDropperPosition >= dropTrigger))
                    {
                        //set the dropped flag
                        bonusDropperPlane.Dropped = true;

                        //calc the middle of the plane's position.
                        int planeXPosition = bonusDropperPlane.PictureBox.Left + bonusDropperPlane.PictureBox.Width / 2;

                        bonusPackage.Location = new System.Drawing.Point(planeXPosition , (this.Height + bonusDropperFlightPath));
                        bonusPackage.Name = "bonusPackage";
                        bonusPackage.Size = new System.Drawing.Size(bonusPackageSize, bonusPackageSize);
                        bonusPackage.Image = Properties.Resources.parachutepackage;
                        bonusPackage.SizeMode = PictureBoxSizeMode.StretchImage;
                        this.Controls.Add(bonusPackage);
                        packageDropTimer.Enabled = true;
                    }
                }

                //disearn the direction of it's travel and move the bonusDropper in the direction of travel.
                if (direction == 1)
                { 
                    bonusDropperPlane.PictureBox.Left = bonusDropperPlane.PictureBox.Left - bonusDropperPlaneMoveAmt;
                } 
                else
                {
                    bonusDropperPlane.PictureBox.Left = bonusDropperPlane.PictureBox.Left + bonusDropperPlaneMoveAmt;
                }
            }
        }

        private void packageDropTimer_Tick (object sender, EventArgs e)
        {
            if (bonusPackage != null)
            {
                //move the package 
                bonusPackage.Top = bonusPackage.Top + bonusPackageMoveAmt;

                //check whether the package is in contact with the player
                if (twoThingsImpacted(bonusPackage, player))
                {
                    //--------------------------
                    //insert player upgrade here.
                    //--------------------------
                }

                //if off screen, remove
                if (bonusPackage.Top > this.Height)
                {
                    //remove package from screen
                    packageDropTimer.Enabled = false;
                    bonusPackage.Top = -100;
                }
            }
        }

        #endregion

        #region Command Key Handling

        //handles arrow key commands.
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //capture left arrow key or A
            if ((keyData == Keys.Left) || (keyData == Keys.A))
            {
                player.Left = player.Left - 25;
                return true;
            }
            //capture right arrow key or D
            if ((keyData == Keys.Right) || (keyData == Keys.D))
            {
                player.Left = player.Left + 25;
                return true;
            }
            //capture right arrow key or D
            if (keyData == Keys.Space)
            {
                fire();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region Helpers

        //a helper to reset the timer and the bullet (hides it as it can't do any damage up there anyway)
        public void resetPlayerBullet()
        {
            bulletTimer.Enabled = false;
            bullet.Visible = false;
        }

        // a helper which checks whether two pictureBoxes are in contact with one another
        private bool twoThingsImpacted(PictureBox firstThing, PictureBox secondThing)
        {

            //-----------------
            // for readability
            int firstTop = firstThing.Top;
            int firstBottom = firstThing.Bottom;
            int firstLeft = firstThing.Left;
            int firstRight = firstThing.Right;

            int secondTop = secondThing.Top;
            int secondBottom = secondThing.Bottom;
            int secondLeft = secondThing.Left;
            int secondRight = secondThing.Right;
            //-----------------

            //is the firstThing horizontally aligned with secondThing?
            bool horizontallyAligned = (firstLeft >= secondLeft && firstLeft <= secondRight) || (firstRight >= secondLeft && firstRight <= secondLeft);

            //initially set to false.
            bool verticallyAligned = false;

            //efficiency - only check for vert alignment if horiz alignment is true
            if (horizontallyAligned)
            {
                //is the firstThing vertically aligned with secondThing? 
                verticallyAligned = ((firstTop >= secondTop && firstTop <= secondBottom) || (firstBottom <= secondBottom && firstBottom >= secondTop));
            }

            return horizontallyAligned && verticallyAligned;
        }

        //a helper which checks whether a column contains an alien.  If an alien is detected, the func returns
        //the position of that alien.  If no alien is in the column, it returns -1.
        //Full details in alienFireTimer_Tick.
        private int checkAlienInColumn(PictureBox[,] array, int firingColumn)
        {
            for (int i = array.GetLength(0) - 1; i >= 0; i--)
            {
                if (array[firingColumn, i].Visible)
                {
                    return i;
                }
            }
            return -1;
        }

        private void increaseIntensity(int difficulty)
        {
            int intensityModifier = difficulty * 5;
            alienMoveTimer.Interval = alienMoveTimer.Interval += intensityModifier;
            alienBulletMoveTimer.Interval = alienBulletMoveTimer.Interval += intensityModifier;
            alienFireTimer.Interval = alienFireTimer.Interval += intensityModifier;
        }

        #endregion

        #region Fire Action Handling

        //handle player fire action.
        public void fire()
        {
            if (!bulletTimer.Enabled)
            {
                //set start position of bullet.
                bullet.Left = (player.Left + player.Width / 2) - bullet.Width / 2;
                bullet.Top = player.Top;
                bullet.Visible = true;
                bulletTimer.Enabled = true;
                bulletsFired++;
            }
        }

        private void alienFire(int column, int row)
        {
            PictureBox alienBullet = new System.Windows.Forms.PictureBox();
            alienBullet.BackColor = Color.White;
            alienBullet.Top = this.aliens[column, row].Top;
            alienBullet.Left = this.aliens[column, row].Left + (this.aliens[column, row].Width / 2);
            alienBullet.Width = 10;
            alienBullet.Height = 10;
            alienBullets.Add(alienBullet);
            this.Controls.Add(alienBullet);
        }

        #endregion

    }


}
