/***************************************************\
* Project:		Lab 1 - Rock Paper Scissors			*
* Files:		main.cpp							*
* Date:			25 Jan 2006							*
* Author:		Addison Babcock		Class:  CNT15	*
* Instructor:	J. D. Silver		Course: CNT151	*
* Description:	A simple game of rock paper			*
*	scissors. A best of seven match played by 1		*
*	user against a random number generator			*
\***************************************************/

#include <iostream>		//Used for cin and cout mostly
#include <iomanip>		//Used to manipulate cin
#include <cmath>		//Used for random numbers
#include <ctime>		//Used to seed the random numbers

using namespace std;

int main (void)
{

	unsigned short int	iComputerScore = 0;		//How many rounds the computer won
	unsigned short int	iHumanScore = 0;		//How many rounds the player won
	unsigned short int	iRoundCount = 1;		//How many rounds have started

    //For the next two variables {1==rock, 2==paper, 3==scissors}
  	unsigned short int	iHumanChoice = 0;		//The humans weapon of choice
    unsigned short int	iComputerChoice = 0;	//The computers weapon of choice

	//For the next variable {1==human, 2==computer, 3==tie}
	unsigned short int	iRoundWinner = 0;		//Who won this round?

	bool				bContinue = true;		//Continue to play?
	char				cChoice = 0;			//Temp variable for containing
												//Input from the keyboard

	srand (static_cast <unsigned int> (time (0)));	//Seed the rand () function

	cout << "\t\t\tPaper\n\t\t\t\tRock\n\t\t\t\t\tScissors\n\n";	//Display a welcome screen

	cout << "\tThe game of paper rock scissors has a long and hallowed tradition\n";
	cout << "In the ancient times, player would hurl rocks at each others head and\n";
	cout << "a winner would be declared over the bloody loser.\n";
	cout << "\tToday, we play a more civilized version using computers where the\n";
	cout << "losers are only humiliated instead of killed.\n";
	cout << "\tThis version of paper rock scissors is a best-of-seven match between\n";
	cout << "a player and a computer. Ties do not count as a round. LET THE GAMES BEGIN!\n\n\n";
	system ("pause");

	//The main loop of the program
	//This loop will run while bContinue == true
	do 
	{
		//Display the menu
		system ("cls");
		cout << "\t\t\tPaper Rock Scissors\n\n";
		cout << "Round #" << iRoundCount 
			<< "\tComputer Score: " << iComputerScore 
			<< "\tPlayer Score: " << iHumanScore << endl << endl;
		cout << "p. Paper" << endl
			<< "r. Rock" << endl
			<< "s. Scissors" << endl 
			<< "q. Quit" << endl << endl
			<< "Your selection: ";

        //Get the users selection from the keyboard
		//Clear any invalid input first
		cin.clear();
		cin.ignore(cin.rdbuf()->in_avail() , '\n');
		cin.get (cChoice);

		//Set iHumanChoice based on cChoice
		switch (cChoice)
		{
		case 'p':	//Paper
		case 'P':
		case '1':
			iHumanChoice = 1; 
			break;

		case 'r':	//Rock
		case 'R':
		case '2':
			iHumanChoice = 2; 
			break;

		case 's':	//Scissors
		case 'S':
		case '3':
			iHumanChoice = 3; 
			break;

		case 'q':	//Bye-Bye
		case 'Q':
		case 'x':
		case 'X':
			iHumanChoice = 0;
			bContinue = false;		//This will exit the program
			break;

		default:
			cout << "Invalid selection, please try again.\n\n\n";
			system ("pause");
		}	//switch (cChoice)

		//If the user input validates, then we can determine a winner
		//If the user input is invalid, then iHumanChoice will be 0 at this point
		if (iHumanChoice)
		{
			//Get the computers choice randomly
            iComputerChoice = rand () % 3 + 1;	//Random number 1-3

            //Time to find a winner!
			switch (iHumanChoice)
			{
			case 1:
				cout << "You have chosen paper.\n";
				switch (iComputerChoice)
				{
				case 1:
					cout << "The computer has chosen paper.\n";	//paper vs paper
					iRoundWinner = 3;							//tie
					break;

				case 2:
					cout << "The computer has chosen rock.\n";	//paper vs rock
					iRoundWinner = 1;							//human wins
					break;

				case 3:
					cout << "The computer has chosen scissors.\n";	//paper vs scissors
					iRoundWinner = 2;								//computer wins
					break;

				default:
                    cout << "An internal error (1) has occured.\n";	//OOPS!
					system ("pause");
					return -1;
				}	//switch (iComputerChoice)
				break;

			case 2:
				cout << "You have chosen rock.\n";
                switch (iComputerChoice)
				{
				case 1:
					cout << "The computer has chosen paper.\n";	//rock vs paper
					iRoundWinner = 2;							//computer wins
					break;

				case 2:
					cout << "The computer has chosen rock.\n";	//rock vs rock
					iRoundWinner = 3;							//tie
					break;

				case 3:
					cout << "The computer has chosen scissors.\n";	//rock vs scissors
					iRoundWinner = 1;								//human wins
					break;

				default:
					cout << "An internal error (2) has occured.\n";	//OOPS!
					system ("pause");
					return -1;
				}	//switch (iComputerChoice)
				break;

			case 3:
				cout << "You have chosen scissors.\n";
				switch (iComputerChoice)
				{
				case 1:
					cout << "The computer has chosen paper.\n";	//scissors vs paper
					iRoundWinner = 1;							//human wins
					break;

				case 2:
					cout << "The computer has chosen rock.\n";	//scissors vs rock
					iRoundWinner = 2;							//computer wins
					break;

				case 3:
					cout << "The computer has chosen scissors.\n";	//scissors vs scissors
					iRoundWinner = 3;								//tie
					break;

				default:
					cout << "An internal error (3) has occured.\n";	//OOPS!
					system ("pause");
					return -1;
				}	//switch (iComputerChoice)
				break;

			default:
				cout << "An internal error (4) has occured.\n";	//OOPS!
				system ("pause");
				return -1;
			}	//switch (iHumanChoice)

			//Now that we have decided the winner, points can be given out
            switch (iRoundWinner)
			{
			case 1:		
				//Human wins!
                cout << "You have won this round!\n";
				iHumanScore++;
				iRoundCount++;
				break;

			case 2:		
				//computer wins
				cout << "You have lost this round.\n";
				iComputerScore++;
				iRoundCount++;
				break;

			case 3:		
				//tie, no points given, round does not count towards total
				cout << "A tie has resulted.\n";
				break;

			default:	
				//OH NO!
				cout << "An internal error (5) has occured.\n";
				system ("pause");
				return -1;
			}	//switch (iRoundWinner)

			//Reset variables for the next round
			iRoundWinner = 0;
			iHumanChoice = 0;
			iComputerChoice = 0;
            cChoice = 0;

			system ("pause");

			//Test for a match winner (best-of-7)
			if ((iHumanScore >= 4) || (iComputerScore >= 4))
			{
				//Display who won
				system ("cls");
				cout << "We have a winner!\n";
				cout << ((iHumanScore > iComputerScore) ? ("And it's YOU!\n") : ("And it's ME!\n"));

				//See if the user would like to continue playing
				cout << endl << "Would you like to play again? ";
				cin.clear();
				cin.ignore(cin.rdbuf()->in_avail() , '\n');
				cin.get (cChoice);

                //Should the program start a new match, or quit?
				switch (cChoice)
				{
				case 'n':	//Any of these will quit the game
				case 'N':	//Even though most of them aren't listed
				case 'x':
				case 'X':
				case 'q':
				case 'Q':
					bContinue = false;	//Bye-Bye
					break;

				default:
					//Assume the user absolutely loves this game
					//and never wants to leave.

					//Reset scores and iRoundCount
					iComputerScore = 0;
					iHumanScore = 0;
					iRoundCount = 1;
					cChoice = 0;	//To prevent errors
					break;
				}	//switch (cChoice)
			}	//if (score >= 4)
		}	//if (iHumanChoice)

	} while (bContinue);

	return 0;
}	//int main (void)