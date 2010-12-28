#include <iostream>

using namespace std;

void fn (char* szStr);
void fn2 (char* szStr);

int main (int argc, char** argv)
{
	if (2 == argc)
	{
		fn (argv [1]);
		cout << endl;
		//fn2 (argv [1]);
	}
	else
		cout << "Invalid command line!";
 
	cout << endl;
	system ("pause");
	return 0;
}

void fn (char* szStr)
{
	if (0 == *szStr)
		return;

	if (isupper (*szStr))
		cout << *szStr;

	fn (szStr + 1);

	if (islower (*szStr))
		cout << *szStr;
}

void fn2 (char* szStr)
{
	for (int i = 0; szStr [i] != 0; i++)
		if (isupper (szStr [i]))
			cout << szStr [i];

	for (int i = strlen (szStr); i >= 0; i--)
		if (islower (szStr [i]))
			cout << szStr [i];
}