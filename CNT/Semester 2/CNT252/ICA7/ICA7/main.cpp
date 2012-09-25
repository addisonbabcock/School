#include "main.h"

int main ()
{
	UTime uTime;

	cout << "Input hours: ";
	cin >> uTime.sTime.m_suiHour;

	cout << "Input minutes: ";
	cin >> uTime.sTime.m_suiMinute;

	cout << "Input seconds: ";
	cin >> uTime.sTime.m_suiSecond;

	uTime.dTime = static_cast <double> (uTime.sTime.m_suiHour) + 
		static_cast <double> (uTime.sTime.m_suiMinute) / 60.0 +
		static_cast <double> (uTime.sTime.m_suiSecond) / 3600.0;

	cout << "Time in hours: " << setprecision (3) << fixed << uTime.dTime << endl;

	cout << endl;
	system ("pause");
	return 0;
}