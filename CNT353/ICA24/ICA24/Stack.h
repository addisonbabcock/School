#pragma once
#include <iostream>

using namespace std;

template <typename T>
class TStack
{
protected:
	int m_iCapacity;
	int m_iCurrent;
	T * m_Stk;
public:
	TStack( int iDim );
	TStack( TStack<T> const & copy );
	virtual ~TStack();
	TStack<T> & operator=( TStack<T> const & c );
	virtual TStack<T> & Push( T const & dValue );
	virtual T Pop( void );
	virtual int Size( void );

	friend ostream & operator << <T> (ostream & out, TStack <T> const & rhs);
};
template <typename T>
TStack<T>::TStack( int iDim ):
m_iCapacity(iDim), m_Stk(new T[iDim]), m_iCurrent(0)
{}
template <typename T>
TStack<T>::TStack( TStack<T> const & copy ):
m_iCapacity(copy.m_iCapacity), m_Stk(new T[copy.m_iCapacity]), m_iCurrent(copy.m_iCurrent)
{
	for( int i = 0; i < copy.m_iCurrent; i++ ) 
		m_Stk[i]= copy.m_Stk[i];
}
template <typename T>
TStack<T> & TStack<T>::operator = (TStack<T> const & copy)
{
	if (this == &copy)
		return *this;

	m_iCapacity = copy.m_iCapacity;
	m_iCurrent = copy.m_iCurrent;

	delete [] m_Stk;
	m_Stk = new T [m_iCapacity];
	for (int i(0); i < copy.m_iCurrent; ++i)
		m_Stk[i] = copy.m_Stk [i];

	return *this;
}
template <typename T>
TStack<T>::~TStack()
{
	delete [] m_Stk; 
	m_iCurrent = m_iCapacity = 0;
	m_Stk = 0;
}
template <typename T>
TStack<T> & TStack<T>::Push( T  const  & dValue  )
{
	if( m_iCurrent > m_iCapacity )
		throw string("TStack<T>::Push - Stack Full");
	m_Stk[m_iCurrent++] = dValue;
	return *this;
}
template <typename T>
T TStack<T>::Pop( void )
{
	if( m_iCurrent <= 0 )
		throw string("TStack<T>::Pop - Stack Empty");
	return m_Stk[--m_iCurrent];
}
template <typename T>
int TStack<T>::Size( void )
{
	return m_iCurrent;
}
template <typename T>
ostream & operator << (ostream & out, TStack<T> const & rhs)
{
	for (int i (rhs.m_iCapacity - 1); i >= 0; --i)
		out << '[' << i << "] : " << rhs.m_Stk[i] << endl;
	return out;
}

