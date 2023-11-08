#pragma once
#include "User.h"

class Like
{
public:
	Like() = default;
	Like(int, User);
	~Like() = default;

	int GetID()const;
	User GetUser()const;

	void SetID( int ID);
	void SetUser(const User& user);


	int operator<(const Like & like)const;
	bool operator==(const Like & like)const;

private:
	int m_ID;
	User m_user;
};

