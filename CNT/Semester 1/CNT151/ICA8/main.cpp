#include <iostream>
#include <iomanip>

using namespace std;

const unsigned int kiRightColumnWidth = 10;
const unsigned int kiLeftColumnWidth = 49;

int main (void)
{
	char cChar = 0;
	int iInt = 0;
        	
	cout << "\t\t\tASCII Codes and Number Bases" << endl << endl;

	cout << "Enter a character: ";
	cin >> cChar;
	
	cout << left << setw (kiLeftColumnWidth) << setfill ('.') << "Character that was entered: "
		<< right << setw (kiRightColumnWidth) << setfill (' ') << cChar << endl;
	cout << left << setw (kiLeftColumnWidth) << setfill ('.') << "ASCII code of character in decimal: " 
		<< right << setw (kiRightColumnWidth) << setfill (' ') << static_cast<unsigned int> (cChar) << endl;
	cout << left << setw (kiLeftColumnWidth) << setfill ('.') << "ASCII code of character in octal: "
		<< right << oct << setw (kiRightColumnWidth) << setfill (' ') << static_cast<unsigned int> (cChar) << endl;
	cout << left << setw (kiLeftColumnWidth) << setfill ('.') << "ASCII code of character in hexadecimal: "
		<< right << hex << setw (kiRightColumnWidth) << setfill (' ') << static_cast<unsigned int> (cChar) << endl << endl;

	cout << "Enter an integer: ";
	cin >> iInt;

	cout << left << setw (kiLeftColumnWidth) << setfill ('.') << "Number that was entered in decimal: "
		<< right << setw (kiRightColumnWidth) << setfill (' ') << dec << iInt << endl;
	cout << left << setw (kiLeftColumnWidth) << setfill ('.') << "Number displayed as a char: "
		<< right << setw (kiRightColumnWidth) << setfill (' ') << static_cast<char> (iInt) << endl;
	cout << left << setw (kiLeftColumnWidth) << setfill ('.') << "Number that was entered in octal: "
		<< right << setw (kiRightColumnWidth) << setfill (' ') << oct << iInt << endl;
	cout << left << setw (kiLeftColumnWidth) << setfill ('.') << "Number that was entered in hexadecimal: "
		<< right << setw (kiRightColumnWidth) << setfill (' ') << hex << iInt << endl;

	system ("pause");
	return 0;
}