#include "main.h"

double Sum (double dMarks [], int iSize)
{
	int iIndex = 0;
	double dSum = 0.0;

	for (iIndex = 0; iIndex < iSize; iIndex++)
		dSum += dMarks [iIndex];
	return dSum;
}

double SumR (double dMarks [], int iSize)
{
	if (iSize > 0)
		return SumR (dMarks, iSize - 1) + dMarks [iSize - 1];

	return 0;
}

int SumR (int iMarks [], int iSize)
{
	if (iSize > 0)
		return SumR (iMarks, iSize - 1) + iMarks [iSize - 1];

	return 0;
}

int main (int argc, char** argv)
{
	double dMarks [5] = {56.6, 89.9, 34.4, 66.7, 77.8};
	int iMarks [7] = {56, 89, 34, 66, 77, 100, 101};

	//returns a double sum, no recursion
	cout << Sum (dMarks, 5) << endl;

	//returns a double sum, using recursion
	cout << SumR (dMarks, 5) << endl;

	//this will return an int sum, using recursion
	cout << SumR (iMarks, 7) << endl;

	system ("pause");
	return 0;
}