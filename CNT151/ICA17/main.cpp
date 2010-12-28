#include <iostream>
#include <string>

using namespace std;

const unsigned int kuiMaxStrLen = 500;

int main ()
{
	char szFirstName[kuiMaxStrLen] = "";
	char szLastName[kuiMaxStrLen] = "";
	char szFullName[kuiMaxStrLen * 2] = "";
	char szInput[kuiMaxStrLen] = "";
	bool bContinue = true;
	unsigned int uiStrLen = 0;

	do {
		cout << "\nEnter your first name: ";
		cin.getline (szFirstName, kuiMaxStrLen);		

		cout << "Enter your last name: ";
		cin.getline (szLastName, kuiMaxStrLen);

	    strcpy (szFullName, szLastName);
		strcat (szFullName, ", ");
		strcat (szFullName, szFirstName);
		uiStrLen = static_cast<unsigned int> (strlen (szFullName));

		cout << endl << szFullName << "\tLength = " << uiStrLen << endl;


		cout << endl << "Run again? ";
		cin.getline (szInput, kuiMaxStrLen);

		while (strcmpi (szInput, "yes") != 0  && strcmpi (szInput, "no"))
		{
			cout << "Please answer \"yes\" or \"no\": ";
			cin.getline (szInput, kuiMaxStrLen);
		}

		if (strcmpi (szInput, "no") == 0)
			bContinue = false;

	} while (bContinue);

	system ("pause");
	return 0;
}