#include <iostream>
using namespace std;

int main ()
{
	char cChar = 0;
	int iInt = 0;

	cout << "\t\t\tAEIOU (but never y)\n\n";
	cout << "Enter a letter from a-Z: ";
	cin >> cChar;

	iInt = static_cast <int> (cChar);	//Get ASCII code value

	while ((iInt < 65) || ((iInt > 90) && (iInt < 97)) || (iInt > 122)) //Error checking... a-z && A-Z are valid
	{
		cout << "Invalid letter, please input a letter from a to Z: ";
		cin >> cChar;
		iInt = static_cast <int> (cChar);	//Get ASCII code value
	}

	switch (cChar)
	{
	case 'a':	//Fall through is on-purpose
	case 'e':
	case 'i':
	case 'o':
	case 'u':
		cout << "The letter entered is a lower case vowel.\n";
		break;
	case 'A':	//Fall through is on-purpose
	case 'E':
	case 'I':
	case 'O':
	case 'U':
		cout << "The letter entered is an upper case vowel.\n";
		break;
	default:
		cout << "The letter entered is a consonant.\n";
	}

	system ("pause");
	return 0;
}