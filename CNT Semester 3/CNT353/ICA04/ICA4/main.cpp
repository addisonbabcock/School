#include <iostream>
#include "MailBox.h"

// Copy Constructor test function
void PassTest( CMailBox copy )
{
  copy.AddMsg("Four");
  cout << "Copy test:\n" ;
  copy.ShowMsg( cout );
  if( copy.AddMsg("Should be full"))
    cout << "Error on AddMsg(), should be full" << endl;
}

int main( int argc, char* argv[] )
{
  _CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF | _CRTDBG_CHECK_ALWAYS_DF);
  { // ICA04 Test code
    CMailBox cBox;
    cBox.ShowMsg( cout ) << endl;
    cin.get();
    if( !cBox.AddMsg("Don't you know who I think I am?"))
      cout << "Error on AddMsg()" << endl;
    cBox.AddMsg("Testing, testing, one");
    cBox.AddMsg("Testing, testing, two");
    cBox.AddMsg("Testing, testing, three");
    cBox.ShowMsg( cout );
    cin.get();
    PassTest( cBox ); //test COPY CTOR via Pass-by-value
    cout << "Back to original\n";
    cBox.ShowMsg( cout );
		cin.get();
    CMailBox const cCopy( cBox ); // explicitly invoke copy ctor
    CMailBox const cOther( cCopy ); // once again based on first const copy, whatsit testing ?
    cOther.ShowMsg( cout );
    cin.get();
  }// end of ICA04 test code
  return 0;
}