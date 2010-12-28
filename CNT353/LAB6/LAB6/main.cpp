#include "BlackJack.h"
#include <crtdbg.h>
#include "e:\lab06_test.h"

void Lab06TestAddison ()
{
	CBlackJack black;
	black.PopulateDeck (1);
	black.Shuffle ();

	//list <int> hand;
	//hand.push_back (1);
	//hand.push_back (9);
	//hand.push_back (1);
	//cout << black.Sum (hand) << endl;

	black.SetDealerMax (17);
	black.SetPlayerMax (17);
	black.PopulateDeck (1);
	black.Shuffle ();
	black.PlayHands ();
	cout << endl;
	black.Stats ();
}

int main ()
{
	_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
//	Lab06TestAddison ();
	Lab06Test();
	cin.get ();
	return 0;
}