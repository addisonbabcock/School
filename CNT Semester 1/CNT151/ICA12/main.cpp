//#define HIGH_SCORES

#include <iostream>
#include <iomanip>
#include <cmath>
#include <ctime>
#include "main.h"

#ifdef HIGH_SCORES
#include <fstream>
#include <string>
#include <sstream>
#endif //HIGH_SCORES

using namespace std;

const double kdPi = 3.14159265358979323846;
const double kdG = 9.81;

int main ()
{
	double dDistance = 0.0;
	double dAngle = 0.0;
	double dVelocity = 0.0;
	double dAngleRad = 0.0;
	double dTarget = 0.0;
	double dHowClose = 0.0;
	bool bContinue  = true;
	bool bGunIsReady = false;
	char cInput = 0;
	unsigned int uiScore = 0;
	unsigned int uiMisses = 0;

	srand (static_cast <unsigned int> (time (0)));
	dTarget = rand () % 1000 + 500;

	do
	{
		
		system ("cls");
		cout << "\t\t\tAtomic Cannon Simulator\n\n";	

		cout << "Target Distance: " << setprecision (0) << fixed << dTarget << endl;
		cout << "Gun settings: Velocity = " << dVelocity << " m/s, angle = " << dAngle << " degrees\n\n";

		cout << "Status: ";
		if (bGunIsReady)
			cout << "Gun is READY, Sir!\n\n";
		else
			cout << "Not ready\n\n";

		cInput = Menu ();
		switch (cInput)
		{
		case 'a':
		case 'A':
			dVelocity = GetVelocity ();
			
			if (dAngle != 0.0)
				bGunIsReady = true;

			break;

		case 'b':
		case 'B':
			dAngle = GetAngle ();

			if (dVelocity != 0.0)
				bGunIsReady = true;

			break;

		case 'c':
		case 'C':
			//fire!
			dAngleRad = dAngle * (kdPi / 180);
			dDistance = (2 * dVelocity * dVelocity * cos (dAngleRad) * sin (dAngleRad)) / kdG;
			dHowClose = dDistance - dTarget;
			uiMisses++;

			cout << "The shot distance was " << setprecision (2) << fixed << dDistance << " meters.\n";
   
			if (abs (dHowClose) <= 10)
			{
				cout << "A DIRECT HIT! FINE SHOT, SIR!\n";

				bGunIsReady = false;
				dVelocity = 0.0;
				dAngle = dAngleRad = 0.0;
				dDistance = 0.0;
				dTarget = rand () % 1000 + 500; //make a new target

                uiScore += 10 - uiMisses; //10 points for every hit, -1 for every miss
				uiMisses = 0;
			}
			else
			{
                cout << "We missed the target by " << setprecision (0) << fixed << abs(dHowClose)
					<< " meters, sir.\n";
			}
			system ("pause");
			break;

		case 'q':
		case 'Q':
		case 'x':
		case 'X':
			bContinue  = false;
			break;
			
		default:
			cout << "Invalid input: " << cInput << endl;
			system ("pause");
		}
	} while (bContinue);

	system ("cls");
	cout << "Thank you for playing. Your score was: " << uiScore << ".\n\n";
	system ("pause");

#ifdef HIGH_SCORES //Available in version 2 only :P
	ifstream highScoresIn;
	unsigned int uiScores [11] = {0,0,0,0,0,0,0,0,0,0,0};	//#11 is discarded
	string sNames [11];
	int iRanking = 10;

	highScoresIn.open ("c:\\atomic.txt", ios::in);
	
	if (highScoresIn.bad())
	{	//file does not exist! don't input from it!
		iRanking = 0;	//Even a score of 0 is in first if he's the first to play
	}
	else
	{	//file does exist! input from it!
		int i;
		for (i = 0; i <= 9; i++)	//Load the high scores file
		{
			highScoresIn >> sNames [i];
			highScoresIn >> uiScores [i];
		}
        for (int i = 9; i >= 0; i--)
		{
			if (uiScores [i] <= uiScore)	//does the player have a better score then whats in the list?
			{
                sNames [i + 1] = sNames [i];	//move the high scores down
				uiScores [i + 1] = uiScores [i];
			}
			else
			{
				iRanking = i + 1;
				break;
			}
		}
	}
	highScoresIn.close();

	if (iRanking < 10)	//Don't get initials if the player sucks :P
	{
		cout << "Congratulations, sir, you've made the high scores!\nYou're number: " << iRanking + 1 << endl;
		cout << "Please input your 3 letter initials: ";
		cin >> sNames [iRanking];
		uiScores [iRanking] = uiScore;
	}

	ofstream highScoresOut ("c:\\atomic.txt", ios::out);
	if (highScoresOut.bad())
	{
		cerr << "Cannot open c:\\atomic.txt for output";
		return -1;
	}

	for (int i = 0; i <= 9; i++)
	{
		highScoresOut << sNames [i] << endl << uiScores [i] << endl;
	}	
	highScoresOut.close ();

	system ("cls");

	const unsigned char cTopOfTable [14] = {201, 205, 205, 205, 205, 205, 203, 205, 205, 205, 205, 205, 187, 0};
	const unsigned char cBottomOfTable [14] = {200, 205, 205, 205, 205, 205, 202, 205, 205, 205, 205, 205, 188, 0};

	cout << "\t\t\t High Scores\n\n";
	cout << "\t\t\t" << cTopOfTable << endl;
	cout << setprecision (3) << fixed << setfill (' ');

	for (int i = 0; i <= 9; i++)
	{
		cout << "\t\t\t" << static_cast <unsigned char> (186);

		switch (3 - (sNames[i].length()))
		{
		/*Please note, fall through is intentional*/
		case 3: cout << ' ';
		case 2: cout << ' ';
		case 1: cout << ' ';
		case 0:	break;
		}
		cout << ' ' << sNames [i] << ' ' << static_cast <unsigned char> (186);

		stringstream ss;
		string str;
		ss << uiScores [i];	//If this isn't the lamest way to convert int to string
		ss >> str;			//I don't know what is

		switch (3 - (str.length()))
		{
		/*Please note, fall through is intentional*/
		case 3: cout << ' ';
		case 2: cout << ' ';
		case 1: cout << ' ';
		case 0:	break;
		}
		cout << ' ' << str << ' ' << static_cast <unsigned char> (186) << endl;
	}

	cout << "\t\t\t" << cBottomOfTable << endl << endl << endl;

	system ("pause");
#endif	//_HIGH_SCORES_
	return 0;
}

char Menu (void)
{
	char cInput = 0;

	cout << "Select action...\n\na. Input muzzle veloctiy.\nb. Input firing angle.\nc. Fire!\nq. Quit.\n\n"
		<< "Please make a selection: ";

	cin.clear ();
	cin.ignore (cin.rdbuf ()->in_avail (), '\n');
	cin.get (cInput);

	return cInput;
}

double GetVelocity (void)
{
	double dVelocity = 0.0;

	do
	{
        cout << "Enter muzzle velocity in m/s: ";

		cin.clear ();
		cin.ignore (cin.rdbuf () -> in_avail (), '\n');
		cin >> dVelocity;

		if (dVelocity >= 50 && dVelocity < 200)
			break;

		cout << "Invalid input, please try again.\n";

	} while (true);

	return dVelocity;
}

double GetAngle (void)
{
	double dAngle = 0.0;

	do 
	{
		cout << "Enter firing angle in degrees: ";
			
		cin.clear ();
		cin.ignore (cin.rdbuf () -> in_avail (), '\n');
		cin >> dAngle;

		if (dAngle > 0 && dAngle < 90)
			break;

		cout << "Invalid input, please try again.\n";

	} while (true);

	return dAngle;
}

void function ();