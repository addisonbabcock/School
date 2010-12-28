#include <iostream>
#include <fstream>
#include <string>
#include <map>
#include <vector>
#include <algorithm>

using namespace std;

int main ()
{
	_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);

	map<string, int> dictionary;
	fstream file;
	string str;
	string letters;
	string five;

	//2
	for (char c ('a'); c <= 'z'; ++c)
		letters.push_back (c);

	//3
	file.open ("y:\\words.txt", ios::in);

	if (!file)
	{
		cerr << "Could not open file!\n";
		cin.get ();
		return 1;
	}

	while (!(file >> str).eof ())
	{
		dictionary [str] = 0;
	}

	int iHits (0);
	//7
	while (iHits <= 37)
	{
		//4
		random_shuffle (letters.begin (), letters.end ());
		five.clear ();
		for (int i(0); i < 5; ++i)
		{
			five.push_back (letters [i]);
		}

		//5
		sort (five.begin (), five.end ());

		//6
		while (next_permutation (five.begin (), five.end ()))
		{
			if (dictionary.find (five) != dictionary.end ())
			{
				dictionary[five] += 1;
				++iHits;
			}
		}
	}

	//8
	for (map<string,int>::iterator iter = dictionary.begin ();
		 iter != dictionary.end (); ++iter)
	{
		if (iter->second)
		{
			cout << "[" << iter->first << "] = " << iter->second << endl;
		}
	}

	//9
	map <char, int> charCount;
	for (map<string, int>::iterator iter = dictionary.begin ();
		 iter != dictionary.end (); ++iter)
	{
		for (string::const_iterator jter = iter->first.begin ();
			 jter != iter->first.end (); ++jter)
		{
			charCount [*jter] += 1;
		}
	}

	//10
	for (char c ('a'); c <= 'z'; ++c)
	{
		cout << "[" << c << "] = " << charCount [c] << endl;
	}
	cin.get ();
	return 0;
}