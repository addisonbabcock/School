#include <iostream>
#include "Utilities.h"
using namespace std;

char ConvertGrade (double);

char ConvertGrade (double dGrade)
{
	if (dGrade < 50.0)
		return 'F';
	if (dGrade < 60.0 && dGrade >= 50.0)
		return 'D';
	if (dGrade < 70.0 && dGrade >= 60.0)
		return 'C';
	if (dGrade < 80.0 && dGrade >= 70.0)
		return 'B';
	if (dGrade <= 100.0 && dGrade >= 80.0)
		return 'A';
}

int main ()
{
	int iSize = 0;
	double dGrades [10];
	char cLetterGrade [10];

	for (int i = 0; i < 10; i++)
	{
		dGrades[i] = 0.0;
		cLetterGrade[i] = 'F';
	}

	cout << "\t\t\tGrade Convertor\n\n";
	cout << "How many grades to convert, max is 10: ";
	iSize = GetInt (1, 10);
	cout << endl;

	for (int i = 0; i < iSize; i++)
	{
		cout << "Enter grade value " << i+1 << ": ";
		dGrades[i] = GetDouble (0.0, 100.0);
        cLetterGrade[i] = ConvertGrade (dGrades[i]);
	}

	cout << endl;
	for (int i = iSize - 1; i >= 0; i--)
		cout << dGrades[i] << " has a letter grade of " << cLetterGrade[i] << endl;

	system ("pause");
	return 0;
}