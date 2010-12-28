#include <iostream>
#include <algorithm>
#include <vector>
#include <queue>
#include <list>
#include <ctime>

using namespace std;

void outList (int i)
{
	cout << i << ' ';
}

int main ()
{
	vector <int> validNums;

	//2
	list <int> win, guess, match;
	//3
	queue <list <int> > nums;

	//4
	srand ((unsigned)time(NULL));

	//5
	for (int i (1); i<= 29; ++i)
		validNums.push_back (i);
	random_shuffle (validNums.begin (), validNums.end ());
	for (int i (0); i < 5; ++i)
		win.push_back (validNums [i]);

	//6
	win.sort ();
	for_each (win.begin (), win.end (), outList);
	cout << endl;

	//7
	for (int attempts (0); attempts < 1000000; ++attempts)
	{
		match.clear ();
		guess.clear ();

		random_shuffle (validNums.begin (), validNums.end ());
		for (int i (0); i < 5; ++i)
			guess.push_back (validNums [i]);

		win.sort ();
		guess.sort ();
		set_intersection (win.begin (), win.end (), guess.begin (), guess.end (), back_inserter (match));

		if (match.size () > 3)
			nums.push (match);
		
		if (match.size () == 5)
			break;
	}

	//8
	list <int> output;
	for (unsigned int i (0); i < nums.size (); ++i)
	{
		output = nums.front ();
		nums.pop ();
		output.sort ();

		cout << output.size () << " : ";
		for_each (output.begin (), output.end (), outList);
		cout << endl;
	}

	//9
	cin.get ();
	return 0;
}