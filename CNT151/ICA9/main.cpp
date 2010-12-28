#include <iostream>
using namespace std;

int main ()
{
	unsigned int iGrade = 0;

	cout << "\t\t\tGrade Converter\n\n";

	cout << "Enter the grade in percent: ";
	cin >> iGrade;

	if ((iGrade < 0) || (iGrade > 100))
	{
		cout << "WTF!!\a\n";

		system ("pause");
		return 0;
	}

	if (iGrade <= 49)		//Spaghetti code of doom!
		cout << "F";
	else
		if (iGrade <= 54)
			cout << "D";
		else
			if (iGrade <= 59)
				cout << "D+";
			else
				if (iGrade <= 62)
					cout << "C-";
				else
					if (iGrade <= 66)
						cout << "C";
					else
						if (iGrade <= 69)
							cout << "C+";
						else
							if (iGrade <= 72)
								cout << "B-";
							else
								if (iGrade <= 76)
									cout << "B";
								else
									if (iGrade <= 79)
										cout << "B+";
									else
										if (iGrade <= 84)
											cout << "A-";
										else
											if (iGrade <= 89)
												cout << "A";
											else
												cout << "A+";
	cout << endl;
	system ("pause");
	return 0;
}