#include <iostream>
using namespace std;

int main ( void )
{
	int iSignedInt = 0;
	unsigned int uiUnsignedInt = 0;
	char cChar = 0;
	double dDouble = 0.0;

	cout << "The original unsigned int is: " << uiUnsignedInt << endl;
	cout << "The original signed int is: " << iSignedInt << endl;
	cout << "The original char is: " << cChar << endl;
	cout << "The original double is: " << dDouble << endl << endl;

	cout << "Enter an unsigned int: ";
	cin >> uiUnsignedInt;
	cout << "Enter a signed int: ";
	cin >> iSignedInt;
	cout << "Enter a char: ";
	cin >> cChar;
	cout << "Enter a double: ";
	cin >> dDouble;

	cout << endl << "The unsigned int was: " << uiUnsignedInt << endl;
	cout << "The signed int was: " << iSignedInt << endl;
	cout << "The char was: " << cChar << endl;
	cout << "The double was: " << dDouble << endl;

	system ("pause");

	return 0;
}